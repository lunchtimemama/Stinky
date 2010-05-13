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

using Stinky.Compiler.Source;
using Stinky.Compiler.Source.Parsing;
using Stinky.Compiler.Source.Tokenization;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler
{
	using Token = Func<Parser, Parser>;
	using Syntax = Action<SyntaxVisitor>;
	using Consumer = Action<int, Action<SyntaxVisitor>>;

	public class Driver
	{
		readonly CompilationContext compilationContext;
		readonly Syntaxifier syntaxifier;
		
		int indentation;
		Tokenizer tokenizer;
		Token token;
		Parser parser;

		public Driver(Consumer consumer, CompilationContext compilationContext)
		{
			if(consumer == null) {
				throw new ArgumentNullException("consumer");
			} else if(compilationContext == null) {
				throw new ArgumentNullException("compilationContext");
			}

			this.compilationContext = compilationContext;
			this.syntaxifier = new Syntaxifier(
				syntax => consumer(indentation, syntax),
				e => compilationContext.HandleSyntaxError(e));
		}

		public void OnCharacter(Character character)
		{
			if(tokenizer != null) {
				tokenizer = tokenizer.OnCharacter(character);
			} else if(character.Char == '\t') {
				indentation = indentation + 1;
			} else {
				parser = compilationContext.CreateParser(source => source(syntaxifier));
				tokenizer = compilationContext.CreateTokenizer(
					token => this.token = token,
					() => parser = token(parser),
					OnLine,
				    compilationContext);
				tokenizer.OnCharacter(character);
			}
		}
		
		void OnLine()
		{
			tokenizer = null;
			indentation = 0;
			parser.OnDone();
		}
		
		public void OnDone()
		{
			if(tokenizer != null) {
				tokenizer.OnDone();
			}
		}
	}
}
