// 
// Interpreter.cs
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
using System.Text;

namespace Stinky.Compiler.Expressions
{
	using Expression = Action<ExpressionVisitor>;

	public class Interpreter : ExpressionVisitor
	{
		readonly Action<object> consumer;
		
		public Interpreter(Action<object> consumer)
		{
			if(consumer == null) {
				throw new ArgumentNullException("consumer");
			}
			this.consumer = consumer;
		}
		
		public override void VisitStringConstant(string @string)
		{
			consumer(@string);
		}

		public override void VisitNumberConstant(double number)
		{
			consumer(number);
		}

		public override void VisitReference(Type type, string identifier, Expression expression)
		{
			expression(this);
		}

		public override void VisitStringConcatinationOperator(Expression left, Expression right)
		{
			left(new Interpreter(leftValue => {
				right(new Interpreter(rightValue => {
					consumer(leftValue.ToString() + rightValue.ToString());
				}));
			}));
		}

		public override void VisitAdditionOperator(Expression left, Expression right)
		{
			VisitArithmeticOperator(left, right, (l, r) => l + r);
		}

		public override void VisitSubtractionOperator(Expression left, Expression right)
		{
			VisitArithmeticOperator(left, right, (l, r) => l - r);
		}

		public override void VisitMultiplicationOperator(Expression left, Expression right)
		{
			VisitArithmeticOperator(left, right, (l, r) => l * r);
		}

		public override void VisitDivisionOperator(Expression left, Expression right)
		{
			VisitArithmeticOperator(left, right, (l, r) => l / r);
		}

		void VisitArithmeticOperator(Expression left, Expression right, Func<double, double, object> @operator)
		{
			left(new Interpreter(leftValue => {
				if(!(leftValue is double)) {
					throw new ArgumentException("The addition operands must be a double.", "left");
				}
				right(new Interpreter(rightValue => {
					if(!(rightValue is double)) {
						throw new ArgumentException("The addition operands must be a double.", "right");
					}
					consumer(@operator((double)leftValue, (double)rightValue));
				}));
			}));
		}
	}
}

