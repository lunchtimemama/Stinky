// 
// ExpressionParser.cs
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

namespace Stinky.Compiler.Source.Parsing
{
	using Source = Action<SourceVisitor>;
	using ParseErrorConsumer = Func<Parser, CompilationError<ParseError>, Parser>;

	public class ExpressionParser :BaseParser
	{
		readonly Source expression;
		readonly Func<Source, Source> @operator;
		readonly int operatorPriority;
		
		public ExpressionParser(Source expression,
								Action<Source> sourceConsumer,
								ParseErrorConsumer parseErrorConsumer,
								BaseParser nextParser)
			: this(expression, null, 0, sourceConsumer, parseErrorConsumer, nextParser)
		{
		}
		
		public ExpressionParser(Source expression,
		                        Func<Source, Source> @operator,
		                        int operatorPriority,
		                        Action<Source> sourceConsumer,
		                        ParseErrorConsumer parseErrorConsumer,
		                        BaseParser nextParser)
			: base(sourceConsumer, parseErrorConsumer, nextParser)
		{
			if(expression == null) {
				throw new ArgumentNullException("expression");
			}
			this.expression = expression;
			this.@operator = @operator;
			this.operatorPriority = operatorPriority;
		}
		
		public override Parser ParsePlus(Location location)
		{
			return ParseBinaryOperator(location,
				(left, right) => PlusOperator(left, right, location), 1);
		}
		
		public override Parser ParseMinus(Location location)
		{
			return ParseBinaryOperator(location,
				(left, right) => MinusOperator(left, right, location), 1);
		}

		public override Parser ParseForwardSlash(Location location)
		{
			return ParseBinaryOperator(location,
				(left, right) => ForwardSlashOperator(left, right, location), 2);
		}
		
		public override Parser ParseAsterisk(Location location)
		{
			return ParseBinaryOperator(location,
				(left, right) => AsteriskOperator(left, right, location), 2);
		}
		
		Parser ParseBinaryOperator(Location location,
								   Func<Source, Source, Source> binaryOperator,
								   int operatorPriority)
		{
			return new RootParser(
				(e, c, ec, p) =>
					new ExpressionParser(e, operand => {
						if(@operator != null) {
							if(this.operatorPriority < operatorPriority) {
								return @operator(binaryOperator(expression, operand));
							} else {
								return binaryOperator(@operator(expression), operand);
							}
						} else {
							return binaryOperator(expression, operand);
						}
					}, operatorPriority, c, ec, p),
				operation => Consumer(operation),
				ErrorConsumer);
		}
		
		public override void OnDone()
		{
			if(@operator!= null) {
				Consumer(@operator(expression));
			} else {
				Consumer(expression);
			}
			base.OnDone();
		}
	}
}
