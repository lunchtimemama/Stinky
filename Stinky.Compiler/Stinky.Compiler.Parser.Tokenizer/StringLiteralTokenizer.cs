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
	public class StringLiteralTokenizer : Tokenizer
	{
		readonly LineTokenizer lineTokenizer;
		readonly Location location;
		
		StringBuilder stringBuilder = new StringBuilder();
		List<Expression> interpolatedExpressions;
		LineTokenizer interpolationTokenizer;
		bool potentiallyInterpolated;
		bool potentiallyUninterpolated;
		bool escaped;
		
		public StringLiteralTokenizer(LineTokenizer lineTokenizer, Location location)
		{
			this.lineTokenizer = lineTokenizer;
			this.location = location;
		}
		
		public override void OnCharacter(Character character)
		{
			if(interpolationTokenizer != null) {
				interpolationTokenizer.OnCharacter(character);
			} else if(potentiallyInterpolated) {
				if(character.Char == '{') {
					stringBuilder.Append('{');
				} else {
					if(interpolatedExpressions == null) {
						interpolatedExpressions = new List<Expression>();
					}
					interpolatedExpressions.Add(new StringLiteral(stringBuilder.ToString(), location));
					stringBuilder.Remove(0, stringBuilder.Length);
					interpolationTokenizer = new InterpolatedLineTokenizer(
						new RootParser(expression => interpolatedExpressions.Add(expression)),
						() => interpolationTokenizer = null);
					interpolationTokenizer.OnCharacter(character);
				}
				potentiallyInterpolated = false;
			} else if(potentiallyUninterpolated) {
				if(character.Char == '}') {
					stringBuilder.Append('}');
				} else {
					throw new TokenizationException(character.Location);
				}
				potentiallyUninterpolated = false;
			} else if(escaped) {
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
					throw new TokenizationException(character.Location);
				}
				escaped = false;
			} else {
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
					throw new TokenizationException(character.Location);
					break;
				case '\\':
					escaped = true;
					break;
				default:
					stringBuilder.Append(character.Char);
					break;
				}
			}
		}
		
		public override void OnDone ()
		{
			if(potentiallyInterpolated || potentiallyUninterpolated || interpolationTokenizer != null) {
				throw new TokenizationException(location);
			} else if(interpolatedExpressions != null) {
				if(stringBuilder.Length > 0) {
					interpolatedExpressions.Add(new StringLiteral(stringBuilder.ToString(), location));
				}
				lineTokenizer.OnToken(parser => parser.ParseInterpolatedStringLiteral(interpolatedExpressions, location));
			} else {
				lineTokenizer.OnToken(parser => parser.ParseStringLiteral(stringBuilder.ToString(), location));
			}
		}
	}
}
