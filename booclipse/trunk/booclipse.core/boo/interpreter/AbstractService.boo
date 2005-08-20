namespace booclipse.server

import System.IO
import booclipse.core

class AbstractService:
	
	_client as ProcessMessengerClient
	_buffer = StringWriter()
	
	def constructor(client as ProcessMessengerClient):
		_client = client
		registerMessageHandlers()
	
	def writeLine(line):
		_buffer.WriteLine(line)
		
	def resetBuffer():
		_buffer.GetStringBuilder().Length = 0
		
	def flush(name as string):
		_client.Send(Message(Name: name, Payload: _buffer.ToString()))
		
	abstract def registerMessageHandlers():
		pass
