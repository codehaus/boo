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

namespace Boo.Lang.Ast.Visitors
{
	using System;
	using System.Text.RegularExpressions;
	using System.Globalization;
	using System.IO;
	using Boo.Lang.Ast;

	/// <summary>
	/// Imprime uma AST boo em boo.
	/// </summary>
	public class BooPrinterVisitor : TextEmitter
	{		
		public BooPrinterVisitor(TextWriter writer) : base(writer)
		{
		}

		public void Print(CompileUnit ast)
		{
			OnCompileUnit(ast);
		}	
		
		#region overridables
		public virtual void WriteKeyword(string text)
		{
			Write(text);
		}
		
		public virtual void WriteOperator(string text)
		{
			Write(text);
		}
		#endregion
		
		#region IVisitor Members	

		override public void OnModule(Module m)
		{
			Switch(m.Namespace);

			if (m.Imports.Count > 0)
			{
				Switch(m.Imports);
				WriteLine();
			}

			foreach (TypeMember member in m.Members)
			{
				Switch(member);
				WriteLine();
			}

			// m.Globals iria causar um Indent()
			// invlido
			if (null != m.Globals)
			{
				Switch(m.Globals.Statements);
			}
		}

		override public void OnNamespaceDeclaration(NamespaceDeclaration node)
		{
			WriteKeyword("namespace");
			WriteLine(" {0}", node.Name);
			WriteLine();
		}

		override public void OnImport(Import p)
		{
			WriteKeyword("import");
			Write(" {0}", p.Namespace);
			if (null != p.AssemblyReference)
			{
				WriteKeyword(" from ");
				Write(p.AssemblyReference.Name);
			}
			if (null != p.Alias)
			{
				WriteKeyword(" as ");
				Write(p.Alias.Name);
			}
			WriteLine();
		}

		public void WriteBlock(Block b)
		{
			Indent();
			if (0 == b.Statements.Count)
			{
				WriteIndented();
				WriteKeyword("pass");
				WriteLine();
			}
			else
			{
				Switch(b.Statements);
			}
			Dedent();
		}
		
		override public void OnClassDefinition(ClassDefinition c)
		{
			WriteTypeDefinition("class", c);
		}

		override public void OnInterfaceDefinition(InterfaceDefinition id)
		{
			WriteTypeDefinition("interface", id);
		}

		override public void OnEnumDefinition(EnumDefinition ed)
		{
			WriteTypeDefinition("enum", ed);
		}

		override public void OnField(Field f)
		{
			WriteAttributes(f.Attributes, true);
			WriteModifiers(f);
			Write(f.Name);
			WriteTypeReference(f.Type);
			if (null != f.Initializer)
			{
				WriteOperator(" = ");
				Switch(f.Initializer);
			}
			WriteLine();
		}
		
		override public void OnProperty(Property node)
		{
			WriteAttributes(node.Attributes, true);			
			WriteModifiers(node);
			WriteIndented(node.Name);
			if (node.Parameters.Count > 0)
			{
				WriteParameterList(node.Parameters);
			}
			WriteTypeReference(node.Type);
			WriteLine(":");
			Indent();
			if (null != node.Getter)
			{
				WriteAttributes(node.Getter.Attributes, true);
				WriteModifiers(node.Getter);
				WriteKeyword("get");
				WriteLine(":");
				WriteBlock(node.Getter.Body);
			}
			if (null != node.Setter)
			{
				WriteAttributes(node.Setter.Attributes, true);
				WriteModifiers(node.Setter);
				WriteKeyword("set");
				WriteLine(":");
				WriteBlock(node.Setter.Body);
			}
			Dedent();
		}
		
		override public void OnEnumMember(EnumMember node)
		{
			WriteAttributes(node.Attributes, true);
			WriteIndented(node.Name);
			if (null != node.Initializer)
			{
				WriteOperator(" = ");
				Switch(node.Initializer);
			}
			WriteLine();
		}

		override public void OnConstructor(Constructor c)
		{
			OnMethod(c);
		}

		override public void OnMethod(Method m)
		{
			WriteAttributes(m.Attributes, true);
			WriteModifiers(m);
			WriteKeyword("def ");
			Write(m.Name);
			WriteParameterList(m.Parameters);
			WriteTypeReference(m.ReturnType);
			if (m.ReturnTypeAttributes.Count > 0)
			{
				Write(" ");
				WriteAttributes(m.ReturnTypeAttributes, false);
			}
			WriteLine(":");
			WriteBlock(m.Body);
		}
		
		void WriteTypeReference(TypeReference t)
		{
			if (null != t)
			{
				WriteKeyword(" as ");
				t.Switch(this);
			}
		}

		override public void OnParameterDeclaration(ParameterDeclaration p)
		{
			WriteAttributes(p.Attributes, false);
			Write(p.Name);
			WriteTypeReference(p.Type);
		}

		override public void OnSimpleTypeReference(SimpleTypeReference t)
		{				
			Write(t.Name);
		}
		
		override public void OnTupleTypeReference(TupleTypeReference t)
		{
			Write("(");
			Switch(t.ElementType);
			Write(")");
		}

		override public void OnMemberReferenceExpression(MemberReferenceExpression e)
		{
			Switch(e.Target);
			Write(".");
			Write(e.Name);
		}
		
		override public void OnAsExpression(AsExpression e)
		{
			Write("(");
			Switch(e.Target);
			WriteTypeReference(e.Type);
			Write(")");
		}
		
		override public void OnNullLiteralExpression(NullLiteralExpression node)
		{
			WriteKeyword("null");
		}
		
		override public void OnSelfLiteralExpression(SelfLiteralExpression node)
		{
			WriteKeyword("self");
		}
		
		override public void OnSuperLiteralExpression(SuperLiteralExpression node)
		{
			WriteKeyword("super");
		}
		
		override public void OnTimeSpanLiteralExpression(TimeSpanLiteralExpression node)
		{
			double days = node.Value.TotalDays;
			if (days >= 1)
			{
				Write(days.ToString(CultureInfo.InvariantCulture) + "d");
			}
			else
			{
				double hours = node.Value.TotalHours;
				if (hours >= 1)
				{
					Write(hours.ToString(CultureInfo.InvariantCulture) + "h");
				}
				else
				{
					double minutes = node.Value.TotalMinutes;
					if (minutes >= 1)
					{
						Write(minutes.ToString(CultureInfo.InvariantCulture) + "m");
					}
					else
					{
						double seconds = node.Value.TotalSeconds;
						if (seconds >= 1)
						{
							Write(seconds.ToString(CultureInfo.InvariantCulture) + "s");
						}
						else
						{
							Write(node.Value.TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + "ms");
						}
					}
				}
			}
		}
		
		override public void OnBoolLiteralExpression(BoolLiteralExpression node)
		{
			if (node.Value)
			{
				WriteKeyword("true");
			}
			else
			{
				WriteKeyword("false");
			}
		}
		
		override public void OnUnaryExpression(UnaryExpression node)
		{
			Write("(");
			WriteOperator(GetUnaryOperatorText(node.Operator));
			Switch(node.Operand);
			Write(")");			
		}

		override public void OnBinaryExpression(BinaryExpression e)
		{
			bool needsParens = !(e.ParentNode is ExpressionStatement);
			if (needsParens)
			{
				Write("(");
			}
			Switch(e.Left);
			Write(" ");
			WriteOperator(GetBinaryOperatorText(e.Operator));
			Write(" ");
			Switch(e.Right);
			if (needsParens)
			{
				Write(")");
			}
		}
		
		override public void OnTernaryExpression(TernaryExpression node)
		{			
			Write("(");
			Switch(node.Condition);
			WriteOperator(" ? ");
			Switch(node.TrueExpression);
			WriteOperator(" : ");
			Switch(node.FalseExpression);
			Write(")");
		}

		override public void OnRaiseStatement(RaiseStatement rs)
		{
			WriteIndented();
			WriteKeyword("raise ");
			Switch(rs.Exception);
			Switch(rs.Modifier);
			WriteLine();
		}

		override public void OnMethodInvocationExpression(MethodInvocationExpression e)
		{
			Switch(e.Target);
			Write("(");
			WriteCommaSeparatedList(e.Arguments);
			if (e.NamedArguments.Count > 0)
			{
				if (e.Arguments.Count > 0)
				{
					Write(", ");
				}
				WriteCommaSeparatedList(e.NamedArguments);
			}			
			Write(")");
		}
		
		override public void OnTupleLiteralExpression(TupleLiteralExpression node)
		{
			WriteTuple(node.Items);
		}
		
		override public void OnListLiteralExpression(ListLiteralExpression node)
		{			
			Write("[");
			WriteCommaSeparatedList(node.Items);
			Write("]");
		}
		
		override public void OnIteratorExpression(IteratorExpression node)
		{			
			Write("(");
			Switch(node.Expression);
			WriteKeyword(" for ");
			WriteCommaSeparatedList(node.Declarations);
			WriteKeyword(" in ");
			Switch(node.Iterator);
			Switch(node.Filter);
			Write(")");
		}

		override public void OnSlicingExpression(SlicingExpression node)
		{
			Switch(node.Target);
			Write("[");
			Switch(node.Begin);
			if (null != node.End || WasOmitted(node.Begin))
			{
				Write(":");
			}			
			Switch(node.End);			
			if (null != node.Step)
			{
				Write(":");
				Switch(node.Step);
			}			
			Write("]");
		}
		
		override public void OnHashLiteralExpression(HashLiteralExpression node)
		{			
			Write("{");
			if (node.Items.Count > 0)
			{
				Write(" ");
				WriteCommaSeparatedList(node.Items);
				Write(" ");
			}
			Write("}");
		}

		override public void OnExpressionPair(ExpressionPair pair)
		{
			Switch(pair.First);
			Write(": ");
			Switch(pair.Second);
		}
		
		override public void OnRELiteralExpression(RELiteralExpression e)
		{			
			Write(e.Value);
		}

		override public void OnStringLiteralExpression(StringLiteralExpression e)
		{			
			WriteStringLiteral(e.Value);			
		}

		override public void OnIntegerLiteralExpression(IntegerLiteralExpression e)
		{
			Write(e.Value.ToString());
			if (e.IsLong)
			{
				Write("L");
			}
		}
		
		override public void OnDoubleLiteralExpression(DoubleLiteralExpression e)
		{
			Write(e.Value.ToString("########0.0##########", CultureInfo.InvariantCulture));
		}

		override public void OnReferenceExpression(ReferenceExpression node)
		{
			Write(node.Name);
		}

		override public void OnExpressionStatement(ExpressionStatement node)
		{
			WriteIndented();
			Switch(node.Expression);
			Switch(node.Modifier);
			WriteLine();
		}

		override public void OnStringFormattingExpression(StringFormattingExpression node)
		{
			string template = node.Template;
			
			int current = 0;
			Match m = Regex.Match(template, @"\{(\d+)\}");
			
			Write("\"");
			foreach (Expression arg in node.Arguments)
			{	
				WriteStringLiteralContents(RuntimeServices.Mid(template, current, m.Index), _writer);				
				current = m.Index + m.Length;

				Write("${");
				Switch(arg);
				Write("}");
				m = m.NextMatch();
			}
			WriteStringLiteralContents(RuntimeServices.Mid(template, current, template.Length), _writer);
			Write("\"");
		}

		override public void OnStatementModifier(StatementModifier sm)
		{
			Write(" ");
			WriteKeyword(sm.Type.ToString().ToLower());
			Write(" ");
			Switch(sm.Condition);
		}
		
		override public void OnMacroStatement(MacroStatement node)
		{
			WriteIndented(node.Name);
			Write(" ");
			WriteCommaSeparatedList(node.Arguments);
			WriteLine(":");
			WriteBlock(node.Block);
		}
		
		override public void OnForStatement(ForStatement fs)
		{
			WriteIndented();
			WriteKeyword("for ");
			for (int i=0; i<fs.Declarations.Count; ++i)
			{
				if (i > 0) { Write(", "); }
				Switch(fs.Declarations[i]);
			}
			WriteKeyword(" in ");
			Switch(fs.Iterator);
			WriteLine(":");
			WriteBlock(fs.Block);
		}
		
		override public void OnRetryStatement(RetryStatement node)
		{
			WriteIndented();
			WriteKeyword("retry");
			WriteLine();
		}
		
		override public void OnTryStatement(TryStatement node)
		{
			WriteIndented();
			WriteKeyword("try:");
			WriteLine();
			WriteBlock(node.ProtectedBlock);
			Switch(node.ExceptionHandlers);
			if (null != node.SuccessBlock)
			{
				WriteIndented();
				WriteKeyword("success:");
				WriteLine();
				WriteBlock(node.SuccessBlock);
			}
			if (null != node.EnsureBlock)
			{
				WriteIndented();
				WriteKeyword("ensure:");
				WriteLine();
				WriteBlock(node.EnsureBlock);
			}
		}
		
		override public void OnExceptionHandler(ExceptionHandler node)
		{
			WriteIndented();
			WriteKeyword("except");
			if (null != node.Declaration)
			{
				Write(" ");
				Switch(node.Declaration);
			}			
			WriteLine(":");
			WriteBlock(node.Block);
		}
		
		override public void OnUnlessStatement(UnlessStatement node)
		{
			WriteConditionalBlock("unless", node.Condition, node.Block);
		}
		
		override public void OnBreakStatement(BreakStatement node)
		{
			WriteIndented();
			WriteKeyword("break");
			WriteLine();
		}
		
		override public void OnWhileStatement(WhileStatement node)
		{
			WriteConditionalBlock("while", node.Condition, node.Block);
		}

		override public void OnIfStatement(IfStatement ifs)
		{
			WriteIndented();
			WriteKeyword("if ");
			Switch(ifs.Condition);
			WriteLine(":");
			WriteBlock(ifs.TrueBlock);
			if (null != ifs.FalseBlock)
			{			
				WriteIndented();
				WriteKeyword("else:");
				WriteLine();
				WriteBlock(ifs.FalseBlock);
			}
		}
		
		override public bool EnterDeclarationStatement(DeclarationStatement node)
		{
			WriteIndented();
			return true;
		}
		
		override public void LeaveDeclarationStatement(DeclarationStatement node)
		{
			WriteLine();
		}

		override public void OnDeclaration(Declaration d)
		{
			Write(d.Name);
			WriteTypeReference(d.Type);
		}

		override public void OnReturnStatement(ReturnStatement r)
		{
			WriteIndented();
			WriteKeyword("return ");
			Switch(r.Expression);
			Switch(r.Modifier);
			WriteLine();
		}

		override public void OnUnpackStatement(UnpackStatement us)
		{
			WriteIndented();
			for (int i=0; i<us.Declarations.Count; ++i)
			{
				if (i > 0)
				{
					Write(", ");
				}
				Switch(us.Declarations[i]);
			}
			WriteOperator(" = ");
			Switch(us.Expression);
			WriteLine();
		}

		#endregion
		
		public static string GetUnaryOperatorText(UnaryOperatorType op)
		{
			switch (op)
			{
				case UnaryOperatorType.Increment:
				{
					return "++";
				}
					
				case UnaryOperatorType.Decrement:
				{
					return "--";
				}
					
				case UnaryOperatorType.UnaryNegation:
				{
					return "-";
				}
				
				case UnaryOperatorType.LogicalNot:
				{
					return "not ";
				}
			}
			throw new ArgumentException("op");
		}

		public static string GetBinaryOperatorText(BinaryOperatorType op)
		{
			switch (op)
			{
				case BinaryOperatorType.Assign:
				{					
					return "=";
				}

				case BinaryOperatorType.Match:
				{
					return "=~";
				}
				
				case BinaryOperatorType.Equality:
				{
					return "==";
				}
				
				case BinaryOperatorType.Inequality:
				{
					return "!=";
				}
				
				case BinaryOperatorType.Addition:
				{
					return "+";
				}
				
				case BinaryOperatorType.Exponentiation:
				{
					return "**";
				}
				
				case BinaryOperatorType.InPlaceAdd:
				{
					return "+=";
				}
				
				case BinaryOperatorType.InPlaceSubtract:
				{
					return "-=";
				}
				
				case BinaryOperatorType.InPlaceMultiply:
				{
					return "*=";
				}
				
				case BinaryOperatorType.InPlaceDivide:
				{
					return "/=";
				}
				
				case BinaryOperatorType.Subtraction:
				{
					return "-";
				}
				
				case BinaryOperatorType.Multiply:
				{
					return "*";
				}
				
				case BinaryOperatorType.Division:
				{
					return "/";
				}
				
				case BinaryOperatorType.GreaterThan:
				{
					return ">";
				}
				
				case BinaryOperatorType.GreaterThanOrEqual:
				{
					return ">=";
				}
				
				case BinaryOperatorType.LessThan:
				{
					return "<";
				}
				
				case BinaryOperatorType.LessThanOrEqual:
				{
					return "<=";
				}
				
				case BinaryOperatorType.Modulus:
				{
					return "%";
				}
				
				case BinaryOperatorType.Member:
				{
					return "in";
				}
				
				case BinaryOperatorType.NotMember:
				{
					return "not in";
				}
				
				case BinaryOperatorType.ReferenceEquality:
				{
					return "is";
				}
				
				case BinaryOperatorType.ReferenceInequality:
				{
					return "is not";
				}
				
				case BinaryOperatorType.TypeTest:
				{
					return "isa";
				}
				
				case BinaryOperatorType.Or:
				{
					return "or";
				}
				
				case BinaryOperatorType.And:
				{
					return "and";
				}
				
				case BinaryOperatorType.BitwiseOr:
				{
					return "|";
				}
			}
			throw new NotImplementedException(op.ToString());
		}
		
		public virtual void WriteStringLiteral(string text)
		{
			WriteStringLiteral(text, _writer);
		}
		
		public static void WriteStringLiteral(string text, TextWriter writer)
		{
			writer.Write("'");
			WriteStringLiteralContents(text, writer);
			writer.Write("'");
		}
		
		public static void WriteStringLiteralContents(string text, TextWriter writer)
		{
			foreach (char ch in text)
			{
				switch (ch)
				{
					case '\r':
					{
						writer.Write("\\r");						
						break;
					}
					
					case '\n':
					{
						writer.Write("\\n");
						break;
					}
					
					case '\t':
					{
						writer.Write("\\t");
						break;
					}
					
					case '\\':
					{
						writer.Write("\\\\");
						break;
					}
					
					default:
					{
						writer.Write(ch);
						break;
					}
				}				
			}
		}
		
		void WriteConditionalBlock(string keyword, Expression condition, Block block)
		{
			WriteIndented();
			WriteKeyword(keyword + " ");
			Switch(condition);
			WriteLine(":");
			WriteBlock(block);
		}
		
		void WriteParameterList(ParameterDeclarationCollection items)
		{
			Write("(");
			WriteCommaSeparatedList(items);
			Write(")");
		}
		
		void WriteCommaSeparatedList(NodeCollection items)
		{			
			for (int i=0; i<items.Count; ++i)
			{
				if (i > 0)
				{
					Write(", ");
				}
				Switch(items.GetNodeAt(i));
			}
		}
		
		void WriteTuple(ExpressionCollection items)
		{
			Write("(");
			if (items.Count > 1)
			{
				for (int i=0; i<items.Count; ++i)
				{
					if (i>0)
					{
						Write(", ");
					}
					Switch(items[i]);
				}
			}
			else
			{
				if (items.Count > 0)
				{
					Switch(items[0]);
				}
				Write(",");
			}
			Write(")");
		}
		
		void WriteAttributes(AttributeCollection attributes, bool addNewLines)
		{
			foreach (Boo.Lang.Ast.Attribute attribute in attributes)
			{
				WriteIndented("[");
				Write(attribute.Name);
				if (attribute.Arguments.Count > 0 ||
				    attribute.NamedArguments.Count > 0)
				{
					Write("(");
					WriteCommaSeparatedList(attribute.Arguments);
					if (attribute.NamedArguments.Count > 0)
					{
						if (attribute.Arguments.Count > 0)
						{
							Write(", ");
						}
						WriteCommaSeparatedList(attribute.NamedArguments);
					}
					Write(")");
				}
				Write("]");
				if (addNewLines)
				{
					WriteLine();
				}
				else
				{
					Write(" ");
				}
			}			
		}
		
		void WriteModifiers(TypeMember member)
		{
			WriteIndented();
			if (member.IsPublic)
			{
				WriteKeyword("public ");
			}
			else if (member.IsProtected)
			{
				WriteKeyword("protected ");
			}
			else if (member.IsPrivate)
			{
				WriteKeyword("private ");
			}
			else if (member.IsInternal)
			{
				WriteKeyword("internal ");
			}
			if (member.IsStatic)
			{
				WriteKeyword("static ");
			}
			else if (member.IsModifierSet(TypeMemberModifiers.Override))
			{
				WriteKeyword("override ");
			}
			else if (member.IsModifierSet(TypeMemberModifiers.Virtual))
			{
				WriteKeyword("virtual ");
			}
			else if (member.IsModifierSet(TypeMemberModifiers.Abstract))
			{
				WriteKeyword("abstract ");
			}
			if (member.IsFinal)
			{
				WriteKeyword("final ");
			}
			if (member.IsTransient)
			{
				WriteKeyword("transient ");
			}
		}

		void WriteTypeDefinition(string keyword, TypeDefinition td)
		{
			WriteAttributes(td.Attributes, true);
			WriteModifiers(td);
			WriteIndented();
			WriteKeyword(keyword);
			Write(" ");
			Write(td.Name);
			if (td.BaseTypes.Count > 0)
			{
				Write("(");
				for (int i=0; i<td.BaseTypes.Count; ++i)
				{
					if (i > 0) { Write(", "); }
					Write(((SimpleTypeReference)td.BaseTypes[i]).Name);
				}
				Write(")");
			}
			WriteLine(":");
			Indent();
			if (td.Members.Count > 0)
			{				
				foreach (TypeMember member in td.Members)
				{
					WriteLine();
					Switch(member);
				}
			}
			else
			{
				WriteIndented();
				WriteKeyword("pass");
				WriteLine();
			}
			Dedent();
		}
		
		bool WasOmitted(Expression node)
		{
			return null != node &&
				NodeType.OmittedExpression == node.NodeType;
		}
	}
}
