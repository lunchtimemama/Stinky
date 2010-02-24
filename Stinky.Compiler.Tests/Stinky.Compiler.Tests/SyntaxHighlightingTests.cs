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

using Stinky.Compiler.Syntax;
using Stinky.Compiler.Syntax.Highlighting;

using SyntaxType = Stinky.Compiler.Syntax.Highlighting.Syntax;

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
			Test(new StringLiteral("foo", Location(0)), SyntaxType.StringLiteral, 0, 3);
		}

		[Test]
		public void TestStringLiteralInDefinition()
		{
			Test(new Definition(
					new Reference("foo", Location(0)),
					new StringLiteral("bar", Location(5)),
					Location(0)),
				SyntaxType.StringLiteral, 5, 3);
		}

		[Test]
		public void TestNumberLiteral()
		{
			Test(new NumberLiteral(123, Location(0)), SyntaxType.NumberLiteral, 0, 3);
		}
		
		[Test]
		public void TestPlusOperatorStringConcatination()
		{
			Test(new PlusOperator(
					new StringLiteral("foo", Location(0)),
					new StringLiteral("bar", Location(4)),
					Location(3)),
				SyntaxType.StringLiteral, 0, 7);
		}

		[Test]
		public void TestPlusOperatorStringAndNumberConcatination()
		{
			Test(new PlusOperator(
					new NumberLiteral(123, Location(0)),
					new StringLiteral("bar", Location(4)),
					Location(3)),
				SyntaxType.StringLiteral, 0, 7);
		}

		[Test]
		public void TestPlusOperatorAddition()
		{
			Test(new PlusOperator(
					new NumberLiteral(123, Location(0)),
					new NumberLiteral(456, Location(4)),
					Location(3)),
				SyntaxType.NumberLiteral, 0, 7);
		}

		static void Test(Expression expression, SyntaxType syntax, int offset, int length)
		{
			var tested = false;
			expression.Visit(
				new Resolver(new Scope(), e => e.Visit(
					new SyntaxHighlighter((s, o, l) => {
						Assert.AreEqual(syntax, s, "The syntax highlighter used the wrong syntax type.");
						Assert.AreEqual(offset, o, "The syntax highlighter began at an incorrect offset.");
						Assert.AreEqual(length, l, "The syntax highlighter stopped after an incorrect length");
						tested = true;
					})
				))
			);
			Assert.IsTrue(tested, "The syntax highlighter did not attempt to highlight the syntax.");
		}
	}
}

