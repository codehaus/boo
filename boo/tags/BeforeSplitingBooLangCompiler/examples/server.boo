import System.IO
import System.Net
import System.Net.Sockets

server = Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
server.Bind(IPEndPoint(IPAddress.Any, 8080))
server.Listen(1)

while true:
	socket = server.Accept()
	using stream=NetworkStream(socket, true):
		print(StreamReader(stream).ReadLine())

		writer=StreamWriter(stream)
		writer.WriteLine("pong!")
		writer.Flush()

	
