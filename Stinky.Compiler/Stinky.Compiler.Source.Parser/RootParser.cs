// 
// RootParser.cs
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

namespace Stinky.Compiler.Source.Parser
{
	using Source = Action<SourceVisitor>;
	// FIXME why does gmcs barf when we change Action<SourceVisitor> to Source?
	using ExpressionParserProvider = Func<Action<SourceVisitor>, Action<Action<SourceVisitor>>, Action<CompilationError<ParseError>>, Parser, Parser>;

	public class RootParser : Parser
	{
		readonly ExpressionParserProvider expressionParserProvider;
		
		public RootParser(Action<Source> consumer, Action<CompilationError<ParseError>> errorConsumer)
			: this((s, c, ec, p) => new ExpressionParser(s, c, ec, p), consumer, errorConsumer)
		{
		}
		
		public RootParser(ExpressionParserProvider expressionParserProvider,
		                  Action<Source> consumer,
		                  Action<CompilationError<ParseError>> errorConsumer)
			: base(consumer, errorConsumer)
		{
			if(expressionParserProvider == null) {
				throw new ArgumentNullException("expressionParserProvider");
			}
			
			this.expressionParserProvider = expressionParserProvider;
		}
		
		public override Parser ParseIdentifier(string identifier, Region region)
		{
			return expressionParserProvider(Reference(identifier, region), Consumer, ErrorConsumer, this);
		}
		
		public override Parser ParseNumberLiteral(double number, Region region)
		{
			return expressionParserProvider(NumberLiteral(number, region), Consumer, ErrorConsumer, this);
		}
		
		public override Parser ParseStringLiteral(string @string, Region region)
		{
			return expressionParserProvider(StringLiteral(@string, region), Consumer, ErrorConsumer, this);
		}
		
		public override Parser ParseInterpolatedStringLiteral(IEnumerable<Source> interpolatedExpressions,
															  Region token)
		{
			return expressionParserProvider(
				InterpolatedStringLiteral(interpolatedExpressions, token), Consumer, ErrorConsumer, this);
		}
	}
}
