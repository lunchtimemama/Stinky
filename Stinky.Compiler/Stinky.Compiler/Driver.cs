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
using Stinky.Compiler.Source.Parser;
using Stinky.Compiler.Source.Parser.Tokenizer;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler
{
	using StinkyParser = Stinky.Compiler.Source.Parser.Parser;

	public class Driver
	{
		readonly ErrorConsumer errorConsumer;
		readonly Syntaxifier syntaxifier;
		
		int indentation;
		Tokenizer tokenizer;
		Func<StinkyParser, StinkyParser> token;
		StinkyParser parser;

		public Driver(Action<int, Action<SyntaxVisitor>> consumer, ErrorConsumer errorConsumer)
			: this(consumer, errorConsumer, null)
		{
		}

		public Driver(Action<int, Action<SyntaxVisitor>> consumer,
		              ErrorConsumer errorConsumer,
		              Action<CompilationError<SyntaxError>> syntaxErrorConsumer)
		{
			if(consumer == null) {
				throw new ArgumentNullException("consumer");
			} else if(errorConsumer == null) {
				throw new ArgumentNullException("errorConsumer");
			}
			if(syntaxErrorConsumer == null) {
				syntaxErrorConsumer = e => {};
			}
			this.errorConsumer = errorConsumer;
			syntaxifier = new Syntaxifier(
				syntax => consumer(indentation, syntax),
				(e, l) => syntaxErrorConsumer(new CompilationError<SyntaxError>(l, e))
			);
		}

		public void OnCharacter(Character character)
		{
			if(tokenizer != null) {
				tokenizer.OnCharacter(character);
			} else if(character.Char == '\t') {
				indentation = indentation + 1;
			} else {
				parser = new LineParser(
					source => source(syntaxifier),
					errorConsumer.ParseErrorConsumer);

				tokenizer = new RootTokenizer(
					token => this.token = token,
					() => parser = token(parser),
					OnLine,
					errorConsumer);
				
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
