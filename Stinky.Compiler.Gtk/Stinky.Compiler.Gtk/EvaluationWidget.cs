// 
// EvaluationWidget.cs
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

using Stinky.Compiler.Expressions;
using Stinky.Compiler.Source;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Gtk
{
	using Syntax = Action<SyntaxVisitor>;
	using Expression = Action<ExpressionVisitor>;

	public class EvaluationWidget : VBox
	{
		readonly ReadEvalPrintLoopView readEvalPrintLoopView;
		
		public EvaluationWidget()
		{
			var interpreter = new Interpreter(value => readEvalPrintLoopView.Print(value.ToString()));
			var scope = new Scope();
			var resolver = new Resolver(scope, (type, expression) => {
				if(type != typeof(void)) {
					expression(interpreter);
				}
			});
			var errorConsumer = new ErrorConsumer(
				e => { throw new Exception("foo"); },
				e => { throw new Exception("bar"); }
			);
			var rootTokenizer = new Driver((indentation, syntax) => syntax(resolver), errorConsumer, e => { throw new Exception("bat"); });
			var scrolledWindow = new ScrolledWindow();
			readEvalPrintLoopView = new ReadEvalPrintLoopView(text => {
				var column = 0;
				foreach(var character in text) {
					rootTokenizer.OnCharacter(new Character(character, new Location(null, 0, column)));
					column++;
				}
				rootTokenizer.OnCharacter(new Character('\n', new Location(null, 0, column)));
			});
			scrolledWindow.Add(readEvalPrintLoopView);
			PackStart(scrolledWindow);
			ShowAll();
		}
	}
}

