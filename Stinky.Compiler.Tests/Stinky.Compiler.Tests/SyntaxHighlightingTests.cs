// 
// SyntaxHighlightingTests.cs
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

using NUnit.Framework;

using Stinky.Compiler;
using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Tests
{
	[TestFixture]
	public class SyntaxHighlightingTests
	{
		static Location Location(int location)
		{
			return new Location(null, 0, location);
		}
		
		[Test]
		public void TestStringLiteral()
		{
			var correct = false;
			new StringLiteral("foo", Location(0)).Visit(
				new SyntaxHighlighter(
					new StringLiteral("fo", Location(0)),
					expression => correct = expression.Location == Location(0)));
			Assert.IsTrue(correct);
		}
		
		[Test]
		public void TestPlusOperatorStringConcatination()
		{
			var correct = false;
			new PlusOperator(
				new StringLiteral("foo", Location(0)),
				new StringLiteral("bar", Location(4)),
				Location(3))
			.Visit(
				new SyntaxHighlighter(
					new PlusOperator(
						new StringLiteral("foo", Location(0)),
						new StringLiteral("ba", Location(4)),
						Location(3)),
					expression => correct = expression.Location == Location(4)));
			Assert.IsTrue(correct);
		}
		
		int DriveSyntaxHighlighting(string code, int[] locations)
		{
			var i = 0;
			var correctCount = 0;
			var driver = new SyntaxHighlightingDriver(expression => {
				if(expression.Location == Location(locations[i])) {
					correctCount = correctCount + 1;
				} else {
					Assert.Fail();
				}
			});
			for(; i < code.Length; i++) {
				try {
					driver.OnCharacter(new Character(code[i], Location(i)));
				} catch {
				}
			}
			return correctCount;
		}
		
		[Test]
		public void TestSyntaxHighlightingDriverWithStringConcatination()
		{
			Assert.AreEqual(6, DriveSyntaxHighlighting("foo+bar", new[] { 0, 0, 0, -1, 3, 4, 4 }));
		}
		
		[Test]
		public void TestSyntaxHighlightingDriverWithMultipleArithmeticOperators()
		{
			Assert.AreEqual(4, DriveSyntaxHighlighting("1*2+3*4", new[] { 0, -1, 1, -1, 3, -1, 5 }));
		}
	}
}
