// 
// Location.cs
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

namespace Stinky.Compiler
{
	public struct Location : IEquatable<Location>
	{
		public readonly string Source;
		public readonly int Line;
		public readonly int Column;

		public Location(string source, int line, int column)
		{
			Source = source;
			Line = line;
			Column = column;
		}
		
		public static bool operator ==(Location location1, Location location2)
		{
			return location1.Source == location2.Source
				&& location1.Line == location2.Line
				&& location1.Column == location2.Column;
		}
		
		public static bool operator !=(Location location1, Location location2)
		{
			return !(location1 == location2);
		}
		
		public bool Equals(Location location)
		{
			return this == location;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Location && Equals((Location)obj);
		}

		public override int GetHashCode()
		{
			return Source.GetHashCode() ^ Line ^ Column;
		}
		
		public override string ToString()
		{
			return string.Format("{0} line:{1} column:{2}", Source, Line, Column);
		}
	}
}