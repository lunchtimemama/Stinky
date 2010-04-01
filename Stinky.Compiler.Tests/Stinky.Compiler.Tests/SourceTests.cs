// 
// SourceTests.cs
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

namespace Stinky.Compiler.Tests
{
	using Source = Action<SourceVisitor>;

	[TestFixture]
	public class SourceTests : Sources
	{
		[Test]
		public void TestEquality()
		{
			AssertAreEqual(null, null);

			AssertAreNotEqual(null, String("foo"));

			AssertAreNotEqual(String("foo"), null);

			AssertAreEqual(
				String("foo"),
			    String("foo")
			);

			AssertAreNotEqual(
				String("foo"),
			    String("bar")
			);

			AssertAreNotEqual(
				String("foo", 0),
			    String("foo", 1)
			);

			AssertAreNotEqual(
				String("foo", 0, 0),
			    String("foo", 0, 1)
			);

			AssertAreEqual(
				Plus(Number(1), Number(2)),
				Plus(Number(1), Number(2))
			);

			AssertAreNotEqual(
				Plus(Number(1), Number(1)),
				Plus(Number(1), Number(2))
			);

			AssertAreNotEqual(
				Plus(Number(1), Number(2)),
				Plus(Number(1), Number(1))
			);

			AssertAreNotEqual(
				Plus(Number(2), Number(2)),
				Plus(Number(1), Number(2))
			);

			AssertAreNotEqual(
				Plus(Number(1), Number(2)),
				Plus(Number(2), Number(2))
			);

			AssertAreNotEqual(
				Plus(Number(1), Number(1, 1)),
				Plus(Number(1), Number(1))
			);

			AssertAreNotEqual(
				Plus(Number(1), Number(2)),
				Minus(Number(1), Number(2))
			);

			AssertAreNotEqual(
				Plus(Number(1), Number(2)),
				Asterisk(Number(1), Number(2))
			);

			AssertAreNotEqual(
				Plus(Number(1), Number(2)),
				ForwardSlash(Number(1), Number(2))
			);

			AssertAreNotEqual(
				Minus(Number(1), Number(2)),
				Asterisk(Number(1), Number(2))
			);

			AssertAreNotEqual(
				Minus(Number(1), Number(2)),
				ForwardSlash(Number(1), Number(2))
			);

			AssertAreNotEqual(
				Asterisk(Number(1), Number(2)),
				ForwardSlash(Number(1), Number(2))
			);

			AssertAreEqual(
				Plus(Plus(Number(1), Number(2)), Number(3)),
				Plus(Plus(Number(1), Number(2)), Number(3))
			);

			AssertAreNotEqual(
				Plus(Number(1), Plus(Number(2), Number(3))),
				Plus(Plus(Number(1), Number(2)), Number(3))
			);

			AssertAreEqual(
				Definition(
					Reference("foo"),
					InterpolatedString(0, 0,
						String("answer: "),
						Plus(Number(40), Number(2)),
						String("!")
					),
				0),
				Definition(
					Reference("foo"),
					InterpolatedString(0, 0,
						String("answer: "),
						Plus(Number(40), Number(2)),
						String("!")
					),
				0)
			);

			AssertAreNotEqual(
				Definition(
					Reference("foo"),
					InterpolatedString(0, 0,
						String("answer: "),
						Plus(Number(40), Number(2)),
						String("!")
					),
				0),
				Definition(
					Reference("foo"),
					InterpolatedString(0, 0,
						String("answer: "),
						Plus(Number(40), Number(2)),
						String("?")
					),
				0)
			);

			AssertAreNotEqual(
				Definition(
					Reference("foo"),
					InterpolatedString(0, 0,
						String("answer: "),
						Plus(Number(40), Number(2)),
						String("!")
					),
				0),
				Definition(
					Reference("foo"),
					InterpolatedString(0, 0,
						String("answer: "),
						Plus(Number(40), Number(2))
					),
				0)
			);

			AssertAreNotEqual(
				Definition(
					Reference("foo"),
					InterpolatedString(0, 0,
						String("answer: "),
						Plus(Number(40), Number(2)),
						String("!")
					),
				0),
				Definition(
					Reference("foo"),
					InterpolatedString(0, 0,
						String("answer: "),
						Plus(Number(40), Number(2))
					),
				0)
			);
		}
	}
}

