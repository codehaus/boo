package booclipse.core.outline;

import java.util.ArrayList;

public class OutlineNode {
	
	public static final OutlineNode[] NO_CHILDREN = new OutlineNode[0];
	
	public static final String CLASS = "ClassDefinition";
	
	public static final String METHOD = "Method";

	private String _name;

	private ArrayList _children;
	
	private OutlineNode _parent;

	private String _type;

	private int _line;

	public OutlineNode() {
	}	
	
	private OutlineNode(OutlineNode parent) {
		_parent = parent;
	}
	
	public OutlineNode create() {
		OutlineNode child = new OutlineNode(this);
		add(child);
		return child;
	}
	
	private void add(OutlineNode child) {
		if (null == _children) _children = new ArrayList();
		_children.add(child);
	}

	public OutlineNode parent() {
		return _parent;
	}

	public OutlineNode[] children() {
		if (null == _children) return NO_CHILDREN;
		return (OutlineNode[]) _children.toArray(new OutlineNode[_children.size()]);
	}

	public String name() {
		return _name;
	}
	
	public void name(String name) {
		_name = name;
	}
	
	public void type(String type) {
		_type = type;
	}

	public String type() {
		return _type;
	}

	public int line() {
		return _line;
	}
	
	public void line(int line) {
		_line = line;
	}
}
