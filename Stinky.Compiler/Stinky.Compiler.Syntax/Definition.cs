// 
// Definition.cs
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
	public class Definition : Expression
	{
		public readonly Reference Reference;
		public readonly Expression Expression;
		
		public Definition(Reference reference, Expression expression, Location location)
			: this(reference, expression, location, null)
		{
		}
		
		public Definition(Reference reference, Expression expression, Location location, Type type)
			: base(location, type)
		{
			Reference = reference;
			Expression = expression;
		}
		
		public override void Visit(Visitor visitor)
		{
			visitor.VisitDefinition(this);
		}
		
		public override bool Equals(object obj)
		{
			var definition = obj as Definition;
			return definition != null
				&& definition.Reference.Equals(Reference)
				&& definition.Expression.Equals(Expression)
				&& definition.Location == Location;
		}

		public override int GetHashCode ()
		{
			return Reference.GetHashCode() ^ Expression.GetHashCode() ^ Location.GetHashCode();
		}
	}
}
