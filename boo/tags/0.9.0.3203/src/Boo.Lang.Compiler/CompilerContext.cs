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

using System;
using System.Diagnostics;
using Boo.Lang;
using Boo.Lang.Compiler.Ast;
using Assembly = System.Reflection.Assembly;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler
{
	/// <summary>
	/// boo compilation context.
	/// </summary>
	public class CompilerContext
	{				
		public static CompilerContext Current
		{
			get { return _current != null ? _current.Value : null; }
		}

		[ThreadStatic] private static DynamicVariable<CompilerContext> _current;

		protected CompilerParameters _parameters;

		protected CompileUnit _unit;

		protected AssemblyCollection _assemblyReferences;

		protected CompilerErrorCollection _errors;
		
		protected CompilerWarningCollection _warnings;

		protected IDictionary<Type, object> _services = new Dictionary<Type, object>();
		
		protected TraceSwitch _traceSwitch;

		protected int _localIndex;
		
		protected Assembly _generatedAssembly;
		
		protected string _generatedAssemblyFileName;
		
		protected Hash _properties;
		
		public CompilerContext() : this(new CompileUnit())
		{
		}

		public CompilerContext(CompileUnit unit) : this(new CompilerParameters(), unit)
		{				
		}

		public CompilerContext(bool stdlib) : this(new CompilerParameters(stdlib), new CompileUnit())
		{
		}
		
		public CompilerContext(CompilerParameters options, CompileUnit unit)
		{
			if (null == options) throw new ArgumentNullException("options");
			if (null == unit) throw new ArgumentNullException("unit");

			_unit = unit;
			_errors = new CompilerErrorCollection();
			_warnings = new CompilerWarningCollection();
			_warnings.Adding += OnCompilerWarning;

			_assemblyReferences = options.References;
			_parameters = options;

			if (_parameters.Debug && !_parameters.Defines.ContainsKey("DEBUG"))
				_parameters.Defines.Add("DEBUG", null);

			_properties = new Hash();
			RegisterService<NameResolutionService>(new NameResolutionService(this));
		}

		public Hash Properties
		{
			get { return _properties; }
		}
		
		public string GeneratedAssemblyFileName
		{
			get { return _generatedAssemblyFileName; }
			
			set
			{
				if (null == value || 0 == value.Length)
				{
					throw new ArgumentException("GeneratedAssemblyFileName");
				}
				_generatedAssemblyFileName = value;
			}
		}
		
		public object this[object key]
		{
			get { return _properties[key]; }
			
			set { _properties[key] = value; }
		}

		public CompilerParameters Parameters
		{
			get { return _parameters; }
		}

		public AssemblyCollection References
		{
			get { return _assemblyReferences; }
		}

		public CompilerErrorCollection Errors
		{
			get { return _errors; }
		}
		
		public CompilerWarningCollection Warnings
		{
			get { return _warnings; }
		}

		public CompileUnit CompileUnit
		{
			get { return _unit; }
		}

		public TypeSystemServices TypeSystemServices
		{
			get { return GetService<TypeSystemServices>(); }
			set { RegisterService<TypeSystemServices>(value); }
		}

		public NameResolutionService NameResolutionService
		{
			get { return GetService<NameResolutionService>(); }
		}

		public TypeSystem.BooCodeBuilder CodeBuilder
		{
			get { return TypeSystemServices.CodeBuilder; }
		}
		
		public Assembly GeneratedAssembly
		{
			get { return _generatedAssembly; }
			
			set { _generatedAssembly = value; }
		}

		public int AllocIndex()
		{
			return ++_localIndex;
		}
		
		[Conditional("TRACE")]
		public void TraceEnter(string format, object param)
		{
			if (_parameters.TraceInfo)
			{
				Trace.WriteLine(string.Format(format, param));
				++Trace.IndentLevel;
			}
		}
		
		[Conditional("TRACE")]
		public void TraceLeave(string format, object param)
		{
			if (_parameters.TraceInfo)
			{
				--Trace.IndentLevel;
				Trace.WriteLine(string.Format(format, param));
			}
		}
		
		[Conditional("TRACE")]
		public void TraceInfo(string format, params object[] args)
		{			
			if (_parameters.TraceInfo)
			{
				Trace.WriteLine(string.Format(format, args));
			}			
		}
		
		[Conditional("TRACE")]
		public void TraceInfo(string message)
		{
			if (_parameters.TraceInfo)
			{
				Trace.WriteLine(message);
			}
		}
		
		[Conditional("TRACE")]
		public void TraceWarning(string message)
		{
			if (_parameters.TraceWarning)
			{
				Trace.WriteLine(message);
			}
		}

		[Conditional("TRACE")]
		public void TraceWarning(string message, params object[] args)
		{
			if (_parameters.TraceWarning)
			{
				Trace.WriteLine(string.Format(message, args));
			}
		}
		
		[Conditional("TRACE")]
		public void TraceVerbose(string format, params object[] args)
		{
			if (_parameters.TraceVerbose)
			{
				Trace.WriteLine(string.Format(format, args));
			}			
		}
		
		[Conditional("TRACE")]
		public void TraceVerbose(string format, object param1, object param2)
		{
			if (_parameters.TraceVerbose)
			{
				Trace.WriteLine(string.Format(format, param1, param2));
			}
		}
		
		[Conditional("TRACE")]
		public void TraceVerbose(string format, object param1, object param2, object param3)
		{
			if (_parameters.TraceVerbose)
			{
				Trace.WriteLine(string.Format(format, param1, param2, param3));
			}
		}
		
		[Conditional("TRACE")]
		public void TraceVerbose(string format, object param)
		{
			if (_parameters.TraceVerbose)
			{
				Trace.WriteLine(string.Format(format, param));
			}
		}
		
		[Conditional("TRACE")]
		public void TraceVerbose(string message)
		{
			if (_parameters.TraceVerbose)
			{
				Trace.WriteLine(message);
			}
		}	
		
		[Conditional("TRACE")]
		public void TraceError(string message, params object[] args)
		{
			if (_parameters.TraceError)
			{
				Trace.WriteLine(string.Format(message, args));
			}
		}
		
		[Conditional("TRACE")]
		public void TraceError(Exception x)
		{
			if (_parameters.TraceError)
			{
				Trace.WriteLine(x);
			}
		}

		/// <summary>
		/// Runs the given action with this context ensuring CompilerContext.Current
		/// returns the right context.
		/// </summary>
		/// <param name="action"></param>
		public void Run(System.Action<CompilerContext> action)
		{
			CurrentVariable().With(this, action);
		}

		private static DynamicVariable<CompilerContext> CurrentVariable()
		{
			if (null == _current) _current = new DynamicVariable<CompilerContext>();
			return _current;
		}

		public void RegisterService<T>(T service)
		{
			_services[typeof(T)] = service;
		}

		public T GetService<T>()
		{
			try
			{
				return (T)_services[typeof(T)];
			}
			catch (KeyNotFoundException)
			{
				throw new ArgumentException(string.Format("Requested compiler service not found: '{0}'.", typeof(T)));
			}
		}

		void OnCompilerWarning(object o, CompilerWarningEventArgs args)
		{
			CompilerWarning warning = args.Warning;
			if (Parameters.NoWarn || Parameters.DisabledWarnings.Contains(warning.Code))
				args.Cancel();
			if (Parameters.WarnAsError || Parameters.WarningsAsErrors.Contains(warning.Code)) {
				Errors.Add(new CompilerError(warning.Code, warning.LexicalInfo, warning.Message, null));
				args.Cancel();
			}
		}
	}
}

