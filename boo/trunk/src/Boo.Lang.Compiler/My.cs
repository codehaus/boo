﻿#region license
// Copyright (c) 2003, 2004, 2005 Rodrigo B. de Oliveira (rbo@acm.org)
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


using System;
using System.Runtime.CompilerServices;
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang.Compiler
{
	/// <summary>
	/// Idiomatic access to compiler services.
	/// 
	/// The service is automatically create if it's not yet available.
	/// 
	/// <example>
	/// <code>
	///	<![CDATA[
	/// 
	/// if (My<TypeSystemServices>.Instance.IsPrimitive(someType))
	///		return true;
	/// 
	/// ]]>
	/// </code>
	/// </example>
	/// </summary>
	/// <typeparam name="TService"></typeparam>
	public class My<TService> where TService: class 
	{
		public static TService Instance
		{
			get
			{
				CompilerContext context = CompilerContext.Current;
				if (null == context)
					throw new InvalidOperationException("CompilerContext is not available!");
				try
				{
					return context.GetService<TService>();
				}
				catch (ArgumentException)
				{
					TService newService = Activator.CreateInstance<TService>();
					context.RegisterService(newService);
					return newService;
				}
			}
		}
	}
	
	[CompilerGlobalScope]
	public static class My
	{
		[Meta]
		public static Expression my(ReferenceExpression typeReference)
		{
			GenericReferenceExpression myReference = new GenericReferenceExpression(typeReference.LexicalInfo);
			myReference.Target = new ReferenceExpression(typeReference.LexicalInfo, "My");
			myReference.GenericArguments.Add(TypeReference.Lift(typeReference));
			return new MemberReferenceExpression(
				typeReference.LexicalInfo,
				new MethodInvocationExpression(myReference),
				"Instance");
		}
	}
}
