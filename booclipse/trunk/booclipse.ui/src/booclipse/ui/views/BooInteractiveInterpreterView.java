package booclipse.ui.views;


import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

import org.eclipse.core.runtime.CoreException;
import org.eclipse.jface.action.Action;
import org.eclipse.jface.action.IMenuListener;
import org.eclipse.jface.action.IMenuManager;
import org.eclipse.jface.action.IToolBarManager;
import org.eclipse.jface.action.MenuManager;
import org.eclipse.jface.action.Separator;
import org.eclipse.jface.dialogs.MessageDialog;
import org.eclipse.jface.resource.JFaceResources;
import org.eclipse.jface.text.Document;
import org.eclipse.jface.text.IDocument;
import org.eclipse.jface.text.ITextViewer;
import org.eclipse.jface.text.TextViewer;
import org.eclipse.jface.text.contentassist.CompletionProposal;
import org.eclipse.jface.text.contentassist.ContentAssistant;
import org.eclipse.jface.text.contentassist.ICompletionProposal;
import org.eclipse.jface.text.contentassist.IContentAssistProcessor;
import org.eclipse.jface.text.contentassist.IContextInformation;
import org.eclipse.jface.text.contentassist.IContextInformationValidator;
import org.eclipse.swt.SWT;
import org.eclipse.swt.custom.StyledText;
import org.eclipse.swt.custom.VerifyKeyListener;
import org.eclipse.swt.events.VerifyEvent;
import org.eclipse.swt.graphics.Image;
import org.eclipse.swt.widgets.Composite;
import org.eclipse.ui.IActionBars;
import org.eclipse.ui.ISharedImages;
import org.eclipse.ui.IWorkbenchActionConstants;
import org.eclipse.ui.PlatformUI;
import org.eclipse.ui.part.ViewPart;

import booclipse.core.compiler.CompilerProposal;
import booclipse.core.interpreter.IInterpreterListener;
import booclipse.core.interpreter.InteractiveInterpreter;
import booclipse.ui.BooUI;
import booclipse.ui.IBooUIConstants;

public class BooInteractiveInterpreterView extends ViewPart {
	
	public static final String ID_VIEW = "booclipse.ui.views.BooInteractiveInterpreterView";
	
	Action _actionReset;
	Action action2;
	
	private TextViewer _text;
	
	InteractiveInterpreter _interpreter;
		
	/**
	 * The constructor.
	 */
	public BooInteractiveInterpreterView() {
	}

	/**
	 * This is a callback that will allow us
	 * to create the viewer and initialize it.
	 */
	public void createPartControl(Composite parent) {
		
		try {
			_interpreter = new InteractiveInterpreter();
		} catch (CoreException x) {
			BooUI.logException(x);
			return;
		}
		
		_text = new TextViewer(parent, SWT.FULL_SELECTION|SWT.MULTI/*|SWT.BORDER*/|SWT.V_SCROLL);
		_text.setDocument(new Document());
		
		final StyledText textWidget = _text.getTextWidget();
		textWidget.setFont(JFaceResources.getTextFont());
		_text.prependVerifyKeyListener(new VerifyKeyListener() {
			public void verifyKey(VerifyEvent e) {
				if (e.character == '\r') {
					e.doit = false;
					try {
						final String currentLine = getCurrentLine();
						_interpreter.eval(currentLine);
						_text.setEditable(false);
					} catch (Exception x) {
						BooUI.logException(x);
					}					
				}
			}
		});
		
		_interpreter.addListener(new IInterpreterListener() {
			public void evalFinished(final String result) {
				textWidget.getDisplay().asyncExec(new Runnable() {
					public void run() {
						prompt(result);
					}
				});
			}
		});
		
		final ContentAssistant assistant = new ContentAssistant();
		assistant.setContentAssistProcessor(
				new InterpreterContentAssistProcessor(),
				IDocument.DEFAULT_CONTENT_TYPE);

		assistant.install(_text);
		assistant.enableAutoActivation(true);
		
		/*
		_text.getControl().addKeyListener(new KeyAdapter() {
			public void keyPressed(KeyEvent e) {
				switch (e.keyCode) {
				case SWT.F1:
					assistant.showPossibleCompletions();
				break;
				default:
					//ignore everything else
					}
				}
			});*/
		
		makeActions();
		hookContextMenu();
		contributeToActionBars();
	}
	
	protected String getCurrentLine() {
		return getLineAtOffset(_text.getTextWidget().getCaretOffset());
	}

	private String getLineAtOffset(final int offset) {
		final StyledText widget = _text.getTextWidget();
		final int line = widget.getLineAtOffset(offset);
		return widget.getContent().getLine(line);
	}

	public void dispose() {
		if (null != _interpreter) {
			_interpreter.dispose();
			_interpreter = null;
		}
		super.dispose();
	}

	private void hookContextMenu() {
		MenuManager menuMgr = new MenuManager("#PopupMenu");
		menuMgr.setRemoveAllWhenShown(true);
		menuMgr.addMenuListener(new IMenuListener() {
			public void menuAboutToShow(IMenuManager manager) {
				BooInteractiveInterpreterView.this.fillContextMenu(manager);
			}
		});
		//Menu menu = menuMgr.createContextMenu(viewer.getControl());
		//viewer.getControl().setMenu(menu);
		//getSite().registerContextMenu(menuMgr, viewer);
	}

	private void contributeToActionBars() {
		IActionBars bars = getViewSite().getActionBars();
		fillLocalPullDown(bars.getMenuManager());
		fillLocalToolBar(bars.getToolBarManager());
	}

	private void fillLocalPullDown(IMenuManager manager) {
		manager.add(_actionReset);
		manager.add(new Separator());
		manager.add(action2);
	}

	private void fillContextMenu(IMenuManager manager) {
		manager.add(_actionReset);
		manager.add(action2);
		// Other plug-ins can contribute there actions here
		manager.add(new Separator(IWorkbenchActionConstants.MB_ADDITIONS));
	}
	
	private void fillLocalToolBar(IToolBarManager manager) {
		manager.add(_actionReset);
		manager.add(action2);
	}

	private void makeActions() {
		_actionReset = new Action() {
			public void run() {
				if (null != _interpreter) {
					_interpreter.unload();
				}
				prompt("interpreter reset");
			}
		};
		_actionReset.setText("Reset Interpreter");
		_actionReset.setToolTipText("resets the interpreter");
		_actionReset.setImageDescriptor(PlatformUI.getWorkbench().getSharedImages().
			getImageDescriptor(ISharedImages.IMG_OBJS_WARN_TSK));
		
		action2 = new Action() {
			public void run() {
				showMessage("Action 2 executed");
			}
		};
		action2.setText("Action 2");
		action2.setToolTipText("Action 2 tooltip");
		action2.setImageDescriptor(PlatformUI.getWorkbench().getSharedImages().
				getImageDescriptor(ISharedImages.IMG_OBJS_INFO_TSK));
	}

	private void showMessage(String message) {
		MessageDialog.openInformation(
			getSite().getShell(),
			"Boo Interactive Interpreter",
			message);
	}

	/**
	 * Passing the focus request to the viewer's control.
	 */
	public void setFocus() {
		_text.getControl().setFocus();
	}

	private void prompt(String message) {
		StyledText widget = _text.getTextWidget();
		widget.setSelection(0, 0);
		widget.insert(message);
		widget.insert("\n");
		widget.setCaretOffset(0);
		widget.setEditable(true);
	}
	
	class InterpreterContentAssistProcessor implements IContentAssistProcessor {
		
		Map _imageMap = new HashMap();
		private CompilerProposal[] _cachedProposals;
		private String _cachedLine;
		
		public InterpreterContentAssistProcessor() {
			mapImage("Class", IBooUIConstants.CLASS);
			mapImage("Method", IBooUIConstants.METHOD);
			mapImage("Constructor", IBooUIConstants.METHOD);
			mapImage("Field", IBooUIConstants.FIELD);
			mapImage("Property", IBooUIConstants.PROPERTY);
			mapImage("Event", IBooUIConstants.EVENT);
			mapImage("Namespace", IBooUIConstants.NAMESPACE);
			mapImage("Interface", IBooUIConstants.INTERFACE);
			mapImage("Callable", IBooUIConstants.CALLABLE);
			mapImage("Struct", IBooUIConstants.STRUCT);
			mapImage("Enum", IBooUIConstants.ENUM);
		}
		
		void mapImage(String entityType, String key) {
			_imageMap.put(entityType, BooUI.getImage(key));
		}

		public ICompletionProposal[] computeCompletionProposals(ITextViewer viewer, int offset) {
			String line = getLineAtOffset(offset);
			
			ICompletionProposal[] proposals = getFromCache(line, offset);
			if (null != proposals) return proposals;
			
			try {
				CompilerProposal[] found = _interpreter.getCompletionProposals(line, offset);
				proposals = newCompletionProposalArray("", offset, found);
				_cachedLine = line;
				_cachedProposals = found;
				return proposals;
			} catch (IOException e) {
				BooUI.logException(e);
			}
			return new ICompletionProposal[0];
		}

		private ICompletionProposal[] newCompletionProposalArray(String existingPrefix, int offset, CompilerProposal[] found) {
			ICompletionProposal[] proposals;
			proposals = new ICompletionProposal[found.length];
			for (int i=0; i<found.length; ++i) {
				proposals[i] = newCompletionProposal(existingPrefix, offset, found[i]);
			}
			return proposals;
		}

		private CompletionProposal newCompletionProposal(String existingPrefix, int offset, CompilerProposal proposal) {
			String name = proposal.getName();
			final String description = isMember(proposal.getEntityType())
				? proposal.getDescription()
				: name;
			String completion = name.substring(existingPrefix.length());
			CompletionProposal completionProposal = new CompletionProposal(completion, offset, 0, completion.length(), getImage(proposal), massageDescription(description), null, description);
			return completionProposal;
		}

		private boolean isMember(String entityType) {
			return entityType.equals("Method")
			|| entityType.equals("Field")
			|| entityType.equals("Property")
			|| entityType.equals("Event");
		}

		private ICompletionProposal[] getFromCache(String line, int offset) {
			if (null == _cachedLine) return null;
			if (line.endsWith(".")) {
				return line.equals(_cachedLine) ? newCompletionProposalArray("", 0, _cachedProposals) : null;
			}
			int lastDot = line.lastIndexOf('.');
			String prefix = line.substring(0, lastDot+1);
			if (!_cachedLine.equals(prefix)) return null;
			return filterCachedProposals(line.substring(lastDot+1), offset);
		}

		private ICompletionProposal[] filterCachedProposals(String prefix, int offset) {
			ArrayList filtered = new ArrayList();
			for (int i=0; i<_cachedProposals.length; ++i) {
				CompilerProposal item = _cachedProposals[i];
				String name = item.getName();
				if (name.startsWith(prefix)) {
					filtered.add(newCompletionProposal(prefix, offset, item));
				}
			}
			return (ICompletionProposal[]) filtered.toArray(new ICompletionProposal[filtered.size()]);
		}

		private Image getImage(CompilerProposal proposal) {
			return (Image)_imageMap.get(proposal.getEntityType());
		}

		private String massageDescription(String description) {
			return removePrefix(removePrefix(removePrefix(description, "static "), "event "), "def ");
		}
		
		private String removePrefix(String s, String prefix) {
			if (s.startsWith(prefix)) return s.substring(prefix.length());
			return s;
		}

		public IContextInformation[] computeContextInformation(ITextViewer viewer, int offset) {
			// TODO Auto-generated method stub
			return null;
		}

		public char[] getCompletionProposalAutoActivationCharacters() {
			return new char[] { '.' };
		}

		public char[] getContextInformationAutoActivationCharacters() {
			return new char[0];
		}

		public String getErrorMessage() {
			// TODO Auto-generated method stub
			return null;
		}

		public IContextInformationValidator getContextInformationValidator() {
			// TODO Auto-generated method stub
			return null;
		}
		
	}
}