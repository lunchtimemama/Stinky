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

		public override TokenizationException OnCharacter(Character character)
		{
			var @char = character.Char;
			if(@char == '\n') {
				OnDone();
				return null;
			} else {
				if(tokenizer != null) {
					return tokenizer.OnCharacter(character);
				} else {
					Location location = character.Location;
					switch(@char) {
					case '+':
						OnToken(parser => parser.ParsePlus(location));
						return null;
					case '}':
						return new TokenizationException(TokenizationError.UnexpectedRightCurlyBracket, location, Environment.StackTrace);
					case '(':
						//OnToken(parser => parser.ParseOpenParentheses(location));
						return null;
					case ')':
						//OnToken(parser => parser.ParseCloseParentheses(location));
						return null;
					case '.':
						//tokenizer = new DotTokenizer(this, location);
						return null;
					case ',':
						//OnToken(parser => parser.ParseComma(location));
						return null;
					case ':':
						OnToken(parser => parser.ParseColon(location));
						return null;
					case '&':
						//OnToken(parser => parser.ParseAmpersand(location));
						return null;
					case '"':
						tokenizer = new StringLiteralTokenizer(this, location);
						return null;
					default:
						if((@char >= 'A' && @char <= 'z') || @char == '_') {
							tokenizer = new AlphanumericTokenizer(this, location);
							return tokenizer.OnCharacter(character);
						} else if(@char >= '0' && @char <= '9') {
							tokenizer = new NumberLiteralTokenizer(this, location);
							return tokenizer.OnCharacter(character);
						} else if(@char != ' ') {
							return new TokenizationException(TokenizationError.UnknownError, location, Environment.StackTrace);
						} else {
							return null;
						}
					}
				}
			}
		}

		public override TokenizationException OnDone()
		{
			if(tokenizer != null) {
				var error = tokenizer.OnDone();
				if(error != null) {
					return error;
				}
			}
			parser.OnDone();
			consumer();
			return null;
		}

		public void OnToken(Func<Parser, Parser> token)
		{
			parser = token(parser);
			tokenizer = null;
		}
	}
}
