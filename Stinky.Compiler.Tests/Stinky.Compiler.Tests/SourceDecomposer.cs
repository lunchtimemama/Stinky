// 
// SourceHasher.cs
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

using Stinky.Compiler.Source;

namespace Stinky.Compiler.Tests
{
	using Source = Action<SourceVisitor>;

	public class SourceDecomposer : SourceVisitor
	{
		readonly Action<object> consumer;

		public SourceDecomposer(Action<object> consumer)
		{
			this.consumer = consumer;
		}

		public override void VisitStringLiteral(string @string, Region region)
		{
			consumer("string literal");
			consumer(@string);
			consumer(region);
		}

		public override void VisitNumberLiteral(double number, Region region)
		{
			consumer(number);
			consumer(region);
		}

		public override void VisitInterpolatedStringLiteral(IEnumerable<Source> sources, Region region)
		{
			consumer("interpolated string literal");
			consumer(region);
			foreach(var source in sources) {
				source(this);
			}
			consumer("end interpolated string literal");
		}

		public override void VisitReference(string identifier, Region region)
		{
			consumer("reference");
			consumer(identifier);
			consumer(region);
		}

		public override void VisitDefinition(Source reference, Source expression, Location location)
		{
			VisitBinaryOperator(reference, expression, location, "definition");
		}

		public override void VisitPlusOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, location, "plus");
		}

		public override void VisitMinusOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, location, "minus");
		}

		public override void VisitAsteriskOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, location, "asterisk");
		}

		public override void VisitForwardSlashOperator(Source left, Source right, Location location)
		{
			VisitBinaryOperator(left, right, location, "forward slash");
		}

		void VisitBinaryOperator(Source left, Source right, Location location, string name)
		{
			left(this);
			right(this);
			consumer(name);
			consumer(location);
		}
	}
}

