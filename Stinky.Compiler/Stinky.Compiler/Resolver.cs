// 
// Resolver.cs
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

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler
{
	public class Resolver : Visitor
	{
		readonly Scope scope;
		readonly Action<Expression> consumer;
		
		public Resolver(Scope scope, Action<Expression> consumer)
		{
			if(scope == null) throw new ArgumentNullException("scope");
			if(consumer == null) throw new ArgumentNullException("consumer");
			
			this.scope = scope;
			this.consumer = consumer;
		}
		
		public override void VisitDefinition(Definition definition)
		{
			definition.Expression.Visit(new Resolver(scope,
				expression => consumer(new Definition(definition.Reference, expression, definition.Location, expression.Type))));
		}
		
		public override void VisitReference(Reference reference)
		{
			var definition = scope.GetDefinition(reference);
			consumer(new Reference(reference.Identifier, definition.Expression, reference.Location, definition.Type));
		}
		
		public override void VisitStringLiteral(StringLiteral stringLiteral)
		{
			consumer(stringLiteral);
		}
		
		public override void VisitInterpolatedStringLiteral(InterpolatedStringLiteral interpolatedStringLiteral)
		{
			var interpolatedExpressions = new List<Expression>();
			foreach(var interpolatedExpression in interpolatedStringLiteral.InterpolatedExpressions) {
				interpolatedExpression.Visit(new Resolver(scope, expression => {
					interpolatedExpressions.Add(expression);
				}));
			}
			consumer(new InterpolatedStringLiteral(interpolatedExpressions, interpolatedStringLiteral.Location));
		}
		
		public override void VisitNumberLiteral(NumberLiteral numberLiteral)
		{
			consumer(numberLiteral);
		}
		
		public override void VisitPlusOperator(PlusOperator plusOperator)
		{
			plusOperator.LeftOperand.Visit(new Resolver(scope, leftOperand => {
				plusOperator.RightOperand.Visit(new Resolver(scope, rightOperand => {
					if(leftOperand.Type == typeof(string) || rightOperand.Type == typeof(string)) {
						consumer(new PlusOperator(leftOperand, rightOperand, plusOperator.Location, typeof(string)));
					} else {
						CheckType<double>(leftOperand);
						CheckType<double>(rightOperand);
						consumer(new PlusOperator(leftOperand, rightOperand, plusOperator.Location, typeof(double)));
					}
				}));
			}));
		}
		
		public override void VisitMinusOperator(MinusOperator minusOperator)
		{
			VisitBinaryNumberOperator(minusOperator, (left, right) => new MinusOperator(left, right, minusOperator.Location, typeof(double)));
		}
		
		public override void VisitAsteriskOperator(AsteriskOperator asteriskOperator)
		{
			VisitBinaryNumberOperator(asteriskOperator, (left, right) => new AsteriskOperator(left, right, asteriskOperator.Location, typeof(double)));
		}
		
		public override void VisitForwardSlashOperator(ForwardSlashOperator forwardSlashOperator)
		{
			VisitBinaryNumberOperator(forwardSlashOperator, (left, right) => new ForwardSlashOperator(left, right, forwardSlashOperator.Location, typeof(double)));
		}
		
		void VisitBinaryNumberOperator(BinaryOperator binaryOperator, Func<Expression, Expression, Expression> binaryOperatorProvider)
		{
			binaryOperator.LeftOperand.Visit(new Resolver(scope, leftOperand => {
				CheckType<double>(leftOperand);
				binaryOperator.RightOperand.Visit(new Resolver(scope, rightOperand => {
					CheckType<double>(rightOperand);
					consumer(binaryOperatorProvider(leftOperand, rightOperand));
				}));
			}));
		}
		
		void CheckType<T>(Expression expression)
		{
			if(expression.Type != typeof(T)) {
				throw new InvalidOperationException("The expression must be of type " + typeof(T));
			}
		}
	}
}

