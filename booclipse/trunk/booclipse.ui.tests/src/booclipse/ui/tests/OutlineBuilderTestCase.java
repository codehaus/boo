package booclipse.ui.tests;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;

import org.apache.commons.io.IOUtils;
import org.eclipse.core.resources.IFile;

import sun.reflect.ReflectionFactory.GetReflectionFactoryAction;

import booclipse.core.outline.OutlineBuilder;
import booclipse.core.outline.OutlineNode;

public class OutlineBuilderTestCase extends AbstractBooTestCase {
	
	public void testGetOutline() throws Exception {
		OutlineBuilder builder = new OutlineBuilder();
		OutlineNode outline = builder.getOutline(loadResourceAsString("Outline.boo"));
		OutlineNode[] children = outline.children();
		assertEquals(2, children.length);
		
		assertEquals("Foo", children[0].name());
		assertEquals(OutlineNode.CLASS, children[0].type());
		assertEquals(3, children[0].line());
		assertEquals("global", children[1].name());
		assertEquals(17, children[1].line());
		assertEquals(OutlineNode.METHOD, children[1].type());
	}

	private String loadResourceAsString(String resource) throws IOException {
		return IOUtils.toString(getResourceStream(resource));
	}

}
