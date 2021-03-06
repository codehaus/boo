#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated documentation 
// files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, 
// publish, distribute, sublicense, and/or sell copies of the Software, 
// and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// Contact Information
//
// mailto:rbo@acm.org
#endregion

using System;
using System.Reflection;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.TypeSystem;

namespace Boo.Lang.Compiler.Steps
{
	public abstract class AbstractVisitorCompilerStep : Boo.Lang.Compiler.Ast.DepthFirstVisitor, ICompilerStep
	{
		protected CompilerContext _context;
		
		protected AbstractVisitorCompilerStep()
		{			
		}
		
		protected CompilerContext Context
		{
			get
			{
				return _context;
			}
		}
		
		protected Boo.Lang.Compiler.Ast.CompileUnit CompileUnit
		{
			get
			{
				return _context.CompileUnit;
			}
		}
		
		protected NameResolutionService NameResolutionService
		{
			get
			{
				return _context.NameResolutionService;
			}
		}
		
		protected CompilerParameters Parameters
		{
			get
			{
				return _context.Parameters;
			}
		}
		
		protected CompilerErrorCollection Errors
		{
			get
			{
				return _context.Errors;
			}
		}
		
		protected TypeSystemServices TypeSystemServices
		{
			get
			{
				return _context.TypeSystemServices;
			}
		}
		
		protected void Error(Expression node, CompilerError error)
		{
			Error(node);
			Errors.Add(error);
		}
		
		protected void Error(CompilerError error)
		{
			Errors.Add(error);
		}
		
		protected void Error(Expression node)
		{
			node.ExpressionType = TypeSystemServices.ErrorEntity;
		}

		protected void Bind(Node node, IEntity tag)
		{
			_context.TraceVerbose("{0}: Node '{1}' bound to '{2}'.", node.LexicalInfo, node, tag);
			node.Entity = tag;
		}		
		
		public IEntity GetEntity(Node node)
		{
			return TypeSystemServices.GetEntity(node);
		}
		
		protected void BindExpressionType(Expression node, IType type)
		{
			_context.TraceVerbose("{0}: Type of expression '{1}' bound to '{2}'.", node.LexicalInfo, node, type);  
			node.ExpressionType = type;
		}
		
		protected IType GetExpressionType(Expression node)
		{			
			IType type = node.ExpressionType;
			if (null == type)
			{
				throw CompilerErrorFactory.InvalidNode(node);
			}
			return type;
		}
		
		public IType GetType(Node node)
		{
			return TypeSystemServices.GetType(node);
		}		
		
		public LocalVariable GetLocalVariable(Node local)
		{
			return (LocalVariable)GetEntity(local);
		}
		
		protected Boo.Lang.Compiler.Ast.TypeReference CreateTypeReference(IType tag)
		{
			return TypeSystemServices.CreateTypeReference(tag);
		}
		
		protected void NotImplemented(Node node, string feature)
		{
			throw CompilerErrorFactory.NotImplemented(node, feature);
		}
		
		public virtual void Initialize(CompilerContext context)
		{
			if (null == context)
			{
				throw new ArgumentNullException("context");
			}
			_context = context;
		}
		
		public abstract void Run();
		
		public virtual void Dispose()
		{
			_context = null;
		}
	}
}
