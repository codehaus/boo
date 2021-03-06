import System.Xml
import System.IO
import Useful.IO from Boo.Lang.Useful

class XmlDocumentWrapper:
	
	_document as XmlDocument
	
	def constructor(document as XmlDocument):
		_document = document
		
	def constructor(fname as string):
		_document = XmlDocument()
		_document.Load(fname)
	
	NamespaceURI:
		get:
			return _document.DocumentElement.Attributes["xmlns"].Value
			
	BaseURI:
		get:
			return System.Uri(_document.BaseURI)
			
	AbsolutePath:
		get:
			return BaseURI.AbsolutePath
			
	def Save():
		_document.Save(AbsolutePath)

class VS2005Project(XmlDocumentWrapper):
	
	static def Load(fname as string):
		doc = XmlDocument()
		doc.Load(fname)
		
		compileItem = doc.SelectSingleNode("//*[local-name()='ItemGroup']/*[local-name()='Compile']")
		compileItemGroup = compileItem.ParentNode
		return VS2005Project(doc, compileItemGroup)

	_compileItemGroup as XmlElement
	
	def constructor(document as XmlDocument, compileItemGroup as XmlElement):
		super(document)
		_compileItemGroup = compileItemGroup
		
	def ResetCompileItemGroup():
		_compileItemGroup.RemoveAll()
		
	def AddCompileItem(item as string):
		compile = _document.CreateElement("Compile", NamespaceURI)
		compile.SetAttribute("Include", item.Replace('/', '\\'))
		_compileItemGroup.AppendChild(compile)		
		
class ResxFile(XmlDocumentWrapper):
	def constructor(fname as string):
		super(fname)
		
	def ResetData():
		for node as XmlNode in _document.SelectNodes("//*[local-name()='data']"):
			node.ParentNode.RemoveChild(node)	
			
	def AddData(key as string, value as string):
		data = _document.CreateElement("data")
		data.SetAttribute("xml:space", "preserve")
		data.SetAttribute("name", key)
		valueNode = _document.CreateElement("value")
		valueNode.InnerText = value
		data.AppendChild(valueNode)
		_document.DocumentElement.AppendChild(data)
		
def updateProjectFile(fname as string):
	project = VS2005Project.Load(fname)
	project.ResetCompileItemGroup()
	
	baseURI = project.BaseURI
	for item as string in listFiles(Path.GetDirectoryName(baseURI.AbsolutePath)):
		if item =~ /\.svn/: continue
		if not item.EndsWith(".cs"): continue
		uri = baseURI.MakeRelative(System.Uri(item))
		project.AddCompileItem(uri)
	project.Save()
	
	print project.AbsolutePath
	
	
def updateStringResources(txtFile as string):
	resourceFile = ResxFile(Path.ChangeExtension(txtFile, ".resx"))
	resourceFile.ResetData()
	
	using file=File.OpenText(txtFile):
		for line in file:
			if line.StartsWith(";"): continue
			line = line.Trim()
			if len(line) == 0: continue
			index = line.IndexOf('=')
			key = line[:index]
			value = line[index+1:]
			
			resourceFile.AddData(key, value)
			
	resourceFile.Save()
	
	print resourceFile.AbsolutePath
	
def rebase(fname as string):
	#return Path.Combine("c:/projects/boo/", fname)
	return fname

fnames = (
"src/Boo.Lang/Boo.Lang-VS2005.csproj",
"src/Boo.Lang.Parser/Boo.Lang.Parser-VS2005.csproj",
"src/Boo.Lang.Compiler/Boo.Lang.Compiler-VS2005.csproj",
"tests/BooCompiler.Tests/BooCompiler.Tests-VS2005.csproj"
)
for fname in fnames:
	updateProjectFile(rebase(fname))
	
updateStringResources(rebase("src/Boo.Lang/Resources/strings.txt"))
