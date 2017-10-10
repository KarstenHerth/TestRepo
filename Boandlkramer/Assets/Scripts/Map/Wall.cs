using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// Type determines the kind of wall that is represented here.
	public enum WallType { None, Wall, Door }
	public WallType type = WallType.None;

	// The node this wall is attached to.
	public Node node;
	// The connectors attached to this wall segment.
	public Connector[] connectors = new Connector[2];
	// The actual GameObject visually representing this wall.
	public GameObject wall;

	// On creation one needs to find neighboring wall pieces and connect to them via the connectors.
	public void Initialize (MapGenerator generator) {
		if (type == WallType.None)
			return;
		foreach (Connector connector in connectors) {
			Connector other = Array.Find (generator.GetConnectors (), (x => (x != connector && connector.IsColliding (x))));
			if (other != null)
				MergeConnectors (connector, other);
		}			
	}

	// Returns the direction of the associated object.
	public Vector3 GetDirection () {
		return transform.rotation.eulerAngles;
	}

	// Check for collision with other.
	public bool IsColliding<T> (T other) where T : MonoBehaviour {
		return (transform.position - other.transform.position).sqrMagnitude < 0.1f;
	}

	// Make two wall segments share a connector.
	public void MergeConnectors (Connector con_a, Connector con_b) {
		int index_a = Array.IndexOf (connectors, con_a);
		connectors[index_a] = con_b;
		con_b.type = Connector.ConnectorType.Shared;
		con_b.walls.Add (this);
		Destroy (con_a.gameObject);
	}

	// Create visual representation for the wall segment.
	public void Show (Tileset tileset) {
		if (wall != null)
			return;
		switch (type) {
			case WallType.Wall:
				wall = Instantiate (tileset.wall, transform.position, transform.rotation) as GameObject;
				wall.transform.SetParent (transform);
				break;
			case WallType.Door:
				wall = Instantiate (tileset.door, transform.position, transform.rotation) as GameObject;
				wall.transform.SetParent (transform);
				break;
		}
		foreach (Connector connector in connectors)
			connector.Show (tileset);
	}
}
