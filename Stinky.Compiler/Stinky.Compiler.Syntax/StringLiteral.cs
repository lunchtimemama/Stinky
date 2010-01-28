// 
// StringLiteral.cs
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

namespace Stinky.Compiler.Syntax
{
	public class StringLiteral : Expression
	{
		public readonly string String;
		
		public StringLiteral(string @string, Location location)
			: base(location, typeof(string))
		{
			String = @string;
		}
		
		public override void Visit(Visitor visitor)
		{
			visitor.VisitStringLiteral(this);
		}
		
		public override bool Equals(object obj)
		{
			StringLiteral stringLiteral = obj as StringLiteral;
			return stringLiteral != null
				&& stringLiteral.String == String
				&& stringLiteral.Location == Location;
		}

		public override int GetHashCode ()
		{
			return String.GetHashCode() ^ Location.GetHashCode();
		}

		public override string ToString ()
		{
			return string.Format(@"""{0}""", String);
		}
	}
}
