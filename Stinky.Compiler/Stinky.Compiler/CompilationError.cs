// 
// CompilationError.cs
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

using Stinky.Compiler.Parser;

namespace Stinky.Compiler
{
	// Why don't enums implement IEquatable? THAT IS SILLY! We could be saving a box...
	public struct CompilationError<T> : IEquatable<CompilationError<T>>
	{
		public readonly Location Location;
		public readonly T Error;
		
		public CompilationError(Location location, T error)
		{
			if(error == null) {
				throw new ArgumentNullException("error");
			}
			Location = location;
			Error = error;
		}
		
		public static bool operator==(CompilationError<T> compilationError1, CompilationError<T> compilationError2)
		{
			
			return
				compilationError1.Location == compilationError2.Location
				&& compilationError1.Error.Equals(compilationError2.Error);
		}
		
		public static bool operator!=(CompilationError<T> compilationError1, CompilationError<T> compilationError2)
		{
			return !(compilationError1 == compilationError2);
		}
		
		public bool Equals(CompilationError<T> compilationError)
		{
			return this == compilationError;
		}
		
		public override bool Equals(object obj)
		{
			return obj is CompilationError<T> && Equals((CompilationError<T>)obj);
		}
		
		public override int GetHashCode()
		{
			return Location.GetHashCode() ^ Error.GetHashCode();
		}
		
		public override string ToString()
		{
			return string.Format("{0} at {1}", Error, Location);
		}
	}
}

