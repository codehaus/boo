#region license
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

namespace Boo.Lang.Compiler.Tests

import System
import Boo.Lang.Compiler.Ast
import NUnit.Framework

[TestFixture]
class AstTestFixture:
	_unit as CompileUnit
	_module as Module
	_method as Method	

	[SetUp]
	def SetUp():
		_module = Module(Name: "Module")
		_module.Namespace = NamespaceDeclaration("Foo.Bar")
		
		_unit = CompileUnit()
		_unit.Modules.Add(_module)
		
		_module.Members.Add(_method = Method(Name: "test"))
		_method.Parameters.Add(ParameterDeclaration("foo", null))
	
	[Test]
	def TestFullyQualifiedName():
		Assert.AreEqual("Foo.Bar.Module", _module.FullName)
	
	[Test]
	def TestEnclosingNamespace():
		Assert.AreSame(_module.Namespace, _module.EnclosingNamespace)

	[Test]
	def TestParentNode():
		Assert.IsNull(_unit.ParentNode)
		Assert.AreSame(_unit, _module.ParentNode)
		Assert.AreSame(_module, _method.ParentNode)
		Assert.AreSame(_method, _method.Parameters[0].ParentNode)

	[Test]
	def TestDeclaringMethod():
		for pd in _method.Parameters:
			Assert.AreSame(_method, pd.ParentNode, "parameter.ParentNode")
	
	[Test]
	def TestCloneBinaryExpression():
		be = BinaryExpression(BinaryOperatorType.Assign,
									ReferenceExpression("foo"),
									StringLiteralExpression("bar"))
		Assert.AreSame(be, be.Left.ParentNode);
		Assert.AreSame(be, be.Right.ParentNode);
		
		clone = be.CloneNode()
		Assert.AreSame(clone, clone.Left.ParentNode, "clone.Left")
		Assert.AreSame(clone, clone.Right.ParentNode, "clone.Right")
