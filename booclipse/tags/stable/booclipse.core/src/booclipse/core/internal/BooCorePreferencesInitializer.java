package booclipse.core.internal;

import org.eclipse.core.runtime.Preferences;
import org.eclipse.core.runtime.preferences.AbstractPreferenceInitializer;

import booclipse.core.BooCore;

public class BooCorePreferencesInitializer extends
		AbstractPreferenceInitializer {

	public void initializeDefaultPreferences() {
		Preferences preferences = BooCore.getDefault().getPluginPreferences();
		preferences.setDefault(BooCore.P_RUNTIME_LOCATION,
				System.getProperty("MONO_HOME", ""));
	}
}
