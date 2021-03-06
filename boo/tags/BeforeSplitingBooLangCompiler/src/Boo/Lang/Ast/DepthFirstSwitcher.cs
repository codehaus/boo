﻿#region license
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

//
// DO NOT EDIT THIS FILE!
//
// This file was generated automatically by
// astgenerator.boo on 4/13/2004 3:42:21 PM
//

namespace Boo.Lang.Ast
{
	using System;
	
	public class DepthFirstSwitcher : IAstSwitcher
	{
		public bool Switch(Node node)
		{			
			if (null != node)
			{
				try
				{
					node.Switch(this);
					return true;
				}
				catch (Boo.Lang.Compiler.CompilerError)
				{
					throw;
				}
				catch (Exception error)
				{
					throw Boo.Lang.Compiler.CompilerErrorFactory.InternalError(node, error);
				}
			}
			return false;
		}
		
		public bool Switch(NodeCollection collection, NodeType nodeType)
		{
			if (null != collection)
			{
				foreach (Node node in collection.ToArray())
				{
					if (node.NodeType == nodeType)
					{
						Switch(node);
					}
				}
				return true;
			}
			return false;
		}
		
		public bool Switch(NodeCollection collection)
		{
			if (null != collection)
			{
				foreach (Node node in collection.ToArray())
				{
					Switch(node);
				}
				return true;
			}
			return false;
		}
		
		public virtual void OnCompileUnit(Boo.Lang.Ast.CompileUnit node)
		{				
			if (EnterCompileUnit(node))
			{
				Switch(node.Modules);
				LeaveCompileUnit(node);
			}
		}
			
		public virtual bool EnterCompileUnit(Boo.Lang.Ast.CompileUnit node)
		{
			return true;
		}
		
		public virtual void LeaveCompileUnit(Boo.Lang.Ast.CompileUnit node)
		{
		}
			
		public virtual void OnSimpleTypeReference(Boo.Lang.Ast.SimpleTypeReference node)
		{
		}
			
		public virtual void OnTupleTypeReference(Boo.Lang.Ast.TupleTypeReference node)
		{				
			if (EnterTupleTypeReference(node))
			{
				Switch(node.ElementType);
				LeaveTupleTypeReference(node);
			}
		}
			
		public virtual bool EnterTupleTypeReference(Boo.Lang.Ast.TupleTypeReference node)
		{
			return true;
		}
		
		public virtual void LeaveTupleTypeReference(Boo.Lang.Ast.TupleTypeReference node)
		{
		}
			
		public virtual void OnNamespaceDeclaration(Boo.Lang.Ast.NamespaceDeclaration node)
		{
		}
			
		public virtual void OnImport(Boo.Lang.Ast.Import node)
		{				
			if (EnterImport(node))
			{
				Switch(node.AssemblyReference);
				Switch(node.Alias);
				LeaveImport(node);
			}
		}
			
		public virtual bool EnterImport(Boo.Lang.Ast.Import node)
		{
			return true;
		}
		
		public virtual void LeaveImport(Boo.Lang.Ast.Import node)
		{
		}
			
		public virtual void OnModule(Boo.Lang.Ast.Module node)
		{				
			if (EnterModule(node))
			{
				Switch(node.Attributes);
				Switch(node.Members);
				Switch(node.BaseTypes);
				Switch(node.Namespace);
				Switch(node.Imports);
				Switch(node.Globals);
				LeaveModule(node);
			}
		}
			
		public virtual bool EnterModule(Boo.Lang.Ast.Module node)
		{
			return true;
		}
		
		public virtual void LeaveModule(Boo.Lang.Ast.Module node)
		{
		}
			
		public virtual void OnClassDefinition(Boo.Lang.Ast.ClassDefinition node)
		{				
			if (EnterClassDefinition(node))
			{
				Switch(node.Attributes);
				Switch(node.Members);
				Switch(node.BaseTypes);
				LeaveClassDefinition(node);
			}
		}
			
		public virtual bool EnterClassDefinition(Boo.Lang.Ast.ClassDefinition node)
		{
			return true;
		}
		
		public virtual void LeaveClassDefinition(Boo.Lang.Ast.ClassDefinition node)
		{
		}
			
		public virtual void OnInterfaceDefinition(Boo.Lang.Ast.InterfaceDefinition node)
		{				
			if (EnterInterfaceDefinition(node))
			{
				Switch(node.Attributes);
				Switch(node.Members);
				Switch(node.BaseTypes);
				LeaveInterfaceDefinition(node);
			}
		}
			
		public virtual bool EnterInterfaceDefinition(Boo.Lang.Ast.InterfaceDefinition node)
		{
			return true;
		}
		
		public virtual void LeaveInterfaceDefinition(Boo.Lang.Ast.InterfaceDefinition node)
		{
		}
			
		public virtual void OnEnumDefinition(Boo.Lang.Ast.EnumDefinition node)
		{				
			if (EnterEnumDefinition(node))
			{
				Switch(node.Attributes);
				Switch(node.Members);
				Switch(node.BaseTypes);
				LeaveEnumDefinition(node);
			}
		}
			
		public virtual bool EnterEnumDefinition(Boo.Lang.Ast.EnumDefinition node)
		{
			return true;
		}
		
		public virtual void LeaveEnumDefinition(Boo.Lang.Ast.EnumDefinition node)
		{
		}
			
		public virtual void OnEnumMember(Boo.Lang.Ast.EnumMember node)
		{				
			if (EnterEnumMember(node))
			{
				Switch(node.Attributes);
				Switch(node.Initializer);
				LeaveEnumMember(node);
			}
		}
			
		public virtual bool EnterEnumMember(Boo.Lang.Ast.EnumMember node)
		{
			return true;
		}
		
		public virtual void LeaveEnumMember(Boo.Lang.Ast.EnumMember node)
		{
		}
			
		public virtual void OnField(Boo.Lang.Ast.Field node)
		{				
			if (EnterField(node))
			{
				Switch(node.Attributes);
				Switch(node.Type);
				Switch(node.Initializer);
				LeaveField(node);
			}
		}
			
		public virtual bool EnterField(Boo.Lang.Ast.Field node)
		{
			return true;
		}
		
		public virtual void LeaveField(Boo.Lang.Ast.Field node)
		{
		}
			
		public virtual void OnProperty(Boo.Lang.Ast.Property node)
		{				
			if (EnterProperty(node))
			{
				Switch(node.Attributes);
				Switch(node.Parameters);
				Switch(node.Getter);
				Switch(node.Setter);
				Switch(node.Type);
				LeaveProperty(node);
			}
		}
			
		public virtual bool EnterProperty(Boo.Lang.Ast.Property node)
		{
			return true;
		}
		
		public virtual void LeaveProperty(Boo.Lang.Ast.Property node)
		{
		}
			
		public virtual void OnLocal(Boo.Lang.Ast.Local node)
		{
		}
			
		public virtual void OnMethod(Boo.Lang.Ast.Method node)
		{				
			if (EnterMethod(node))
			{
				Switch(node.Attributes);
				Switch(node.Parameters);
				Switch(node.ReturnType);
				Switch(node.ReturnTypeAttributes);
				Switch(node.Body);
				Switch(node.Locals);
				LeaveMethod(node);
			}
		}
			
		public virtual bool EnterMethod(Boo.Lang.Ast.Method node)
		{
			return true;
		}
		
		public virtual void LeaveMethod(Boo.Lang.Ast.Method node)
		{
		}
			
		public virtual void OnConstructor(Boo.Lang.Ast.Constructor node)
		{				
			if (EnterConstructor(node))
			{
				Switch(node.Attributes);
				Switch(node.Parameters);
				Switch(node.ReturnType);
				Switch(node.ReturnTypeAttributes);
				Switch(node.Body);
				Switch(node.Locals);
				LeaveConstructor(node);
			}
		}
			
		public virtual bool EnterConstructor(Boo.Lang.Ast.Constructor node)
		{
			return true;
		}
		
		public virtual void LeaveConstructor(Boo.Lang.Ast.Constructor node)
		{
		}
			
		public virtual void OnParameterDeclaration(Boo.Lang.Ast.ParameterDeclaration node)
		{				
			if (EnterParameterDeclaration(node))
			{
				Switch(node.Type);
				Switch(node.Attributes);
				LeaveParameterDeclaration(node);
			}
		}
			
		public virtual bool EnterParameterDeclaration(Boo.Lang.Ast.ParameterDeclaration node)
		{
			return true;
		}
		
		public virtual void LeaveParameterDeclaration(Boo.Lang.Ast.ParameterDeclaration node)
		{
		}
			
		public virtual void OnDeclaration(Boo.Lang.Ast.Declaration node)
		{				
			if (EnterDeclaration(node))
			{
				Switch(node.Type);
				LeaveDeclaration(node);
			}
		}
			
		public virtual bool EnterDeclaration(Boo.Lang.Ast.Declaration node)
		{
			return true;
		}
		
		public virtual void LeaveDeclaration(Boo.Lang.Ast.Declaration node)
		{
		}
			
		public virtual void OnAttribute(Boo.Lang.Ast.Attribute node)
		{				
			if (EnterAttribute(node))
			{
				Switch(node.Arguments);
				Switch(node.NamedArguments);
				LeaveAttribute(node);
			}
		}
			
		public virtual bool EnterAttribute(Boo.Lang.Ast.Attribute node)
		{
			return true;
		}
		
		public virtual void LeaveAttribute(Boo.Lang.Ast.Attribute node)
		{
		}
			
		public virtual void OnStatementModifier(Boo.Lang.Ast.StatementModifier node)
		{				
			if (EnterStatementModifier(node))
			{
				Switch(node.Condition);
				LeaveStatementModifier(node);
			}
		}
			
		public virtual bool EnterStatementModifier(Boo.Lang.Ast.StatementModifier node)
		{
			return true;
		}
		
		public virtual void LeaveStatementModifier(Boo.Lang.Ast.StatementModifier node)
		{
		}
			
		public virtual void OnBlock(Boo.Lang.Ast.Block node)
		{				
			if (EnterBlock(node))
			{
				Switch(node.Modifier);
				Switch(node.Statements);
				LeaveBlock(node);
			}
		}
			
		public virtual bool EnterBlock(Boo.Lang.Ast.Block node)
		{
			return true;
		}
		
		public virtual void LeaveBlock(Boo.Lang.Ast.Block node)
		{
		}
			
		public virtual void OnDeclarationStatement(Boo.Lang.Ast.DeclarationStatement node)
		{				
			if (EnterDeclarationStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Declaration);
				Switch(node.Initializer);
				LeaveDeclarationStatement(node);
			}
		}
			
		public virtual bool EnterDeclarationStatement(Boo.Lang.Ast.DeclarationStatement node)
		{
			return true;
		}
		
		public virtual void LeaveDeclarationStatement(Boo.Lang.Ast.DeclarationStatement node)
		{
		}
			
		public virtual void OnAssertStatement(Boo.Lang.Ast.AssertStatement node)
		{				
			if (EnterAssertStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Condition);
				Switch(node.Message);
				LeaveAssertStatement(node);
			}
		}
			
		public virtual bool EnterAssertStatement(Boo.Lang.Ast.AssertStatement node)
		{
			return true;
		}
		
		public virtual void LeaveAssertStatement(Boo.Lang.Ast.AssertStatement node)
		{
		}
			
		public virtual void OnMacroStatement(Boo.Lang.Ast.MacroStatement node)
		{				
			if (EnterMacroStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Arguments);
				Switch(node.Block);
				LeaveMacroStatement(node);
			}
		}
			
		public virtual bool EnterMacroStatement(Boo.Lang.Ast.MacroStatement node)
		{
			return true;
		}
		
		public virtual void LeaveMacroStatement(Boo.Lang.Ast.MacroStatement node)
		{
		}
			
		public virtual void OnTryStatement(Boo.Lang.Ast.TryStatement node)
		{				
			if (EnterTryStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.ProtectedBlock);
				Switch(node.ExceptionHandlers);
				Switch(node.SuccessBlock);
				Switch(node.EnsureBlock);
				LeaveTryStatement(node);
			}
		}
			
		public virtual bool EnterTryStatement(Boo.Lang.Ast.TryStatement node)
		{
			return true;
		}
		
		public virtual void LeaveTryStatement(Boo.Lang.Ast.TryStatement node)
		{
		}
			
		public virtual void OnExceptionHandler(Boo.Lang.Ast.ExceptionHandler node)
		{				
			if (EnterExceptionHandler(node))
			{
				Switch(node.Declaration);
				Switch(node.Block);
				LeaveExceptionHandler(node);
			}
		}
			
		public virtual bool EnterExceptionHandler(Boo.Lang.Ast.ExceptionHandler node)
		{
			return true;
		}
		
		public virtual void LeaveExceptionHandler(Boo.Lang.Ast.ExceptionHandler node)
		{
		}
			
		public virtual void OnIfStatement(Boo.Lang.Ast.IfStatement node)
		{				
			if (EnterIfStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Condition);
				Switch(node.TrueBlock);
				Switch(node.FalseBlock);
				LeaveIfStatement(node);
			}
		}
			
		public virtual bool EnterIfStatement(Boo.Lang.Ast.IfStatement node)
		{
			return true;
		}
		
		public virtual void LeaveIfStatement(Boo.Lang.Ast.IfStatement node)
		{
		}
			
		public virtual void OnUnlessStatement(Boo.Lang.Ast.UnlessStatement node)
		{				
			if (EnterUnlessStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Condition);
				Switch(node.Block);
				LeaveUnlessStatement(node);
			}
		}
			
		public virtual bool EnterUnlessStatement(Boo.Lang.Ast.UnlessStatement node)
		{
			return true;
		}
		
		public virtual void LeaveUnlessStatement(Boo.Lang.Ast.UnlessStatement node)
		{
		}
			
		public virtual void OnForStatement(Boo.Lang.Ast.ForStatement node)
		{				
			if (EnterForStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Declarations);
				Switch(node.Iterator);
				Switch(node.Block);
				LeaveForStatement(node);
			}
		}
			
		public virtual bool EnterForStatement(Boo.Lang.Ast.ForStatement node)
		{
			return true;
		}
		
		public virtual void LeaveForStatement(Boo.Lang.Ast.ForStatement node)
		{
		}
			
		public virtual void OnWhileStatement(Boo.Lang.Ast.WhileStatement node)
		{				
			if (EnterWhileStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Condition);
				Switch(node.Block);
				LeaveWhileStatement(node);
			}
		}
			
		public virtual bool EnterWhileStatement(Boo.Lang.Ast.WhileStatement node)
		{
			return true;
		}
		
		public virtual void LeaveWhileStatement(Boo.Lang.Ast.WhileStatement node)
		{
		}
			
		public virtual void OnGivenStatement(Boo.Lang.Ast.GivenStatement node)
		{				
			if (EnterGivenStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Expression);
				Switch(node.WhenClauses);
				Switch(node.OtherwiseBlock);
				LeaveGivenStatement(node);
			}
		}
			
		public virtual bool EnterGivenStatement(Boo.Lang.Ast.GivenStatement node)
		{
			return true;
		}
		
		public virtual void LeaveGivenStatement(Boo.Lang.Ast.GivenStatement node)
		{
		}
			
		public virtual void OnWhenClause(Boo.Lang.Ast.WhenClause node)
		{				
			if (EnterWhenClause(node))
			{
				Switch(node.Condition);
				Switch(node.Block);
				LeaveWhenClause(node);
			}
		}
			
		public virtual bool EnterWhenClause(Boo.Lang.Ast.WhenClause node)
		{
			return true;
		}
		
		public virtual void LeaveWhenClause(Boo.Lang.Ast.WhenClause node)
		{
		}
			
		public virtual void OnBreakStatement(Boo.Lang.Ast.BreakStatement node)
		{				
			if (EnterBreakStatement(node))
			{
				Switch(node.Modifier);
				LeaveBreakStatement(node);
			}
		}
			
		public virtual bool EnterBreakStatement(Boo.Lang.Ast.BreakStatement node)
		{
			return true;
		}
		
		public virtual void LeaveBreakStatement(Boo.Lang.Ast.BreakStatement node)
		{
		}
			
		public virtual void OnContinueStatement(Boo.Lang.Ast.ContinueStatement node)
		{				
			if (EnterContinueStatement(node))
			{
				Switch(node.Modifier);
				LeaveContinueStatement(node);
			}
		}
			
		public virtual bool EnterContinueStatement(Boo.Lang.Ast.ContinueStatement node)
		{
			return true;
		}
		
		public virtual void LeaveContinueStatement(Boo.Lang.Ast.ContinueStatement node)
		{
		}
			
		public virtual void OnRetryStatement(Boo.Lang.Ast.RetryStatement node)
		{				
			if (EnterRetryStatement(node))
			{
				Switch(node.Modifier);
				LeaveRetryStatement(node);
			}
		}
			
		public virtual bool EnterRetryStatement(Boo.Lang.Ast.RetryStatement node)
		{
			return true;
		}
		
		public virtual void LeaveRetryStatement(Boo.Lang.Ast.RetryStatement node)
		{
		}
			
		public virtual void OnReturnStatement(Boo.Lang.Ast.ReturnStatement node)
		{				
			if (EnterReturnStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Expression);
				LeaveReturnStatement(node);
			}
		}
			
		public virtual bool EnterReturnStatement(Boo.Lang.Ast.ReturnStatement node)
		{
			return true;
		}
		
		public virtual void LeaveReturnStatement(Boo.Lang.Ast.ReturnStatement node)
		{
		}
			
		public virtual void OnYieldStatement(Boo.Lang.Ast.YieldStatement node)
		{				
			if (EnterYieldStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Expression);
				LeaveYieldStatement(node);
			}
		}
			
		public virtual bool EnterYieldStatement(Boo.Lang.Ast.YieldStatement node)
		{
			return true;
		}
		
		public virtual void LeaveYieldStatement(Boo.Lang.Ast.YieldStatement node)
		{
		}
			
		public virtual void OnRaiseStatement(Boo.Lang.Ast.RaiseStatement node)
		{				
			if (EnterRaiseStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Exception);
				LeaveRaiseStatement(node);
			}
		}
			
		public virtual bool EnterRaiseStatement(Boo.Lang.Ast.RaiseStatement node)
		{
			return true;
		}
		
		public virtual void LeaveRaiseStatement(Boo.Lang.Ast.RaiseStatement node)
		{
		}
			
		public virtual void OnUnpackStatement(Boo.Lang.Ast.UnpackStatement node)
		{				
			if (EnterUnpackStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Declarations);
				Switch(node.Expression);
				LeaveUnpackStatement(node);
			}
		}
			
		public virtual bool EnterUnpackStatement(Boo.Lang.Ast.UnpackStatement node)
		{
			return true;
		}
		
		public virtual void LeaveUnpackStatement(Boo.Lang.Ast.UnpackStatement node)
		{
		}
			
		public virtual void OnExpressionStatement(Boo.Lang.Ast.ExpressionStatement node)
		{				
			if (EnterExpressionStatement(node))
			{
				Switch(node.Modifier);
				Switch(node.Expression);
				LeaveExpressionStatement(node);
			}
		}
			
		public virtual bool EnterExpressionStatement(Boo.Lang.Ast.ExpressionStatement node)
		{
			return true;
		}
		
		public virtual void LeaveExpressionStatement(Boo.Lang.Ast.ExpressionStatement node)
		{
		}
			
		public virtual void OnOmittedExpression(Boo.Lang.Ast.OmittedExpression node)
		{
		}
			
		public virtual void OnExpressionPair(Boo.Lang.Ast.ExpressionPair node)
		{				
			if (EnterExpressionPair(node))
			{
				Switch(node.First);
				Switch(node.Second);
				LeaveExpressionPair(node);
			}
		}
			
		public virtual bool EnterExpressionPair(Boo.Lang.Ast.ExpressionPair node)
		{
			return true;
		}
		
		public virtual void LeaveExpressionPair(Boo.Lang.Ast.ExpressionPair node)
		{
		}
			
		public virtual void OnMethodInvocationExpression(Boo.Lang.Ast.MethodInvocationExpression node)
		{				
			if (EnterMethodInvocationExpression(node))
			{
				Switch(node.Target);
				Switch(node.Arguments);
				Switch(node.NamedArguments);
				LeaveMethodInvocationExpression(node);
			}
		}
			
		public virtual bool EnterMethodInvocationExpression(Boo.Lang.Ast.MethodInvocationExpression node)
		{
			return true;
		}
		
		public virtual void LeaveMethodInvocationExpression(Boo.Lang.Ast.MethodInvocationExpression node)
		{
		}
			
		public virtual void OnUnaryExpression(Boo.Lang.Ast.UnaryExpression node)
		{				
			if (EnterUnaryExpression(node))
			{
				Switch(node.Operand);
				LeaveUnaryExpression(node);
			}
		}
			
		public virtual bool EnterUnaryExpression(Boo.Lang.Ast.UnaryExpression node)
		{
			return true;
		}
		
		public virtual void LeaveUnaryExpression(Boo.Lang.Ast.UnaryExpression node)
		{
		}
			
		public virtual void OnBinaryExpression(Boo.Lang.Ast.BinaryExpression node)
		{				
			if (EnterBinaryExpression(node))
			{
				Switch(node.Left);
				Switch(node.Right);
				LeaveBinaryExpression(node);
			}
		}
			
		public virtual bool EnterBinaryExpression(Boo.Lang.Ast.BinaryExpression node)
		{
			return true;
		}
		
		public virtual void LeaveBinaryExpression(Boo.Lang.Ast.BinaryExpression node)
		{
		}
			
		public virtual void OnTernaryExpression(Boo.Lang.Ast.TernaryExpression node)
		{				
			if (EnterTernaryExpression(node))
			{
				Switch(node.Condition);
				Switch(node.TrueExpression);
				Switch(node.FalseExpression);
				LeaveTernaryExpression(node);
			}
		}
			
		public virtual bool EnterTernaryExpression(Boo.Lang.Ast.TernaryExpression node)
		{
			return true;
		}
		
		public virtual void LeaveTernaryExpression(Boo.Lang.Ast.TernaryExpression node)
		{
		}
			
		public virtual void OnReferenceExpression(Boo.Lang.Ast.ReferenceExpression node)
		{
		}
			
		public virtual void OnMemberReferenceExpression(Boo.Lang.Ast.MemberReferenceExpression node)
		{				
			if (EnterMemberReferenceExpression(node))
			{
				Switch(node.Target);
				LeaveMemberReferenceExpression(node);
			}
		}
			
		public virtual bool EnterMemberReferenceExpression(Boo.Lang.Ast.MemberReferenceExpression node)
		{
			return true;
		}
		
		public virtual void LeaveMemberReferenceExpression(Boo.Lang.Ast.MemberReferenceExpression node)
		{
		}
			
		public virtual void OnStringLiteralExpression(Boo.Lang.Ast.StringLiteralExpression node)
		{
		}
			
		public virtual void OnTimeSpanLiteralExpression(Boo.Lang.Ast.TimeSpanLiteralExpression node)
		{
		}
			
		public virtual void OnIntegerLiteralExpression(Boo.Lang.Ast.IntegerLiteralExpression node)
		{
		}
			
		public virtual void OnDoubleLiteralExpression(Boo.Lang.Ast.DoubleLiteralExpression node)
		{
		}
			
		public virtual void OnNullLiteralExpression(Boo.Lang.Ast.NullLiteralExpression node)
		{
		}
			
		public virtual void OnSelfLiteralExpression(Boo.Lang.Ast.SelfLiteralExpression node)
		{
		}
			
		public virtual void OnSuperLiteralExpression(Boo.Lang.Ast.SuperLiteralExpression node)
		{
		}
			
		public virtual void OnBoolLiteralExpression(Boo.Lang.Ast.BoolLiteralExpression node)
		{
		}
			
		public virtual void OnRELiteralExpression(Boo.Lang.Ast.RELiteralExpression node)
		{
		}
			
		public virtual void OnStringFormattingExpression(Boo.Lang.Ast.StringFormattingExpression node)
		{				
			if (EnterStringFormattingExpression(node))
			{
				Switch(node.Arguments);
				LeaveStringFormattingExpression(node);
			}
		}
			
		public virtual bool EnterStringFormattingExpression(Boo.Lang.Ast.StringFormattingExpression node)
		{
			return true;
		}
		
		public virtual void LeaveStringFormattingExpression(Boo.Lang.Ast.StringFormattingExpression node)
		{
		}
			
		public virtual void OnHashLiteralExpression(Boo.Lang.Ast.HashLiteralExpression node)
		{				
			if (EnterHashLiteralExpression(node))
			{
				Switch(node.Items);
				LeaveHashLiteralExpression(node);
			}
		}
			
		public virtual bool EnterHashLiteralExpression(Boo.Lang.Ast.HashLiteralExpression node)
		{
			return true;
		}
		
		public virtual void LeaveHashLiteralExpression(Boo.Lang.Ast.HashLiteralExpression node)
		{
		}
			
		public virtual void OnListLiteralExpression(Boo.Lang.Ast.ListLiteralExpression node)
		{				
			if (EnterListLiteralExpression(node))
			{
				Switch(node.Items);
				LeaveListLiteralExpression(node);
			}
		}
			
		public virtual bool EnterListLiteralExpression(Boo.Lang.Ast.ListLiteralExpression node)
		{
			return true;
		}
		
		public virtual void LeaveListLiteralExpression(Boo.Lang.Ast.ListLiteralExpression node)
		{
		}
			
		public virtual void OnTupleLiteralExpression(Boo.Lang.Ast.TupleLiteralExpression node)
		{				
			if (EnterTupleLiteralExpression(node))
			{
				Switch(node.Items);
				LeaveTupleLiteralExpression(node);
			}
		}
			
		public virtual bool EnterTupleLiteralExpression(Boo.Lang.Ast.TupleLiteralExpression node)
		{
			return true;
		}
		
		public virtual void LeaveTupleLiteralExpression(Boo.Lang.Ast.TupleLiteralExpression node)
		{
		}
			
		public virtual void OnIteratorExpression(Boo.Lang.Ast.IteratorExpression node)
		{				
			if (EnterIteratorExpression(node))
			{
				Switch(node.Expression);
				Switch(node.Declarations);
				Switch(node.Iterator);
				Switch(node.Filter);
				LeaveIteratorExpression(node);
			}
		}
			
		public virtual bool EnterIteratorExpression(Boo.Lang.Ast.IteratorExpression node)
		{
			return true;
		}
		
		public virtual void LeaveIteratorExpression(Boo.Lang.Ast.IteratorExpression node)
		{
		}
			
		public virtual void OnSlicingExpression(Boo.Lang.Ast.SlicingExpression node)
		{				
			if (EnterSlicingExpression(node))
			{
				Switch(node.Target);
				Switch(node.Begin);
				Switch(node.End);
				Switch(node.Step);
				LeaveSlicingExpression(node);
			}
		}
			
		public virtual bool EnterSlicingExpression(Boo.Lang.Ast.SlicingExpression node)
		{
			return true;
		}
		
		public virtual void LeaveSlicingExpression(Boo.Lang.Ast.SlicingExpression node)
		{
		}
			
		public virtual void OnAsExpression(Boo.Lang.Ast.AsExpression node)
		{				
			if (EnterAsExpression(node))
			{
				Switch(node.Target);
				Switch(node.Type);
				LeaveAsExpression(node);
			}
		}
			
		public virtual bool EnterAsExpression(Boo.Lang.Ast.AsExpression node)
		{
			return true;
		}
		
		public virtual void LeaveAsExpression(Boo.Lang.Ast.AsExpression node)
		{
		}
			
		public virtual void OnCastExpression(Boo.Lang.Ast.CastExpression node)
		{				
			if (EnterCastExpression(node))
			{
				Switch(node.Type);
				Switch(node.Target);
				LeaveCastExpression(node);
			}
		}
			
		public virtual bool EnterCastExpression(Boo.Lang.Ast.CastExpression node)
		{
			return true;
		}
		
		public virtual void LeaveCastExpression(Boo.Lang.Ast.CastExpression node)
		{
		}
			
		public virtual void OnTypeofExpression(Boo.Lang.Ast.TypeofExpression node)
		{				
			if (EnterTypeofExpression(node))
			{
				Switch(node.Type);
				LeaveTypeofExpression(node);
			}
		}
			
		public virtual bool EnterTypeofExpression(Boo.Lang.Ast.TypeofExpression node)
		{
			return true;
		}
		
		public virtual void LeaveTypeofExpression(Boo.Lang.Ast.TypeofExpression node)
		{
		}
			
	}
}
