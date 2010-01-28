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
using System.Collections.Generic;
using System.Text;

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler
{
	public class Interpreter : Visitor
	{
		readonly Action<object> consumer;
		
		public Interpreter(Action<object> consumer)
		{
			if(consumer == null) throw new ArgumentNullException("consumer");
			
			this.consumer = consumer;
		}
		
		public override void VisitDefinition(Definition definition)
		{
			definition.Expression.Visit(this);
		}
		
		public override void VisitInterpolatedStringLiteral(InterpolatedStringLiteral interpolatedStringLiteral)
		{
			var stringBuilder = new StringBuilder();
			var interpreter = new Interpreter(result => stringBuilder.Append(result));
			foreach(var expression in interpolatedStringLiteral.InterpolatedExpressions) {
				expression.Visit(interpreter);
			}
			consumer(stringBuilder.ToString());
		}
		
		public override void VisitNumberLiteral(NumberLiteral numberLiteral)
		{
			consumer(numberLiteral.Number);
		}
		
		public override void VisitPlusOperator(PlusOperator plusOperator)
		{
			if(plusOperator.Type == null) {
				throw new InvalidOperationException("You cannot evaluate an unresolved expression.");
			} else if(plusOperator.Type == typeof(string)) {
				plusOperator.LeftOperand.Visit(new Interpreter(left => {
					plusOperator.RightOperand.Visit(new Interpreter(right => {
						consumer(left.ToString() + right.ToString());
					}));
				}));
			} else {
				plusOperator.LeftOperand.Visit(new Interpreter(left => {
					plusOperator.RightOperand.Visit(new Interpreter(right => {
						consumer((double)left + (double)right);
					}));
				}));;
			}
		}
		
		public override void VisitReference(Reference reference)
		{
			if(reference.Expression == null) {
				throw new InvalidOperationException("You cannot evaluate an unresolved expression.");
			} else {
				reference.Expression.Visit(this);
			}
		}
		
		public override void VisitStringLiteral(StringLiteral stringLiteral)
		{
			consumer(stringLiteral.String);
		}
	}
}

