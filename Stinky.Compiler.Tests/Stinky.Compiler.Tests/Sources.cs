// 
// SourceTester.cs
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

using NUnit.Framework;

using Stinky.Compiler.Source;

namespace Stinky.Compiler.Tests
{
	using Source = Action<SourceVisitor>;

	public class Sources
	{
		public static Source String(string @string)
		{
			return String(@string, 0);
		}

		public static Source String(string @string, int column)
		{
			return String(@string, new Location(null, 0, column));
		}

		public static Source String(string @string, Location location)
		{
			return String(@string, new Region(location, @string.Length + 2));
		}

		public static Source String(string @string, int column, int length)
		{
			return String(@string, new Region(new Location(null, 0, column), length));
		}

		public static Source String(string @string, Region region)
		{
			return visitor => visitor.VisitStringLiteral(@string, region);
		}

		public static Source Number(double number)
		{
			return Number(number, 0);
		}

		public static Source Number(double number, int column)
		{
			return Number(number, new Location(null, 0, column));
		}

		public static Source Number(double number, Location location)
		{
			return Number(number, new Region(location, number.ToString().Length));
		}

		public static Source Number(double number, Region region)
		{
			return visitor => visitor.VisitNumberLiteral(number, region);
		}

		public static Source InterpolatedString(params Source[] sources)
		{
			return InterpolatedString(0, 0, sources);
		}

		public static Source InterpolatedString(int column, int length, params Source[] sources)
		{
			return InterpolatedString(new Region(new Location(null, 0, column), length), sources);
		}

		public static Source InterpolatedString(Region region, params Source[] sources)
		{
			return visitor => visitor.VisitInterpolatedStringLiteral(sources, region);
		}

		public static Source Reference(string identifier)
		{
			return Reference(identifier, 0);
		}

		public static Source Reference(string identifier, int column)
		{
			return Reference(identifier, new Location(null, 0, column));
		}

		public static Source Reference(string identifier, Location location)
		{
			return Reference(identifier, new Region(location, identifier.Length));
		}

		public static Source Reference(string identifier, Region region)
		{
			return visitor => visitor.VisitReference(identifier, region);
		}

		public static Source Definition(Source reference, Source expression, int column)
		{
			return Definition(reference, expression, new Location(null, 0, column));
		}

		public static Source Definition(Source reference, Source expression, Location location)
		{
			return visitor => visitor.VisitDefinition(reference, expression, location);
		}

		public static Source Plus(Source left, Source right)
		{
			return Plus(left, right, 0);
		}

		public static Source Plus(Source left, Source right, int column)
		{
			return Plus(left, right, new Location(null, 0, column));
		}

		public static Source Plus(Source left, Source right, Location location)
		{
			return visitor => visitor.VisitPlusOperator(left, right, location);
		}

		public static Source Minus(Source left, Source right)
		{
			return Minus(left, right, 0);
		}

		public static Source Minus(Source left, Source right, int column)
		{
			return Minus(left, right, new Location(null, 0, column));
		}

		public static Source Minus(Source left, Source right, Location location)
		{
			return visitor => visitor.VisitMinusOperator(left, right, location);
		}

		public static Source Asterisk(Source left, Source right)
		{
			return Asterisk(left, right, 0);
		}

		public static Source Asterisk(Source left, Source right, int column)
		{
			return Asterisk(left, right, new Location(null, 0, column));
		}

		public static Source Asterisk(Source left, Source right, Location location)
		{
			return visitor => visitor.VisitAsteriskOperator(left, right, location);
		}

		public static Source ForwardSlash(Source left, Source right)
		{
			return ForwardSlash(left, right, 0);
		}

		public static Source ForwardSlash(Source left, Source right, int column)
		{
			return ForwardSlash(left, right, new Location(null, 0, column));
		}

		public static Source ForwardSlash(Source left, Source right, Location location)
		{
			return visitor => visitor.VisitForwardSlashOperator(left, right, location);
		}

		public static void AssertAreEqual(Source source1, Source source2)
		{
			TestEquality(source1, source2, error => Assert.Fail(error));
		}

		public static void AssertAreNotEqual(Source source1, Source source2)
		{
			string error = null;
			TestEquality(source1, source2, e => error = e);
			if(error == null) {
				Assert.Fail("Sources are equal.");
			}
		}

		public static void TestEquality(Source source1, Source source2, Action<string> consumer)
		{
			if(source1 == null) {
				if(source2 != null) {
					consumer("The first source is null but the second is not.");
				}
			} else if (source2 == null) {
				consumer("The second source is null but the first is not");
			} else {
				var components = new Queue<object>();
				source1(new SourceDecomposer(component => components.Enqueue(component)));
				source2(new SourceDecomposer(component => {
					if(components.Count == 0) {
						consumer("The second source is larger than the first.");
					} else {
						var c = components.Dequeue();
						if(!c.Equals(component)) {
							consumer(string.Format("The sources are inequal.\nExpected:{0}\nBut was: {1}", c, component));
						}
					}
				}));
				if(components.Count != 0) {
					consumer("The first source is larger than the second.");
				}
			}
		}
	}
}

