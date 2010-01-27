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
		public void TestStringDefinitionTypeResolution()
		{
			var definition = new Definition(new Reference("foo", Nowhere), new StringLiteral("bar", Nowhere), Nowhere)
				.Resolve(new Scope());
			Assert.AreEqual(typeof(string), definition.Type);
		}
		
		[Test]
		public void TestStringDefinitionAndReferenceTypeResolution()
		{
			var scope = new Scope();
			var definition = new Definition(new Reference("foo", Nowhere), new StringLiteral("bar", Nowhere), Nowhere)
				.Resolve(scope);
			scope.OnExpression(definition);
			var reference = new Reference("foo", Nowhere)
				.Resolve(scope);
			scope.OnExpression(reference);
			Assert.AreEqual(typeof(string), definition.Type);
			Assert.AreEqual(typeof(string), reference.Type);
		}
		
		[Test]
		public void TestTransitiveStringDefinitionAndReferenceTypeResolution()
		{
			var scope = new Scope();
			var fooDefinition = new Definition(new Reference("foo", Nowhere), new StringLiteral("", Nowhere), Nowhere)
				.Resolve(scope);
			scope.OnExpression(fooDefinition);
			var barDefinition = new Definition(new Reference("bar", Nowhere), new Reference("foo", Nowhere), Nowhere)
				.Resolve(scope);
			scope.OnExpression(barDefinition);
			var reference = new Reference("bar", Nowhere)
				.Resolve(scope);
			scope.OnExpression(reference);
			Assert.AreEqual(typeof(string), fooDefinition.Type);
			Assert.AreEqual(typeof(string), barDefinition.Type);
			Assert.AreEqual(typeof(string), reference.Type);
		}
	}
}
