// 
// ResolvedSyntaxVisitor.cs
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

namespace Stinky.Compiler.Expressions
{
	using Expression = Action<ExpressionVisitor>;

	public class ExpressionVisitor
	{
		public virtual void VisitStringConstant(string @string)
		{
			VisitExpression();
		}

		public virtual void VisitNumberConstant(double number)
		{
			VisitExpression();
		}

		public virtual void VisitReference(Type type, string identifier, Expression expression)
		{
			VisitExpression();
		}

		public virtual void VisitDefinition(Type type, Expression reference, Expression expression)
		{
			VisitExpression();
		}

		public virtual void VisitAdditionOperator(Expression left, Expression right)
		{
			VisitBinaryOperator(left, right);
		}

		public virtual void VisitStringConcatinationOperator(Expression left, Expression right)
		{
			VisitBinaryOperator(left, right);
		}

		public virtual void VisitSubtractionOperator(Expression left, Expression right)
		{
			VisitBinaryOperator(left, right);
		}

		public virtual void VisitDivisionOperator(Expression left, Expression right)
		{
			VisitBinaryOperator(left, right);
		}

		public virtual void VisitMultiplicationOperator(Expression left, Expression right)
		{
			VisitBinaryOperator(left, right);
		}

		protected virtual void VisitBinaryOperator(Expression left, Expression right)
		{
			VisitExpression();
		}

		protected virtual void VisitExpression()
		{
		}
	}
}

