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

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser
{
	public class RootParser : Parser
	{
		readonly Func<Expression, Action<Expression>, Parser, Parser> expressionParserProvider;
		
		public RootParser(Action<Expression> consumer)
			: this((e, c, p) => new ExpressionParser(e, c, p), consumer)
		{
		}
		
		public RootParser(Func<Expression, Action<Expression>, Parser, Parser> expressionParserProvider, Action<Expression> consumer)
			: base(consumer)
		{
			if(expressionParserProvider == null) throw new ArgumentNullException("expressionParserProvider");
			
			this.expressionParserProvider = expressionParserProvider;
		}
		
		public override Parser ParseIdentifier(string identifier, Location location)
		{
			return expressionParserProvider(new Reference(identifier, location), Consumer, this);
		}
		
		public override Parser ParseNumberLiteral(double number, Location location)
		{
			return expressionParserProvider(new NumberLiteral(number, location), Consumer, this);
		}
		
		public override Parser ParseStringLiteral(string @string, Location location)
		{
			return expressionParserProvider(new StringLiteral(@string, location), Consumer, this);
		}
		
		public override Parser ParseInterpolatedStringLiteral(IEnumerable<Expression> interpolatedExpressions, Location location)
		{
			return expressionParserProvider(new InterpolatedStringLiteral(interpolatedExpressions, location), Consumer, this);
		}
	}
}
