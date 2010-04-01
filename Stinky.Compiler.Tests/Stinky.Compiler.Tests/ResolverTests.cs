// 
// ResolverTests.cs
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
	public class ResolverTests : CompilerTests
	{
		/*[Test]
		public void TestStringDefinition()
		{
			AssertResolvesToType<string>(Definition("foo", String("bar")));
		}

		[Test]
		public void TestStringDefinitionAndReference()
		{
			var scope = new Scope();
			AssertResolvesToType<string>(Definition("foo", String("bar")), scope);
			AssertResolvesToType<string>(Reference("foo"), scope);
		}

		[Test]
		public void TestTransitiveStringDefinitionAndReference()
		{
			var scope = new Scope();
			AssertResolvesToType<string>(Definition("foo", String("hello world")), scope);
			AssertResolvesToType<string>(Definition("bar", Reference("foo")), scope);
			AssertResolvesToType<string>(Reference("bar"), scope);
		}

		[Test]
		public void TestStringConcatinationWithTwoStrings()
		{
			AssertResolvesToType<string>(Plus(String("foo"), String("bar")));
		}

		[Test]
		public void TestStringConcatinationWithStringAndNumber()
		{
			AssertResolvesToType<string>(Plus(String("foo"), Number(42)));
		}

		[Test]
		public void TestStringConcatinationWithNumberAndString()
		{
			AssertResolvesToType<string>(Plus(Number(42), String("foo")));
		}

		[Test]
		public void TestAddition()
		{
			AssertResolvesToType<double>(Plus(Number(1), Number(1)));
		}

		[Test]
		public void TestSubtraction()
		{
			AssertResolvesToType<double>(Minus(Number(1), Number(1)));
		}

		[Test]
		public void TestMultiplication()
		{
			AssertResolvesToType<double>(Asterisk(Number(1), Number(1)));
		}

		[Test]
		public void TestDivision()
		{
			AssertResolvesToType<double>(ForwardSlash(Number(1), Number(1)));
		}

		static void AssertResolvesToType<T>(Syntax expression)
		{
			AssertResolvesToType<T>(expression, new Scope());
		}

		static void AssertResolvesToType<T>(Syntax expression, Scope scope)
		{
			expression.Visit(new Resolver(scope, resolution => {
				Assert.AreEqual(typeof(T), resolution.Type);
				scope.OnExpression(resolution);
			}));
		}*/
	}
}

