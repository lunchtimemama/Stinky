// 
// Token.cs
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

namespace Stinky.Compiler.Parser
{
	public struct Region : IEquatable<Region>
	{
		public readonly Location Location;
		public readonly int Length;

		public Region(Location location, int length)
		{
			Location = location;
			Length = length;
		}

		public static bool operator ==(Region token1, Region token2)
		{
			return token1.Location == token2.Location && token1.Length == token2.Length;
		}

		public static bool operator !=(Region token1, Region token2)
		{
			return !(token1 == token2);
		}

		public bool Equals(Region token)
		{
			return this == token;
		}

		public override bool Equals(object obj)
		{
			return obj is Region && Equals((Region)obj);
		}

		public override int GetHashCode()
		{
			var hash = 17;
			hash = 31 * Location.GetHashCode();
			hash = 31 * Length.GetHashCode();
			return hash;
		}

		public override string ToString()
		{
			return string.Format("@({0}) x({1})", Location, Length);
		}
	}
}
