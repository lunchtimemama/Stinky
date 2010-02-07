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
		
		public StringLiteralTokenizer(RootTokenizer lineTokenizer, Location location)
			: base (lineTokenizer)
		{
			this.location = location;
			Token = parser => interpolatedExpressions != null
				? parser.ParseInterpolatedStringLiteral(interpolatedExpressions, location)
				: parser.ParseStringLiteral(stringBuilder.ToString(), location);
		}
		
		public override TokenizationException OnCharacter(Character character)
		{
			if(interpolationTokenizer != null) {
				return interpolationTokenizer.OnCharacter(character);
			} else if(potentiallyInterpolated) {
				if(character.Char == '{') {
					stringBuilder.Append('{');
				} else {
					if(interpolatedExpressions == null) {
						interpolatedExpressions = new List<Expression>();
					}
					if(stringBuilder.Length > 0) {
						interpolatedExpressions.Add(new StringLiteral(stringBuilder.ToString(), location));
						stringBuilder.Remove(0, stringBuilder.Length);
					}
					interpolationTokenizer = new InterpolatedLineTokenizer(
						new RootParser(expression => interpolatedExpressions.Add(expression)),
						() => interpolationTokenizer = null);
					interpolationTokenizer.OnCharacter(character);
				}
				potentiallyInterpolated = false;
				return null;
			} else if(potentiallyUninterpolated) {
				if(character.Char == '}') {
					stringBuilder.Append('}');
					potentiallyUninterpolated = false;
					return null;
				} else {
					return new TokenizationException(
						TokenizationError.UnexpectedRightCurlyBracket,
						character.Location, Environment.StackTrace);
				}
			} else if(escaped) {
				escaped = false;
				switch(character.Char) {
				case '"':
					stringBuilder.Append('"');
					return null;
				case '\\':
					stringBuilder.Append('\\');
					return null;
				case 'n':
					stringBuilder.Append('\n');
					return null;
				case 't':
					stringBuilder.Append('\t');
					return null;
				default:
					return new TokenizationException(TokenizationError.UnknownError, character.Location, Environment.StackTrace);
				}
			} else {
				switch(character.Char) {
				case '{':
					potentiallyInterpolated = true;
					return null;
				case '}':
					potentiallyUninterpolated = true;
					return null;
				case '"':
					OnDone();
					return null;
				case '\n':
					return new TokenizationException(TokenizationError.UnknownError, character.Location, Environment.StackTrace);
					break;
				case '\\':
					escaped = true;
					return null;
				default:
					stringBuilder.Append(character.Char);
					return null;
				}
			}
		}
		
		public override TokenizationException OnDone ()
		{
			if(potentiallyInterpolated || (interpolationTokenizer != null && !potentiallyUninterpolated)) {
				return new TokenizationException(TokenizationError.UnknownError, location, Environment.StackTrace);
			} else if(interpolatedExpressions != null) {
				if(stringBuilder.Length > 0) {
					interpolatedExpressions.Add(new StringLiteral(stringBuilder.ToString(), location));
				}
				return base.OnDone();
			} else {
				return base.OnDone();
			}
		}
	}
}
