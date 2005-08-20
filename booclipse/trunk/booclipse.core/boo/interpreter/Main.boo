namespace booclipse.server

import booclipse.core

portNumber, = argv
client = ProcessMessengerClient()
InterpreterService(client)
OutlineService(client)
client.Start(int.Parse(portNumber))
