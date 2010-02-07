// 
// NumberLiteralTokenizer.cs
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
using System.Text;

namespace Stinky.Compiler.Parser.Tokenizer
{
	public class NumberLiteralTokenizer : SubTokenizer
	{
		StringBuilder stringBuilder = new StringBuilder();
		
		public NumberLiteralTokenizer(Location location, RootTokenizer rootTokenizer)
			: base(rootTokenizer)
		{
			rootTokenizer.OnToken(parser => parser.ParseNumberLiteral(double.Parse(stringBuilder.ToString()), location));
		}
		
		public override TokenizationException OnCharacter(Character character)
		{
			var @char = character.Char;
			if((@char >= '0' && @char <= '9') || @char == '.') {
				stringBuilder.Append(@char);
				return null;
			} else {
				OnDone();
				return base.OnCharacter(character);
			}
		}
	}
}
