// 
// IncrementalSyntaxHighlighterTester.cs
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

using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;
//using Stinky.Compiler.Syntax.Highlighting;

namespace Stinky.Compiler.Tests
{
		/*
	[TestFixture]
	public class IncrementalSyntaxHighlightingTests : HighlightingTests
	{
		[Test]
		public void TestStringLiteral()
		{
			Test(
				new StringLiteral("fo", Token(0, 2)),
				new StringLiteral("foo", Token(0, 3)),
				SyntaxType.StringLiteral, 2, 1);
		}

		[Test]
		public void TestDriveStringLiteral()
		{
			DriveTest(
				@"""foo""",
				String(1, 1),
				String(2, 1),
				String(3, 1));
		}

		static void DriveTest(string source, params Highlighting[] highlightings)
		{
			var highlightingCount = 0;
			var driver = new IncrementalSyntaxHighlightingDriver((syntax, offset, length) =>
			{
				if(highlightingCount == highlightings.Length) {
					Assert.Fail("The syntax highlighter is performing more highlighting than expected.");
				}
				var highlighting = highlightings[highlightingCount];
				AssertSyntaxAreEqual(highlighting.Syntax, syntax);
				AssertOffsetsAreEqual(highlighting.Offset, offset);
				AssertLengthsAreEqual(highlighting.Length, length);
				highlightingCount++;
			});
			foreach(var character in source) {
				driver.OnCharacter(character);
			}
			Assert.AreEqual(highlightings.Length, highlightingCount,
				"The syntax highlighter performed less highlighting than expected");
		}

		static void Test(Syntax oldExpression, Syntax newExpression, SyntaxType syntax, int offset, int length)
		{
			Test(newExpression, test => new IncrementalSyntaxHighlighter(oldExpression, test), syntax, offset, length);
		}
	}
		*/
}

