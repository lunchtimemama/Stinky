// 
// Highlighter.cs
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

using Stinky.Compiler.Source.Parsing;
using Stinky.Compiler.Source.Tokenization;

namespace Stinky.Compiler.Source.Highlighting
{
	using Source = Action<SourceVisitor>;
	using Token = Func<Parser, Parser>;

	public class IncrementalSourceHighlighter
	{
		readonly HighlightingParser highlightingParser;
		readonly Tokenizer tokenizer;

		public IncrementalSourceHighlighter(Highlighter highlighter)
		{
			highlightingParser = new HighlightingParser(highlighter);
			var context = new CompilationContext();
			tokenizer = context.CreateTokenizer(
				token => highlightingParser.Token = token,
				() => highlightingParser.OnDone(),
				() => {}
			);
		}

		public void Tokenize(Character character)
		{
			tokenizer.Tokenize(character);
			highlightingParser.Consumer(character.Location);
		}

		class HighlightingParser : Parser
		{
			readonly Highlighter highlighter;
			readonly SourceHighlighter sourceHightlighter;
			public Action<Location> Consumer;
			Parser parser;
			Token token;

			public HighlightingParser(Highlighter highlighter)
			{
				this.highlighter = highlighter;
				this.sourceHightlighter = new SourceHighlighter(highlighter);
				parser = new LineParser(source => source(sourceHightlighter), error => {});
			}

			public override Parser ParseStringLiteral(Func<string> @string, Region region)
			{
				Consumer = location => highlighter.HighlightStringLiteral(new Region(location, 1));
				return this;
			}

			public override Parser ParseNumberLiteral(Func<double> number, Region region)
			{
				Consumer = location => highlighter.HighlightNumberLiteral(new Region(location, 1));
				return this;
			}

			protected override Parser ParseRegion(Region region)
			{
				Consumer = location => { parser.OnDone(); };
				return this;
			}

			public override void OnDone ()
			{
				parser = token(parser);
			}

			public Token Token {
				set {
					token = value;
					token(this);
				}
			}
		}
	}
}
