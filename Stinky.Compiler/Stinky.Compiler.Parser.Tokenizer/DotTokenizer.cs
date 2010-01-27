// 
// DotTokenizer.cs
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

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser.Tokenizer
{
	public class DotTokenizer : Tokenizer
	{
		readonly LineTokenizer lineTokenizer;
		readonly Location location;
		
		bool doubleDot;

		public DotTokenizer(LineTokenizer lineTokenizer, Location location)
		{
			this.lineTokenizer = lineTokenizer;
			this.location = location;
		}

		public override void OnCharacter(Character character)
		{
			if(character.Char == '.') {
				if(doubleDot) {
					throw new TokenizationException(character.Location);
				}
				doubleDot = true;
			} else {
				OnDone();
				lineTokenizer.OnCharacter(character);
			}
		}

		public override void OnDone()
		{
			if(doubleDot) {
				//lineTokenizer.OnToken(parser => parser.ParseDot(location));
			} else {
				//lineTokenizer.OnToken(parser => parser.ParseDoubleDot(location));
			}
		}
	}
}
