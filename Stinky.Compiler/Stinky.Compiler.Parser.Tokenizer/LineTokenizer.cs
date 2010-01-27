// 
// LineTokenizer.cs
//  
// Author:
//       Scott Thomas <lunchtimemama@gmail.com>
// 
// Copyright (c) 2009 Scott Thomas
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

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser.Tokenizer
{
	public class LineTokenizer : Tokenizer
	{
		readonly Action consumer;
		
		Parser parser;
		Tokenizer tokenizer;
		
		public LineTokenizer(Parser parser, Action consumer)
		{
			this.parser = parser;
			this.consumer = consumer;
		}

		public override void OnCharacter(Character character)
		{
			var @char = character.Char;
			if(@char == '\n') {
				OnDone();
			} else {
				if(tokenizer != null) {
					tokenizer.OnCharacter(character);
				} else {
					Location location = character.Location;
					switch(@char) {
					case '+':
						OnToken(parser => parser.ParsePlus(location));
						break;
					case '{':
						OnToken(parser => parser.ParseLeftCurlyBracket(location));
						break;
					case '}':
						OnToken(parser => parser.ParseRightCurlyBracket(location));
						break;
					case '(':
						//OnToken(parser => parser.ParseOpenParentheses(location));
						break;
					case ')':
						//OnToken(parser => parser.ParseCloseParentheses(location));
						break;
					case '.':
						//tokenizer = new DotTokenizer(this, location);
						break;
					case ',':
						//OnToken(parser => parser.ParseComma(location));
						break;
					case ':':
						OnToken(parser => parser.ParseColon(location));
						break;
					case '&':
						//OnToken(parser => parser.ParseAmpersand(location));
						break;
					case '"':
						tokenizer = new StringLiteralTokenizer(this, location);
						break;
					default:
						if((@char >= 'A' && @char <= 'z') || @char == '_') {
							tokenizer = new AlphanumericTokenizer(this, location);
							tokenizer.OnCharacter(character);
						} else if(@char >= '0' && @char <= '9') {
							tokenizer = new NumberLiteralTokenizer(this, location);
							tokenizer.OnCharacter(character);
						} else if(@char != ' ') {
							throw new TokenizationException(location);
						}
						break;
					}
				}
			}
		}

		public override void OnDone()
		{
			if(tokenizer != null) {
				tokenizer.OnDone();
			}
			parser.OnDone();
			consumer();
		}

		public void OnToken(Func<Parser, Parser> token)
		{
			parser = token(parser);
			tokenizer = null;
		}
	}
}
