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
		bool escaped;
		
		public StringLiteralTokenizer(LineTokenizer lineTokenizer, Location location)
		{
			this.lineTokenizer = lineTokenizer;
			this.location = location;
		}
		
		public override void OnCharacter(Character character)
		{
			if(escaped) {
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
			var @string = stringBuilder.ToString();
			lineTokenizer.OnToken(parser => parser.ParseStringLiteral(@string, location));
		}
	}
}
