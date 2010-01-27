// 
// Character.cs
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

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser.Tokenizer
{
	public struct Character : IEquatable<Character>
	{
		public readonly char Char;
		public readonly Location Location;

		public Character(char @char, Location location)
		{
			Char = @char;
			Location = location;
		}
		
		public static bool operator ==(Character character1, Character character2)
		{
			return character1.Location == character2.Location;
		}
		
		public static bool operator !=(Character character1, Character character2)
		{
			return !(character1 == character2);
		}
		
		public bool Equals(Character character)
		{
			return this == character;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Character && Equals((Character)obj);
		}

		public override int GetHashCode()
		{
			return Location.GetHashCode();
		}
		
		public override string ToString()
		{
			return Char.ToString();
		}
	}
}