using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviour {

	// All the tiles in this template
	[SerializeField]
	public List<Tile> tiles = new List<Tile> ();

	// Handle initialization by checking validity of all tiles and initializing them.
	public bool Initialize (MapGenerator generator) {
		Tile[] tmp_tiles = tiles.ToArray ();
		Tile[] other_tiles = generator.GetTiles ();
		Node[] other_nodes = generator.GetNodes ();
		// Collision with another tile invalidates the template.
		foreach (Tile tile in tmp_tiles)
			foreach (Tile other in other_tiles)
				if (tile.IsColliding (other))
					return false;
		// Collision with certain nodes invalidates the template.
		foreach (Tile tile in tmp_tiles)
			foreach (Node other in other_nodes)
				if (tile.IsNotAllowed (other))
					return false;
		// If valid, merge the required nodes.
		foreach (Tile tile in tmp_tiles)
			foreach (Node node in tile.nodes)
				foreach (Tile other in other_tiles)
					if (node.IsColliding (other))
						tile.MergeNodes (node, Array.Find (other.nodes, (x => x.IsColliding (tile))));
		return true;
	}

	// Returns an array with all the nodes attached to one of the tiles in this template. Might contain duplicates.
	public Node[] GetNodes () {
		List<Node> tmp_nodes = new List<Node> ();
		foreach (Tile tile in tiles)
			tmp_nodes.AddRange (tile.nodes);
		return tmp_nodes.ToArray ();
	}
}
