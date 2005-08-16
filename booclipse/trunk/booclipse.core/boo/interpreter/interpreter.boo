import System
import System.IO
import Boo.Lang.Compiler.TypeSystem
import Boo.Lang.Interpreter
import booclipse.core

class Application:
	_client = ProcessMessengerClient()
	_buffer = StringWriter()
	
	def writeLine(line):
		_buffer.WriteLine(line)
		
	def resetBuffer():
		_buffer.GetStringBuilder().Length = 0
		
	def flush(name as string):
		_client.Send(Message(Name: name, Payload: _buffer.ToString()))
		
	def getEntityType(entity as IEntity):
		if EntityType.Type == entity.EntityType:
			type = entity as IType
			return "Interface" if type.IsInterface
			return "Enum" if type.IsEnum
			return "Struct" if type.IsValueType
			return "Callable" if type isa ICallableType
			return "Class"
		return entity.EntityType.ToString()

	def run():	
		interpreter = InteractiveInterpreter(RememberLastValue: true, Print: writeLine)
		_client.OnMessage("EVAL") do (message as Message):
			resetBuffer()
			try:
				interpreter.LoopEval(message.Payload)
			except x:
				writeLine(x)
			flush("EVAL-FINISHED")

		_client.OnMessage("GET-PROPOSALS") do (message as Message):
			resetBuffer()
			try:
				entities = interpreter.SuggestCodeCompletion(message.Payload)
				for member in entities:
					writeLine("${getEntityType(member)}:${member.Name}:${InteractiveInterpreter.DescribeEntity(member)}")
			except x:
				Console.Error.WriteLine(x)
			flush("PROPOSALS")
	
		_client.Start(0xB00)
		
Application().run()
