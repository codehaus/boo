package ITL.WebTools.Controls

using System
using System.Web
using System.Web.UI
using System.Web.UI.WebControls
using Boo.Web


[ParseChildren(ChildrenAsProperties: true)]
class MediaViewer(Control, INamingContainer):
"""
<summary>
Cria a tag HTML apropriada para visualização de qualquer tipo
de arquivo.
</summary>
"""
	[Template(Container: typeof(MediaViewer))]
	FlashTemplate			

	[ViewState]
	Path as string

	[ViewState(Default: 70)]
	Width as int

	[ViewState(Default: 70)]
	Height as int
	
	def OnDataBinding(e):
		super(e)
		EnsureChildControls()
	
	def CreateChildControls():
		super()
		
		return unless Path
		
		extension = System.IO.Path.GetExtension(path).ToLower()
		given extension:
			when ".jpg", ".gif", ".bmp", ".png":
				Controls.Add(LiteralControl("<img src='${Path}' width='${Width}' height='${Height}' />"))
				
			when ".swf":
				if FlashTemplate:
					FlashTemplate.InstantiateIn(self)
				else:
					Controls.Add(
						LiteralControl("<embed src='${Path}' quality='low' width='${Width}' height='${Height}'></embed>")
						)
						
			otherwise:
				Controls.Add(LiteralControl("<b>A extensão ${extension} não é suportada.</b>"))
