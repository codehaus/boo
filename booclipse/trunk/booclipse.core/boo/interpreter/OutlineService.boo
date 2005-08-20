namespace booclipse.server

import System
import System.IO
import Boo.Lang.Compiler
import Boo.Lang.Compiler.TypeSystem
import booclipse.core

class OutlineService(AbstractService):
	def constructor(client as ProcessMessengerClient):
		super(client)
		
	def registerMessageHandlers():
		_client.OnMessage("GET-OUTLINE") do (message as Message):
			resetBuffer()
			try:
				compiler = BooCompiler()
				compiler.Parameters.Pipeline = Pipelines.Parse()
				compiler.Parameters.Input.Add(Boo.Lang.Compiler.IO.StringInput("outline", message.Payload))
				module = compiler.Run().CompileUnit.Modules[0]
				module.Accept(OutlineVisitor(_buffer))
			except x:
				Console.Error.WriteLine(x)
				resetBuffer()
			flush("OUTLINE-RESPONSE")	