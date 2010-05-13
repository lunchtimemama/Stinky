// 
// Syntaxer.cs
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

namespace Stinky.Compiler.Source
{
	using Source = Action<SourceVisitor>;

	public class SourceVisitor
	{
		public virtual void VisitStringLiteral(string @string, Region region)
		{
			VisitRegion(region);
		}

		public virtual void VisitNumberLiteral(double number, Region region)
		{
			VisitRegion(region);
		}

		public virtual void VisitPlusOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, location);
		}

		public virtual void VisitMinusOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, location);
		}

		public virtual void VisitAsteriskOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, location);
		}

		public virtual void VisitForwardSlashOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, location);
		}

		public virtual void VisitReference(string identifier, Region region)
		{
			VisitRegion(region);
		}

		public virtual void VisitDefinition(Source reference, Source expression, Location location)
		{
			VisitLocation(location);
		}

		public virtual void VisitInterpolatedStringLiteral(IEnumerable<Source> sources, Region region)
		{
			VisitRegion(region);
		}

		protected virtual void VisitBinaryOperator(Source left, Source right, Location location)
		{
			VisitLocation(location);
		}

		protected virtual void VisitRegion(Region region)
		{
			VisitLocation(region.Location);
		}

		protected virtual void VisitLocation(Location location)
		{
		}
	}
}

