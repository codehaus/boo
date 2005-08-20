namespace booclipse.server

import System
import System.IO
import Boo.Lang.Compiler
import Boo.Lang.Compiler.TypeSystem
import Boo.Lang.Interpreter
import booclipse.core

class InterpreterService(AbstractService):
	
	_interpreter as InteractiveInterpreter
	
	def constructor(client as ProcessMessengerClient):
		super(client)
		
	def getEntityType(entity as IEntity):
		if EntityType.Type == entity.EntityType:
			type = entity as IType
			return "Interface" if type.IsInterface
			return "Enum" if type.IsEnum
			return "Struct" if type.IsValueType
			return "Callable" if type isa ICallableType
			return "Class"
		return entity.EntityType.ToString()
		
	def getInterpreter():
		if _interpreter is null:
			_interpreter = InteractiveInterpreter(RememberLastValue: true, Print: writeLine)		
		return _interpreter

	def registerMessageHandlers():	
		_client.OnMessage("EVAL") do (message as Message):
			resetBuffer()
			try:
				getInterpreter().LoopEval(message.Payload)
			except x:
				writeLine(x)
			flush("EVAL-FINISHED")

		_client.OnMessage("GET-PROPOSALS") do (message as Message):
			resetBuffer()
			try:
				entities = getInterpreter().SuggestCodeCompletion(message.Payload)
				for member in entities:
					writeLine("${getEntityType(member)}:${member.Name}:${InteractiveInterpreter.DescribeEntity(member)}")
			except x:
				Console.Error.WriteLine(x)
			flush("PROPOSALS")
