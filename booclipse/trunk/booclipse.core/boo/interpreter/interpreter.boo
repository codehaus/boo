import System
import System.IO
import Boo.Lang.Compiler.TypeSystem
import Boo.Lang.Interpreter
import booclipse.core

// TODO: pensar em um OutlineProcessor	
def getEntityType(entity as IEntity):
	if EntityType.Type == entity.EntityType:
		type = entity as IType
		return "Interface" if type.IsInterface
		return "Enum" if type.IsEnum
		return "Struct" if type.IsValueType
		return "Callable" if type isa ICallableType
		return "Class"
	return entity.EntityType.ToString()

client = ProcessMessengerClient()
buffer = StringWriter()

writeLine = def(line):
	buffer.WriteLine(line)
	
resetBuffer = def():
	buffer.GetStringBuilder().Length = 0
	
send = def(name as string, payload as string):
	client.Send(Message(Name: name, Payload: payload))
	
interpreter = InteractiveInterpreter(RememberLastValue: true, Print: writeLine)
client.OnMessage("EVAL") do (message as Message):
	resetBuffer()
	try:
		interpreter.LoopEval(message.Payload)
	except x:
		writeLine(x)
	send("EVAL-FINISHED", buffer.ToString())

client.OnMessage("GET-PROPOSALS") do (message as Message):
	resetBuffer()
	try:
		entities = interpreter.SuggestCodeCompletion(message.Payload)
		for member in entities:
			buffer.WriteLine("${getEntityType(member)}:${member.Name}:${InteractiveInterpreter.DescribeEntity(member)}")
	except x:
		Console.Error.WriteLine(x)
	send("PROPOSALS", buffer.ToString())
	
client.Start(0xB00)
