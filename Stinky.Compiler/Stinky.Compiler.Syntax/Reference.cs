// 
// Reference.cs
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
	public class Reference : Expression
	{
		public readonly string Identifier;
		public readonly Expression Expression;
		
		public Reference(string identifier, Location location)
			: this(identifier, null, location, null)
		{
		}
		
		public Reference(string identifier, Expression expression, Location location, Type type)
			: base(location, type)
		{
			Identifier = identifier;
			Expression = expression;
		}
		
		public override void Visit(Visitor visitor)
		{
			visitor.VisitReference(this);
		}
		
		public override bool Equals(object obj)
		{
			var reference = obj as Reference;
			return reference != null
				&& reference.Identifier == Identifier
				&& reference.Location == Location;
		}
		
		public override int GetHashCode ()
		{
			return Identifier.GetHashCode () ^ Location.GetHashCode();
		}
		
		public override string ToString ()
		{
			return Identifier;
		}
	}
}
