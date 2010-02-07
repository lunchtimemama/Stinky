// 
// RootTokenizer.cs
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

using Stinky.Compiler.Parser;
using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler
{
	public class Driver
	{
		readonly Action<int, Expression> consumer;
		
		int indentation;
		Tokenizer tokenizer;

		public Driver(Action<int, Expression> consumer)
		{
			this.consumer = consumer;
		}

		public TokenizationException OnCharacter(Character character)
		{
			if(tokenizer != null) {
				return tokenizer.OnCharacter(character);
			} else if(character.Char == '\t') {
				indentation = indentation + 1;
				return null;
			} else {
				tokenizer = new RootTokenizer(new LineParser(expression => consumer(indentation, expression)), OnLine);
				return tokenizer.OnCharacter(character);
			}
		}
		
		void OnLine()
		{
			tokenizer = null;
			indentation = 0;
		}
		
		public TokenizationException OnDone()
		{
			if(tokenizer != null) {
				return tokenizer.OnDone();
			} else {
				return null;
			}
		}
	}
}
