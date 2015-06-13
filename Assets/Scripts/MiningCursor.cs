using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiningCursor : MonoBehaviour
{
	public static MiningCursor instance = null;

	private GameManager gameManager;
  	private GridManager gridManager;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		
		DontDestroyOnLoad(transform.gameObject);

		gameManager = GameManager.instance;
		gridManager = GridManager.instance;

		Vector2 startPosition = new Vector2(0f, gridManager.gridSize.x - 1f);
		transform.position = startPosition;

		StartCoroutine(Move());
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space)) {
			BreakRock();
		}
	}

	void BreakRock()
	{
		Vector2 key = transform.position;
		if (gridManager.grid.ContainsKey(key)) {
			List<GameObject> objects = gridManager.grid[key];
			foreach (GameObject obj in objects) {
				if (obj != null) {
					if (obj.tag == "Rock") {
						Destroy(obj);
						int miningEventIndex = Random.Range(0, gameManager.miningEvents.Count);
						MiningEvent miningEvent = gameManager.miningEvents[miningEventIndex];
						int miningEventChance = Random.Range(0, 11) * gameManager.depth;
						if (miningEventChance <= miningEvent.Chance) {
							int minerIndex = Random.Range(0, gameManager.miners.Count);
							Miner miner = gameManager.miners[minerIndex];
							string description = string.Format(Genderize(miner.Gender, miningEvent.Description), miner.Name);
							Debug.Log(description);
						} else {
							Debug.Log("Nothing happens...");
						}
					}
				}
			}
		}
	}

	string Genderize(string gender, string str)
	{
		if (gender == "male") {
			str.Replace("{he/she}", "he");
			str.Replace("{him/her}", "him");
			str.Replace("{himself/herself}", "himself");
			str.Replace("{his/her}", "his");
		} else if (gender == "female") {
			str.Replace("{he/she}", "she");
			str.Replace("{him/her}", "her");
			str.Replace("{himself/herself}", "herself");
			str.Replace("{his/her}", "her");
		}
		return str;
	}

	IEnumerator Move()
	{
		while (true) {
			Vector2 newPosition = transform.position;
			if (Input.GetKey(KeyCode.RightArrow))
				newPosition.x = newPosition.x + 1;
			else if (Input.GetKey(KeyCode.LeftArrow))
				newPosition.x = newPosition.x - 1;
			else if (Input.GetKey(KeyCode.UpArrow))
				newPosition.y = newPosition.y + 1;
			else if (Input.GetKey(KeyCode.DownArrow))
				newPosition.y = newPosition.y - 1;

			transform.position = newPosition;

			yield return new WaitForSeconds(.1f);
		}
	}
}
