/**
 * 
 */
package booclipse.core.interpreter;

public class InterpreterProposal {
	
	String _entityType;
	
	String _name;
	
	String _description;
	
	public InterpreterProposal(String entityType, String name, String description) {
		_entityType = entityType;
		_name = name;
		_description = description;
	}
	
	public String getEntityType() {
		return _entityType;
	}

	public String getName() {
		return _name;
	}
	
	public String getDescription() {
		return _description;
	}
}