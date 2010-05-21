// 
// SourceHighlighter.cs
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

namespace Stinky.Compiler.Source.Highlighting
{
	public class SourceHighlighter : SourceVisitor
	{
		public readonly Highlighter highlighter;

		Region lastRegion;

		public SourceHighlighter(Highlighter highlighter)
		{
			this.highlighter = highlighter;
		}

		public override void VisitReference(string identifier, Region region)
		{
			var highlightedRegion = region;
			if(region.Location == lastRegion.Location) {
				var delta = region.Length - lastRegion.Length;
				if(delta > 0) {
					highlightedRegion = new Region(
						new Location(
							region.Location.Source,
							region.Location.Line,
							region.Location.Column + delta),
						delta);
				}
			}
			highlighter.HighlightOther(highlightedRegion);
			lastRegion = region;
		}
	}
}

