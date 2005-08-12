namespace TestClientRunner

import System
import System.IO
import NUnit.Core
import NUnit.Util
import booclipse.core

// TODO: report errors
class TextWriterListener(EventListener):

	_client as ProcessMessengerClient
	_error as Exception
	
	def constructor(client as ProcessMessengerClient):
		_client = client
		
	def RunFinished(e as Exception):
		_error = e
		
	def RunFinished(r as (TestResult)):
		pass
	
	def RunStarted(t as (Test)):
		pass

	def SuiteFinished(r as TestSuiteResult):
		pass
		
	def SuiteStarted(s as TestSuite):
		pass
		
	def TestStarted(t as TestCase):
		_client.Send("TEST-STARTED", t.FullName)
		_error = null

	def TestFinished(r as TestCaseResult):
		if r.IsFailure:
			Console.Error.WriteLine(r.Message)
			Console.Error.WriteLine(r.StackTrace)
			writer = StringWriter()
			writer.WriteLine(r.Test.FullName)
			writer.WriteLine(r.StackTrace)
			_client.Send("TEST-FAILED", writer.ToString())
		
	def UnhandledException(e as Exception):
		pass

client = ProcessMessengerClient()
client.OnMessage("RUN") do (message as Message):
	try:
		assemblyName = message.Payload.Trim()
		domain = TestDomain(Console.Out, Console.Error)
		domain.Load(assemblyName)
		client.Send("TESTS-STARTED", domain.CountTestCases().ToString())
		domain.Run(TextWriterListener(client))
	except x:
		Console.WriteLine(x)
	client.Send("TESTS-FINISHED", "")
	client.Stop()
	
try:
	client.Start(0xB01)
except x:
	Console.Error.WriteLine(x)