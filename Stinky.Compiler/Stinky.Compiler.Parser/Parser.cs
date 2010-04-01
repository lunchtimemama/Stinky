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

using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Parser
{
	using Source = Action<SourceVisitor>;

	public class Parser
	{
		protected readonly Action<Source> Consumer;
		protected readonly Action<CompilationError<ParseError>> ErrorConsumer;
		protected readonly Parser NextParser;
		
		public Parser(Action<Source> consumer, Action<CompilationError<ParseError>> errorConsumer)
			: this(consumer, errorConsumer, null)
		{
		}
		
		public Parser(Action<Source> consumer,
					  Action<CompilationError<ParseError>> errorConsumer,
					  Parser nextParser)
		{
			Consumer = consumer;
			ErrorConsumer = errorConsumer;
			NextParser = nextParser;
		}
		
		public virtual Parser ParseIdentifier(string identifier, Region region)
		{
			return ParseIdentifier(identifier, region, CreateError(region));
		}
		
		protected Parser ParseIdentifier(string identifier, Region region, CompilationError<ParseError> error)
		{
			return ParseToken(nextParser => nextParser.ParseStringLiteral(identifier, region, error), error);
		}
		
		public virtual Parser ParseColon(Location location)
		{
			return ParseColon(location, CreateError(location));
		}
		
		protected Parser ParseColon(Location location, CompilationError<ParseError> error)
		{
			return ParseToken(nextParser => nextParser.ParseColon(location, error), error);
		}

		public virtual Parser ParseNumberLiteral(double number, Region region)
		{
			return ParseNumberLiteral(number, region, CreateError(region));
		}

		protected Parser ParseNumberLiteral(double number, Region region, CompilationError<ParseError> error)
		{
			return ParseToken(nextParser => nextParser.ParseNumberLiteral(number, region, error), error);
		}
		
		public virtual Parser ParseStringLiteral(string @string, Region region)
		{
			return ParseStringLiteral(@string, region, CreateError(region));
		}
		
		protected Parser ParseStringLiteral(string @string, Region region, CompilationError<ParseError> error)
		{
			return ParseToken(nextParser => nextParser.ParseStringLiteral(@string, region, error), error);
		}
		
		public virtual Parser ParseInterpolatedStringLiteral(IEnumerable<Source> interpolatedExpressions,
															 Region region)
		{
			return ParseInterpolatedStringLiteral(interpolatedExpressions, region, CreateError(region));
		}
		
		protected Parser ParseInterpolatedStringLiteral(IEnumerable<Source> interpolatedExpressions,
														Region region,
														CompilationError<ParseError> error)
		{
			return ParseToken(
				nextParser => nextParser.ParseInterpolatedStringLiteral(interpolatedExpressions, region, error),
				error);
		}

		public virtual Parser ParsePlus(Location location)
		{
			return ParsePlus(location, CreateError(location));
		}
		
		protected Parser ParsePlus(Location location, CompilationError<ParseError> error)
		{
			return ParseToken(nextParser => nextParser.ParsePlus(location, error), error);
		}

		public virtual Parser ParseMinus(Location location)
		{
			return ParseMinus(location, CreateError(location));
		}
		
		protected Parser ParseMinus(Location location, CompilationError<ParseError> error)
		{
			return ParseToken(nextParser => nextParser.ParseMinus(location, error), error);
		}

		public virtual Parser ParseForwardSlash(Location location)
		{
			return ParseForwardSlash(location, CreateError(location));
		}
		
		protected Parser ParseForwardSlash(Location location, CompilationError<ParseError> error)
		{
			return ParseToken(nextParser => nextParser.ParseForwardSlash(location, error), error);
		}

		public virtual Parser ParseAsterisk(Location location)
		{
			return ParseAsterisk(location, CreateError(location));
		}
		
		protected Parser ParseAsterisk(Location location, CompilationError<ParseError> error)
		{
			return ParseToken(nextParser => nextParser.ParseAsterisk(location, error), error);
		}
		
		Parser ParseToken(Func<Parser, Parser> nextParser, CompilationError<ParseError> error)
		{
			if(NextParser != null) {
				return nextParser(NextParser);
			} else {
				ErrorConsumer(error);
				return null;
			}
		}

		CompilationError<ParseError> CreateError(Region region)
		{
			return CreateError(region.Location);
		}
		
		CompilationError<ParseError> CreateError(Location location)
		{
			return new CompilationError<ParseError>(location, Error);
		}
		
		protected virtual ParseError Error {
			get { return ParseError.UnknownError; }
		}
		
		public virtual void OnDone()
		{
		}

		protected static Source StringLiteral(string @string, Region region)
		{
			return visitor => visitor.VisitStringLiteral(@string, region);
		}

		protected static Source NumberLiteral(double number, Region region)
		{
			return visitor => visitor.VisitNumberLiteral(number, region);
		}

		protected static Source Reference(string identifier, Region region)
		{
			return visitor => visitor.VisitReference(identifier, region);
		}

		protected static Source Definition(Source reference, Source expression, Location location)
		{
			return visitor => visitor.VisitDefinition(reference, expression, location);
		}

		protected static Source InterpolatedStringLiteral(IEnumerable<Source> interpolatedExpressions, Region region)
		{
			return visitor => visitor.VisitInterpolatedStringLiteral(interpolatedExpressions, region);
		}

		protected static Source PlusOperator(Source left, Source right, Location location)
		{
			return visitor => visitor.VisitPlusOperator(left, right, location);
		}

		protected static Source MinusOperator(Source left, Source right, Location location)
		{
			return visitor => visitor.VisitMinusOperator(left, right, location);
		}

		protected static Source AsteriskOperator(Source left, Source right, Location location)
		{
			return visitor => visitor.VisitAsteriskOperator(left, right, location);
		}

		protected static Source ForwardSlashOperator(Source left, Source right, Location location)
		{
			return visitor => visitor.VisitForwardSlashOperator(left, right, location);
		}
	}
}
