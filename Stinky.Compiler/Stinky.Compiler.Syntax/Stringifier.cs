// 
// ToStringVisitor.cs
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

namespace Stinky.Compiler.Syntax
{
	using ErrorConsumer = Action<SyntaxError>;
	using Syntax = Action<SyntaxVisitor>;

	public class Stringifier : SyntaxVisitor
	{
		StringBuilder stringBuilder;

		public Stringifier(StringBuilder stringBuilder)
		{
			if(stringBuilder == null) {
				throw new ArgumentNullException("stringBuilder");
			}
			this.stringBuilder = stringBuilder;
		}

		public override void VisitStringLiteral(string @string, ErrorConsumer errorConsumer)
		{
			stringBuilder.AppendFormat(@"""{0}""", @string);
		}

		public override void VisitNumberLiteral(double number, ErrorConsumer errorConsumer)
		{
			stringBuilder.Append(number);
		}

		public override void VisitReference(string identifier, ErrorConsumer errorConsumer)
		{
			stringBuilder.Append(identifier);
		}

		public override void VisitDefinition(Syntax reference, Syntax expression, ErrorConsumer errorConsumer)
		{
			VisitBinaryOperator(reference, ':', expression);
		}

		public override void VisitPlusOperator(Syntax left, Syntax right, ErrorConsumer errorConsumer)
		{
			VisitBinaryOperator(left, '+', right);
		}

		public override void VisitMinusOperator(Syntax left, Syntax right, ErrorConsumer errorConsumer)
		{
			VisitBinaryOperator(left, '-', right);
		}

		public override void VisitForwardSlashOperator(Syntax left, Syntax right, ErrorConsumer errorConsumer)
		{
			VisitBinaryOperator(left, '/', right);
		}

		public override void VisitAsteriskOperator(Syntax left, Syntax right, ErrorConsumer errorConsumer)
		{
			VisitBinaryOperator(left, '*', right);
		}

		void VisitBinaryOperator(Syntax left, char @operator, Syntax right)
		{
			left(this);
			stringBuilder.Append(@operator);
			right(this);
		}
	}
}

