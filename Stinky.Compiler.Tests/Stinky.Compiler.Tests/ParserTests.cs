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
	[TestFixture]
	public class ParserTests : CompilerTests
	{
		[Test]
		public void TestReference()
		{
			AssertCompilation("foo", Reference("foo"));
		}
		
		[Test]
		public void TestReferences()
		{
			var expressions = Compile("foo\nbar");
			Assert.AreEqual(Reference("foo"), expressions[0]);
			Assert.AreEqual(Reference("bar"), expressions[1]);
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
			AssertCompilation("1+2", Plus(Number(1), Number(2)));
		}
		
		[Test]
		public void TestTwoPlusOperators()
		{
			AssertCompilation("1+2+3", Plus(Plus(Number(1), Number(2)), Number(3)));
		}
		
		[Test]
		public void TestPlusOperatorWithWhitespace()
		{
			AssertCompilation("1 + 2", Plus(Number(1), Number(2)));
		}
		
		[Test]
		public void TestMinusOperator()
		{
			AssertCompilation("1-2", Minus(Number(1), Number(2)));
		}
		
		[Test]
		public void TestForwardSlashOperator()
		{
			AssertCompilation("1/2", ForwardSlash(Number(1), Number(2)));
		}
		
		[Test]
		public void TestAsteriskOperator()
		{
			AssertCompilation("1*2", Asterisk(Number(1), Number(2)));
		}
		
		[Test]
		public void TestArithmeticOrderOfOperations()
		{
			AssertCompilation("1*2+3/4", Plus(Asterisk(Number(1), Number(2)), ForwardSlash(Number(3), Number(4))));
		}
		
		[Test]
		public void TestDefinition()
		{
			AssertCompilation("foo:42", Definition("foo", Number(42)));
		}
		
		[Test, ExpectedException(typeof(ParseException))]
		public void TestDoubleDefinitionFailure()
		{
			Compile("foo:42:10");
		}
		
		[Test]
		public void TestReferenceDefinition()
		{
			AssertCompilation("foo:bar", Definition("foo", Reference("bar")));
		}
		
		[Test]
		public void TestStringLiteral()
		{
			AssertCompilation(@"""foo""", String("foo"));
		}
		
		[Test]
		public void TestEscapedStringLiteral()
		{
			AssertCompilation(@"""\""\n\t\\""", String("\"\n\t\\"));
		}
		
		[Test]
		public void TestUninterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {{bar}} bat""", String(@"foo {bar} bat"));
		}
		
		[Test, ExpectedException(typeof(TokenizationException))]
		public void TestUninterpolatedStringLiteralFailure()
		{
			Compile(@"""foo {{bar} bat""");
		}
		
		[Test, ExpectedException(typeof(TokenizationException))]
		public void TestTerminalUninterpolatedStringLiteralFailure()
		{
			Compile(@"""foo {{bar}""");
		}
		
		[Test, ExpectedException(typeof(TokenizationException))]
		public void TestInterpolatedStringLiteralFailure()
		{
			Compile(@"""foo { """);
		}
		
		[Test, ExpectedException(typeof(TokenizationException))]
		public void TestTerminalInterpolatedStringLiteralFailure()
		{
			Compile(@"""foo {""");
		}
		
		[Test]
		public void TestSimpleInterpolatedStringLiteral()
		{
			AssertCompilation(@"""{foo}""", InterpolatedString(Reference("foo")));
		}
		
		[Test]
		public void TestInterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {bar} bat""",
				InterpolatedString(String("foo "), Reference("bar"), String(" bat")));
		}
		
		[Test]
		public void TestTerminalInterpolatedStringLiteral()
		{
			AssertCompilation(@"""foo {bar}""", InterpolatedString(String("foo "), Reference("bar")));
		}
		
		[Test]
		public void TestComplexInterpolatedStringLiteral()
		{
			AssertCompilation(@"""{{foo}}: {bar + 42 + ""%""}, {bat}""", InterpolatedString(
				String(@"{foo}: "),
				Plus(Plus(Reference("bar"), Number(42)), String("%")),
				String(", "),
				Reference("bat")
			));
		}
		
		[Test]
		public void TestNestedInterpolatedStringLiterals()
		{
			AssertCompilation(@"""{""{foo}""}""", InterpolatedString(InterpolatedString(Reference("foo"))));
		}
		
		[Test]
		public void TestNestedInterpolatedStringLiteralsWithTrailingString()
		{
			AssertCompilation(@"""{""{foo}""} bar""",
				InterpolatedString(InterpolatedString(Reference("foo")), String(" bar")));
		}
		
		[Test]
		public void TestComplexNestedInterpolatedStringLiterals()
		{
			AssertCompilation(@"""{1+1 + "" is the lonliest number, {name}!""} < she said it""",
				InterpolatedString(
					Plus(
						Plus(Number(1), Number(1)),
						InterpolatedString(
							String(" is the lonliest number, "),
							Reference("name"),
							String("!"))),
					String(" < she said it")));
		}

		public static void AssertCompilation(string source, Expression expression)
		{
			Assert.AreEqual(expression, Compile(source)[0]);
		}
		
		public static List<Expression> Compile(string source)
		{
			var expressions = new List<Expression>();
			var driver = new Driver(
				(indentation, expression) => expressions.Add(expression),
				new ErrorConsumer(
					error => { throw new ParseException(); },
					error => { throw new TokenizationException(); }));
			foreach(var character in source) {
				driver.OnCharacter(new Character(character, Nowhere));
			}
			driver.OnDone();
			return expressions;
		}
	}
}
