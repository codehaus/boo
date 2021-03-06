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
using System.Reflection;
using System.Runtime.CompilerServices;
using Boo.Lang.Compiler.Ast;
using Module=Boo.Lang.Compiler.Ast.Module;

namespace Boo.Lang.Compiler.MetaProgramming
{
	public class CompilationErrorsException : System.Exception
	{
		private CompilerErrorCollection _errors;

		public CompilationErrorsException(CompilerErrorCollection errors) : base(errors.ToString())
		{
			_errors = errors;
		}

		public CompilerErrorCollection Errors
		{
			get { return _errors;  }
		}
	}

	[CompilerGlobalScope]
	public sealed class Compilation
	{
		public static Type compile(ClassDefinition klass, params System.Reflection.Assembly[] references)
		{
			BooCompiler compiler = CreateLibraryCompiler(references);
			CompilerContext result = compiler.Run(CreateCompileUnit(klass));
			if (result.Errors.Count > 0) throw new CompilationErrorsException(result.Errors);
			return result.GeneratedAssembly.GetType(klass.Name);
		}

		private static BooCompiler CreateLibraryCompiler(Assembly[] references)
		{
			BooCompiler compiler = new BooCompiler();
			compiler.Parameters.OutputType = CompilerOutputType.Library;
			compiler.Parameters.Pipeline = new Boo.Lang.Compiler.Pipelines.CompileToMemory();
			compiler.Parameters.References.Extend(references);
			return compiler;
		}

		private static CompileUnit CreateCompileUnit(ClassDefinition klass)
		{
			return new CompileUnit(CreateModule(klass));
		}

		private static Module CreateModule(ClassDefinition klass)
		{
			Module module = new Module();
			module.Name = klass.Name;
			module.Members.Add(klass);
			return module;
		}
	}
}
