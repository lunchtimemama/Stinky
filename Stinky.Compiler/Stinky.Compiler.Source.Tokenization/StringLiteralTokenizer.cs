// 
// StringLiteralTokenizer.cs
//  
// Author:
//       Scott Thomas <lunchtimemama@gmail.com>
// 
// Copyright (c) 2009 Scott Thomas
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
using System.Text;

using Stinky.Compiler.Source.Parsing;
using Stinky.Compiler.Syntax;

namespace Stinky.Compiler.Source.Tokenization
{
	using Source = Action<SourceVisitor>;
	using Token = Func<Parser, Parser>;

	public class StringLiteralTokenizer : SubTokenizer
	{
		readonly Location location;

		StringBuilder stringBuilder = new StringBuilder();
		Tokenizer interpolationTokenizer;
		List<Source> interpolatedExpressions;
		int startColumn;
		int endColumn;
		bool potentiallyInterpolated;
		bool potentiallyUninterpolated;
		bool escaped;
		
		public StringLiteralTokenizer(Location location, RootTokenizer rootTokenizer)
			: base(rootTokenizer)
		{
			this.location = location;
			this.startColumn = location.Column;
			rootTokenizer.OnToken(
				parser => parser.ParseStringLiteral(() => stringBuilder.ToString(), GetStringLiteralRegion()));
		}
		
		public override Tokenizer OnCharacter(Character character)
		{
			if(interpolationTokenizer != null) {
				interpolationTokenizer = interpolationTokenizer.OnCharacter(character);
			} else if(potentiallyInterpolated) {
				if(character.Char == '{') {
					stringBuilder.Append('{');
				} else {
					OnFirstInterpolatedCharacter(character);
				}
				potentiallyInterpolated = false;
			} else if(potentiallyUninterpolated) {
				if(character.Char == '}') {
					stringBuilder.Append('}');
					potentiallyUninterpolated = false;
					interpolationTokenizer.OnDone();
					interpolationTokenizer = null;
				} else {
					OnError(character.Location, TokenizationError.UnexpectedRightCurlyBracket);
				}
			} else if(escaped) {
				OnEspacedCharacter(character);
			} else {
				return OnRegularCharacter(character);
			}
			endColumn = character.Location.Column;
			return this;
		}
		
		Tokenizer OnRegularCharacter(Character character)
		{
			switch(character.Char) {
			case '{':
				potentiallyInterpolated = true;
				return this;
			case '}':
				potentiallyUninterpolated = true;
				return this;
			case '"':
				RootTokenizer.OnTokenReady();
				return RootTokenizer;
			case '\n':
				OnError(character.Location, TokenizationError.UnknownError);
				 return this;
			case '\\':
				escaped = true;
				return this;
			default:
				stringBuilder.Append(character.Char);
				return this;
			}
		}
		
		void OnEspacedCharacter(Character character)
		{
			escaped = false;
			switch(character.Char) {
			case '"':
				stringBuilder.Append('"');
				break;
			case '\\':
				stringBuilder.Append('\\');
				break;
			case 'n':
				stringBuilder.Append('\n');
				break;
			case 't':
				stringBuilder.Append('\t');
				break;
			default:
				OnError(character.Location, TokenizationError.UnknownError);
				break;
			}
		}

		void OnFirstInterpolatedCharacter(Character character)
		{
			if(interpolatedExpressions == null) {
				interpolatedExpressions = new List<Source>();
				RootTokenizer.OnToken(
					p => p.ParseInterpolatedStringLiteral(() => interpolatedExpressions, new Region(location, 0)));
			}

			if(stringBuilder.Length > 0) {
				ConsumeStringLiteral();
			}

			var context = new InterpolatedCompilationContext(this);
			var parser = context.CreateParser(
				expression => interpolatedExpressions.Add(expression)); // FIXME this error lambda

			Token token = null;
			interpolationTokenizer = context.CreateTokenizer(
				t => token = t,
				() => parser = token(parser),
				() => {
					parser.OnDone();
					interpolationTokenizer = null;
				},
				context);

			interpolationTokenizer.OnCharacter(character);
		}

		Region GetStringLiteralRegion()
		{
			return new Region(new Location(location.Source, location.Line, startColumn), endColumn - startColumn + 1);
		}

		void ConsumeStringLiteral()
		{
			var @string = stringBuilder.ToString();
			stringBuilder.Remove(0, stringBuilder.Length);
			var region = GetStringLiteralRegion();
			interpolatedExpressions.Add(visitor => visitor.VisitStringLiteral(@string, region));
		}
		
		public override void OnDone()
		{
			// TODO make sure there is a closing quote mark
			if(interpolationTokenizer != null) {
				interpolationTokenizer.OnDone();
			}

			if(interpolatedExpressions != null) {
				if(stringBuilder.Length > 0) {
					ConsumeStringLiteral();
				}
			}

			if(potentiallyInterpolated || (interpolationTokenizer != null && !potentiallyUninterpolated)) {
				OnError(location, TokenizationError.UnknownError);
			} else {
				base.OnDone();
			}
		}

		class InterpolatedCompilationContext : CompilationContext
		{
			readonly StringLiteralTokenizer interpolationTokenizer;

			public InterpolatedCompilationContext(StringLiteralTokenizer interpolationTokenizer)
			{
				this.interpolationTokenizer = interpolationTokenizer;
			}

			CompilationContext ParentContext {
				get { return interpolationTokenizer.RootTokenizer.CompilationContext; }
			}

			public override void HandleTokenError(CompilationError<TokenizationError> error)
			{
				if(error.Error == TokenizationError.UnexpectedRightCurlyBracket) {
					interpolationTokenizer.interpolationTokenizer.OnDone();
				} else {
					ParentContext.HandleTokenError(error);
				}
			}

			public override void HandleParseError(CompilationError<ParseError> error)
			{
				ParentContext.HandleParseError(error);
			}

			public override void HandleSyntaxError(CompilationError<SyntaxError> error)
			{
				ParentContext.HandleSyntaxError (error);
			}

			public override Parser CreateParser(Action<Action<SourceVisitor>> sourceConsumer)
			{
				return ParentContext.CreateParser(sourceConsumer);
			}

			public override Tokenizer CreateTokenizer (Action<Token> tokenConsumer,
			                                           Action tokenReady,
			                                           Action lineReady,
			                                           CompilationContext context)
			{
				return ParentContext.CreateTokenizer(tokenConsumer, tokenReady, lineReady, context);
			}
		}
	}
}