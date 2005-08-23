/*
 * Boo Development Tools for the Eclipse IDE
 * Copyright (C) 2005 Rodrigo B. de Oliveira (rbo@acm.org)
 * 
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307 USA
 */
package booclipse.ui.editors;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.jdt.internal.ui.text.HTMLTextPresenter;
import org.eclipse.jface.text.DefaultInformationControl;
import org.eclipse.jface.text.IAutoEditStrategy;
import org.eclipse.jface.text.IDocument;
import org.eclipse.jface.text.IInformationControl;
import org.eclipse.jface.text.IInformationControlCreator;
import org.eclipse.jface.text.ITextDoubleClickStrategy;
import org.eclipse.jface.text.contentassist.ContentAssistant;
import org.eclipse.jface.text.contentassist.IContentAssistant;
import org.eclipse.jface.text.presentation.IPresentationReconciler;
import org.eclipse.jface.text.presentation.PresentationReconciler;
import org.eclipse.jface.text.reconciler.IReconciler;
import org.eclipse.jface.text.reconciler.MonoReconciler;
import org.eclipse.jface.text.rules.DefaultDamagerRepairer;
import org.eclipse.jface.text.rules.ITokenScanner;
import org.eclipse.jface.text.source.IAnnotationHover;
import org.eclipse.jface.text.source.ISharedTextColors;
import org.eclipse.jface.text.source.ISourceViewer;
import org.eclipse.jface.text.source.SourceViewerConfiguration;
import org.eclipse.swt.SWT;
import org.eclipse.swt.widgets.Shell;

import booclipse.core.compiler.CompilerServices;
import booclipse.ui.BooUI;
import booclipse.ui.views.BooContentAssistProcessor;

public class BooSourceViewerConfiguration extends SourceViewerConfiguration {
	private BooDoubleClickStrategy _doubleClickStrategy;
	private BooScanner _scanner;
	private ISharedTextColors _colorManager;
	private MultiLineCommentScanner _multiLineCommentScanner;
	private StringScanner _tqsScanner;
	private StringScanner _dqsScanner;
	private RegexScanner _regexScanner;
	private ContentAssistant _assistant;

	public BooSourceViewerConfiguration(ISharedTextColors colors) {
		this._colorManager = colors;
	}
	public String[] getConfiguredContentTypes(ISourceViewer sourceViewer) {
		return BooPartitionScanner.PARTITION_TYPES;
	}
	
	public IReconciler getReconciler(ISourceViewer sourceViewer) {
		return new MonoReconciler(new BooReconcilingStrategy(), false);
	}
	
	public IContentAssistant getContentAssistant(ISourceViewer sourceViewer) {
		if (null == _assistant) {
			ContentAssistant assistant = new ContentAssistant();
			try {
				assistant.setContentAssistProcessor(
						new BooEditorContentAssistProcessor(),
						IDocument.DEFAULT_CONTENT_TYPE);
			} catch (CoreException e) {
				BooUI.logException(e);
				return null;
			}
			assistant.enableAutoActivation(true);
			_assistant = assistant;
		}
		_assistant.install(sourceViewer);
		return _assistant;
	}
	
	public IAutoEditStrategy[] getAutoEditStrategies(ISourceViewer sourceViewer, String contentType) {
		if (IDocument.DEFAULT_CONTENT_TYPE == contentType) {
			return new IAutoEditStrategy[] { new BooAutoEditStrategy() };
		}
		return super.getAutoEditStrategies(sourceViewer, contentType);
	}
	
	public ITextDoubleClickStrategy getDoubleClickStrategy(
		ISourceViewer sourceViewer,
		String contentType) {
		if (_doubleClickStrategy == null) {
			_doubleClickStrategy = new BooDoubleClickStrategy();
		}
		return _doubleClickStrategy;
	}

	protected BooScanner getBooScanner() {
		if (_scanner == null) {
			_scanner = new BooScanner(_colorManager);
		}
		return _scanner;
	}
	
	protected MultiLineCommentScanner getMultiLineCommentScanner() {
		if (_multiLineCommentScanner == null) {
			_multiLineCommentScanner = new MultiLineCommentScanner(_colorManager);
		}
		return _multiLineCommentScanner;
	}
	
	protected RegexScanner getRegexScanner() {
		if (_regexScanner == null) {
			_regexScanner = new RegexScanner(_colorManager);
		}
		return _regexScanner;
	}
	
	protected StringScanner getTripleQuotedStringScanner() {
		if (_tqsScanner == null) {
			_tqsScanner = new StringScanner(_colorManager.getColor(BooColorConstants.TRIPLE_QUOTED_STRING));
		}
		return _tqsScanner;
	}
	
	protected StringScanner getDoubleQuotedStringScanner() {
		if (_dqsScanner == null) {
			_dqsScanner = new StringScanner(_colorManager.getColor(BooColorConstants.STRING));
		}
		return _dqsScanner;
	}
	
	public IAnnotationHover getAnnotationHover(ISourceViewer sourceViewer) {
		return new MarkerAnnotationHover();
	}
	
	public String[] getDefaultPrefixes(ISourceViewer sourceViewer, String contentType) {
		if (IDocument.DEFAULT_CONTENT_TYPE == contentType
			|| BooPartitionScanner.SINGLELINE_COMMENT_TYPE == contentType) {
			return new String[] { "//", "#" };
		}
		return null;
	}
	
	public IInformationControlCreator getInformationControlCreator(ISourceViewer sourceViewer) {
		return new IInformationControlCreator() {
			public IInformationControl createInformationControl(Shell parent) {
				return new DefaultInformationControl(parent, SWT.NONE, new HTMLTextPresenter(true));
			}
		};
	}

	public IPresentationReconciler getPresentationReconciler(ISourceViewer sourceViewer) {
		PresentationReconciler reconciler = new PresentationReconciler();

		configureReconciler(reconciler, IDocument.DEFAULT_CONTENT_TYPE, getBooScanner());
		configureReconciler(reconciler, BooPartitionScanner.MULTILINE_COMMENT_TYPE, getMultiLineCommentScanner());		
		configureReconciler(reconciler, BooPartitionScanner.SINGLELINE_COMMENT_TYPE, getMultiLineCommentScanner());
		configureReconciler(reconciler, BooPartitionScanner.DOUBLE_QUOTED_STRING, getDoubleQuotedStringScanner());
		configureReconciler(reconciler, BooPartitionScanner.TRIPLE_QUOTED_STRING, getTripleQuotedStringScanner());
		configureReconciler(reconciler, BooPartitionScanner.REGEX_TYPE, getRegexScanner());

		return reconciler;
	}
	private void configureReconciler(PresentationReconciler reconciler, String partitionType, ITokenScanner scanner) {
		DefaultDamagerRepairer dr;
		dr = new DefaultDamagerRepairer(scanner);
		reconciler.setDamager(dr, partitionType);
		reconciler.setRepairer(dr, partitionType);
	}

}