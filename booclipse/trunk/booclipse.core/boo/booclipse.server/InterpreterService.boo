namespace booclipse.server

import System
import System.IO
import Boo.Lang.Interpreter
import Useful.Attributes
import booclipse.core

class InterpreterService(AbstractService):
	
	_interpreter as InteractiveInterpreter
	
	def constructor(client as ProcessMessengerClient):
		super(client)

//	[once]
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

		_client.OnMessage("GET-INTERPRETER-PROPOSALS") do (message as Message):
			resetBuffer()
			try:
				writeTypeSystemEntities(getInterpreter().SuggestCodeCompletion(message.Payload))
			except x:
				Console.Error.WriteLine(x)
			flush("INTERPRETER-PROPOSALS")
