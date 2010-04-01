// 
// CompilerTests.cs
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

using Stinky.Compiler.Parser.Tokenizer;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Tests
{
	public class CompilerTests
	{
		/*
		protected static Location Location(int column)
		{
			return new Location(null, 0, column);
		}

		protected static Region Token(int offset, int length)
		{
			return new Region(Location(offset), length);
		}

		protected static Reference Reference(string identifier)
		{
			return Reference(identifier, 0);
		}

		protected static Reference Reference(string identifier, int offset)
		{
			return new Reference(identifier, Token(offset, identifier.Length));
		}

		protected static Definition Definition(string identifier, Syntax expression)
		{
			return Definition(identifier, expression, 0);
		}

		protected static Definition Definition(string identifier, Syntax expression, int offset)
		{
			return new Definition(Reference(identifier, offset), expression,
				Token(offset, identifier.Length + 1));
		}

		protected static NumberLiteral Number(double number)
		{
			return Number(number, 0);
		}

		protected static NumberLiteral Number(double number, int offset)
		{
			return new NumberLiteral(number, Token(offset, number.ToString().Length));
		}

		protected static StringLiteral String(string @string)
		{
			return String(@string, 0, @string.Length + 2);
		}

		protected static StringLiteral String(string @string, int offset, int length)
		{
			return new StringLiteral(@string, Token(offset, length));
		}

		protected static InterpolatedStringLiteral InterpolatedString(params Syntax[] expressions)
		{
			return InterpolatedString(0, expressions);
		}

		protected static InterpolatedStringLiteral InterpolatedString(int offset, params Syntax[] expressions)
		{
			return new InterpolatedStringLiteral(expressions, Token(offset, 0));
		}

		protected static PlusOperator Plus(Syntax left, Syntax right)
		{
			return Plus(left, right, null);
		}

		protected static PlusOperator Plus(Syntax left, Syntax right, Type type)
		{
			return Plus(left, right, GetOperatorOffset(left), type);
		}

		protected static PlusOperator Plus(Syntax left, Syntax right, int offset)
		{
			return Plus(left, right, offset, null);
		}

		protected static PlusOperator Plus(Syntax left, Syntax right, int offset, Type type)
		{
			return new PlusOperator(left, right, Token(offset, 1), type);
		}

		protected static MinusOperator Minus(Syntax left, Syntax right)
		{
			return Minus(left, right, null);
		}

		protected static MinusOperator Minus(Syntax left, Syntax right, Type type)
		{
			return Minus(left, right, GetOperatorOffset(left), type);
		}

		protected static MinusOperator Minus(Syntax left, Syntax right, int offset)
		{
			return Minus(left, right, offset, null);
		}

		protected static MinusOperator Minus(Syntax left, Syntax right, int offset, Type type)
		{
			return new MinusOperator(left, right, Token(offset, 1), type);
		}

		protected static AsteriskOperator Asterisk(Syntax left, Syntax right)
		{
			return Asterisk(left, right, null);
		}

		protected static AsteriskOperator Asterisk(Syntax left, Syntax right, Type type)
		{
			return Asterisk(left, right, GetOperatorOffset(left), type);
		}

		protected static AsteriskOperator Asterisk(Syntax left, Syntax right, int offset)
		{
			return Asterisk(left, right, offset, null);
		}

		protected static AsteriskOperator Asterisk(Syntax left, Syntax right, int offset, Type type)
		{
			return new AsteriskOperator(left, right, Token(offset, 1), type);
		}

		protected static ForwardSlashOperator ForwardSlash(Syntax left, Syntax right)
		{
			return ForwardSlash(left, right, null);
		}

		protected static ForwardSlashOperator ForwardSlash(Syntax left, Syntax right, Type type)
		{
			return ForwardSlash(left, right, GetOperatorOffset(left), type);
		}

		protected static ForwardSlashOperator ForwardSlash(Syntax left, Syntax right, int offset)
		{
			return ForwardSlash(left, right, offset, null);
		}

		protected static ForwardSlashOperator ForwardSlash(Syntax left, Syntax right, int offset, Type type)
		{
			return new ForwardSlashOperator(left, right, Token(offset, 1), type);
		}

		static int GetOperatorOffset(Syntax left)
		{
			if(left == null) {
				return 0;
			}

			return left.Source.Location.Column + left.Source.Length;
		}
		*/
	}
}
