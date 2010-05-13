// 
// Parser.cs
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

namespace Stinky.Compiler.Source.Parsing
{
	using Source = Action<SourceVisitor>;

	public class Parser
	{
		public virtual Parser ParseIdentifier(Func<string> identifier, Region region)
		{
			return ParseRegion(region);
		}
		
		public virtual Parser ParseColon(Location location)
		{
			return ParseLocation(location);
		}

		public virtual Parser ParseNumberLiteral(Func<double> number, Region region)
		{
			return ParseRegion(region);
		}
		
		public virtual Parser ParseStringLiteral(Func<string> @string, Region region)
		{
			return ParseRegion(region);
		}
		
		public virtual Parser ParseInterpolatedStringLiteral(Func<IEnumerable<Source>> interpolatedExpressions,
		                                                     Region region)
		{
			return ParseRegion(region);
		}

		public virtual Parser ParsePlus(Location location)
		{
			return ParseLocation(location);
		}

		public virtual Parser ParseMinus(Location location)
		{
			return ParseLocation(location);
		}

		public virtual Parser ParseForwardSlash(Location location)
		{
			return ParseLocation(location);
		}

		public virtual Parser ParseAsterisk(Location location)
		{
			return ParseLocation(location);
		}

		protected virtual Parser ParseRegion(Region region)
		{
			return ParseLocation(region.Location);
		}

		protected virtual Parser ParseLocation(Location location)
		{
			return this;
		}
		
		public virtual void OnDone()
		{
		}
	}
}
