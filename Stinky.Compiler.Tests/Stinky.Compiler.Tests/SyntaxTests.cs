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
			new Definition(new Reference("foo", Nowhere), new StringLiteral("bar", Nowhere), Nowhere).Visit(
				new Resolver(new Scope(), definition => Assert.AreEqual(typeof(string), definition.Type)));
		}
		
		[Test]
		public void TestStringDefinitionAndReferenceTypeResolution()
		{
			var scope = new Scope();
			new Definition(new Reference("foo", Nowhere), new StringLiteral("bar", Nowhere), Nowhere).Visit(
				new Resolver(scope, definition => {
					scope.OnExpression(definition);
					new Reference("foo", Nowhere).Visit(new Resolver(scope, reference => {
						Assert.AreEqual(typeof(string), definition.Type);
						Assert.AreEqual(typeof(string), reference.Type);
					}));
				}));
		}
		
		[Test]
		public void TestTransitiveStringDefinitionAndReferenceTypeResolution()
		{
			var scope = new Scope();
			new Definition(new Reference("foo", Nowhere), new StringLiteral("", Nowhere), Nowhere).Visit(
				new Resolver(scope, fooDefinition => {
					scope.OnExpression(fooDefinition);
					new Definition(new Reference("bar", Nowhere), new Reference("foo", Nowhere), Nowhere).Visit(
						new Resolver(scope, barDefinition => {
							scope.OnExpression(barDefinition);
							new Reference("bar", Nowhere).Visit(new Resolver(scope, reference => {
								Assert.AreEqual(typeof(string), fooDefinition.Type);
								Assert.AreEqual(typeof(string), barDefinition.Type);
								Assert.AreEqual(typeof(string), reference.Type);
							}));
						}));
				}));
		}
		
		[Test]
		public void TestInterpolatedStringLiteralEqualityLogic()
		{
			Assert.AreEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere),
					new Reference("bar", Nowhere),
					new StringLiteral("bat", Nowhere)
				}, Nowhere),
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere),
					new Reference("bar", Nowhere),
					new StringLiteral("bat", Nowhere)
				}, Nowhere)
			);
			Assert.AreNotEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere),
					new Reference("bar", Nowhere)
				}, Nowhere),
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere),
					new Reference("bar", Nowhere),
					new StringLiteral("bat", Nowhere)
				}, Nowhere)
			);
			Assert.AreNotEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere),
					new Reference("bar", Nowhere),
					new StringLiteral("bat", Nowhere)
				}, Nowhere),
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere),
					new Reference("bar", Nowhere)
				}, Nowhere)
			);
			Assert.AreNotEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("bar", Nowhere),
					new Reference("foo", Nowhere),
					new StringLiteral("bat", Nowhere)
				}, Nowhere),
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere),
					new Reference("bar", Nowhere),
					new StringLiteral("bat", Nowhere)
				}, Nowhere)
			);
			Assert.AreNotEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("bar", Nowhere)
				}, Nowhere),
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere)
				}, Nowhere)
			);
			Assert.AreNotEqual(
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere)
				}, Nowhere),
				new InterpolatedStringLiteral(new Expression[0], Nowhere)
			);
			Assert.AreNotEqual(
				new InterpolatedStringLiteral(new Expression[0], Nowhere),
				new InterpolatedStringLiteral(new Expression[] {
					new StringLiteral("foo", Nowhere)
				}, Nowhere)
			);
		}
		
		[Test]
		public void TestPlusOperatorStringConcatinationWithTwoStrings()
		{
			new PlusOperator(new StringLiteral("foo", Nowhere), new StringLiteral("bar", Nowhere), Nowhere)
				.Visit(new Resolver(new Scope(), plusOperator => Assert.AreEqual(typeof(string), plusOperator.Type)));
		}
		
		[Test]
		public void TestPlusOperatorStringConcatinationWithOneString()
		{
			new PlusOperator(new StringLiteral("foo", Nowhere), new NumberLiteral(1, Nowhere), Nowhere)
				.Visit(new Resolver(new Scope(), plusOperator => Assert.AreEqual(typeof(string), plusOperator.Type)));
		}
		
		[Test]
		public void TestPlusOperatorAddition()
		{
			new PlusOperator(new NumberLiteral(1, Nowhere), new NumberLiteral(1, Nowhere), Nowhere)
				.Visit(new Resolver(new Scope(), plusOperator => Assert.AreEqual(typeof(double), plusOperator.Type)));
		}
		
		[Test]
		public void TestPlusOperatorEquality()
		{
			Assert.AreEqual(
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere),
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere));
			
			Assert.AreEqual(
				new PlusOperator(
					null,
					new NumberLiteral(0, Nowhere),
					Nowhere),
				new PlusOperator(
					null,
					new NumberLiteral(0, Nowhere),
					Nowhere));
			
			Assert.AreEqual(
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					null,
					Nowhere),
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					null,
					Nowhere));
			
			Assert.AreEqual(
				new PlusOperator(
					null,
					null,
					Nowhere),
				new PlusOperator(
					null,
					null,
					Nowhere));
			
			Assert.AreNotEqual(
				new PlusOperator(
					new NumberLiteral(1, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere),
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere));
			
			Assert.AreNotEqual(
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere),
				new PlusOperator(
					new NumberLiteral(1, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere));
			
			Assert.AreNotEqual(
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(1, Nowhere),
					Nowhere),
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere));
			
			Assert.AreNotEqual(
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere),
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(1, Nowhere),
					Nowhere));
			
			Assert.AreNotEqual(
				new PlusOperator(
					null,
					new NumberLiteral(0, Nowhere),
					Nowhere),
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere));
			
			Assert.AreNotEqual(
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere),
				new PlusOperator(
					null,
					new NumberLiteral(0, Nowhere),
					Nowhere));
			
			Assert.AreNotEqual(
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					null,
					Nowhere),
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere));
			
			Assert.AreNotEqual(
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					new NumberLiteral(0, Nowhere),
					Nowhere),
				new PlusOperator(
					new NumberLiteral(0, Nowhere),
					null,
					Nowhere));
		}
	}
}
