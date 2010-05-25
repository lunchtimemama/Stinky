// 
// HighlightingTests.cs
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
using Stinky.Compiler.Source.Highlighting;

namespace Stinky.Compiler.Tests
{
	[TestFixture]
	public class HighlightingTests
	{
		[Test]
		public void HighlightShortIdentifier()
		{
			AssertHighlights("f", new Highlight(Color.Other, 0));
		}

		[Test]
		public void HighlightLongIdentifier()
		{
			AssertIncrementalHighlights("foo", new Highlight(Color.Other, 0, 3));
		}

		static IEnumerable<Highlight> Incremental(IEnumerable<Highlight> highlights)
		{
			foreach(var highlight in highlights) {
				for(var i = highlight.Region.Location.Column; i < highlight.Region.Length; i++) {
					yield return new Highlight(
						highlight.Color,
						new Region(
							new Location(highlight.Region.Location.Source, highlight.Region.Location.Line, i), 1));
				}
			}
		}

		static void AssertIncrementalHighlights(IEnumerable<char> code, params Highlight[] highlights)
		{
			AssertHighlights(code, Incremental(highlights));
		}

		static void AssertHighlights(IEnumerable<char> code, params Highlight[] highlights)
		{
			AssertHighlights(code, (IEnumerable<Highlight>)highlights);
		}

		static void AssertHighlights(IEnumerable<char> code, IEnumerable<Highlight> highlights)
		{
			using(var testHighlighter = new TestHighlighter(highlights)) {
				var highlighter = new IncrementalHighlighter(testHighlighter);
				var column = 0;
				foreach(var character in code) {
					highlighter.Tokenize(new Character(character, new Location(null, 0, column)));
					column++;
				}
			}
		}

		class TestHighlighter : Highlighter, IDisposable
		{
			IEnumerator<Highlight> highlights;

			public TestHighlighter(IEnumerable<Highlight> highlights)
			{
				this.highlights = highlights.GetEnumerator();
			}

			public override void HighlightNumberLiteral(Region region)
			{
				AssertHighlight(region, Color.NumberLiteral);
			}

			public override void HighlightStringLiteral(Region region)
			{
				AssertHighlight(region, Color.StringLiteral);
			}

			public override void HighlightOther(Region region)
			{
				AssertHighlight(region, Color.Other);
			}

			void AssertHighlight(Region region, Color color)
			{
				if(!highlights.MoveNext()) {
					Assert.Fail("There are more highlights than expected.");
				}
				Assert.AreEqual(highlights.Current.Color, color, "Unexpected highlight type.");
				Assert.AreEqual(highlights.Current.Region, region, "Unexpected highlight region.");
			}

			public void Dispose()
			{
				if(highlights.MoveNext()) {
					Assert.Fail("There are fewer highlights than expected.");
				}
				highlights.Dispose();
			}
		}

		enum Color
		{
			Other,
			StringLiteral,
			NumberLiteral
		}

		struct Highlight
		{
			public readonly Color Color;
			public readonly Region Region;

			public Highlight(Color color, int column)
				: this(color, column, 1)
			{
			}

			public Highlight(Color color, int column, int length)
				: this(color, new Region(new Location(null, 0, column), length))
			{
			}

			public Highlight(Color color, Region region)
			{
				Color = color;
				Region = region;
			}
		}
	}
}
