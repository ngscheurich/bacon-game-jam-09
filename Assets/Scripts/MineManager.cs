using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MineManager : MonoBehaviour
{
	public Vector2 gridSize = new Vector2(20f, 20f);
	public Dictionary<Vector2, List<GameObject>> grid = new Dictionary<Vector2, List<GameObject>>();
	public GameObject stonePrefab;
	public GameObject entrancePrefab;
	public GameObject entrance;
	public bool entranceLocated;
	public Text dateText;
	public Text timeText;
	public Text depthText;
	public Text moraleText;
	public Text enterText;
	public Text eventText;


	private GameManager gameManager;

	void Awake()
	{
		gameManager = GameManager.instance;

		dateText   = GameObject.Find("DateText").GetComponent<Text>();
		timeText   = GameObject.Find("TimeText").GetComponent<Text>();
		depthText  = GameObject.Find("DepthText").GetComponent<Text>();
		moraleText = GameObject.Find("MoraleText").GetComponent<Text>();
		enterText  = GameObject.Find("EnterText").GetComponent<Text>();
		eventText  = GameObject.Find("EventText").GetComponent<Text>();

		enterText.enabled = false;
		eventText.text = "";

		gameManager.depth++;

		grid.Clear();
		GenerateGrid();
		AddStone();
		AddEntrance();
	}

	void Update()
	{
		string depthString = string.Format("Depth: {0}00 ft", gameManager.depth.ToString());
		depthText.text = depthString;

		int maxMorale = gameManager.miners.Count * gameManager.minerMorale;
		float moralePercent = Mathf.Round(gameManager.morale / maxMorale * 100);
		string moraleString = string.Format("Morale: {0}%", moralePercent);
		moraleText.text = moraleString;

		dateText.text = string.Format(gameManager.currentDateTime.ToString("MMM d, yyyy"));
		timeText.text = gameManager.currentDateTime.ToShortTimeString();

		if (entranceLocated && Input.GetKey(KeyCode.Return))
			Application.LoadLevel("Explore");
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

	public IEnumerator FlashText(Text text, float interval)
	{
		while (true) {
			text.enabled = !text.enabled;
			yield return new WaitForSeconds(interval);
		}
  	}
}
