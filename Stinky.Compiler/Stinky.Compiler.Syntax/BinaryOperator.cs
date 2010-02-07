// 
// BinaryOperator.cs
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

namespace Stinky.Compiler.Syntax
{
	public abstract class BinaryOperator : Expression
	{
		public readonly Expression LeftOperand;
		public readonly Expression RightOperand;
		
		protected BinaryOperator(Expression leftOperand, Expression rightOperand, Location location, Type type)
			: base(location, type)
		{
			LeftOperand = leftOperand;
			RightOperand = rightOperand;
		}
		
		protected bool EqualsOther(BinaryOperator binaryOperator)
		{
			if(Location != binaryOperator.Location) {
				return false;
			}
			
			if(LeftOperand != null) {
				if(!LeftOperand.Equals(binaryOperator.LeftOperand)) {
					return false;
				}
			} else {
				if(binaryOperator.LeftOperand != null) {
					return false;
				}
			}
			
			if(RightOperand != null) {
				if(!RightOperand.Equals(binaryOperator.RightOperand)) {
					return false;
				}
			} else {
				if(binaryOperator.RightOperand != null) {
					return false;
				}
			}
			
			return true;
		}
	}
}
