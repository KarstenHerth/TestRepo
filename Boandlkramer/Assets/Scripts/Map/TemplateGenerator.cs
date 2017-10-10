using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateGenerator : MonoBehaviour {

	// The tile prefab to be used.
	public GameObject tilePrefab;

	// Create a room of size width x height and its origin at (origin, 0).
	public Template Room (int width, int height, int origin) {
		GameObject room = new GameObject ("Room");
		Template template = room.AddComponent<Template> ();
		room.transform.position = new Vector3 (origin, 0, 0);
		GameObject instance;
		Tile tile;
		Node[] tmp_nodes;
		for (int i = 0; i < width; i++)
			for (int j = 0; j < height; j++) {
				instance = Instantiate (tilePrefab, new Vector3 (i, 0, j), Quaternion.identity) as GameObject;
				instance.transform.SetParent (room.transform);
				tile = instance.GetComponent<Tile> ();
				tile.type = Tile.TileType.Room;
				tmp_nodes = tile.nodes;
				foreach (Node node in tmp_nodes) {
					node.type = Node.NodeType.Hallway;
					foreach (Tile other in template.tiles)
						if (node.IsColliding (other))
							tile.MergeNodes (node, Array.Find (other.nodes, (x => x.IsColliding (tile))));
				}
				template.tiles.Add (tile);
			}
		return template;
	}

	// Create a hallway of length length.
	public Template Hallway (int length) {
		GameObject hallway = new GameObject ("Hallway");
		Template template = hallway.AddComponent<Template> ();
		GameObject instance;
		Tile tile;
		for (int i = 0; i < length; i++) {
			instance = Instantiate (tilePrefab, new Vector3 (0, 0, i), Quaternion.identity) as GameObject;
			instance.transform.SetParent (hallway.transform);
			tile = instance.GetComponent<Tile> ();
			tile.type = Tile.TileType.Hallway;
			for (int n = 0; n < 4; n++) {
				Node node = tile.nodes[n];
				if (n == 0)
					node.type = Node.NodeType.Room;
				else
					node.type = Node.NodeType.Hallway;
				foreach (Tile other in template.tiles)
					if (node.IsColliding (other))
						tile.MergeNodes (node, Array.Find (other.nodes, (x => x.IsColliding (tile))));
			}
			template.tiles.Add (tile);
		}
		return template;
	}
}
