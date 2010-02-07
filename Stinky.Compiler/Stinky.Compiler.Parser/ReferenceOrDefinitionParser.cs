// 
// ReferenceOrDefinitionParser.cs
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

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser
{
	public class ReferenceOrDefinitionParser : ExpressionParser
	{
		readonly Reference reference;
		
		public ReferenceOrDefinitionParser(string identifier,
										   Location location,
										   Action<Expression> consumer,
										   Action<CompilationError<ParseError>> errorConsumer,
										   Parser nextParser)
			: base(new Reference(identifier, location), consumer, errorConsumer, nextParser)
		{
			reference = new Reference(identifier, location);
		}
		
		public override Parser ParseColon(Location location)
		{
			return new RootParser(
				expression => Consumer(new Definition(reference, expression, location)),
				ErrorConsumer);
		}
	}
}
