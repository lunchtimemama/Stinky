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
		
		Tokenizer tokenizer;
		
		public RootTokenizer(Action<Func<Parser, Parser>> tokenConsumer, Action tokenReady, Action lineReady)
		{
			this.tokenConsumer = tokenConsumer;
			this.tokenReady = tokenReady;
			this.lineReady = lineReady;
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
						OnTokenReady(parser => parser.ParsePlus(location));
						return null;
					case '-':
						OnTokenReady(parser => parser.ParseMinus(location));
						return null;
					case '/':
						OnTokenReady(parser => parser.ParseForwardSlash(location));
						return null;
					case '*':
						OnTokenReady(parser => parser.ParseAsterisk(location));
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
						OnTokenReady(parser => parser.ParseColon(location));
						return null;
					case '&':
						//OnToken(parser => parser.ParseAmpersand(location));
						return null;
					case '"':
						tokenizer = new StringLiteralTokenizer(location, this);
						return null;
					default:
						if((@char >= 'A' && @char <= 'z') || @char == '_') {
							tokenizer = new AlphanumericTokenizer(location, this);
							return tokenizer.OnCharacter(character);
						} else if(@char >= '0' && @char <= '9') {
							tokenizer = new NumberLiteralTokenizer(location, this);
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
			lineReady();
			return null;
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
	}
}
