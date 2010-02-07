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
	public class Scope
	{
		readonly Scope parentScope;
		
		List<Expression> expressions = new List<Expression>();
		Dictionary<string, Definition> definitions = new Dictionary<string, Definition>();
		
		public Scope()
		{
		}
		
		public Scope(Scope parentScope)
		{
			this.parentScope = parentScope;
		}
		
		public void OnExpression(Expression expression)
		{
			Definition definition = expression as Definition;
			if(definition != null) {
				if(definitions.ContainsKey(definition.Reference.Identifier)) {
					throw new ArgumentException(
						"expression", "The scope already contains a definition for " + definition.Reference.Identifier);
				}
				definitions[definition.Reference.Identifier] = definition;
			}
			expressions.Add(expression);
		}
		
		public Definition GetDefinition(Reference reference)
		{
			Definition definition;
			if(definitions.TryGetValue(reference.Identifier, out definition)) {
				return definition;
			} else if(parentScope != null) {
				return parentScope.GetDefinition(reference);
			} else {
				return null;
			}
		}
	}
}
