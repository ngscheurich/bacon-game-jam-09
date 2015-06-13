using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
	public static GridManager instance = null;

	public Vector2 gridSize = new Vector2(20f, 20f);
	public Dictionary<Vector2, List<GameObject>> grid = new Dictionary<Vector2, List<GameObject>>();
	public List<Vector2> gridPositions = new List<Vector2>();

	public GameObject rockPrefab;
	public GameObject pebblePrefab;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		
		DontDestroyOnLoad(transform.gameObject);

		GenerateGrid();
	}

	void GenerateGrid()
	{
		for (int y = 0; y < gridSize.y; y++) {
			for (int x = 0; x < gridSize.x; x++) {
				Vector2 position = new Vector2(x, y);
				grid.Add(position, new List<GameObject>());
				GameObject clone = Instantiate(rockPrefab);
				AddToGrid(position, clone);
			}
		}
	}

	public void AddToGrid(Vector2 key, GameObject obj)
	{
		if (grid.ContainsKey(key)) {
			var list = grid[key];
			list.Add(obj);
			grid[key] = list;
			obj.transform.parent = transform;
			obj.transform.position = key;
		} else {
			Debug.Log("Not a valid grid key");
		}
	}
}
