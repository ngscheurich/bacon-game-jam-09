using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : Activatable
{
	public static GridManager instance = null;

	public Vector2 gridSize = new Vector2(20f, 20f);

	public Dictionary<Vector2, List<GameObject>> grid = new Dictionary<Vector2, List<GameObject>>();

	public GameObject stonePrefab;
	public GameObject entrancePrefab;
	public GameObject entrance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		
		DontDestroyOnLoad(transform.gameObject);
	}

	public virtual void Activate()
	{
		NewGrid();
		base.Activate();
	}

	public void NewGrid()
	{
		grid.Clear();
		GenerateGrid();
		AddStone();
		AddEntrance();
	}

	void GenerateGrid()
	{
		for (int y = 0; y < gridSize.y; y++) {
			for (int x = 0; x < gridSize.x; x++) {
				Vector2 position = new Vector2(x, y);
				grid.Add(position, new List<GameObject>());
			}
		}
	}

	void AddStone()
	{
		var positions = new List<Vector2>(grid.Keys);
		foreach (Vector2 position in positions) {
			GameObject stone = Instantiate(stonePrefab);
			AddToGrid(position, stone);
		}
	}
	
	void AddEntrance()
	{
		float x = Mathf.Round(Random.Range(0, gridSize.x - 1));
		float y = Mathf.Round(Random.Range(0, gridSize.y - 1));
		Vector2 position = new Vector2(x, y);
		entrance = Instantiate(entrancePrefab);
		AddToGrid(position, entrance);
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
			Debug.LogError(string.Format("{0} is not a valid grid key", key));
		}
	}
}
