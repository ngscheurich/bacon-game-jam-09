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
						if (miningEventChance >= miningEvent.Chance) {
							int minerIndex = Random.Range(0, gameManager.miners.Count);
							Miner miner = gameManager.miners[minerIndex];
							Debug.Log(string.Format(miningEvent.Description, miner.Name));
						} else {
							Debug.Log("Nothing happens...");
						}
					}
				}
			}
		}
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
