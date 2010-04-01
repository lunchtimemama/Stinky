// 
// Scope.cs
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

namespace Stinky.Compiler.Syntax
{
	using Syntax = Action<SyntaxVisitor>;

	public class Scope
	{
		Dictionary<string, Syntax> definitions = new Dictionary<string, Syntax>();

		public Syntax this[string identifier] {
			get {
				if(identifier == null) {
					throw new ArgumentNullException("identifier");
				}
				Syntax syntax;
				var success = TryGetValue(identifier, out syntax);
				if(!success) {
					throw new ArgumentNullException(
						string.Format("The identifier '{0}' does not exist in the scope.", identifier), "identifier");
				}
				return syntax;
			}
			set {
				if(identifier == null) {
					throw new ArgumentNullException("identifier");
				}
				if(value == null) {
					throw new ArgumentNullException("value");
				}
				if(definitions.ContainsKey(identifier)) {
					throw new ArgumentException(
						string.Format("The identifier '{0}' already exists in the scope.", identifier), "identifier");
				}
				definitions[identifier] = value;
			}
		}

		public bool TryGetValue(string identifier, out Syntax syntax)
		{
			return definitions.TryGetValue(identifier, out syntax);
		}
	}
}

