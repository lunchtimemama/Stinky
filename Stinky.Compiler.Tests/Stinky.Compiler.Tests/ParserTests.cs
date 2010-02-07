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
			var expressions = Compile("foo");
			Assert.AreEqual(new Reference("foo", Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestReferences()
		{
			var expressions = Compile("foo\nbar");
			Assert.AreEqual(new Reference("foo", Nowhere), expressions[0]);
			Assert.AreEqual(new Reference("bar", Nowhere), expressions[1]);
		}
		
		[Test]
		public void TestNumberLiteralZero()
		{
			var expressions = Compile("0");
			Assert.AreEqual(new NumberLiteral(0, Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestNumberLiteralOne()
		{
			var expressions = Compile("1");
			Assert.AreEqual(new NumberLiteral(1, Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestNumberLiteralOnePointFive()
		{
			var expressions = Compile("1.5");
			Assert.AreEqual(new NumberLiteral(1.5, Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestPlusOperator()
		{
			var expressions = Compile("1+2");
			Assert.AreEqual(new PlusOperator(new NumberLiteral(1, Nowhere), new NumberLiteral(2, Nowhere), Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestTwoPlusOperators()
		{
			var expressions = Compile("1+2+3");
			Assert.AreEqual(
				new PlusOperator(
					new PlusOperator(
						new NumberLiteral(1, Nowhere),
						new NumberLiteral(2, Nowhere),
						Nowhere),
					new NumberLiteral(3, Nowhere),
					Nowhere),
				expressions[0]);
		}
		
		[Test]
		public void TestPlusOperatorWithWhitespace()
		{
			var expressions = Compile("1 + 2");
			Assert.AreEqual(new PlusOperator(new NumberLiteral(1, Nowhere), new NumberLiteral(2, Nowhere), Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestMinusOperator()
		{
			var expressions = Compile("1-2");
			Assert.AreEqual(new MinusOperator(new NumberLiteral(1, Nowhere), new NumberLiteral(2, Nowhere), Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestForwardSlashOperator()
		{
			var expressions = Compile("1/2");
			Assert.AreEqual(new ForwardSlashOperator(new NumberLiteral(1, Nowhere), new NumberLiteral(2, Nowhere), Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestAsteriskOperator()
		{
			var expressions = Compile("1*2");
			Assert.AreEqual(new AsteriskOperator(new NumberLiteral(1, Nowhere), new NumberLiteral(2, Nowhere), Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestArithmeticOrderOfOperations()
		{
			var expressions = Compile("1*2+3/4");
			Assert.AreEqual(
				new PlusOperator(
					new AsteriskOperator(
						new NumberLiteral(1, Nowhere),
						new NumberLiteral(2, Nowhere),
						Nowhere),
					new ForwardSlashOperator(
						new NumberLiteral(3, Nowhere),
						new NumberLiteral(4, Nowhere),
						Nowhere),
					Nowhere),
				expressions[0]);
		}
		
		[Test]
		public void TestDefinition()
		{
			var expressions = Compile("foo:42");
			Assert.AreEqual(new Definition(new Reference("foo", Nowhere), new NumberLiteral(42, Nowhere), Nowhere), expressions[0]);
		}
		
		[Test, ExpectedException(typeof(ParseException))]
		public void TestDoubleDefinitionFailure()
		{
			var expressions = Compile("foo:42:10");
			Assert.AreEqual(new Definition(new Reference("foo", Nowhere), new NumberLiteral(42, Nowhere), Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestReferenceDefinition()
		{
			var expressions = Compile("foo:bar");
			Assert.AreEqual(new Definition(new Reference("foo", Nowhere), new Reference("bar", Nowhere), Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestStringLiteral()
		{
			var expressions = Compile(@"""foo""");
			Assert.AreEqual(new StringLiteral("foo", Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestEscapedStringLiteral()
		{
			var expressions = Compile(@"""\""\n\t\\""");
			Assert.AreEqual(new StringLiteral("\"\n\t\\", Nowhere), expressions[0]);
		}
		
		[Test]
		public void TestUninterpolatedStringLiteral()
		{
			var expressions = Compile(@"""foo {{bar}} bat""");
			Assert.AreEqual(new StringLiteral(@"foo {bar} bat", Nowhere), expressions[0]);
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
			var expressions = Compile(@"""{foo}""");
			Assert.AreEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new Reference("foo", Nowhere)
				}, Nowhere),
				expressions[0]
			);
		}
		
		[Test]
		public void TestInterpolatedStringLiteral()
		{
			var expressions = Compile(@"""foo {bar} bat""");
			Assert.AreEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo ", Nowhere),
					new Reference("bar", Nowhere),
					new StringLiteral(" bat", Nowhere)
				}, Nowhere),
				expressions[0]
			);
		}
		
		[Test]
		public void TestTerminalInterpolatedStringLiteral()
		{
			var expressions = Compile(@"""foo {bar}""");
			Assert.AreEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo ", Nowhere),
					new Reference("bar", Nowhere)
				}, Nowhere),
				expressions[0]
			);
		}
		
		[Test]
		public void TestComplexInterpolatedStringLiteral()
		{
			var expressions = Compile(@"""{{foo}}: {bar + 42 + ""%""}, {bat}""");
			Assert.AreEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral(@"{foo}: ", Nowhere),
					new PlusOperator(
						new PlusOperator(
							new Reference("bar", Nowhere),
							new NumberLiteral(42, Nowhere), Nowhere),
						new StringLiteral("%", Nowhere),
						Nowhere),
					new StringLiteral(", ", Nowhere),
					new Reference("bat", Nowhere)
				}, Nowhere),
				expressions[0]
			);
		}
		
		[Test]
		public void TestNestedInterpolatedStringLiterals()
		{
			var expressions = Compile(@"""{""{foo}""}""");
			Assert.AreEqual(new InterpolatedStringLiteral(new Expression[] {
				new InterpolatedStringLiteral(new Expression[] {
					new Reference("foo", Nowhere)
				}, Nowhere),
			}, Nowhere),
			expressions[0]);
		}
		
		[Test]
		public void TestNestedInterpolatedStringLiteralsWithTrailingString()
		{
			var expressions = Compile(@"""{""{foo}""} bar""");
			Assert.AreEqual(new InterpolatedStringLiteral(new Expression[] {
				new InterpolatedStringLiteral(new Expression[] {
					new Reference("foo", Nowhere)
				}, Nowhere),
				new StringLiteral(" bar", Nowhere)
			}, Nowhere),
			expressions[0]);
		}
		
		[Test]
		public void TestComplexNestedInterpolatedStringLiterals()
		{
			var expressions = Compile(@"""{1+1 + "" is the lonliest number, {name}!""} < she said it""");
			Assert.AreEqual(
				new InterpolatedStringLiteral(
					new Expression[] {
						new PlusOperator(
							new PlusOperator(
								new NumberLiteral(1, Nowhere),
								new NumberLiteral(1, Nowhere), Nowhere),
			                new InterpolatedStringLiteral(
								new Expression[] {
									new StringLiteral(" is the lonliest number, ", Nowhere),
									new Reference("name", Nowhere),
									new StringLiteral("!", Nowhere)
								}, Nowhere),
							Nowhere),
						new StringLiteral(" < she said it", Nowhere),
					}, Nowhere),
				expressions[0]);
				
		}
		
		public static List<Expression> Compile(string source)
		{
			var expressions = new List<Expression>();
			var driver = new Driver((indentation, expression) => expressions.Add(expression));
			foreach(var character in source) {
				var exception = driver.OnCharacter(new Character(character, Nowhere));
				if(exception != null) {
					Console.WriteLine(exception.StackTrace);
					throw exception;
				}
			}
			var error = driver.OnDone();
			if(error != null) {
				throw error;
			}
			return expressions;
		}
	}
}
