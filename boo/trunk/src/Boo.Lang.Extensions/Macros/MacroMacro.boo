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

namespace Boo.Lang.Extensions

import Boo.Lang.Compiler
import Boo.Lang.Compiler.Ast
import Boo.Lang.Compiler.TypeSystem


class MacroMacro(LexicalInfoPreservingGeneratorMacro):

	private static final Usage = "Usage: `macro [<parent.>+]<name>[(arg0,...)]`"

	_macro as MacroStatement
	_name as string

	_apb as ArgumentsPatternBuilder

	ArgumentsPattern:
		get:
			return _apb.Pattern if _apb

	ArgumentsPrologue:
		get:
			return _apb.Prologue if _apb


	override protected def ExpandGeneratorImpl(macro as MacroStatement):
		raise Usage if len(macro.Arguments) != 1

		_macro = macro

		arg = macro.Arguments[0]
		mie = arg as MethodInvocationExpression
		if mie and mie.Arguments.Count:
			_apb = ArgumentsPatternBuilder(Context, mie.Arguments)
			arg = mie.Target

		if arg.NodeType == NodeType.ReferenceExpression:
			_name = cast(ReferenceExpression, arg).Name
			yield CreateMacroType()
		elif arg.NodeType == NodeType.MemberReferenceExpression:
			if macro.GetAncestor[of MacroStatement]() is not null:
				raise "Nested macro extension cannot be itself a nested macro"
			_name = cast(MemberReferenceExpression, arg).Name
			CreateMacroExtensionType(cast(MemberReferenceExpression, arg))
		else:
			raise Usage


	private static def BuildMacroTypeName(name as string) as string:
		return char.ToUpper(name[0]) + name[1:] + "Macro"

	private static def BuildMacroTypeName(name as Expression) as string:
		return BuildMacroTypeName(name, false)

	private static def BuildMacroTypeName(name as Expression, external as bool) as string:
		sb = System.Text.StringBuilder()
		for part in UnpackReferences(name):
			sb.Append(char('.')) if sb.Length and not external
			sb.Append(char('$')) if external
			sb.Append(BuildMacroTypeName(part))
		return sb.ToString()


	private static def UnpackReferences(refs as Expression) as string*:
		if refs.NodeType == NodeType.ReferenceExpression:
			re = cast(ReferenceExpression, refs)
			raise "Extending macro `macro` is not supported" if re.Name == "macro"
			yield re.Name
		elif refs.NodeType == NodeType.MemberReferenceExpression:
			mre = cast(MemberReferenceExpression, refs)
			for name in UnpackReferences(mre.Target):
				yield name
			yield mre.Name
		else:
			raise "Invalid node type: ${refs.NodeType}"


	private static def GetParentMacroNames(macro as MacroStatement) as string*:
		for parent in macro.GetAncestors[of MacroStatement]():
			continue if parent.Name != 'macro'

			arg = parent.Arguments[0]
			if arg.NodeType == NodeType.MethodInvocationExpression:
				arg = cast(MethodInvocationExpression, arg).Target

			if arg.NodeType == NodeType.ReferenceExpression:
				yield cast(ReferenceExpression, arg).Name

			elif arg.NodeType == NodeType.MemberReferenceExpression:
				for name in UnpackReferences(cast(MemberReferenceExpression, arg)):
					yield name


	private def CreateMacroExtensionType(mre as MemberReferenceExpression):
		#resolve the macro type we want to expand
		parentTypeName = BuildMacroTypeName(mre.Target)
		parent = NameResolutionService.ResolveQualifiedName(parentTypeName, EntityType.Type)
		if parent is null: #oh, this is a external extension maybe?
			parentTypeName = BuildMacroTypeName(mre.Target, true)
			parent = NameResolutionService.ResolveQualifiedName(parentTypeName, EntityType.Type)
		if parent is null:
			raise "No macro `${mre.Target.ToString()}` has been found to extend"

		elif parent isa InternalClass:
			#create macro type and add it as usual into its parent macro type
			type = CreateMacroType(UnpackReferences(mre.Target))
			cast(InternalClass, parent).TypeDefinition.Members.Add(type)

		elif parent isa ExternalType:
			#create macro type with parent(s)-based prefix then add an extension method for redirection
			type = CreateMacroType(BuildMacroTypeName(mre, true), UnpackReferences(mre.Target))
			module = _macro.GetAncestor[of Module]()
			module.Members.Add(type)
			method = CreateMacroExtensionProxy(parent, type.Name)
			module.Members.Add(method)

		else:
			raise "Cannot bind a nested macro extension to entity: ${parent}"


	private def CreateMacroExtensionProxy(parent as IType, extension as string):
		return [|
			[Boo.Lang.ExtensionAttribute]
			[System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			static def $(_name)(parent as $(parent.FullName), context as Boo.Lang.Compiler.CompilerContext) as $(extension):
				return $(ReferenceExpression(extension))(context)
		|]


	private def CreateMacroType() as ClassDefinition:
		return CreateMacroType(BuildMacroTypeName(_name), GetParentMacroNames(_macro))

	private def CreateMacroType(parents as string*) as ClassDefinition:
		return CreateMacroType(BuildMacroTypeName(_name), parents)

	private def CreateMacroType(typeName as string, parents as string*) as ClassDefinition:
		if YieldFinder(_macro).Found:
			macroType = CreateGeneratorMacroType(typeName)
		else:
			macroType = CreateOldStyleMacroType(typeName)

		#add parent macro(s) accessor(s)
		for parent in parents:
			if not macroType.Members[parent]:
				for accessor in CreateParentMacroAccessor(parent):
					macroType.Members.Add(accessor)
		return macroType


	#BOO-1077 style
	private def CreateGeneratorMacroType(typeName as string) as ClassDefinition:
		arg = ReferenceExpression(_name)
		return [|
				final class $(typeName) (Boo.Lang.Compiler.LexicalInfoPreservingGeneratorMacro):
					[System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					private __macro as Boo.Lang.Compiler.Ast.MacroStatement
					def constructor():
						super()
					def constructor(context as Boo.Lang.Compiler.CompilerContext):
						raise System.ArgumentNullException("context") if not context
						super(context)
					override protected def ExpandGeneratorImpl($_name as Boo.Lang.Compiler.Ast.MacroStatement) as Boo.Lang.Compiler.Ast.Node*:
						raise System.ArgumentNullException($_name) if not $arg
						self.__macro = $arg
						$(ExpandBody())
					[System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					override protected def ExpandImpl($_name as Boo.Lang.Compiler.Ast.MacroStatement) as Boo.Lang.Compiler.Ast.Statement:
						raise System.NotImplementedException("Boo installed version is older than the new macro syntax '${$(_name)}' uses. Read BOO-1077 for more info.")
			|]

	private def CreateOldStyleMacroType(typeName as string) as ClassDefinition:
		arg = ReferenceExpression(_name)
		return [|
				final class $(typeName) (Boo.Lang.Compiler.LexicalInfoPreservingMacro):
					[System.Runtime.CompilerServices.CompilerGeneratedAttribute]
					private __macro as Boo.Lang.Compiler.Ast.MacroStatement
					def constructor():
						super()
					def constructor(context as Boo.Lang.Compiler.CompilerContext):
						raise System.ArgumentNullException("context") if not context
						super(context)
					override protected def ExpandImpl($_name as Boo.Lang.Compiler.Ast.MacroStatement) as Boo.Lang.Compiler.Ast.Statement:
						raise System.ArgumentNullException($_name) if not $arg
						self.__macro = $arg
						$(ExpandBody())
			|]


	private def ExpandBody():
		body = _macro.Body

		if ArgumentsPattern and ArgumentsPattern.Count > 0:
			case = CaseStatement()
			case.Pattern = QuasiquoteExpression(pattern = MacroStatement(_name))
			pattern.Arguments = ArgumentsPattern
			if ArgumentsPrologue:
				case.Body = [|
					$ArgumentsPrologue
					$body
				|].ToBlock()
			else:
				case.Body = body
			otherwise = OtherwiseStatement() #TODO: macro overload without arguments def
			otherwise.Body = [| raise "`${$(_name)}` macro invocation argument(s) did not match definition: `${$(_macro.Arguments[0].ToString())}`" |].ToBlock()
			body = [|
				$case
				$otherwise
			|]

		if ContainsCase(body):
			return ExpandWithPatternMatching(_name, body)

		if ArgumentsPrologue:
			return [|
				$ArgumentsPrologue
				$body
			|].ToBlock()
		else:
			return body


	#region PatternMatching
	private class CustomBlockStatement(CustomStatement):
		public Body as Block

	private class CaseStatement(CustomBlockStatement):
		public Pattern as Expression

	private class OtherwiseStatement(CustomBlockStatement):
		pass

	class CaseMacro(LexicalInfoPreservingGeneratorMacro):
		override protected def ExpandGeneratorImpl(case as MacroStatement):
			if len(case.Arguments) != 1:
				raise "Usage: case <pattern>"
			pattern, = case.Arguments
			yield CaseStatement(Pattern: pattern, Body: case.Body)

	class OtherwiseMacro(LexicalInfoPreservingGeneratorMacro):
		override protected def ExpandGeneratorImpl(case as MacroStatement):
			if len(case.Arguments) != 0:
				raise "Usage: otherwise: <block>"
			yield OtherwiseStatement(Body: case.Body)

	private static def ContainsCase(body as Block):
		for stmt in body.Statements:
			return true if stmt isa CaseStatement

	private static def ExpandWithPatternMatching(name as string, body as Block):
		matchBlock = [|
			match $(ReferenceExpression(name)):
				pass
		|]
		statementsEnumerator = body.Statements.GetEnumerator()
		while statementsEnumerator.MoveNext():
			stmt as Statement = statementsEnumerator.Current
			if stmt isa CaseStatement:
				case as CaseStatement = stmt
				caseBlock = [|
					case $(case.Pattern):
						$(case.Body)
				|]
				matchBlock.Body.Add(caseBlock)
			elif stmt isa OtherwiseStatement:
				otherwise as OtherwiseStatement = stmt
				otherwiseBlock = [|
					otherwise:
						$(otherwise.Body)
				|]
				matchBlock.Body.Add(otherwiseBlock)
				
				// otherwise marks the end of the match block
				resultingBlock = Block()
				resultingBlock.Add(matchBlock)
				for remaining as Statement in statementsEnumerator:
					resultingBlock.Add(remaining)
				return resultingBlock
			else:
				// statements after a sequence of 'case <pattern>' macros
				// also mark the end of a match block
				resultingBlock = Block()
				resultingBlock.Add(matchBlock)
				resultingBlock.Add(stmt)
				for remaining as Statement in statementsEnumerator:
					resultingBlock.Add(remaining)
				return resultingBlock
		return matchBlock
	#endregion


	private static def CreateParentMacroAccessor(name as string):
		cacheField = [|
			[System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			private $("$" + name) as Boo.Lang.Compiler.Ast.MacroStatement
		|]
		yield cacheField
		
		cacheFieldRef = ReferenceExpression(cacheField.Name)
		prop = [|
			[System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			private $name:
				get:
					$cacheFieldRef = __macro.GetParentMacroByName($name) unless $cacheFieldRef
					return $cacheFieldRef
		|]
		prop.IsSynthetic = true #avoid BCW0014 if not used
		yield prop


	private final class YieldFinder(DepthFirstVisitor):
		private _found = false

		def constructor(macro as MacroStatement):
			macro.Body.Accept(self)
			
		Found:
			get: return _found
			
		override def OnMethod(node as Method):
			pass
			
		override def OnCustomStatement(node as CustomStatement):
			customBlock = node as CustomBlockStatement
			if customBlock is null:
				super(node)
			else:
				customBlock.Body.Accept(self)

		override def OnYieldStatement(node as YieldStatement):
			_found = true

		override def EnterMacroStatement(node as MacroStatement) as bool:
			return false


	private final class ArgumentsPatternBuilder():
		static final MacroField = ReferenceExpression("__macro")

		_input as ExpressionCollection
		_pattern as ExpressionCollection
		_prologue as Block
		_tss as TypeSystemServices
		_nrs as NameResolutionService
		_arg as ReferenceExpression
		_argIndex = 0
		_enumerable = false

		def constructor(context as CompilerContext, input as ExpressionCollection):
			_tss = context.TypeSystemServices
			_nrs = context.NameResolutionService
			_input = input

		Pattern:
			get:
				Build() if not _pattern
				return _pattern

		Prologue:
			get:
				Build() if not _pattern
				return _prologue

		TypeSystemServices:
			get: return _tss

		NameResolutionService:
			get: return _nrs


		def Build():
			_pattern = ExpressionCollection()
			for arg in _input:
				_enumerable = false
				Append(arg) #FIXME: run-time (duck) dispatch here makes mono cry
				++_argIndex


		private def Append(e as Expression):
			if e.NodeType == NodeType.ReferenceExpression:
				_arg = cast(ReferenceExpression, e)
				Append()
			elif e.NodeType == NodeType.TryCastExpression:
				Append(e as TryCastExpression)
			else:
				raise "Invalid macro argument declaration: `${e}`"


		private def Append(e as TryCastExpression):
			re = e.Target as ReferenceExpression
			raise "Invalid macro argument name: `${e.Target}`" unless re

			NameResolutionService.ResolveTypeReference(e.Type)
			type = TypeSystemServices.GetType(e.Type)
			_arg = cast(ReferenceExpression, re)
			Append(type)


		private def Append():
			#TODO: body arg context => defaults to Node()
			_pattern.Add(SpliceExpression([| $_arg = Boo.Lang.Compiler.Ast.Expression() |]))


		private def Append(type as IType):
			#TODO: body arg context
			if TypeSystemServices.IsAstNode(type):
				AppendNode(type)
			elif TypeSystemServices.IsLiteralPrimitive(type):
				AppendPrimitive(type)
			elif not _enumerable and null != (etype = TypeSystemServices.GetEnumeratorItemType(type)):
				if _argIndex != _input.Count-1:
					raise "Enumerable or array type argument `${_arg.Name}` must be the last argument"
				if _pattern.Count > 0:
					_pattern.Add(SpliceExpression(UnaryExpression(UnaryOperatorType.Explode, [| _ |])))
				_enumerable = true
				Append(etype)
			else:
				raise "Unsupported type `${type.FullName}` for argument `${_arg.Name}`, a macro argument type must be a literal-able primitive or an AST node"


		private def AppendNode(type as IType):
			if not _enumerable:
				_pattern.Add(SpliceExpression([| $_arg = $(ReferenceExpression(type.FullName))() |]))
			else:
				_prologue = GetPrologue(type)


		private def AppendPrimitive(type as IType):
			if type == TypeSystemServices.StringType:
				AppendString()
			elif type == TypeSystemServices.BoolType:
				AppendBool()
			elif type == TypeSystemServices.LongType or type == TypeSystemServices.ULongType:
				AppendLong()
			elif TypeSystemServices.IsIntegerNumber(type):
				AppendInt()
			elif type == TypeSystemServices.SingleType:
				AppendSingle()
			elif type == TypeSystemServices.DoubleType:
				AppendDouble()
			elif type == TypeSystemServices.RegexType:
				AppendRegex()
			elif type == TypeSystemServices.CharType:
				AppendChar()
			elif type == TypeSystemServices.TimeSpanType:
				AppendTimeSpan()
			else:
				raise "Unknown primitive `${type}`"


		private def AppendString():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.StringLiteralExpression(Value: $_arg) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.StringLiteralExpression, string]()

		private def AppendBool():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.BoolLiteralExpression(Value: $_arg) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.BoolLiteralExpression, bool]()

		private def AppendLong():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.IntegerLiteralExpression(Value: $_arg, IsLong: true) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.IntegerLiteralExpression, long]()

		private def AppendInt():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.IntegerLiteralExpression(Value: $_arg, IsLong: false) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.IntegerLiteralExpression, int]()

		private def AppendSingle():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.DoubleLiteralExpression(Value: $_arg, IsSingle: true) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.DoubleLiteralExpression, single]()

		private def AppendDouble():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.DoubleLiteralExpression(Value: $_arg, IsSingle: false) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.DoubleLiteralExpression, double]()

		private def AppendRegex():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.RELiteralExpression(Regex: $_arg) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.RELiteralExpression, regex]()

		private def AppendChar():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.CharLiteralExpression(Value: $_arg) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.CharLiteralExpression, char]()

		private def AppendTimeSpan():
			if not _enumerable:
				_pattern.Add(SpliceExpression([| Boo.Lang.Compiler.Ast.TimeSpanLiteralExpression(Value: $_arg) |]))
			else:
				_prologue = GetPrologue[of Boo.Lang.Compiler.Ast.TimeSpanLiteralExpression, timespan]()


		private def GetPrologue(type as IType):
			return [|
				try:
					$_arg = $(MacroField).Arguments.Cast[of $(Boo.Lang.Compiler.Ast.SimpleTypeReference(type.FullName))]($_argIndex)
				except as System.InvalidCastException:
					raise
			|].ToBlock()

		private def GetPrologue[of TNode, TValue]():
			temp = CreateTemp()
			return [|
				try:
					$temp = $(MacroField).Arguments.Cast[of $TNode]($_argIndex)
					$_arg = Boo.Lang.Compiler.Ast.AstUtil.GetValues[of $TNode, $TValue]($temp)
				except as System.InvalidCastException:
					raise
			|].ToBlock()

		private def CreateTemp():
			return ReferenceExpression("$temp${CompilerContext.Current.AllocIndex()}")

