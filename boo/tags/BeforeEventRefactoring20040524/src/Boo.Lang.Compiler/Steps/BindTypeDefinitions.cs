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

namespace Boo.Lang.Compiler.Steps
{
	using System;
	using System.Collections;
	using Boo.Lang.Compiler.Ast;
	using Boo.Lang.Compiler;
	using Boo.Lang.Compiler.TypeSystem;
	
	[Serializable]
	public class BindTypeDefinitions : AbstractVisitorCompilerStep
	{
		override public void Run()
		{
			Visit(CompileUnit.Modules);
		}
		
		override public void OnModule(Boo.Lang.Compiler.Ast.Module node)
		{			
			Visit(node.Members);
		}
		
		override public void OnClassDefinition(ClassDefinition node)
		{	
			if (null == node.Entity)
			{				
				node.Entity = new InternalType(TypeSystemServices, node);				
			}			
			
			NormalizeVisibility(node);
			Visit(node.Members);
		}
		
		override public void OnInterfaceDefinition(InterfaceDefinition node)
		{
			if (null != node.Entity)
			{
				return;
			}			
			
			NormalizeVisibility(node);
			node.Entity = new InternalType(TypeSystemServices, node);
		}
		
		override public void OnEnumDefinition(EnumDefinition node)
		{
			if (null != node.Entity)
			{
				return;
			}
			
			NormalizeVisibility(node);
			node.Entity = new EnumType(TypeSystemServices, node);			
			
			long lastValue = 0;
			foreach (EnumMember member in node.Members)
			{
				if (null == member.Initializer)
				{
					member.Initializer = new IntegerLiteralExpression(lastValue);
				}
				lastValue = member.Initializer.Value + 1;
				
				if (null == member.Entity)
				{
					member.Entity = new InternalEnumMember(TypeSystemServices, member);
				}
			}
		}
		
		void NormalizeVisibility(TypeMember node)
		{
			if (!node.IsVisibilitySet)
			{
				node.Modifiers |= TypeMemberModifiers.Public;
			}
		}
		
		override public void OnMethod(Method method)
		{
		}
		
		override public void OnProperty(Property property)
		{
		}
		
		override public void OnField(Field field)
		{
		}
	}
}
