namespace booclipse.core

import System.IO
import System.Net.Sockets

class Message:
	public static final EndMarker = "END-MESSAGE"	
	public Name as string	
	public Payload as string
	
callable MessageHandler(message as Message)
	
transient class ProcessMessengerClient(System.MarshalByRefObject):
	
	_client as TcpClient
	_reader as TextReader
	_writer as TextWriter
	
	_handlers = {}
	
	_running = false
	
	def OnMessage([required] name as string, [required] handler as MessageHandler):
		_handlers.Add(name, handler)
		
	def Start(portNumber as int):
		try:
			using _client = TcpClient("127.0.0.1", portNumber):
				stream = _client.GetStream()
				using _reader = StreamReader(stream), _writer = StreamWriter(stream):
					_running = true
					MessageLoop()
		ensure:			
			_client = null
			_reader = null
			_writer = null
				
	def Stop():
		assert _client is not null
		_client.Close()
		_running = false
				
	def Send([required] message as Message):
		assert _writer is not null
		_writer.WriteLine(message.Name)
		_writer.Write(message.Payload)
		_writer.WriteLine(Message.EndMarker)
		_writer.Flush()

	def Send([required] name as string, [required] payload as string):		
		self.Send(Message(Name: name, Payload: payload))
				
	private def MessageLoop():
		while _running:
			message = ReadMessage()
			break if message is null
			if message.Name == "QUIT":
				Send(message)
				break
			DispatchMessage(message)
			
	private def DispatchMessage(message as Message):
		handler as MessageHandler = _handlers[message.Name]
		return if handler is null
		handler(message)
	
	private def ReadMessage():
		name = _reader.ReadLine()
		writer = StringWriter()
		while true:
			line = _reader.ReadLine()
			return null if line is null
			break if line == Message.EndMarker
			if line.EndsWith(Message.EndMarker):
				writer.WriteLine(line[:-Message.EndMarker.Length])
			else:
				writer.WriteLine(line)
		return Message(Name: name, Payload: writer.ToString())
	
