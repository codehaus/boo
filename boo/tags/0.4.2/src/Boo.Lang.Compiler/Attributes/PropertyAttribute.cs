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
using Boo.Lang.Compiler.Ast;

namespace Boo.Lang
{
	/// <summary>
	/// Creates a property over a field.
	/// </summary>
	public class PropertyAttribute : Boo.Lang.Compiler.AbstractAstAttribute
	{
		protected ReferenceExpression _propertyName;
		
		protected Expression _setPreCondition;

		public PropertyAttribute(ReferenceExpression propertyName) : this(propertyName, null)
		{
		}
		
		public PropertyAttribute(ReferenceExpression propertyName, Expression setPreCondition)
		{
			if (null == propertyName)
			{
				throw new ArgumentNullException("propertyName");
			}
			_propertyName = propertyName;
			_setPreCondition = setPreCondition;
		}
		
		override public void Apply(Node node)
		{
			Field f = node as Field;
			if (null == f)
			{
				InvalidNodeForAttribute("Field");
				return;
			}			
			
			Property p = new Property();
			if (f.IsStatic)
			{
				p.Modifiers |= TypeMemberModifiers.Static;
			}
			p.Name = _propertyName.Name;
			p.Type = f.Type;
			p.Getter = CreateGetter(f);
			p.Setter = CreateSetter(f);
			p.LexicalInfo = LexicalInfo;			
			((TypeDefinition)f.ParentNode).Members.Add(p);
		}
		
		virtual protected Method CreateGetter(Field f)
		{
			// get:
			//		return <f.Name>
			Method getter = new Method();
			getter.Name = "get";
			getter.Body.Statements.Add(
				new ReturnStatement(
					new ReferenceExpression(f.Name)
					)
				);
			return getter;
		}
		
		virtual protected Method CreateSetter(Field f)
		{
			Method setter = new Method();
			setter.Name = "set";
			
			if (null != _setPreCondition)
			{
				setter.Body.Add(
					new RaiseStatement(
						AstUtil.CreateMethodInvocationExpression(
							AstUtil.CreateReferenceExpression("System.ArgumentException"),
							new StringLiteralExpression(_propertyName.Name)),
						new StatementModifier(
							StatementModifierType.Unless,
							_setPreCondition)));						
			}
			setter.Body.Add(
				new BinaryExpression(
					BinaryOperatorType.Assign,
					new ReferenceExpression(f.Name),
					new ReferenceExpression("value")
					)
				);
			return setter;
		}
	}
}
