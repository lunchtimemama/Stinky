// 
// InterpreterTests.cs
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
	public class InterpreterTests : CompilerTests
	{
		[Test]
		public void TestStringStringConcatination()
		{
			TestPlusOperator("foobar", String("foo"), String("bar"));
		}

		[Test]
		public void TestStringNumberConcatination()
		{
			TestPlusOperator("foo42", String("foo"), Number(42));
		}

		[Test]
		public void TestNumberStringConcatination()
		{
			TestPlusOperator("42foo", Number(42), String("foo"));
		}

		[Test]
		public void TestAddition()
		{
			TestPlusOperator(42.0, Number(40), Number(2));
		}

		[Test]
		public void TestSubtraction()
		{
			TestBinaryOperator(38.0, type => Minus(Number(40), Number(2), type));
		}

		[Test]
		public void TestDivision()
		{
			TestBinaryOperator(3, type => ForwardSlash(Number(12), Number(4), type));
		}

		[Test]
		public void TestMultiplication()
		{
			TestBinaryOperator(25, type => Asterisk(Number(5), Number(5), type));
		}

		static void TestPlusOperator<T>(T expectedResult, Expression left, Expression right)
		{
			TestBinaryOperator(expectedResult, type => Plus(left, right, type));
		}

		static void TestBinaryOperator<T>(T expectedResult, Func<Type, Expression> @operator)
		{
			@operator(typeof(T)).Visit(new Interpreter(result => Assert.AreEqual(expectedResult, result)));
		}
	}
}

