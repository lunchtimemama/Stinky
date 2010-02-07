// 
// LineTokenizer.cs
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

namespace Stinky.Compiler.Parser.Tokenizer
{
	public class RootTokenizer : Tokenizer
	{
		readonly Action<Func<Parser, Parser>> tokenConsumer;
		readonly Action tokenReady;
		readonly Action lineReady;
		readonly ErrorConsumer errorConsumer;
		
		Tokenizer tokenizer;
		
		public RootTokenizer(Action<Func<Parser, Parser>> tokenConsumer, Action tokenReady, Action lineReady, ErrorConsumer errorConsumer)
		{
			this.tokenConsumer = tokenConsumer;
			this.tokenReady = tokenReady;
			this.lineReady = lineReady;
			this.errorConsumer = errorConsumer;
		}
		
		public ErrorConsumer ErrorConsumer {
			get { return errorConsumer; }
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
						OnTokenReady(parser => parser.ParsePlus(location));
						break;
					case '-':
						OnTokenReady(parser => parser.ParseMinus(location));
						break;
					case '/':
						OnTokenReady(parser => parser.ParseForwardSlash(location));
						break;
					case '*':
						OnTokenReady(parser => parser.ParseAsterisk(location));
						break;
					case '}':
						OnError(location, TokenizationError.UnexpectedRightCurlyBracket);
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
						OnTokenReady(parser => parser.ParseColon(location));
						break;
					case '&':
						//OnToken(parser => parser.ParseAmpersand(location));
						break;
					case '"':
						tokenizer = new StringLiteralTokenizer(location, this);
						break;
					default:
						if((@char >= 'A' && @char <= 'z') || @char == '_') {
							tokenizer = new AlphanumericTokenizer(location, this);
							tokenizer.OnCharacter(character);
						} else if(@char >= '0' && @char <= '9') {
							tokenizer = new NumberLiteralTokenizer(location, this);
							tokenizer.OnCharacter(character);
						} else if(@char != ' ') {
							OnError(location, TokenizationError.UnknownError);
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
			lineReady();
		}
		
		void OnTokenReady(Func<Parser, Parser> token)
		{
			tokenConsumer(token);
			tokenReady();
		}

		public void OnToken(Func<Parser, Parser> token)
		{
			tokenConsumer(token);
		}
		
		public void OnTokenReady()
		{
			tokenReady();
			tokenizer = null;
		}
		
		public void OnError(CompilationError<ParseError> error)
		{
			errorConsumer.ParseErrorConsumer(error);
		}
		
		public void OnError(Location location, TokenizationError error)
		{
			OnError(new CompilationError<TokenizationError>(location, error));
		}
		
		public void OnError(CompilationError<TokenizationError> error)
		{
			errorConsumer.TokenizationErrorConsumer(error);
		}
	}
}
