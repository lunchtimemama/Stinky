// 
// ParserTests.cs
//  
// Author:
//       Scott Thomas <lunchtimemama@gmail.com>
// 
// Copyright (c) 2010 Scott Thomas
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;

using NUnit.Framework;

using Stinky.Compiler.Source;
using Stinky.Compiler.Source.Parsing;
using Stinky.Compiler.Source.Tokenization;

namespace Stinky.Compiler.Tests
{
	using Source = Action<SourceVisitor>;
	using Token = Func<Parser, Parser>;

	[TestFixture]
	public class ParserTests : Sources
	{
		[Test]
		public void ParseReference()
		{
			AssertCompilation("foo", Reference("foo"));
		}
		
		[Test]
		public void ParseLiteralZero()
		{
			AssertCompilation("0", Number(0));
		}
		
		[Test]
		public void ParseNumberLiteralOne()
		{
			AssertCompilation("1", Number(1));
		}
		
		[Test]
		public void ParseNumberLiteralOnePointFive()
		{
			AssertCompilation("1.5", Number(1.5));
		}
		
		[Test]
		public void ParsePlusOperator()
		{
			AssertCompilation("1+2", Plus(Number(1, 0), Number(2, 2), 1));
		}
		
		[Test]
		public void ParseTwoPlusOperators()
		{
			AssertCompilation("1+2+3", Plus(Plus(Number(1, 0), Number(2, 2), 1), Number(3, 4), 3));
		}
		
		[Test]
		public void ParsePlusOperatorWithWhitespace()
		{
			AssertCompilation("1 + 2", Plus(Number(1, 0), Number(2, 4), 2));
		}
		
		[Test]
		public void ParseMinusOperator()
		{
			AssertCompilation("1-2", Minus(Number(1, 0), Number(2, 2), 1));
		}
		
		[Test]
		public void ParseForwardSlashOperator()
		{
			AssertCompilation("1/2", ForwardSlash(Number(1, 0), Number(2, 2), 1));
		}
		
		[Test]
		public void ParseAsteriskOperator()
		{
			AssertCompilation("1*2", Asterisk(Number(1, 0), Number(2, 2), 1));
		}
		
		[Test]
		public void ParseArithmeticOrderOfOperations()
		{
			AssertCompilation("1*2+3/4", Plus(
				Asterisk(Number(1, 0), Number(2, 2), 1),
				ForwardSlash(Number(3, 4), Number(4, 6), 5), 3));
		}
		
		[Test]
		public void ParseDefinition()
		{
			AssertCompilation("foo:42", Definition(Reference("foo"), Number(42, 4), 3));
		}
		
//		[Test, ExpectedException(typeof(ParseException))]
//		public void TestDoubleDefinitionFailure()
//		{
//			Compile("foo:42:10");
//		}
		
		[Test]
		public void ParseReferenceDefinition()
		{
			AssertCompilation("foo:bar", Definition(Reference("foo"), Reference("bar", 4), 3));
		}

		[Test]
		public void ParseEmptyStringLiteral()
		{
			AssertCompilation(@"""""", String("", 1, 0));
		}
		
		[Test]
		public void ParseStringLiteral()
		{
			AssertCompilation(@"""foo""", String("foo", 1, 3));
		}
		
		[Test]
		public void ParseEscapedStringLiteral()
		{
			AssertCompilation(@"""\""\n\t\\""", String("\"\n\t\\", 1, 8));
		}
		
		[Test]
		public void ParseUninterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {{bar}} bat""", String(@"foo {bar} bat", 1, 15));
		}
		
//		[Test, ExpectedException(typeof(TokenizationException))]
//		public void TestUninterpolatedStringLiteralFailure()
//		{
//			Compile(@"""foo {{bar} bat""");
//		}
//		
//		[Test, ExpectedException(typeof(TokenizationException))]
//		public void TestTerminalUninterpolatedStringLiteralFailure()
//		{
//			Compile(@"""foo {{bar}""");
//		}
//		
//		[Test, ExpectedException(typeof(TokenizationException))]
//		public void TestInterpolatedStringLiteralFailure()
//		{
//			Compile(@"""foo { """);
//		}
//		
//		[Test, ExpectedException(typeof(TokenizationException))]
//		public void TestTerminalInterpolatedStringLiteralFailure()
//		{
//			Compile(@"""foo {""");
//		}
		
		[Test]
		public void ParseSimpleInterpolatedStringLiteral()
		{
			AssertCompilation(@"""{foo}""", InterpolatedString(1, 5, Reference("foo", 2)));
		}
		
		[Test]
		public void ParseInterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {bar} bat""",
				InterpolatedString(1, 13, String("foo ", 1, 4), Reference("bar", 6), String(" bat", 10, 4)));
		}
		
		[Test]
		public void ParseTerminalInterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {bar}""", InterpolatedString(1, 9, String("foo ", 1, 4), Reference("bar", 6)));
		}
		
		[Test]
		public void ParseComplexInterpolatedStringLiteral()
		{
			AssertCompilation(@"""{{foo}}: {bar + 42 + ""%""}, {bat}""", InterpolatedString(1, 32,
				String(@"{foo}: ", 1, 9),
				Plus(Plus(Reference("bar", 11), Number(42, 17), 15), String("%", 23, 1), 20),
				String(", ", 26, 2),
				Reference("bat", 29)
			));
		}
		
		[Test]
		public void ParseNestedInterpolatedStringLiterals()
		{
			AssertCompilation(@"""{""{foo}""}""", InterpolatedString(1, 9, InterpolatedString(3, 5, Reference("foo", 4))));
		}
		
		[Test]
		public void ParseNestedInterpolatedStringLiteralsWithTrailingString()
		{
			AssertCompilation(@"""{""{foo}""} bar""",
				InterpolatedString(1, 13, InterpolatedString(3, 5, Reference("foo", 4)), String(" bar", 10, 4)));
		}
		
		[Test]
		public void ParseComplexNestedInterpolatedStringLiterals()
		{
			AssertCompilation(@"""{1+1 + "" is the lonliest number, {name}!""} < she said it""",
				InterpolatedString(1, 56,
					Plus(
						Plus(Number(1, 2), Number(1, 4), 3),
						InterpolatedString(9, 32,
							String(" is the lonliest number, ", 9, 25),
							Reference("name", 35),
							String("!", 40, 1)),
						6),
					String(" < she said it", 43, 14)));
		}

		static void AssertCompilation(IEnumerable<char> code, Source source)
		{
			Source parsedSource = null;
			Token token = null;
			var context = new CompilationContext();
			var parser = context.CreateParser(s => parsedSource = s);
			var rootTokenizer = context.CreateTokenizer(
				t => token = t, () => parser = token(parser), () => parser.OnDone());
			var column = 0;
			foreach(var character in code) {
				rootTokenizer = rootTokenizer.Tokenize(new Character(character, new Location(null, 0, column)));
				column++;
			}
			rootTokenizer.OnDone();
			AssertAreEqual(source, parsedSource);
		}
	}
}
