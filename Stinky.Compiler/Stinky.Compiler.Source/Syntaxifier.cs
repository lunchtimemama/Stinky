// 
// SyntaxBuilder.cs
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

namespace Stinky.Compiler.Source
{
	using Source = Action<SourceVisitor>;
	using Syntax = Action<SyntaxVisitor>;
	using BinaryOperator = Func<Syntax, Syntax, Action<SyntaxError>, Syntax>;

	public class Syntaxifier : SourceVisitor
	{
		readonly Action<Syntax> consumer;
		readonly Action<SyntaxError, Location> errorConsumer;

		public Syntaxifier(Action<Syntax> consumer, Action<SyntaxError, Location> errorConsumer)
		{
			if(consumer == null) {
				throw new ArgumentNullException("consumer");
			}

			if(errorConsumer == null) {
				throw new ArgumentNullException("errorConsumer");
			}

			this.consumer = consumer;
			this.errorConsumer = errorConsumer;
		}

		public override void VisitStringLiteral(string @string, Region region)
		{
			consumer(v => v.VisitStringLiteral(@string, Error(region.Location)));
		}

		public override void VisitNumberLiteral(double number, Region region)
		{
			consumer(v => v.VisitNumberLiteral(number, Error(region.Location)));
		}

		public override void VisitDefinition(Source reference, Source expression, Location location)
		{
			VisitBinaryOperator(reference, expression, (l, r, e) => v => v.VisitDefinition(l, r, e), location);
		}

		public override void VisitReference(string identifier, Region region)
		{
			consumer(v => v.VisitReference(identifier, Error(region)));
		}

		public override void VisitPlusOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, (l, r, e) => v => v.VisitPlusOperator(l, r, e), location);
		}

		public override void VisitMinusOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, (l, r, e) => v => v.VisitMinusOperator(l, r, e), location);
		}

		public override void VisitAsteriskOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, (l, r, e) => v => v.VisitAsteriskOperator(l, r, e), location);
		}

		public override void VisitForwardSlashOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, (l, r, e) => v => v.VisitForwardSlashOperator(l, r, e), location);
		}

		void VisitBinaryOperator(Source left, Source right, BinaryOperator @operator, Location location)
		{
			left(Builder(leftSyntax => {
				right(Builder(rightSyntax => {
					consumer(@operator(leftSyntax, rightSyntax, Error(location)));
				}));
			}));
		}

		Syntaxifier Builder(Action<Syntax> consumer)
		{
			return new Syntaxifier(consumer, errorConsumer);
		}

		Action<SyntaxError> Error(Region region)
		{
			return Error(region.Location);
		}

		Action<SyntaxError> Error(Location location)
		{
			return error => errorConsumer(error, location);
		}
	}
}

