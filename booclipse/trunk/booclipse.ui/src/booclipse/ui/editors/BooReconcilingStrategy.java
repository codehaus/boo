package booclipse.ui.editors;

import org.eclipse.jface.text.IDocument;
import org.eclipse.jface.text.IRegion;
import org.eclipse.jface.text.reconciler.DirtyRegion;
import org.eclipse.jface.text.reconciler.IReconcilingStrategy;

public class BooReconcilingStrategy implements IReconcilingStrategy {

	private IDocument _document;

	public void setDocument(IDocument document) {
		_document = document;
	}

	public void reconcile(DirtyRegion dirtyRegion, IRegion subRegion) {
		System.out.println("reconcile1");
	}

	public void reconcile(IRegion partition) {
		System.out.println("reconcile2");
	}

}
