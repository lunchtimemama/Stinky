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

using Stinky.Compiler.Parser;
using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Tests
{
	using Source = Action<SourceVisitor>;
	using StinkyParser = Stinky.Compiler.Parser.Parser;

	[TestFixture]
	public class ParserTests : Sources
	{
		[Test]
		public void TestReference()
		{
			AssertCompilation("foo", Reference("foo"));
		}
		
		[Test]
		public void TestNumberLiteralZero()
		{
			AssertCompilation("0", Number(0));
		}
		
		[Test]
		public void TestNumberLiteralOne()
		{
			AssertCompilation("1", Number(1));
		}
		
		[Test]
		public void TestNumberLiteralOnePointFive()
		{
			AssertCompilation("1.5", Number(1.5));
		}
		
		[Test]
		public void TestPlusOperator()
		{
			AssertCompilation("1+2", Plus(Number(1, 0), Number(2, 2), 1));
		}
		
		[Test]
		public void TestTwoPlusOperators()
		{
			AssertCompilation("1+2+3", Plus(Plus(Number(1, 0), Number(2, 2), 1), Number(3, 4), 3));
		}
		
		[Test]
		public void TestPlusOperatorWithWhitespace()
		{
			AssertCompilation("1 + 2", Plus(Number(1, 0), Number(2, 4), 2));
		}
		
		[Test]
		public void TestMinusOperator()
		{
			AssertCompilation("1-2", Minus(Number(1, 0), Number(2, 2), 1));
		}
		
		[Test]
		public void TestForwardSlashOperator()
		{
			AssertCompilation("1/2", ForwardSlash(Number(1, 0), Number(2, 2), 1));
		}
		
		[Test]
		public void TestAsteriskOperator()
		{
			AssertCompilation("1*2", Asterisk(Number(1, 0), Number(2, 2), 1));
		}
		
		[Test]
		public void TestArithmeticOrderOfOperations()
		{
			AssertCompilation("1*2+3/4", Plus(
				Asterisk(Number(1, 0), Number(2, 2), 1),
				ForwardSlash(Number(3, 4), Number(4, 6), 5), 3));
		}
		
		[Test]
		public void TestDefinition()
		{
			AssertCompilation("foo:42", Definition(Reference("foo"), Number(42, 4), 3));
		}
		
//		[Test, ExpectedException(typeof(ParseException))]
//		public void TestDoubleDefinitionFailure()
//		{
//			Compile("foo:42:10");
//		}
		
		[Test]
		public void TestReferenceDefinition()
		{
			AssertCompilation("foo:bar", Definition(Reference("foo"), Reference("bar", 4), 3));
		}
		
		[Test]
		public void TestStringLiteral()
		{
			AssertCompilation(@"""foo""", String("foo", 0, 5));
		}
		
		[Test]
		public void TestEscapedStringLiteral()
		{
			AssertCompilation(@"""\""\n\t\\""", String("\"\n\t\\", 0, 10));
		}
		
		[Test]
		public void TestUninterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {{bar}} bat""", String(@"foo {bar} bat", 0, 17));
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
		
		/*[Test]
		public void TestSimpleInterpolatedStringLiteral()
		{
			AssertCompilation(@"""{foo}""", InterpolatedString(0, 7, Reference("foo", 0)));
		}
		
		[Test]
		public void TestInterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {bar} bat""",
				InterpolatedString(0, String("foo ", 0, 5), Reference("bar", 6), String(" bat", 9, 5)));
		}
		
		[Test]
		public void TestTerminalInterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {bar}""", InterpolatedString(0, String("foo ", 0, 5), Reference("bar", 6)));
		}
		
		[Test]
		public void TestComplexInterpolatedStringLiteral()
		{
			AssertCompilation(@"""{{foo}}: {bar + 42 + ""%""}, {bat}""", InterpolatedString(0,
				String(@"{foo}: ", 0, 9),
				Plus(Plus(Reference("bar", 10), Number(42, 16), 14), String("%", 21, 3), 19),
				String(", ", 25, 2),
				Reference("bat", 28)
			));
		}
		
		[Test]
		public void TestNestedInterpolatedStringLiterals()
		{
			AssertCompilation(@"""{""{foo}""}""", InterpolatedString(0, InterpolatedString(2, Reference("foo", 4))));
		}
		
		[Test]
		public void TestNestedInterpolatedStringLiteralsWithTrailingString()
		{
			AssertCompilation(@"""{""{foo}""} bar""",
				InterpolatedString(0, InterpolatedString(2, Reference("foo", 4)), String(" bar", 10, 5)));
		}
		
		[Test]
		public void TestComplexNestedInterpolatedStringLiterals()
		{
			AssertCompilation(@"""{1+1 + "" is the lonliest number, {name}!""} < she said it""",
				InterpolatedString(0,
					Plus(
						Plus(Number(1, 2), Number(1, 4), 3),
						InterpolatedString(8,
							String(" is the lonliest number, ", 7, 24),
							Reference("name", -1),
							String("!", -1, -1)),
						-1),
					String(" < she said it", -1, -1)));
		}*/

		static void AssertCompilation(string code, Source source)
		{
			Source parsedSource = null;
			Func<StinkyParser, StinkyParser> token = null;
			StinkyParser parser = new LineParser(s => parsedSource = s, e => {});
			var rootTokenizer = new RootTokenizer(
				t => token = t, () => parser = token(parser), () => parser.OnDone(), new ErrorConsumer(null, null));
			var column = 0;
			foreach(var character in code) {
				rootTokenizer.OnCharacter(new Character(character, new Location(null, 0, column)));
				column++;
			}
			rootTokenizer.OnDone();
			AssertAreEqual(source, parsedSource);
		}
	}
}
