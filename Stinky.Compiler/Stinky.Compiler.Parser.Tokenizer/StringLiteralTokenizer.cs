// 
// StringLiteralTokenizer.cs
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
using System.Text;

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser.Tokenizer
{
	public class StringLiteralTokenizer : SubTokenizer
	{
		readonly Location location;
		
		StringBuilder stringBuilder = new StringBuilder();
		List<Expression> interpolatedExpressions;
		RootTokenizer interpolationTokenizer;
		bool potentiallyInterpolated;
		bool potentiallyUninterpolated;
		bool escaped;
		
		public StringLiteralTokenizer(Location location, RootTokenizer rootTokenizer)
			: base (rootTokenizer)
		{
			this.location = location;
			rootTokenizer.OnToken(parser => interpolatedExpressions != null
				? parser.ParseInterpolatedStringLiteral(interpolatedExpressions, location)
				: parser.ParseStringLiteral(stringBuilder.ToString(), location));
		}
		
		public override void OnCharacter(Character character)
		{
			if(interpolationTokenizer != null) {
				interpolationTokenizer.OnCharacter(character);
			} else if(potentiallyInterpolated) {
				if(character.Char == '{') {
					stringBuilder.Append('{');
				} else {
					OnFirstInterpolatedCharacter(character);
				}
				potentiallyInterpolated = false;
			} else if(potentiallyUninterpolated) {
				if(character.Char == '}') {
					stringBuilder.Append('}');
					potentiallyUninterpolated = false;
				} else {
					OnError(character.Location, TokenizationError.UnexpectedRightCurlyBracket);
				}
			} else if(escaped) {
				OnEspacedCharacter(character);
			} else {
				OnRegularCharacter(character);
			}
		}
		
		void OnFirstInterpolatedCharacter(Character character)
		{
			if(interpolatedExpressions == null) {
				interpolatedExpressions = new List<Expression>();
			}
			if(stringBuilder.Length > 0) {
				interpolatedExpressions.Add(new StringLiteral(stringBuilder.ToString(), location));
				stringBuilder.Remove(0, stringBuilder.Length);
			}
			Parser parser = new RootParser(expression => interpolatedExpressions.Add(expression), ErrorConsumer.ParseErrorConsumer);
			Func<Parser, Parser> token = null;
			interpolationTokenizer = new RootTokenizer(
				t => token = t,
				() => parser = token(parser),
				() => {
					parser.OnDone();
					interpolationTokenizer = null;
				},
				new ErrorConsumer(ErrorConsumer.ParseErrorConsumer, error => {
					if(error.Error == TokenizationError.UnexpectedRightCurlyBracket) {
						interpolationTokenizer.OnDone();
					} else {
						OnError(error);
					}
				}));
			interpolationTokenizer.OnCharacter(character);
		}
		
		void OnEspacedCharacter(Character character)
		{
			escaped = false;
			switch(character.Char) {
			case '"':
				stringBuilder.Append('"');
				break;
			case '\\':
				stringBuilder.Append('\\');
				break;
			case 'n':
				stringBuilder.Append('\n');
				break;
			case 't':
				stringBuilder.Append('\t');
				break;
			default:
				OnError(character.Location, TokenizationError.UnknownError);
				break;
			}
		}
		
		void OnRegularCharacter(Character character)
		{
			switch(character.Char) {
			case '{':
				potentiallyInterpolated = true;
				break;
			case '}':
				potentiallyUninterpolated = true;
				break;
			case '"':
				OnDone();
				break;
			case '\n':
				OnError(character.Location, TokenizationError.UnknownError);
				 break;
			case '\\':
				escaped = true;
				break;
			default:
				stringBuilder.Append(character.Char);
				break;
			}
		}
		
		public override void OnDone()
		{
			if(potentiallyInterpolated || (interpolationTokenizer != null && !potentiallyUninterpolated)) {
				OnError(location, TokenizationError.UnknownError);
			} else if(interpolatedExpressions != null) {
				if(stringBuilder.Length > 0) {
					interpolatedExpressions.Add(new StringLiteral(stringBuilder.ToString(), location));
				}
				base.OnDone();
			} else {
				base.OnDone();
			}
		}
	}
}
