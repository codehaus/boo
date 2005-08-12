package booclipse.core.foundation;

import java.lang.reflect.Array;

public class ArrayUtilities {

	public static Object[] append(Object[] sourceArray, Object newElement) {
		return append(sourceArray.getClass().getComponentType(), sourceArray, newElement);
	}

	public static Object[] append(Class componentType, Object[] sourceArray, Object newElement) {
		Object[] newArray = copySourceArray(componentType, sourceArray, true);
		newArray[newArray.length-1] = newElement;
		return newArray;
	}

	private static Object[] copySourceArray(Class componentType, Object[] sourceArray, boolean append) {
		Object[] newArray = (Object[])Array.newInstance(componentType, sourceArray.length + 1);
		System.arraycopy(sourceArray, 0, newArray, append ? 0 : 1, sourceArray.length);
		return newArray;
	}

	public static Object[] prepend(Class componentType, Object[] sourceArray, Object newElement) {
		Object[] newArray = copySourceArray(componentType, sourceArray, false);
		newArray[0] = newElement;
		return newArray;
	}

	public static Object[] replace(Class newComponentType, Object[] sourceArray, int index, Object element) {
		try {
			sourceArray[index] = element; 
		} catch (ArrayStoreException e) {
			Object[] newArray = (Object[])Array.newInstance(newComponentType, sourceArray.length);
			System.arraycopy(sourceArray, 0, newArray, 0, sourceArray.length);
			newArray[index] = element;
			return newArray;
		}
		return sourceArray;
	}

	public static String join(String[] items) {
		StringBuffer builder = new StringBuffer();
		for (int i=0; i<items.length; ++i) {
			if (i > 0) builder.append(" ");
			builder.append(items[i]);
		}
		return builder.toString();
	}
}
