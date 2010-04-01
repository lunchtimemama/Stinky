// 
// Main.cs
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

using Stinky.Compiler;
using Stinky.Compiler.Parser;
using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;
using Stinky.Compiler.Syntax.Highlighting;

using SyntaxType = Stinky.Compiler.Syntax.Highlighting.Syntax;

namespace Smell
{
	class MainClass
	{
		public static void Main(string[] args)
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
					//Assert.Fail("The syntax highlighter is performing more highlighting than expected.");
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
			//Assert.AreEqual(highlightings.Length, highlightingCount, "The syntax highlighter performed less highlighting than expected");
		}

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

		protected static Location Location(int location)
		{
			return new Location(null, 0, location);
		}

		protected static void AssertSyntaxAreEqual(SyntaxType expectedSyntax, SyntaxType actualSyntax)
		{
			//Assert.AreEqual(expectedSyntax, actualSyntax, "The syntax highlighter used the wrong syntax type.");
		}

		protected static void AssertOffsetsAreEqual(int expectedOffset, int actualOffset)
		{
			//Assert.AreEqual(expectedOffset, actualOffset, "The syntax highlighter began at an incorrect offset.");
		}

		protected static void AssertLengthsAreEqual(int expectedLength, int actualLength)
		{
			//Assert.AreEqual(expectedLength, actualLength, "The syntax highlighter stopped after an incorrect length");
		}
	}
}

