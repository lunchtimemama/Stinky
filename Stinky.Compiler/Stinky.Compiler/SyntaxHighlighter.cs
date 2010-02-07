// 
// ReverseEqualityVisitor.cs
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

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler
{
	public class SyntaxHighlighter : Visitor
	{
		readonly Expression expression;
		readonly Action<Expression> consumer;
		
		public SyntaxHighlighter(Expression expression, Action<Expression> consumer)
		{
			this.expression = expression;
			this.consumer = consumer;
		}
		
		public override void VisitStringLiteral(StringLiteral stringLiteral)
		{
			if(!stringLiteral.Equals(expression)) {
				consumer(stringLiteral);
			}
		}
		
		public override void VisitNumberLiteral(NumberLiteral numberLiteral)
		{
			if(!numberLiteral.Equals(expression)) {
				consumer(numberLiteral);
			}
		}
		
		public override void VisitAsteriskOperator(AsteriskOperator asteriskOperator)
		{
			VisitBinaryOperator(asteriskOperator);
		}
		
		public override void VisitForwardSlashOperator(ForwardSlashOperator forwardSlashOperator)
		{
			VisitBinaryOperator(forwardSlashOperator);
		}
		
		public override void VisitMinusOperator(MinusOperator minusOperator)
		{
			VisitBinaryOperator(minusOperator);
		}
		
		public override void VisitPlusOperator(PlusOperator plusOperator)
		{
			VisitBinaryOperator(plusOperator);
		}
		
		public override void VisitReference(Reference reference)
		{
			if(!reference.Equals(expression)) {
				consumer(reference);
			}
		}
		
		void VisitBinaryOperator<T>(T binaryOperator)
			where T : BinaryOperator
		{
			var expression = this.expression as T;
			if(expression == null) {
				consumer(binaryOperator);
			} else {
				if(!binaryOperator.LeftOperand.Equals(expression.LeftOperand)) {
					consumer(binaryOperator);
				} else {
					binaryOperator.RightOperand.Visit(new SyntaxHighlighter(expression.RightOperand, consumer));
				}
			}
		}
	}
}

