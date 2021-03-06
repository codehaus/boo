import System.Windows.Forms from System.Windows.Forms
import System.Drawing from System.Drawing

class PropertyEditor(Form):
	def constructor([required] obj):
		grid = PropertyGrid(Dock: DockStyle.Fill,
							SelectedObject: obj)
		
		Controls.Add(grid)
		
class Options:
	
	[property(Message)]
	_message as string
	
	[property(Font)]
	_font as System.Drawing.Font
		
options = Options(Message: "Hello!",
				Font: Font("Lucida Console", 12.0))
editor = PropertyEditor(options)
editor.ShowDialog()

print(options.Message)
