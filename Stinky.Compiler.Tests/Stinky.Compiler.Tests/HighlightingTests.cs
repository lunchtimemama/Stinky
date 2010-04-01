// 
// HighlightingTest.cs
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

//using SyntaxType = Stinky.Compiler.Syntax.Highlighting.Syntax;

namespace Stinky.Compiler.Tests
{
	/*
	public class HighlightingTests
	{
		protected class Highlighting
		{
			public readonly SyntaxType Syntax;
			public readonly int Offset;
			public readonly int Length;

			public Highlighting(SyntaxType syntax, int offset, int length)
			{
				Syntax = syntax;
				Offset = offset;
				Length = length;
			}
		}

		protected static Highlighting String(int offset, int length)
		{
			return new Highlighting(SyntaxType.StringLiteral, offset, length);
		}

		protected static Region Token(int location, int length)
		{
			return new Region(new Location(null, 0, location), length);
		}

		protected static void Test(Syntax expression,
						 Func<Action<SyntaxType, int, int>, SyntaxVisitor> highlighterProvider,
						 SyntaxType syntax,
						 int offset,
						 int length)
		{
			var tested = false;
			expression.Visit(new Resolver(new Scope(), e => e.Visit(highlighterProvider((s, o, l) =>
			{
				AssertSyntaxAreEqual(syntax, s);
				AssertOffsetsAreEqual(offset, o);
				AssertLengthsAreEqual(length, l);
				tested = true;
			}))));
			Assert.IsTrue(tested, "The syntax highlighter did not attempt to highlight the syntax.");
		}

		protected static void AssertSyntaxAreEqual(SyntaxType expectedSyntax, SyntaxType actualSyntax)
		{
			Assert.AreEqual(expectedSyntax, actualSyntax, "The syntax highlighter used the wrong syntax type.");
		}

		protected static void AssertOffsetsAreEqual(int expectedOffset, int actualOffset)
		{
			Assert.AreEqual(expectedOffset, actualOffset, "The syntax highlighter began at an incorrect offset.");
		}

		protected static void AssertLengthsAreEqual(int expectedLength, int actualLength)
		{
			Assert.AreEqual(expectedLength, actualLength, "The syntax highlighter stopped after an incorrect length");
		}
	}
	*/
}

