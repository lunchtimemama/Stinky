// 
// SyntaxHighlighter.cs
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

namespace Stinky.Compiler.Syntax.Highlighting
{
	public class SyntaxHighlighter : Visitor
	{
		readonly Action<Syntax, int, int> consumer;

		public SyntaxHighlighter(Action<Syntax, int, int> consumer)
		{
			this.consumer = consumer;
		}

		public override void VisitDefinition<T>(T definition)
		{
			definition.Expression.Visit(this);
		}

		public override void VisitStringLiteral<T>(T stringLiteral)
		{
			consumer(Syntax.StringLiteral, stringLiteral.Location.Column, stringLiteral.String.Length);
		}

		public override void VisitNumberLiteral<T>(T numberLiteral)
		{
			consumer(Syntax.NumberLiteral, numberLiteral.Location.Column, numberLiteral.Number.ToString().Length);
		}

		public override void VisitPlusOperator<T>(T plusOperator)
		{
			if(plusOperator.Type == typeof(string)) {
				VisitBinaryOperator(plusOperator, Syntax.StringLiteral);
			} else {
				VisitBinaryOperator(plusOperator);
			}
		}

		protected override void VisitBinaryOperator<T>(T binaryOperator)
		{
			if(binaryOperator.Type != typeof(double)) {
				throw new InvalidOperationException("The expression must be of type double.");
			}
			VisitBinaryOperator(binaryOperator, Syntax.NumberLiteral);
		}

		void VisitBinaryOperator(BinaryOperator binaryOperator, Syntax syntax)
		{
			binaryOperator.LeftOperand.Visit(new SyntaxHighlighter((leftSyntax, leftOffset, leftLength) => {
				binaryOperator.RightOperand.Visit(new SyntaxHighlighter((rightSyntax, rightOffset, rightLength) => {
					consumer(syntax, leftOffset, rightOffset - leftOffset + rightLength);
				}));
			}));
		}
	}
}

