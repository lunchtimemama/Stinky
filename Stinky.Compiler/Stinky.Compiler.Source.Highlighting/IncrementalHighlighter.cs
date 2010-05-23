// 
// IncrementalHighlighter.cs
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

	public class IncrementalHighlighter
	{
		readonly HighlightingParser highlightingParser;
		Tokenizer tokenizer;

		public IncrementalHighlighter(Highlighter highlighter)
		{
			highlightingParser = new HighlightingParser(highlighter);
			var context = new CompilationContext();
			tokenizer = context.CreateTokenizer(
				token => highlightingParser.OnToken(token),
				() => highlightingParser.OnDone(),
				() => {}
			);
		}

		public void Tokenize(Character character)
		{
			tokenizer = tokenizer.Tokenize(character);
			highlightingParser.OnLocation(character.Location);
		}

		class HighlightingParser : Parser
		{
			readonly Highlighter highlighter;
			readonly IncrementalSourceHighlighter sourceHightlighter;
			Action<Location> consumer;
			Parser parser;
			Token token;

			public HighlightingParser(Highlighter highlighter)
			{
				this.highlighter = highlighter;
				this.sourceHightlighter = new IncrementalSourceHighlighter(highlighter);
				parser = new LineParser(source => source(sourceHightlighter), (p, e) => p);
			}

			public override Parser ParseStringLiteral(Func<string> @string, Region region)
			{
				consumer = location => highlighter.HighlightStringLiteral(new Region(location, 1));
				return this;
			}

			public override Parser ParseNumberLiteral(Func<double> number, Region region)
			{
				consumer = location => highlighter.HighlightNumberLiteral(new Region(location, 1));
				return this;
			}

			protected override Parser ParseRegion(Region region)
			{
				consumer = location => parser.OnDone();
				return this;
			}

			public override void OnDone ()
			{
				parser = token(parser);
				consumer = location => highlighter.HighlightOther(new Region(location, 1));
			}

			public void OnToken(Token token)
			{
				this.token = token;
				token(this);
				parser = token(parser);
			}

			public void OnLocation(Location location)
			{
				consumer(location);
				sourceHightlighter.OnLocation(location);
			}
		}
	}
}
