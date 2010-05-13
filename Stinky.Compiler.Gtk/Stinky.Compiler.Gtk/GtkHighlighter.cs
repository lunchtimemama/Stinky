// 
// GtkHighlighter.cs
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

using Gtk;

using Stinky.Compiler.Source;

using Stinky.Compiler.Source.Highlighting;

namespace Stinky.Compiler.Gtk
{
	public class GtkHighlighter : Highlighter
	{
		readonly TextBuffer buffer;
		readonly TextMark lineStart;

		public GtkHighlighter(TextBuffer buffer)
		{
			this.buffer = buffer;
			lineStart = buffer.CreateMark(null, buffer.EndIter, true);
		}

		public override void HighlightStringLiteral(Region region)
		{
			Highlight("string", region);
		}

		public override void HighlightNumberLiteral (Region region)
		{
			Highlight("number", region);
		}

		void Highlight(string tag, Region region)
		{
			var start = buffer.GetIterAtMark(lineStart);
			start.ForwardChars(region.Location.Column);
			var end = start;
			end.ForwardChars(region.Length);
			buffer.ApplyTag(tag, start, end);
		}
	}
}

