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

using Stinky.Compiler.Source.Parsing;

namespace Stinky.Compiler.Source.Tokenization
{
	using Token = Func<Parser, Parser>;

	public class RootTokenizer : Tokenizer
	{
		readonly Action<Token> tokenConsumer;
		readonly Action tokenReady;
		readonly Action lineReady;
		readonly CompilationContext compilationContext;
		
		public RootTokenizer(Action<Token> tokenConsumer,
							 Action tokenReady,
							 Action lineReady,
							 CompilationContext compilationContext)
		{
			if(tokenConsumer == null) {
				throw new ArgumentNullException("tokenConsumer");
			} else if(tokenReady == null) {
				throw new ArgumentNullException("tokenReady");
			} else if(lineReady == null) {
				throw new ArgumentNullException("lineReady");
			} else if(compilationContext == null) {
				throw new ArgumentNullException("compilationContext");
			}
			this.tokenConsumer = tokenConsumer;
			this.tokenReady = tokenReady;
			this.lineReady = lineReady;
			this.compilationContext = compilationContext;
		}

		public CompilationContext CompilationContext {
			get { return compilationContext; }
		}

		public override Tokenizer Tokenize(Character character)
		{
			var @char = character.Char;
			if(@char == '\n') {
				OnDone();
				return this;
			}

			Location location = character.Location;
			switch(@char) {
			case '+':
				return OnTokenReady(parser => parser.ParsePlus(location));
			case '-':
				return OnTokenReady(parser => parser.ParseMinus(location));
			case '/':
				return OnTokenReady(parser => parser.ParseForwardSlash(location));
			case '*':
				return OnTokenReady(parser => parser.ParseAsterisk(location));
			case '}':
				return OnError(this, location, TokenizationError.UnexpectedRightCurlyBracket);
			case '(':
				//OnToken(parser => parser.ParseOpenParentheses(location));
				return this;
			case ')':
				//OnToken(parser => parser.ParseCloseParentheses(location));
				return this;
			case '.':
				//tokenizer = new DotTokenizer(this, location);
				return this;
			case ',':
				//OnToken(parser => parser.ParseComma(location));
				return this;
			case ':':
				return OnTokenReady(parser => parser.ParseColon(location));
			case '&':
				//OnToken(parser => parser.ParseAmpersand(location));
				return this;
			case '"':
				return new StringLiteralTokenizer(this);
			default:
				if((@char >= 'A' && @char <= 'z') || @char == '_') {
					return new AlphanumericTokenizer(location, this).Tokenize(character);
				} else if(@char >= '0' && @char <= '9') {
					return new NumberLiteralTokenizer(location, this).Tokenize(character);
				} else if(@char != ' ') {
					return OnError(this, location, TokenizationError.UnknownError);
				} else {
					// TODO handle this
					return this;
				}
			}
		}

		public override void OnDone()
		{
			lineReady();
		}
		
		Tokenizer OnTokenReady(Token token)
		{
			tokenConsumer(token);
			tokenReady();
			return this;
		}

		public void OnToken(Token token)
		{
			tokenConsumer(token);
		}
		
		public void OnTokenReady()
		{
			tokenReady();
		}
		
		public Parser OnError(Parser parser, CompilationError<ParseError> error)
		{
			return compilationContext.HandleParseError(parser, error);
		}
		
		public Tokenizer OnError(Tokenizer tokenizer, Location location, TokenizationError error)
		{
			return OnError(tokenizer, new CompilationError<TokenizationError>(location, error));
		}
		
		public Tokenizer OnError(Tokenizer tokenizer, CompilationError<TokenizationError> error)
		{
			return compilationContext.HandleTokenError(tokenizer, error);
		}
	}
}
