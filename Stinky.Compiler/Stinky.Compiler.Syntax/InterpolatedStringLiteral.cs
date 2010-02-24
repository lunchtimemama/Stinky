// 
// InterpolatedStringLiteral.cs
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

namespace Stinky.Compiler.Syntax
{
	public class InterpolatedStringLiteral : Expression
	{
		public readonly IEnumerable<Expression> InterpolatedExpressions;
		
		public InterpolatedStringLiteral(IEnumerable<Expression> interpolatedExpressions, Location location)
			: base(location, typeof(string))
		{
			InterpolatedExpressions = interpolatedExpressions;
		}
		
		public override void Visit(Visitor visitor)
		{
			visitor.VisitInterpolatedStringLiteral(this);
		}
		
		public override bool Equals(object obj)
		{
			var interpolatedStringLiteral = obj as InterpolatedStringLiteral;
			if(interpolatedStringLiteral == null || interpolatedStringLiteral.Location != Location) {
				return false;
			}
			var thisEnumerator = InterpolatedExpressions.GetEnumerator();
			var thatEnumerator = interpolatedStringLiteral.InterpolatedExpressions.GetEnumerator();
			if(thisEnumerator.MoveNext()) {
				do {
					if(!thatEnumerator.MoveNext()) {
						return false;
					} else if(!thisEnumerator.Current.Equals(thatEnumerator.Current)) {
						return false;
					}
				} while(thisEnumerator.MoveNext());
				if(thatEnumerator.MoveNext()) {
					return false;
				}
			} else if(thatEnumerator.MoveNext()) {
				return false;
			}
			return true;
		}
		
		public override int GetHashCode()
		{
			var hashCode = Location.GetHashCode();
			foreach(var expression in InterpolatedExpressions) {
				hashCode ^= expression.GetHashCode();
			}
			return hashCode;
		}
		
		public override string ToString ()
		{
			var stringBuilder = new StringBuilder();
			var first = true;
			foreach(var expression in InterpolatedExpressions) {
				if(!first) {
					stringBuilder.Append(" + ");
				} else {
					first = false;
				}
				stringBuilder.Append(expression);
			}
			return stringBuilder.ToString();
		}
	}
}
