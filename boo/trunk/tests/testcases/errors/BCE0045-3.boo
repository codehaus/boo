"""
BCE0045-3.boo(18,1): BCE0045: Macro expansion error: `foo` macro invocation argument(s) did not match definition: `foo((x as string))`.
BCE0045-3.boo(22,1): BCE0045: Macro expansion error: `bar` macro invocation argument(s) did not match definition: `bar((x as bool))`.
BCE0045-3.boo(24,1): BCE0045: Macro expansion error: `invoke` macro invocation argument(s) did not match definition: `invoke((mie as Boo.Lang.Compiler.Ast.MethodInvocationExpression))`.
"""
import Boo.Lang.Compiler.Ast
import Boo.Lang.PatternMatching

macro foo(x as string):
	pass

macro bar(x as bool):
	pass

macro invoke(mie as MethodInvocationExpression):
	pass

foo 31
foo "ok"

bar true
bar 1

invoke "foo".Length > 2
invoke Console.WriteLine("foo")

