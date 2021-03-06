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
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Boo.Lang.Compiler.Ast;
using Boo.Lang.Compiler;
using Boo.Lang.Compiler.IO;
using Boo.Lang.Compiler.Steps;
using Boo.AntlrParser;

namespace Boo.AntlrParser.Tests
{
	/// <summary>
	/// Test cases for the BooParser class.
	/// </summary>
	[TestFixture]
	public class BooParserTestCase
	{
		string GetTestCasePath(string fname)
		{
			return Path.Combine(
						Boo.Lang.Compiler.Tests.BooTestCaseUtil.GetTestCasePath("parser"),
						fname);
		}
		
		Boo.Lang.Compiler.Ast.Module ParseTestCase(string fname)
		{
			return BooParser.ParseFile(GetTestCasePath(fname)).Modules[0];
		}
		
		[Test]
		public void TestSimple()
		{
			string fname = GetTestCasePath("simple.boo");
			CompileUnit cu = BooParser.ParseFile(fname);
			Assert.IsNotNull(cu);
			
			Boo.Lang.Compiler.Ast.Module module = cu.Modules[0];
			Assert.IsNotNull(module);
			Assert.AreEqual("simple", module.Name);
			Assert.AreEqual("module doc string", module.Documentation);
			Assert.AreEqual("Empty.simple", module.FullName);
			Assert.AreEqual(fname, module.LexicalInfo.FileName);

			Assert.IsNotNull(module.Namespace);

			Assert.AreEqual("Empty", module.Namespace.Name);
			Assert.AreEqual(4, module.Namespace.LexicalInfo.Line);
			Assert.AreEqual(1, module.Namespace.LexicalInfo.StartColumn);
			Assert.AreEqual(fname, module.Namespace.LexicalInfo.FileName);
		}

		[Test]
		public void TestSimpleClasses()
		{
			string fname = GetTestCasePath("simple_classes.boo");

			Boo.Lang.Compiler.Ast.Module module = BooParser.ParseFile(fname).Modules[0];
			Assert.AreEqual("Foo.Bar", module.Namespace.Name);
			
			Assert.IsNotNull(module.Members);
			Assert.AreEqual(2, module.Members.Count);

			TypeMember cd = module.Members[0];
			Assert.IsTrue(cd is ClassDefinition);
			Assert.AreEqual("Customer", cd.Name);
			Assert.AreEqual("Foo.Bar.Customer", ((TypeDefinition)cd).FullName);
			Assert.AreSame(module.Namespace, ((TypeDefinition)cd).EnclosingNamespace);

			cd = module.Members[1];
			Assert.AreEqual("Person", cd.Name);
		}

		[Test]
		public void TestSimpleClassMethods()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("simple_class_methods.boo");
			Assert.AreEqual("ITL.Content", module.Namespace.Name);
			Assert.AreEqual(1, module.Imports.Count);

			Import i = module.Imports[0];
			Assert.AreEqual("System", i.Namespace);
			Assert.AreEqual(3, i.LexicalInfo.Line);

			Assert.AreEqual(1, module.Members.Count);

			ClassDefinition cd = (ClassDefinition)module.Members[0];
			Assert.AreEqual("Article", cd.Name);

			Assert.AreEqual(3, cd.Members.Count);
			
			Method m = (Method)cd.Members[0];
			Assert.AreEqual("getTitle", m.Name);
			Assert.IsNotNull(m.ReturnType, "ReturnType");
			Assert.AreEqual("string", ((SimpleTypeReference)m.ReturnType).Name);

			m = (Method)cd.Members[1];
			Assert.AreEqual("getBody", m.Name);
			Assert.IsNotNull(m.ReturnType, "ReturnType");
			Assert.AreEqual("string", ((SimpleTypeReference)m.ReturnType).Name);

			m = (Method)cd.Members[2];
			Assert.AreEqual("getTag", m.Name);
			Assert.IsNull(m.ReturnType, "methods without a return type must have ReturnType set to null!");
		}

		[Test]
		public void TestSimpleClassFields()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("simple_class_fields.boo");

			Assert.AreEqual(1, module.Members.Count);
			ClassDefinition cd = (ClassDefinition)module.Members[0];
			
			Assert.AreEqual(3, cd.Members.Count, "Members");

			Field f = (Field)cd.Members[0];
			Assert.AreEqual("_name", f.Name);
			Assert.IsNotNull(f.Type, "Field.Type");
			Assert.AreEqual("string", ((SimpleTypeReference)f.Type).Name);

			Constructor c = (Constructor)cd.Members[1];
			Assert.AreEqual("constructor", c.Name);
			Assert.IsNull(c.ReturnType);
			Assert.AreEqual(1, c.Parameters.Count, "Parameters.Count");
			Assert.AreEqual("name", c.Parameters[0].Name);
			Assert.AreEqual("string", ((SimpleTypeReference)c.Parameters[0].Type).Name);

			Method m = (Method)cd.Members[2];
			Assert.AreEqual("getName", m.Name);
			Assert.IsNull(m.ReturnType);
			Assert.AreEqual(0, m.Parameters.Count);
			Assert.IsNotNull(m.Body, "Body");
			Assert.AreEqual(1, m.Body.Statements.Count);

			ReturnStatement rs = (ReturnStatement)m.Body.Statements[0];
			ReferenceExpression i = (ReferenceExpression)rs.Expression;
			Assert.AreEqual("_name", i.Name);
		}

		[Test]
		public void TestSimpleGlobalDefs()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("simple_global_defs.boo");
			Assert.AreEqual("Math", module.Namespace.Name);
			Assert.AreEqual(3, module.Members.Count);
			Assert.AreEqual("Rational", module.Members[0].Name);
			Assert.AreEqual("pi", module.Members[1].Name);
			Assert.AreEqual("rationalPI", module.Members[2].Name);
			Assert.AreEqual(0, module.Globals.Statements.Count);
		}
		
		[Test]
		public void CallableDefinitions()
		{
			RunParserTestCase("CallableDefinitions.boo");
		}
		
		[Test]
		public void StringEscapeSequences()
		{
			RunParserTestCase("StringEscapeSequences.boo");
		}
		
		[Test]
		public void RegularExpressionEscapeSequences()
		{
			RunParserTestCase("RegularExpressionEscapeSequences.boo");
		}
		
		[Test]
		public void RegularExpressionsWithWhitespace()
		{
			RunParserTestCase("RegularExpressionsWithWhitespace.boo");
		}
		
		[Test]
		public void InlineIf()
		{
			RunParserTestCase("inlineif.boo");
		}
		
		[Test]
		public void Class()
		{
			RunParserTestCase("class_1.boo");
		}
		
		[Test]
		public void YieldStatement()
		{
			RunParserTestCase("yield.boo");
		}
		
		[Test]
		public void TestGlobalDefs1()
		{
			RunParserTestCase("global_defs_1.boo");
		}

		[Test]
		public void TestGlobalDefs2()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("global_defs_2.boo");			

			Method m = (Method)module.Members[0];
			Assert.AreEqual("square", m.Name);
			Assert.AreEqual(1, m.Parameters.Count);
			Assert.AreEqual("x", m.Parameters[0].Name);
			Assert.AreEqual("int", ((SimpleTypeReference)m.Parameters[0].Type).Name);
			Assert.AreEqual("int", ((SimpleTypeReference)m.ReturnType).Name);
			
			Block b = m.Body;
			Assert.AreEqual(1, b.Statements.Count);

			ReturnStatement rs = b.Statements[0] as ReturnStatement;
			Assert.IsNotNull(rs, "ReturnStatement");

			BinaryExpression bs = rs.Expression as BinaryExpression;
			Assert.IsNotNull(bs, "BinaryExpression");

			Assert.AreEqual(BinaryOperatorType.Multiply, bs.Operator);
			Assert.IsTrue(bs.Left is ReferenceExpression);
			Assert.IsTrue(bs.Right is ReferenceExpression);
			Assert.AreEqual("x", ((ReferenceExpression)bs.Left).Name);
			Assert.AreEqual("x", ((ReferenceExpression)bs.Right).Name);

			m = (Method)module.Members[1];
			b = m.Body;

			Assert.AreEqual(2, b.Statements.Count);

			ExpressionStatement es = b.Statements[0] as ExpressionStatement;
			Assert.IsNotNull(es, "ExpressionStatement");

			MethodInvocationExpression mce = es.Expression as MethodInvocationExpression;
			Assert.IsNotNull(mce, "MethodInvocationExpression");
			Assert.AreEqual("print", ((ReferenceExpression)mce.Target).Name);
			Assert.AreEqual(1, mce.Arguments.Count);
			Assert.IsTrue(mce.Arguments[0] is MethodInvocationExpression);
			mce = (MethodInvocationExpression)mce.Arguments[0];
			Assert.AreEqual(3, mce.Arguments.Count);
			Assert.AreEqual("x = {0}, y = {1}", ((StringLiteralExpression)mce.Arguments[0]).Value);

			rs = b.Statements[1] as ReturnStatement;
			Assert.IsNotNull(rs, "rs");
			bs = rs.Expression as BinaryExpression;
			Assert.IsNotNull(bs, "bs");

			Assert.AreEqual(BinaryOperatorType.Addition, bs.Operator);			
			Assert.AreEqual("x", ((ReferenceExpression)bs.Left).Name);
			Assert.AreEqual("y", ((ReferenceExpression)bs.Right).Name);
		}

		[Test]
		public void TestGlobalStmts1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("global_stmts_1.boo");
			
			Block g = module.Globals;
			Assert.AreEqual(1, g.Statements.Count);

			ExpressionStatement es = (ExpressionStatement)g.Statements[0];
			MethodInvocationExpression mce = (MethodInvocationExpression)es.Expression;
			Assert.AreEqual(1, mce.Arguments.Count);

			BinaryExpression be = (BinaryExpression)mce.Arguments[0];
			Assert.AreEqual(BinaryOperatorType.Addition, be.Operator);

			mce = (MethodInvocationExpression)be.Left;
			IntegerLiteralExpression ile = (IntegerLiteralExpression)mce.Arguments[0];
			Assert.AreEqual(3, ile.Value);

			mce = (MethodInvocationExpression)be.Right;
			ile = (IntegerLiteralExpression)mce.Arguments[0];
			Assert.AreEqual(5, ile.Value);
		}

		[Test]
		public void TestStmtModifiers1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("stmt_modifiers_1.boo");

			Method m = (Method)module.Members[0];
			ReturnStatement rs = (ReturnStatement)m.Body.Statements[0];
			Assert.IsNotNull(rs.Modifier, "Modifier");
			Assert.AreEqual(StatementModifierType.If, rs.Modifier.Type);

			BinaryExpression be = (BinaryExpression)rs.Modifier.Condition;
			Assert.AreEqual(BinaryOperatorType.LessThan, be.Operator);
			Assert.AreEqual("n", ((ReferenceExpression)be.Left).Name);
			Assert.AreEqual(2, ((IntegerLiteralExpression)be.Right).Value);
		}

		[Test]
		public void TestStmtModifiers2()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("stmt_modifiers_2.boo");

			ExpressionStatement s = (ExpressionStatement)module.Globals.Statements[0];
			BinaryExpression a = (BinaryExpression)s.Expression;			
			Assert.AreEqual(BinaryOperatorType.Assign, a.Operator);
			Assert.AreEqual("f", ((ReferenceExpression)a.Left).Name);
			Assert.AreEqual(BinaryOperatorType.Division, ((BinaryExpression)a.Right).Operator);
		}

		[Test]
		public void TestStaticMethod()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("static_method.boo");
			Assert.AreEqual(1, module.Members.Count);

			ClassDefinition cd = (ClassDefinition)module.Members[0];
			Assert.AreEqual("Math", cd.Name);
			Assert.AreEqual(1, cd.Members.Count);

			Method m = (Method)cd.Members[0];
			Assert.AreEqual(TypeMemberModifiers.Static, m.Modifiers);
			Assert.AreEqual("square", m.Name);
			Assert.AreEqual("int", ((SimpleTypeReference)m.ReturnType).Name);
		}

		[Test]
		public void TestClass2()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("class_2.boo");
			ClassDefinition cd = (ClassDefinition)module.Members[0];

			Assert.AreEqual(6, cd.Members.Count);
			for (int i=0; i<5; ++i)
			{
				Assert.AreEqual(TypeMemberModifiers.None, cd.Members[i].Modifiers);
			}
			Assert.AreEqual(TypeMemberModifiers.Public | TypeMemberModifiers.Static, cd.Members[5].Modifiers);
		}

		[Test]
		public void TestForStmt1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("for_stmt_1.boo");

			ForStatement fs = (ForStatement)module.Globals.Statements[0];
			Assert.AreEqual(1, fs.Declarations.Count);
			
			Declaration d = fs.Declarations[0];
			Assert.AreEqual("i", d.Name);
			Assert.IsNull(d.Type);

			ListLiteralExpression lle = (ListLiteralExpression)fs.Iterator;
			Assert.AreEqual(3, lle.Items.Count);
			for (int i=0; i<3; ++i)
			{
				Assert.AreEqual(i+1, ((IntegerLiteralExpression)lle.Items[i]).Value);
			}

			Assert.AreEqual(1, fs.Block.Statements.Count);
			Assert.AreEqual("print", ((ReferenceExpression)((MethodInvocationExpression)((ExpressionStatement)fs.Block.Statements[0]).Expression).Target).Name);
		}

		[Test]
		public void TestRELiteral1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("re_literal_1.boo");
			Assert.AreEqual(2, module.Globals.Statements.Count);

			ExpressionStatement es = (ExpressionStatement)module.Globals.Statements[1];
			Assert.AreEqual("print", ((ReferenceExpression)((MethodInvocationExpression)es.Expression).Target).Name);

			Assert.AreEqual(StatementModifierType.If, es.Modifier.Type);
			
			BinaryExpression be = (BinaryExpression)es.Modifier.Condition;
			Assert.AreEqual(BinaryOperatorType.Match, be.Operator);
			Assert.AreEqual("s", ((ReferenceExpression)be.Left).Name);
			Assert.AreEqual("/foo/", ((RELiteralExpression)be.Right).Value);
		}

		[Test]
		public void TestRELiteral2()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("re_literal_2.boo");

			StatementCollection stmts = module.Globals.Statements;
			Assert.AreEqual(2, stmts.Count);

			BinaryExpression ae = (BinaryExpression)((ExpressionStatement)stmts[0]).Expression;
			Assert.AreEqual(BinaryOperatorType.Assign, ae.Operator);
			Assert.AreEqual("\"Bamboo\"\n", ((StringLiteralExpression)ae.Right).Value);

			ae = (BinaryExpression)((ExpressionStatement)stmts[1]).Expression;
			Assert.AreEqual(BinaryOperatorType.Assign, ae.Operator);
			Assert.AreEqual("/foo\\(bar\\)/", ((RELiteralExpression)ae.Right).Value);
		}

		[Test]
		public void TestIfElse1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("if_else_1.boo");

			StatementCollection stmts = module.Globals.Statements;
			Assert.AreEqual(1, stmts.Count);

			IfStatement s = (IfStatement)stmts[0];
			BinaryExpression be = (BinaryExpression)s.Condition;
			Assert.AreEqual(BinaryOperatorType.Match, be.Operator);
			Assert.AreEqual("gets", ((ReferenceExpression)((MethodInvocationExpression)be.Left).Target).Name);
			Assert.AreEqual("/foo/", ((RELiteralExpression)be.Right).Value);
			Assert.AreEqual(3, s.TrueBlock.Statements.Count);
			Assert.IsNull(s.FalseBlock);

			s = (IfStatement)s.TrueBlock.Statements[2];
			be = (BinaryExpression)s.Condition;
			Assert.AreEqual("/bar/", ((RELiteralExpression)be.Right).Value);
			Assert.AreEqual(1, s.TrueBlock.Statements.Count);
			Assert.IsNotNull(s.FalseBlock);
			Assert.AreEqual(1, s.FalseBlock.Statements.Count);
			Assert.AreEqual("foobar, eh?", ((StringLiteralExpression)((MethodInvocationExpression)((ExpressionStatement)s.TrueBlock.Statements[0]).Expression).Arguments[0]).Value);
			Assert.AreEqual("nah?", ((StringLiteralExpression)((MethodInvocationExpression)((ExpressionStatement)s.FalseBlock.Statements[0]).Expression).Arguments[0]).Value);
		}

		[Test]
		public void TestInterface1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("interface_1.boo");

			Assert.AreEqual(1, module.Members.Count);

			InterfaceDefinition id = (InterfaceDefinition)module.Members[0];
			Assert.AreEqual("IContentItem", id.Name);

			Assert.AreEqual(5, id.Members.Count);
			
			Property p = (Property)id.Members[0];
			Assert.AreEqual("Parent", p.Name);
			Assert.AreEqual("IContentItem", ((SimpleTypeReference)p.Type).Name);
			Assert.IsNotNull(p.Getter, "Getter");
			Assert.IsNull(p.Setter, "Setter");

			p = (Property)id.Members[1];
			Assert.AreEqual("Name", p.Name);
			Assert.AreEqual("string", ((SimpleTypeReference)p.Type).Name);
			Assert.IsNotNull(p.Getter, "Getter");
			Assert.IsNotNull(p.Setter, "Setter");

			Method m = (Method)id.Members[2];
			Assert.AreEqual("SelectItem", m.Name);
			Assert.AreEqual("IContentItem", ((SimpleTypeReference)m.ReturnType).Name);
			Assert.AreEqual("expression", m.Parameters[0].Name);
			Assert.AreEqual("string", ((SimpleTypeReference)m.Parameters[0].Type).Name);

			Assert.AreEqual("Validate", ((Method)id.Members[3]).Name);
			Assert.AreEqual("OnRemove", ((Method)id.Members[4]).Name);
		}

		[Test]
		public void TestEnum1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("enum_1.boo");

			Assert.AreEqual(2, module.Members.Count);

			EnumDefinition ed = (EnumDefinition)module.Members[0];
			Assert.AreEqual("Priority", ed.Name);
			Assert.AreEqual(3, ed.Members.Count);
			Assert.AreEqual("Low", ed.Members[0].Name);
			Assert.AreEqual("Normal", ed.Members[1].Name);
			Assert.AreEqual("High", ed.Members[2].Name);

			ed = (EnumDefinition)module.Members[1];
			Assert.AreEqual(3, ed.Members.Count);
			Assert.AreEqual("Easy", ed.Members[0].Name);
			Assert.AreEqual(0, ((EnumMember)ed.Members[0]).Initializer.Value);
			Assert.AreEqual("Normal", ed.Members[1].Name);
			Assert.AreEqual(5, ((EnumMember)ed.Members[1]).Initializer.Value);
			Assert.AreEqual("Hard", ed.Members[2].Name);
			Assert.IsNull(((EnumMember)ed.Members[2]).Initializer, "Initializer");
		}

		[Test]
		public void TestProperties1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("properties_1.boo");

			ClassDefinition cd = (ClassDefinition)module.Members[0];
			Assert.AreEqual("Person", cd.Name);
			Assert.AreEqual("_id", cd.Members[0].Name);
			Assert.AreEqual("_name", cd.Members[1].Name);

			Property p = (Property)cd.Members[3];
			Assert.AreEqual("ID", p.Name);
			Assert.AreEqual("string", ((SimpleTypeReference)p.Type).Name);
			Assert.IsNotNull(p.Getter, "Getter");
			Assert.AreEqual(1, p.Getter.Body.Statements.Count);
			Assert.AreEqual("_id", ((ReferenceExpression)((ReturnStatement)p.Getter.Body.Statements[0]).Expression).Name);
			Assert.IsNull(p.Setter, "Setter");

			p = (Property)cd.Members[4];
			Assert.AreEqual("Name", p.Name);
			Assert.AreEqual("string", ((SimpleTypeReference)p.Type).Name);
			Assert.IsNotNull(p.Getter, "Getter ");
			Assert.AreEqual(1, p.Getter.Body.Statements.Count);
			Assert.AreEqual("_name", ((ReferenceExpression)((ReturnStatement)p.Getter.Body.Statements[0]).Expression).Name);

			Assert.IsNotNull(p.Setter, "Setter");
			Assert.AreEqual(1, p.Setter.Body.Statements.Count);

			BinaryExpression a = (BinaryExpression)((ExpressionStatement)p.Setter.Body.Statements[0]).Expression;
			Assert.AreEqual(BinaryOperatorType.Assign, a.Operator);
			Assert.AreEqual("_name", ((ReferenceExpression)a.Left).Name);
			Assert.AreEqual("value", ((ReferenceExpression)a.Right).Name);
		}

		[Test]
		public void TestWhileStmt1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("while_stmt_1.boo");

			WhileStatement ws = (WhileStatement)module.Globals.Statements[3];
			Assert.AreEqual(true, ((BoolLiteralExpression)ws.Condition).Value); 
			Assert.AreEqual(4, ws.Block.Statements.Count);

			BreakStatement bs = (BreakStatement)ws.Block.Statements[3];
			BinaryExpression condition = (BinaryExpression)bs.Modifier.Condition;
			Assert.AreEqual(BinaryOperatorType.Equality, condition.Operator);
		}

		[Test]
		public void TestCppComments()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("cpp_comments.boo");
			Assert.AreEqual("CPlusPlusStyleComments", module.Namespace.Name);
		}

		[Test]
		public void TestUnpackStmt1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("unpack_stmt_1.boo");
			UnpackStatement us = (UnpackStatement)module.Globals.Statements[0];
			Assert.AreEqual(2, us.Declarations.Count);
			Assert.AreEqual("arg0", us.Declarations[0].Name);
			Assert.AreEqual("arg1", us.Declarations[1].Name);

			MethodInvocationExpression mce = (MethodInvocationExpression)us.Expression;
			MemberReferenceExpression mre = ((MemberReferenceExpression)mce.Target);
			Assert.AreEqual("GetCommandLineArgs", mre.Name);
			Assert.AreEqual("Environment", ((ReferenceExpression)mre.Target).Name);
		}

		[Test]
		public void TestYieldStmt1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("yield_stmt_1.boo");

			Method m = (Method)module.Members[0];
			ForStatement fs = (ForStatement)m.Body.Statements[0];
			YieldStatement ys = (YieldStatement)fs.Block.Statements[0];
			Assert.AreEqual("i", ((ReferenceExpression)ys.Expression).Name);
			Assert.AreEqual(StatementModifierType.If, ys.Modifier.Type);

		}

		[Test]
		public void TestNonSignificantWhitespaceRegions1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("nonsignificant_ws_regions_1.boo");

			StatementCollection stmts = module.Globals.Statements;
			Assert.AreEqual(2, stmts.Count);

			ExpressionStatement es = (ExpressionStatement)stmts[0];
			BinaryExpression ae = (BinaryExpression)es.Expression;
			Assert.AreEqual(BinaryOperatorType.Assign, ae.Operator);
			Assert.AreEqual("a", ((ReferenceExpression)ae.Left).Name);
			Assert.AreEqual(2, ((ListLiteralExpression)ae.Right).Items.Count);

			ForStatement fs = (ForStatement)stmts[1];
			MethodInvocationExpression mce = (MethodInvocationExpression)fs.Iterator;
			Assert.AreEqual("map", ((ReferenceExpression)mce.Target).Name);
			Assert.AreEqual(2, mce.Arguments.Count);

			Assert.AreEqual(1, fs.Block.Statements.Count);
		}

		[Test]
		public void TestArrays1()
		{
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("arrays_1.boo");

			StatementCollection sc = module.Globals.Statements;
			Assert.AreEqual(4, sc.Count);

			BinaryExpression ae = (BinaryExpression)((ExpressionStatement)sc[0]).Expression;
			Assert.AreEqual("names", ((ReferenceExpression)ae.Left).Name);

			ArrayLiteralExpression tle = (ArrayLiteralExpression)ae.Right;
			Assert.AreEqual(3, tle.Items.Count);

			ae = (BinaryExpression)((ExpressionStatement)sc[1]).Expression;
			tle = (ArrayLiteralExpression)ae.Right;
			Assert.AreEqual(3, tle.Items.Count);

			ae = (BinaryExpression)((ExpressionStatement)sc[3]).Expression;
			tle = (ArrayLiteralExpression)ae.Right;
			Assert.AreEqual(1, tle.Items.Count);
		}

		[Test]
		public void TestExpressions1()
		{
			RunParserTestCase("expressions_1.boo");
		}

		[Test]
		public void TestExpressions2()
		{
			RunParserTestCase("expressions_2.boo");
		}

		[Test]
		public void TestExpressions3()
		{
			RunParserTestCase("expressions_3.boo");
		}

		[Test]
		public void TestBoolLiterals()
		{
			RunParserTestCase("bool_literals.boo");
		}

		[Test]
		public void TestNullLiteral()
		{
			RunParserTestCase("null_literal.boo");
		}

		[Test]
		public void TestSelf()
		{
			RunParserTestCase("self.boo");
		}

		[Test]
		public void TestStringInterpolation()
		{
			RunParserTestCase("string_interpolation.boo");
		}

		[Test]
		public void TestBaseMembers()
		{
			RunParserTestCase("base_types.boo");
		}

		[Test]
		public void TestTimeSpanLiteral()
		{
			RunParserTestCase("timespan.boo");
		}

		[Test]
		public void TestAssert()
		{
			RunParserTestCase("assert.boo");
		}

		[Test]
		public void TestRichAssign()
		{
			RunParserTestCase("rich_assign.boo");
		}

		[Test]
		public void TestTryCatchRetry()
		{
			RunParserTestCase("try_except_retry.boo");
		}

		[Test]
		public void TestSlicing()
		{
			RunParserTestCase("slicing.boo");
		}

		[Test]
		public void TestDict()
		{
			RunParserTestCase("dict.boo");
		}

		[Test]
		public void TestListDisplay()
		{
			RunParserTestCase("list_display.boo");
		}

		[Test]
		public void TestArrays2()
		{
			RunParserTestCase("arrays_2.boo");
		}
		
		[Test]
		public void Arrays3()
		{
			RunParserTestCase("arrays_3.boo");
		}

		[Test]
		public void TestMethodCalls()
		{
			RunParserTestCase("method_calls.boo");
		}

		[Test]
		public void TestAttributes()
		{
			RunParserTestCase("attributes.boo");
		}

		[Test]
		public void TestAttributeWithNamedParameters()
		{
			RunParserTestCase("named_parameters_1.boo");
		}

		[Test]
		public void TestConstructorWithNamedParameters()
		{
			RunParserTestCase("named_parameters_2.boo");
		}

		[Test]
		public void TestImport()
		{
			RunParserTestCase("import.boo");
		}
		
		[Test]
		public void TestUnless0()
		{
			RunParserTestCase("unless0.boo");
		}
		
		[Test]
		public void InNotIn()
		{
			RunParserTestCase("in_notin0.boo");
		}
		
		[Test]
		public void SimpleImportMacro()
		{
			RunParserTestCase("macro0.boo");
		}
		
		[Test]
		public void RealSimpleConstants()
		{
			RunParserTestCase("double0.boo");
		}
		
		[Test]
		public void CommentsBetweenClassesAndGlobalMethods()
		{
			RunParserTestCase("comments0.boo");
		}
		
		[Test]
		public void CommentsBetweenMethods()
		{
			RunParserTestCase("comments1.boo");
		}
		
		[Test]
		public void NestedComments()
		{
			RunParserTestCase("nested_comments.boo");
		}
		
		[Test]
		public void GeneratorExpressions()
		{
			RunParserTestCase("GeneratorExpression1.boo");
		}
		
		[Test]
		public void LineContinuation()
		{
			RunParserTestCase("line_continuation0.boo");
		}
		
		[Test]
		public void VariableParameters()
		{
			RunParserTestCase("variable_parameters.boo");
		}
		
		[Test]
		public void SimpleAnonymousMethods()
		{
			RunParserTestCase("anonymous_methods_1.boo");
		}
		
		[Test]
		public void Docstrings()
		{
			/*
"""
A module can have a docstring.
"""
namespace Foo.Bar
"""
And so can the namespace declaration.
"""

class Person:
"""
A class can have it.
With multiple lines.
"""
	_fname as string
	"""Fields can have one."""
	
	def constructor([required] fname as string):
	"""
	And so can a method or constructor.
	"""
		_fname = fname
		
	FirstName as string:
	"""And why couldn't a property?"""
		get:
			return _fname
interface ICustomer:
"""an interface."""

	def Initialize()
	"""interface method"""
	
	Name as string:
	"""interface property"""
		get
	
	
			*/
			
			Boo.Lang.Compiler.Ast.Module module = ParseTestCase("docstrings_1.boo");
			Assert.AreEqual("A module can have a docstring.", module.Documentation);
			Assert.AreEqual("And so can the namespace declaration.", module.Namespace.Documentation);
			
			ClassDefinition person = (ClassDefinition)module.Members[0];
			Assert.AreEqual("A class can have it.\nWith multiple lines.", person.Documentation);
			Assert.AreEqual("Fields can have one.", person.Members[0].Documentation);
			Assert.AreEqual("\tAnd so can a method or constructor.\n\t", person.Members[1].Documentation);
			Assert.AreEqual("And why couldn't a property?", person.Members[2].Documentation);
			
			InterfaceDefinition customer = (InterfaceDefinition)module.Members[1];
			Assert.AreEqual("an interface.", customer.Documentation);
			
			Assert.AreEqual("interface method", customer.Members[0].Documentation);
			Assert.AreEqual("interface property", customer.Members[1].Documentation);
			
		}
		
		[TestFixtureSetUp]
		public void SetUpFixture()
		{
			_compiler = new BooCompiler();
			_compiler.Parameters.Pipeline = new Boo.Lang.Compiler.Pipelines.ParseAndPrint();			
		}
		
		[SetUp]
		public void SetUp()
		{
			_compiler.Parameters.Input.Clear();
		}
		
		BooCompiler _compiler;
		
		void RunParserTestCase(string testfile)
		{			
			TextWriter oldStdOut = Console.Out;
			try
			{
				StringWriter stdout = new StringWriter();
				Console.SetOut(stdout);
			
				_compiler.Parameters.Input.Add(new FileInput(GetTestCasePath(testfile)));
				CompilerContext context = _compiler.Run();
				if (context.Errors.Count > 0)
				{
					Assert.Fail(context.Errors.ToString(true));
				}
				
				Assert.AreEqual(1, context.CompileUnit.Modules.Count, "expected a module as output");				
				
				string expected = context.CompileUnit.Modules[0].Documentation;
				if (null == expected)
				{
					Assert.Fail(string.Format("Test case '{0}' does not have a docstring!", testfile));
				}
				Assert.AreEqual(expected.Trim(), stdout.ToString().Trim().Replace("\r\n", "\n"), testfile);				
			}
			finally
			{
				Console.SetOut(oldStdOut);
			}
		}
	}
}
