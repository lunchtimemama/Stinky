// 
// SyntaxHighlightingDriver.cs
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
using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;

using StinkyParser = Stinky.Compiler.Parser.Parser;

namespace Stinky.Compiler
{
	public class SyntaxHighlightingDriver
	{
		readonly Action<Expression> consumer;
		
		RootTokenizer rootTokenizer;
		Func<StinkyParser, StinkyParser> token;
		StinkyParser parser;
		Expression expression;
		
		public SyntaxHighlightingDriver(Action<Expression> consumer)
		{
			parser = new LineParser(expression => {
				expression.Visit(new SyntaxHighlighter(this.expression, consumer));
				this.expression = expression;
			});
			rootTokenizer = new RootTokenizer(
				token => this.token = token,
				() => parser = token(parser),
				() => {}
			);
		}
		
		public TokenizationException OnCharacter(Character character)
		{
			var result = rootTokenizer.OnCharacter(character);
			token(parser).OnDone();
			return result;
		}
	}
}

