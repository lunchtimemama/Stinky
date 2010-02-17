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

using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Tests
{
	public class CompilerTests
	{
		protected static readonly Location Nowhere = new Location(null, 0, 0);

		protected static Reference Reference(string identifier)
		{
			return new Reference(identifier, Nowhere);
		}

		protected static Definition Definition(string identifier, Expression expression)
		{
			return new Definition(Reference(identifier), expression, Nowhere);
		}

		protected static NumberLiteral Number(double number)
		{
			return new NumberLiteral(number, Nowhere);
		}

		protected static StringLiteral String(string @string)
		{
			return new StringLiteral(@string, Nowhere);
		}

		protected static InterpolatedStringLiteral InterpolatedString(params Expression[] expressions)
		{
			return new InterpolatedStringLiteral(expressions, Nowhere);
		}

		protected static PlusOperator Plus(Expression left, Expression right)
		{
			return Plus(left, right, null);
		}

		protected static PlusOperator Plus(Expression left, Expression right, Type type)
		{
			return new PlusOperator(left, right, Nowhere, type);
		}

		protected static MinusOperator Minus(Expression left, Expression right)
		{
			return Minus(left, right, null);
		}

		protected static MinusOperator Minus(Expression left, Expression right, Type type)
		{
			return new MinusOperator(left, right, Nowhere, type);
		}

		protected static AsteriskOperator Asterisk(Expression left, Expression right)
		{
			return Asterisk(left, right, null);
		}

		protected static AsteriskOperator Asterisk(Expression left, Expression right, Type type)
		{
			return new AsteriskOperator(left, right, Nowhere, type);
		}

		protected static ForwardSlashOperator ForwardSlash(Expression left, Expression right)
		{
			return ForwardSlash(left, right, null);
		}

		protected static ForwardSlashOperator ForwardSlash(Expression left, Expression right, Type type)
		{
			return new ForwardSlashOperator(left, right, Nowhere, type);
		}
	}
}
