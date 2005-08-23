namespace booclipse.server

import booclipse.core

portNumber, = argv
client = ProcessMessengerClient()
InterpreterService(client)
CompilerService(client)
client.Start(int.Parse(portNumber))
