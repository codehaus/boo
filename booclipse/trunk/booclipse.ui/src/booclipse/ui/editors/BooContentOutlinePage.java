package booclipse.ui.editors;

import org.eclipse.jface.text.BadLocationException;
import org.eclipse.jface.text.IRegion;
import org.eclipse.jface.viewers.ISelectionChangedListener;
import org.eclipse.jface.viewers.IStructuredSelection;
import org.eclipse.jface.viewers.ITreeContentProvider;
import org.eclipse.jface.viewers.LabelProvider;
import org.eclipse.jface.viewers.SelectionChangedEvent;
import org.eclipse.jface.viewers.TreeViewer;
import org.eclipse.jface.viewers.Viewer;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.ui.IEditorInput;
import org.eclipse.ui.texteditor.IDocumentProvider;
import org.eclipse.ui.views.contentoutline.ContentOutlinePage;

import booclipse.core.outline.OutlineNode;

public class BooContentOutlinePage extends ContentOutlinePage {
	
	public class OutlineLabelProvider extends LabelProvider {
		public String getText(Object element) {
			return ((OutlineNode)element).name();
		}
	}

	public class OutlineContentProvider implements ITreeContentProvider {

		public Object[] getChildren(Object parentElement) {
			return ((OutlineNode)parentElement).children();
		}

		public Object getParent(Object element) {
			return ((OutlineNode)element).parent();
		}

		public boolean hasChildren(Object element) {
			return getChildren(element).length > 0;
		}

		public Object[] getElements(Object inputElement) {
			return getChildren(inputElement);
		}

		public void dispose() {
		}

		public void inputChanged(Viewer viewer, Object oldInput, Object newInput) {
		}		
	}

	private IDocumentProvider _documentProvider;
	private OutlineNode _outline;
	private IEditorInput _editorInput;
	private BooEditor _editor;

	public BooContentOutlinePage(IDocumentProvider documentProvider, BooEditor editor) {
		_documentProvider = documentProvider;
		_editor = editor;
	}

	public void setInput(IEditorInput editorInput) {
		_editorInput = editorInput;
	}
	
	private void gotoLine(int line) {
		try {
			BooDocument document = getDocument();
			IRegion info = document.getLineInformation(line);
			_editor.selectAndReveal(info.getOffset(), info.getLength());
		} catch (BadLocationException e) {
			e.printStackTrace();
		}
	}
	
	public void createControl(Composite parent) {
		super.createControl(parent);
		
		setUpOutline();
		
		TreeViewer tree = getTreeViewer();
		tree.setAutoExpandLevel(4);
		tree.setContentProvider(new OutlineContentProvider());
		tree.setLabelProvider(new OutlineLabelProvider());
		tree.setInput(_outline);
		tree.addSelectionChangedListener(new ISelectionChangedListener() {
			public void selectionChanged(SelectionChangedEvent event) {
				Object selected = ((IStructuredSelection) event.getSelection()).getFirstElement();
				if (null == selected) return;
				int line = ((OutlineNode)selected).line()-1;
				gotoLine(line);
			}
		});
	}
	
	void setUpOutline() {
		BooDocument document = getDocument();
		_outline = document.getOutline();
		document.addOutlineListener(new BooDocument.OutlineListener() {
			public void outlineChanged(OutlineNode node) {
				_outline = node;
				
				final TreeViewer tree = getTreeViewer();
				tree.getControl().getDisplay().asyncExec(new Runnable() {
					public void run() {
						tree.setInput(_outline);
					};
				});
			}
		});
	}

	private BooDocument getDocument() {
		return (BooDocument) _documentProvider.getDocument(_editorInput);
	}

}
