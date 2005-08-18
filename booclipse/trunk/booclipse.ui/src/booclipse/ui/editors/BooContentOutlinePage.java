package booclipse.ui.editors;

import org.eclipse.jface.viewers.ITreeContentProvider;
import org.eclipse.jface.viewers.LabelProvider;
import org.eclipse.jface.viewers.TreeViewer;
import org.eclipse.jface.viewers.Viewer;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.ui.IEditorInput;
import org.eclipse.ui.texteditor.IDocumentProvider;
import org.eclipse.ui.views.contentoutline.ContentOutlinePage;

public class BooContentOutlinePage extends ContentOutlinePage {
	
	public class OutlineLabelProvider extends LabelProvider {
		public String getText(Object element) {
			return element.toString();
		}
	}

	public class OutlineContentProvider implements ITreeContentProvider {

		public Object[] getChildren(Object parentElement) {
			return null;
		}

		public Object getParent(Object element) {
			// TODO Auto-generated method stub
			return null;
		}

		public boolean hasChildren(Object element) {
			// TODO Auto-generated method stub
			return false;
		}

		public Object[] getElements(Object inputElement) {
			return new String[] { "foo", "bar" };
		}

		public void dispose() {
			// TODO Auto-generated method stub
			
		}

		public void inputChanged(Viewer viewer, Object oldInput, Object newInput) {
			// TODO Auto-generated method stub
			
		}		
	}

	public BooContentOutlinePage(IDocumentProvider documentProvider, BooEditor editor) {
	}

	public void setInput(IEditorInput editorInput) {
		// TODO Auto-generated method stub
		
	}
	
	public void createControl(Composite parent) {
		super.createControl(parent);
		
		TreeViewer tree = getTreeViewer();
		tree.setContentProvider(new OutlineContentProvider());
		tree.setLabelProvider(new OutlineLabelProvider());
		tree.setInput(new Object());
	}

}
