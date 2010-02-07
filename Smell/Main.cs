// 
// Main.cs
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

using Stinky.Compiler;
using Stinky.Compiler.Parser;
using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;

namespace Smell
{
	class MainClass
	{
		static readonly Location Nowhere = new Location(null, 0, 0);
		public static void Main(string[] args)
		{
			var expressions = Compile("1+1");
			var eq = expressions[0].Equals(new InterpolatedStringLiteral(
					new Expression[] {
						new PlusOperator(
							new NumberLiteral(1, Nowhere),
							new PlusOperator(
								new NumberLiteral(1, Nowhere),
				                new InterpolatedStringLiteral(
									new Expression[] {
										new StringLiteral(" is the lonliest number, ", Nowhere),
										new Reference("name", Nowhere),
										new StringLiteral("!", Nowhere)
									}, Nowhere),
								Nowhere),
							Nowhere),
						new StringLiteral(" < she said it", Nowhere),
					}, Nowhere));
			Console.WriteLine(eq);
		}
		
		public static List<Expression> Compile(string source)
		{
			var expressions = new List<Expression>();
			var driver = new Driver((indentation, expression) => expressions.Add(expression));
			foreach(var character in source) {
				driver.OnCharacter(new Character(character, new Location(null, 0, 0)));
			}
			driver.OnDone();
			return expressions;
		}
	}
}

