using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (MapGenerator))]
public class MapGeneratorEditor : Editor {

	public override void OnInspectorGUI () {

		base.OnInspectorGUI ();

		MapGenerator generator = (MapGenerator) target;
		if (GUILayout.Button ("Show Nodes"))
			generator.ShowNodes ();
		if (GUILayout.Button ("Hide Nodes"))
			generator.HideNodes ();
	}
}
