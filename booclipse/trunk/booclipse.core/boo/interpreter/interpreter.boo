import System
import System.IO
import Boo.Lang.Compiler
import Boo.Lang.Compiler.Ast
import Boo.Lang.Compiler.TypeSystem
import Boo.Lang.Interpreter
import booclipse.core

class Application:
	_client = ProcessMessengerClient()
	_buffer = StringWriter()
	_interpreter as InteractiveInterpreter
	
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
		
	def getInterpreter():
		if _interpreter is null:
			_interpreter = InteractiveInterpreter(RememberLastValue: true, Print: writeLine)		
		return _interpreter

	def run(portNumber as int):	
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
	
		_client.Start(portNumber)
		
class OutlineVisitor(DepthFirstVisitor):
	
	_writer as TextWriter
	
	def constructor(writer as TextWriter):
		_writer = writer
		
	override def OnClassDefinition(node as ClassDefinition):
		_writer.WriteLine("BEGIN-NODE")
		WriteTypeMember(node)
		Visit(node.Members)
		_writer.WriteLine("END-NODE")
		
	override def OnMethod(node as Method):
		_writer.WriteLine("BEGIN-NODE")
		WriteTypeMember(node)
		_writer.WriteLine("END-NODE")
		
	def WriteTypeMember(node as TypeMember):
		_writer.WriteLine("${node.NodeType}:${node.Name}:${node.LexicalInfo.Line}")
		
portNumber, = argv
Application().run(int.Parse(portNumber))
