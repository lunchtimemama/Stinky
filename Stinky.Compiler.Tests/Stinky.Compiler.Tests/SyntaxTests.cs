// 
// SyntaxTests.cs
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

namespace Stinky.Compiler.Tests
{
	[TestFixture]
	public class SyntaxTests : CompilerTests
	{
		[Test]
		public void TestInterpolatedStringLiteralEquality()
		{
			Assert.AreEqual(
				InterpolatedString(String("foo"), Reference("bar"), String("bat")),
				InterpolatedString(String("foo"), Reference("bar"), String("bat")));

			Assert.AreNotEqual(
				InterpolatedString(String("foo"), Reference("bar")),
				InterpolatedString(String("foo"), Reference("bar"), String("bat")));

			Assert.AreNotEqual(
				InterpolatedString(String("foo"), Reference("bar"), String("bat")),
				InterpolatedString(String("foo"), Reference("bar")));

			Assert.AreNotEqual(
				InterpolatedString(String("bar"), Reference("foo"), String("bat")),
				InterpolatedString(String("foo"), Reference("bar"), String("bat")));

			Assert.AreNotEqual(
				InterpolatedString(String("bar")),
				InterpolatedString(String("foo")));

			Assert.AreNotEqual(
				InterpolatedString(String("foo")),
				InterpolatedString());
			
			Assert.AreNotEqual(
				InterpolatedString(),
				InterpolatedString(String("foo")));
		}
		
		[Test]
		public void TestPlusOperatorEquality()
		{
			Assert.AreEqual(
				Plus(Number(0), Number(0)),
				Plus(Number(0), Number(0)));

			Assert.AreEqual(
				Plus(null, Number(0)),
				Plus(null, Number(0)));

			Assert.AreEqual(
				Plus(Number(0), null),
				Plus(Number(0), null));

			Assert.AreEqual(
				Plus(null, null),
				Plus(null, null));

			Assert.AreNotEqual(
				Plus(Number(1), Number(0)),
				Plus(Number(0), Number(0)));

			Assert.AreNotEqual(
				Plus(Number(0), Number(0)),
				Plus(Number(1), Number(0)));

			Assert.AreNotEqual(
				Plus(Number(0), Number(1)),
				Plus(Number(0), Number(0)));

			Assert.AreNotEqual(
				Plus(Number(0), Number(0)),
				Plus(Number(0), Number(1)));

			Assert.AreNotEqual(
				Plus(null, Number(0)),
				Plus(Number(0), Number(0)));

			Assert.AreNotEqual(
				Plus(Number(0), Number(0)),
				Plus(null, Number(0)));

			Assert.AreNotEqual(
				Plus(Number(0), null),
				Plus(Number(0), Number(0)));

			Assert.AreNotEqual(
				Plus(Number(0), Number(0)),
				Plus(Number(0), null));
		}
	}
}
