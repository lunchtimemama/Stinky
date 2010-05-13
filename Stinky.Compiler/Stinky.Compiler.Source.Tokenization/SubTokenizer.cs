// 
// SubTokenizer.cs
//  
// Author:
//       Scott Thomas <lunchtimemama@gmail.com>
// 
// Copyright (c) 2010 Scott Thomas
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

namespace Stinky.Compiler.Source.Tokenization
{
	using TokenErrorConsumer = Action<CompilationError<TokenizationError>>;

	public abstract class SubTokenizer : Tokenizer
	{
		protected readonly RootTokenizer RootTokenizer;
		
		protected SubTokenizer(RootTokenizer rootTokenizer)
		{
			if(rootTokenizer == null) {
				throw new ArgumentNullException("rootTokenizer");
			}
			this.RootTokenizer = rootTokenizer;
		}
		
		public override Tokenizer Tokenize(Character character)
		{
			return RootTokenizer.Tokenize(character);
		}
		
		public override void OnDone()
		{
			RootTokenizer.OnTokenReady();
			RootTokenizer.OnDone();
		}
		
		protected void OnError(Location location, TokenizationError error)
		{
			RootTokenizer.OnError(location, error);
		}
		
		protected void OnError(CompilationError<TokenizationError> error)
		{
			RootTokenizer.OnError(error);
		}

		protected CompilationContext CompilationContext {
			get { return RootTokenizer.CompilationContext; }
		}
	}
}

