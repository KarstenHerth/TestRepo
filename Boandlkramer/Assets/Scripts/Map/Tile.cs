using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	// Classify the kind of room this tile belongs to.
	public enum TileType { None, Hallway, Room }
	public TileType type = TileType.None;

	// All the nodes that are associated with this tile.
	public Node[] nodes = new Node[4];
	// The floor object.
	public GameObject floor;

	// Pass the GenerateWall command to al attached nodes.
	public void GenerateWalls (MapGenerator generator) {
		foreach (Node node in nodes)
			node.GenerateWall (generator);
	}

	// Create the floor.
	public void ShowFloor (Tileset tileset) {
		if (floor != null)
			return;
		floor = Instantiate (tileset.floor, transform.position, Quaternion.identity) as GameObject;
		floor.transform.SetParent (transform);
	}

	// Hide the floor.
	public void HideFloor () {
		if (floor == null)
			return;
		Destroy (floor.gameObject);
	}

	// Pass the ShowWall command to the attached nodes.
	public void ShowWalls (Tileset tileset) {
		foreach (Node node in nodes)
			node.ShowWall (tileset);
	}

	// Pass the HideWall command along to the nodes.
	public void HideWalls () {

	}

	// Pass the Show command to the nodes.
	public void ShowNodes (GameObject obj) {
		foreach (Node node in nodes)
			node.Show (obj);
	}

	// Pass the Hide command to the nodes.
	public void HideNodes () {
		foreach (Node node in nodes)
			node.Hide ();
	}

	// When creating a tile at a node the backwards facing node of the new tile needs to be merged with that node.
	public void Initialize (Node node) {
		MergeNodes (nodes[2], node);
	}

	// Make two tiles share a node.
	public void MergeNodes (Node node_a, Node node_b) {
		int index_a = Array.IndexOf (nodes, node_a);
		nodes[index_a] = node_b;
		node_b.type = Node.NodeType.Shared;
		node_b.tiles.Add (this);
		node_b.transform.position = (transform.position + node_b.tiles[0].transform.position) / 2f;
		node_b.wall.transform.localPosition = new Vector3 (0, 0, 0);
		Destroy (node_a.gameObject);
	}

	// Get the direction this object is facing.
	public Vector3 GetDirection () {
		return transform.rotation.eulerAngles;
	}

	// Check for collision with some other object.
	public bool IsColliding<T> (T other) where T : MonoBehaviour {
		return (transform.position - other.transform.position).sqrMagnitude < 0.1f;
	}

	// Check wether this tile is allowed at a certain node.
	public bool IsNotAllowed (Node other) {
		if (other.type == Node.NodeType.Shared)
			return false;
		if (type == TileType.Hallway && other.type == Node.NodeType.Hallway && GetDirection () == other.GetDirection ())
			return false;
		if (type == TileType.Room && other.type == Node.NodeType.Room)
			return false;
		else
			return (transform.position - other.transform.position).sqrMagnitude < 0.1f;
	}
}
