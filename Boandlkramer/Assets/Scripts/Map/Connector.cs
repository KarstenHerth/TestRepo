using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour {

	// Type determines whether connector is in use or not.
	public enum ConnectorType { None, Shared } //, Inner, Outer, Straight }
	public ConnectorType type = ConnectorType.None;

	// List of attached wall segments.
	public List<Wall> walls = new List<Wall> ();
	// Columns represent connectors.
	public GameObject column;

	// Check for collision with other.
	public bool IsColliding<T> (T other) where T : MonoBehaviour {
		return (transform.position - other.transform.position).sqrMagnitude < 0.1f;
	}

	// Create visual representation.
	public void Show (Tileset tileset) {
		if (column != null)
			return;
		if (type == ConnectorType.Shared) {
			column = Instantiate (tileset.column, transform.position, transform.rotation) as GameObject;
			column.transform.SetParent (transform);
		}
	}
}
