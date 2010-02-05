// 
// AlphanumericTokenizer.cs
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
// THE SOFTWARE.using System.Text;

using System;
using System.Text;

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser.Tokenizer
{
	public class AlphanumericTokenizer : Tokenizer
	{
		readonly LineTokenizer lineTokenizer;
		readonly Location location;
		
		StringBuilder stringBuilder = new StringBuilder();

		public AlphanumericTokenizer(LineTokenizer lineTokenizer, Location location)
		{
 			this.lineTokenizer = lineTokenizer;
			this.location = location;
		}

		public override TokenizationException OnCharacter(Character character)
		{
			var @char = character.Char;
			if((@char >= '0' && @char <= '9') || (@char >= 'A' && @char <= 'z') || @char == '#' || @char == '_') {
				stringBuilder.Append(@char);
				return null;
			} else {
				OnDone();
				return lineTokenizer.OnCharacter(character);
			}
		}

		public override TokenizationException OnDone()
		{
			lineTokenizer.OnToken(parser => parser.ParseIdentifier(stringBuilder.ToString(), location));
			return null;
		}
	}
}
