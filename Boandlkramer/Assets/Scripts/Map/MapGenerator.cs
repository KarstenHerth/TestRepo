using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class MapGenerator : MonoBehaviour {

    public int deineMutterSchwitztBeimKacken = 1;


	// Contains all the tiles of this map.
	public List<Tile> map = new List<Tile> ();
	// Contains a reference of the TemplateGenerator to be used.
	public TemplateGenerator templateGenerator;

	// Objects for visually representing the map and its components.
	public GameObject sphere;
	public Tileset tileset;

	// Create a new map on Start.
	void Start () {
		GenerateMap ();
	}

	// Generates a new map with certain parameters.
	public void GenerateMap () {
		// Remove an existing map.
		for (int i = 0; i < map.Count; i++) {
			if (map[i] != null)
				Destroy (map[i].gameObject);
		}	
		map = new List<Tile> ();
		// Create initial room
		Template template = templateGenerator.Room (3, 3, 1);
		template.transform.SetParent (transform);
		map.AddRange (template.tiles);
		Node node;
		// Create more rooms and hallways at randomly selected nodes.
		for (int i = 0; i < 5; i++) {
			node = SelectRandomNode (Node.NodeType.Hallway);
			if (node != null)
				CreateHallway (node);
			node = SelectRandomNode (Node.NodeType.Room);
			if (node != null)
				CreateRoom (node);
		}
		// Show the map.
		// ShowNodes ();
		ShowFloor ();
		GenerateWalls ();
		ShowWalls ();
	}

	// Select a node of a certain type at random.
	Node SelectRandomNode (Node.NodeType type) {
		Node[] tmp_nodes = GetNodes (type);
		if (tmp_nodes.Length > 0)
			return tmp_nodes[Random.Range (0, tmp_nodes.Length)];
		else
			return null;
	}

	// Use the TemplateGenerator to create a hallway.
	void CreateHallway (Node node) {
		Template template = templateGenerator.Hallway (Random.Range (3, 8));
		template.transform.position = node.transform.position;
		template.transform.rotation = node.transform.rotation;
		template.transform.SetParent (transform);
		if (!template.Initialize (this))
			Destroy (template.gameObject);
		else
			map.AddRange (template.tiles);
	}

	// Use the TemplateGenerator to create a room.
	void CreateRoom (Node node) {
		int width = Random.Range (3, 6);
		int height = Random.Range (3, 6);
		int origin = Random.Range (0, width);
		Template template = templateGenerator.Room (width, height, origin);
		template.transform.position = node.transform.position;
		template.transform.rotation = node.transform.rotation;
		template.transform.SetParent (transform);
		if (!template.Initialize (this))
			Destroy (template.gameObject);
		else
			map.AddRange (template.tiles);
	}

	// Get an array of all tiles.
	public Tile[] GetTiles () {
		return map.ToArray ();
	}

	// Get an array of all the nodes.
	public Node[] GetNodes () {
		List<Node> nodes = new List<Node> ();
		foreach (Tile tile in map)
			nodes.AddRange (tile.nodes);
		return nodes.ToArray ();
	}

	// Get an array of all nodes of a certain type.
	public Node[] GetNodes (Node.NodeType type) {
		List<Node> nodes = new List<Node> ();
		foreach (Tile tile in map)
			nodes.AddRange (tile.nodes.Where (node => node.type == type));
		return nodes.ToArray ();
	}

	// Get all walls that are not of type None.
	public Wall[] GetWalls () {
		List<Wall> walls = new List<Wall> ();
		foreach (Tile tile in map)
			foreach (Node node in tile.nodes)
				if (node.wall.type != Wall.WallType.None)
					walls.Add (node.wall);
		return walls.ToArray ();
	}

	// Get an array of all connectors.
	public Connector[] GetConnectors () {
		List<Connector> connectors = new List<Connector> ();
		foreach (Wall wall in GetWalls ())
			connectors.AddRange (wall.connectors);
		return connectors.ToArray ();
	}

	// Helper functions for visual representation
	public void GenerateWalls () {
		foreach (Tile tile in map)
			tile.GenerateWalls (this);
	}

	public void ShowFloor () {
		foreach (Tile tile in map)
			tile.ShowFloor (tileset);
	}

	public void HideFloor () {
		foreach (Tile tile in map)
			tile.HideFloor ();
	}

	public void ShowWalls () {
		foreach (Tile tile in map)
			tile.ShowWalls (tileset);
	}

	public void HideWalls () {
		foreach (Tile tile in map)
			tile.HideWalls ();
	}

	public void ShowNodes () {
		foreach (Tile tile in map)
			tile.ShowNodes (sphere);
	}

	public void HideNodes () {
		foreach (Tile tile in map)
			tile.HideNodes ();
	}
}
