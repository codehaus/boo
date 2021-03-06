﻿#region license
// Copyright (c) 2004, Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace Boo.Lang
{
	using System;
	using System.IO;
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler.Ast.Visitors;
	
	/// <summary>
	/// assert condition[, message]
	/// </summary>
	public class AssertMacro : AbstractCompilerComponent, IAstMacro
	{		
		private static Expression ExceptionTypeReference = 
			AstUtil.CreateReferenceExpression("Boo.AssertionFailedException");
		
		public Statement Expand(MacroStatement macro)
		{
			int argc = macro.Arguments.Count;
			if (argc != 1 && argc != 2)
			{
				// TODO: localize this message
				throw new System.ArgumentException(
					Boo.ResourceManager.Format("AssertArgCount", argc));
			}
			
			// figure out the msg for the exception
			Expression condition = macro.Arguments[0];
			Expression message = (argc == 1) ?
				new StringLiteralExpression(
					condition.LexicalInfo, condition.ToString()) : 
				macro.Arguments[1];
				
			// unless <condition>:
			//     raise Boo.AssertionFailedException(<msg>)
			UnlessStatement stmt = new UnlessStatement(macro.LexicalInfo);
			stmt.Condition = condition;
			stmt.Block = new Block(macro.LexicalInfo);
			
			RaiseStatement raise = new RaiseStatement(macro.LexicalInfo);			
			raise.Exception = 
				AstUtil.CreateMethodInvocationExpression(ExceptionTypeReference, message);
			stmt.Block.Add(raise);
			
			return stmt;
		}
	}	
}
