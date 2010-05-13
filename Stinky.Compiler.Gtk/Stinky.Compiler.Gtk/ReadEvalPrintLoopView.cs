// 
// ReadEvalPrintLoopBox.cs
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
using Gdk;
using Pango;

using Stinky.Compiler.Source;
using Stinky.Compiler.Source.Highlighting;

namespace Stinky.Compiler.Gtk
{
	public class ReadEvalPrintLoopView : TextView
	{
		readonly Action<string> evaluator;

		IncrementalSourceHighlighter sourceHighlighter;
		
		TextMark promptStart;
		int column;
		
		public ReadEvalPrintLoopView(Action<string> evaluator)
			: base(new TextBuffer(new TextTagTable()))
		{
			this.evaluator = evaluator;
			var startIter = Buffer.StartIter;
			promptStart = Buffer.CreateMark(null, startIter, true);
			Buffer.TagTable.Add(new TextTag("prompt") { Editable = false, Weight = Weight.Bold });
			Buffer.TagTable.Add(new TextTag("history") { Editable = false });
			Buffer.TagTable.Add(new TextTag("string") { Foreground ="red" });
			Buffer.TagTable.Add(new TextTag("number") { Foreground ="green" });
			
			Buffer.InsertWithTagsByName(ref startIter, "> ", "prompt");
			promptStart = Buffer.CreateMark(null, startIter, true);

			this.sourceHighlighter = new IncrementalSourceHighlighter(new GtkHighlighter(Buffer));
		}
		
		protected override bool OnKeyPressEvent(EventKey evnt)
		{
			if(evnt.Key == Gdk.Key.Return) {
				var startIter = Buffer.GetIterAtMark(promptStart);
				var endIter = Buffer.EndIter;
				var text = Buffer.GetText(startIter, endIter, true);
				Buffer.ApplyTag("history", startIter, endIter);
				promptStart = Buffer.CreateMark(null, endIter, true);
				evaluator(text);
				NewLine();
				return true;
			} else if(evnt.Key == Gdk.Key.Shift_L) {
				return base.OnKeyPressEvent(evnt);
			}
			var result = base.OnKeyPressEvent(evnt);
			sourceHighlighter.OnCharacter(new Character((char)evnt.KeyValue, new Location(null, 0, column)));
			column++;
			return result;
		}
		
		public void Print(string text)
		{
			var startIter = Buffer.GetIterAtMark(promptStart);
			Buffer.InsertWithTagsByName(ref startIter, "\n", "history");
			Buffer.InsertWithTagsByName(ref startIter, "> ", "prompt");
			Buffer.InsertWithTagsByName(ref startIter, text, "history");
			promptStart = Buffer.CreateMark(null, startIter, true);
			Buffer.PlaceCursor(startIter);
		}

		public void NewLine()
		{
			var startIter = Buffer.GetIterAtMark(promptStart);
			Buffer.InsertWithTagsByName(ref startIter, "\n", "history");
			Buffer.InsertWithTagsByName(ref startIter, "> ", "prompt");
			promptStart = Buffer.CreateMark(null, startIter, true);
			Buffer.PlaceCursor(startIter);
			this.sourceHighlighter = new IncrementalSourceHighlighter(new GtkHighlighter(Buffer));
			column = 0;
		}
	}
}

