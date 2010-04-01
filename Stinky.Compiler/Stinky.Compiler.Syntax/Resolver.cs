// 
// ExpressionResolver.cs
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

using Stinky.Compiler.Expressions;

namespace Stinky.Compiler.Syntax
{
	using ErrorConsumer = Action<SyntaxError>;
	using Syntax = Action<SyntaxVisitor>;
	using Expression = Action<Stinky.Compiler.Expressions.ExpressionVisitor>;
	using BinaryOperator = Func<Expression, Expression, Expression>;

	public class Resolver : SyntaxVisitor
	{
		readonly Action<Type, Expression> consumer;
		Scope scope;

		public Resolver(Scope scope, Action<Type, Expression> consumer)
		{
			if(consumer == null) {
				throw new ArgumentNullException("consumer");
			}
			if(scope == null) {
				throw new ArgumentNullException("scope");
			}
			this.consumer = consumer;
			this.scope = scope;
		}

		public override void VisitStringLiteral(string @string, ErrorConsumer errorConsumer)
		{
			consumer(typeof(string), visitor => visitor.VisitStringConstant(@string));
		}

		public override void VisitNumberLiteral(double number, ErrorConsumer errorConsumer)
		{
			consumer(typeof(double), visitor => visitor.VisitNumberConstant(number));
		}

		public override void VisitInterpolatedStringLiteral(IEnumerable<Syntax> interpolatedExpressions,
		                                                    ErrorConsumer errorConsumer)
		{

		}

		public override void VisitReference(string identifier, ErrorConsumer errorConsumer)
		{
			Syntax syntax;
			var success = scope.TryGetValue(identifier, out syntax);
			if(!success) {
				errorConsumer(new SyntaxError());
			} else {
				syntax(new Resolver(scope, (type, expression) => {
					consumer(type, visitor => visitor.VisitReference(type, identifier, expression));
				}));
			}
		}

		public override void VisitDefinition(Syntax reference, Syntax expression, ErrorConsumer errorConsumer)
		{
			reference(new DefinitionResolver(identifier => scope[identifier] = expression));
		}

		public override void VisitPlusOperator(Syntax left, Syntax right, ErrorConsumer errorConsumer)
		{
			left(new Resolver(scope, (leftType, leftExpression) => {
				if(leftType == typeof(string)) {
					right(new Resolver(scope, (rightType, rightExpression) => {
						consumer(leftType,
							visitor => visitor.VisitStringConcatinationOperator(leftExpression, rightExpression));
					}));
				} else if(leftType == typeof(double)) {
					VisitBinaryOperator<double>(
						leftExpression, right, (l, r) => visitor => visitor.VisitAdditionOperator(l, r), errorConsumer);
				} else {
					errorConsumer(new SyntaxError());
				}
			}));
		}

		public override void VisitMinusOperator(Syntax left, Syntax right, ErrorConsumer errorConsumer)
		{
			VisitBinaryOperator<double>(left, right,
				(l, r) => visitor => visitor.VisitSubtractionOperator(l, r), errorConsumer);
		}

		public override void VisitAsteriskOperator(Syntax left, Syntax right, ErrorConsumer errorConsumer)
		{
			VisitBinaryOperator<double>(left, right,
				(l, r) => visitor => visitor.VisitMultiplicationOperator(l, r), errorConsumer);
		}

		public override void VisitForwardSlashOperator(Syntax left, Syntax right, ErrorConsumer errorConsumer)
		{
			VisitBinaryOperator<double>(left, right,
				(l, r) => visitor => visitor.VisitDivisionOperator(l, r), errorConsumer);
		}

		void VisitBinaryOperator<T>(Syntax left,
		                            Syntax right,
		                            BinaryOperator binaryOperator,
		                            ErrorConsumer errorConsumer)
		{
			left(new Resolver(scope, (leftType, leftExpression) => {
				if(leftType != typeof(T)) {
					errorConsumer(new SyntaxError());
				} else {
					VisitBinaryOperator<T>(leftExpression, right, binaryOperator, errorConsumer);
				}
			}));
		}

		void VisitBinaryOperator<T>(Expression leftExpression,
		                            Syntax right,
		                            BinaryOperator binaryOperator,
		                            ErrorConsumer errorConsumer)
		{
			right(new Resolver(scope, (rightType, rightExpression) => {
				if(rightType != typeof(T)) {
					errorConsumer(new SyntaxError());
				} else {
					consumer(rightType, binaryOperator(leftExpression, rightExpression));
				}
			}));
		}

		class DefinitionResolver : SyntaxVisitor
		{
			readonly Action<string> consumer;

			public DefinitionResolver(Action<string> consumer)
			{
				this.consumer = consumer;
			}

			public override void VisitReference(string identifier, ErrorConsumer errorConsumer)
			{
				consumer(identifier);
			}
		}
	}
}

