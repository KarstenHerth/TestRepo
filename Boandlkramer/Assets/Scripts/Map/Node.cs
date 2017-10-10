using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

	// Type determines the possible attachments at this node. Shared if its between two tiles.
	public enum NodeType { None, Hallway, Room, Shared }
	public NodeType type = NodeType.None;

	// The tiles that neighbor this node.
	public List<Tile> tiles = new List<Tile> ();
	// The wall that is attached to this node.
	public Wall wall;

	// The object that visually represents the node.
	GameObject m_show;

	// Get the rotation of the object this script is attached to.
	public Vector3 GetDirection () {
		return transform.rotation.eulerAngles;
	}

	// Check wether this object is close to a certain other one.
	public bool IsColliding<T> (T other) where T : MonoBehaviour {
		return (transform.position - other.transform.position).sqrMagnitude < 0.1f;
	}

	// Generate the appropiate wall segment.
	public void GenerateWall (MapGenerator generator) {
		if (type == Node.NodeType.Shared) {
			if (tiles[0].type != tiles[1].type)
				wall.type = Wall.WallType.Door;
			else
				wall.type = Wall.WallType.None;
		}
		else {
			wall.type = Wall.WallType.Wall;
		}
		wall.Initialize (generator);
	}

	// Pass the show command to the attached wall.
	public void ShowWall (Tileset tileset) {
		wall.Show (tileset);
	}

	// Create the visual representation of this node, if it does not exist.
	public void Show (GameObject obj) {
		if (m_show != null) {
			m_show.SetActive (true);
			return;
		}
		m_show = Instantiate (obj, transform.position, transform.rotation) as GameObject;
		m_show.transform.SetParent (transform);
	}

	// Hide the visual representation of this node.
	public void Hide () {
		if (m_show == null)
			return;
		m_show.SetActive (false);
	}
}
