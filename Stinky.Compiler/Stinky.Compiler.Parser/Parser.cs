// 
// Parser.cs
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
using System.Collections.Generic;

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser
{
	public class Parser
	{
		protected readonly Action<Expression> Consumer;
		protected readonly Parser NextParser;
		
		public Parser(Action<Expression> consumer)
			: this(consumer, null)
		{
		}
		
		public Parser(Action<Expression> consumer, Parser nextParser)
		{
			Consumer = consumer;
			NextParser = nextParser;
		}
		
		public virtual Parser ParseIdentifier(string identifier, Location location)
		{
			if(NextParser != null) {
				return NextParser.ParseIdentifier(identifier, location);
			} else {
				throw new ParseException(Error, location);
			}
		}
		
		public virtual Parser ParseColon(Location location)
		{
			if(NextParser != null) {
				return NextParser.ParseColon(location);
			} else {
				throw new ParseException(Error, location);
			}
		}

		public virtual Parser ParseNumberLiteral(double number, Location location)
		{
			if(NextParser != null) {
				return NextParser.ParseNumberLiteral(number, location);
			} else {
				throw new ParseException(Error, location);
			}
		}
		
		public virtual Parser ParseStringLiteral(string @string, Location location)
		{
			if(NextParser != null) {
				return NextParser.ParseStringLiteral(@string, location);
			} else {
				throw new ParseException(Error, location);
			}
		}
		
		public virtual Parser ParseInterpolatedStringLiteral(IEnumerable<Expression> interpolatedExpressions, Location location)
		{
			if(NextParser != null) {
				return NextParser.ParseInterpolatedStringLiteral(interpolatedExpressions, location);
			} else {
				throw new ParseException(Error, location);
			}
		}

		public virtual Parser ParsePlus(Location location)
		{
			if(NextParser != null) {
				return NextParser.ParsePlus(location);
			} else {
				throw new ParseException(Error, location);
			}
		}
		
		protected virtual ParseError Error {
			get { return ParseError.UnknownError; }
		}
		
		public virtual void OnDone()
		{
		}
	}
}
