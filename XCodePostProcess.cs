using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
using System.IO;

public static class XCodePostProcess
{
    [PostProcessBuild(100)]
	public static void OnPostProcessBuild( BuildTarget target, string path )
	{
		if (target != BuildTarget.iPhone) {
			Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

        Debug.Log("Executing XUPorter post processor...");

        XCodePostProcess.Log(" >>> Opening XCode project...");

		// Create a new project object from build target
		XCProject project = new XCProject( path );

		// Find and run through all projmods files to patch the project.
		//Please pay attention that ALL projmods files in your project folder will be excuted!
		string[] files = Directory.GetFiles( Application.dataPath, "*.projmods", SearchOption.AllDirectories );
		foreach( string file in files ) {
            XCodePostProcess.Log(" >>> Applying mod: " + file);
			project.ApplyMod( file );
		}

        XCodePostProcess.Log(" >>> Saving XCode project...");

		// Finally save the xcode project
		project.Save();
	}

    static bool DEBUG = false;

    public static void Log(string msg)
    {
        if (DEBUG)
            Debug.Log(msg);
    }
}
