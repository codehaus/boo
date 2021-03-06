#region license
// boo - an extensible programming language for the CLI
// Copyright (C) 2004 Rodrigo B. de Oliveira
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// As a special exception, if you link this library with other files to
// produce an executable, this library does not by itself cause the
// resulting executable to be covered by the GNU General Public License.
// This exception does not however invalidate any other reasons why the
// executable file might be covered by the GNU General Public License.
//
// Contact Information
//
// mailto:rbo@acm.org
#endregion

//
// DO NOT EDIT THIS FILE!
//
// This file was generated automatically by the
// ast.py script on Thu Feb 19 02:30:14 2004
//
using System;

namespace Boo.Lang.Ast
{
	public interface IAstSwitcher
	{
		void OnCompileUnit(CompileUnit node);
		void OnSimpleTypeReference(SimpleTypeReference node);
		void OnTupleTypeReference(TupleTypeReference node);
		void OnNamespaceDeclaration(NamespaceDeclaration node);
		void OnImport(Import node);
		void OnModule(Module node);
		void OnClassDefinition(ClassDefinition node);
		void OnInterfaceDefinition(InterfaceDefinition node);
		void OnEnumDefinition(EnumDefinition node);
		void OnEnumMember(EnumMember node);
		void OnField(Field node);
		void OnProperty(Property node);
		void OnLocal(Local node);
		void OnMethod(Method node);
		void OnConstructor(Constructor node);
		void OnParameterDeclaration(ParameterDeclaration node);
		void OnDeclaration(Declaration node);
		void OnAttribute(Attribute node);
		void OnStatementModifier(StatementModifier node);
		void OnBlock(Block node);
		void OnDeclarationStatement(DeclarationStatement node);
		void OnAssertStatement(AssertStatement node);
		void OnMacroStatement(MacroStatement node);
		void OnTryStatement(TryStatement node);
		void OnExceptionHandler(ExceptionHandler node);
		void OnIfStatement(IfStatement node);
		void OnUnlessStatement(UnlessStatement node);
		void OnForStatement(ForStatement node);
		void OnWhileStatement(WhileStatement node);
		void OnGivenStatement(GivenStatement node);
		void OnWhenClause(WhenClause node);
		void OnBreakStatement(BreakStatement node);
		void OnContinueStatement(ContinueStatement node);
		void OnRetryStatement(RetryStatement node);
		void OnReturnStatement(ReturnStatement node);
		void OnYieldStatement(YieldStatement node);
		void OnRaiseStatement(RaiseStatement node);
		void OnUnpackStatement(UnpackStatement node);
		void OnExpressionStatement(ExpressionStatement node);
		void OnOmittedExpression(OmittedExpression node);
		void OnExpressionPair(ExpressionPair node);
		void OnMethodInvocationExpression(MethodInvocationExpression node);
		void OnUnaryExpression(UnaryExpression node);
		void OnBinaryExpression(BinaryExpression node);
		void OnTernaryExpression(TernaryExpression node);
		void OnReferenceExpression(ReferenceExpression node);
		void OnMemberReferenceExpression(MemberReferenceExpression node);
		void OnStringLiteralExpression(StringLiteralExpression node);
		void OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node);
		void OnIntegerLiteralExpression(IntegerLiteralExpression node);
		void OnDoubleLiteralExpression(DoubleLiteralExpression node);
		void OnNullLiteralExpression(NullLiteralExpression node);
		void OnSelfLiteralExpression(SelfLiteralExpression node);
		void OnSuperLiteralExpression(SuperLiteralExpression node);
		void OnBoolLiteralExpression(BoolLiteralExpression node);
		void OnRELiteralExpression(RELiteralExpression node);
		void OnStringFormattingExpression(StringFormattingExpression node);
		void OnHashLiteralExpression(HashLiteralExpression node);
		void OnListLiteralExpression(ListLiteralExpression node);
		void OnTupleLiteralExpression(TupleLiteralExpression node);
		void OnIteratorExpression(IteratorExpression node);
		void OnSlicingExpression(SlicingExpression node);
		void OnAsExpression(AsExpression node);
	}
}
