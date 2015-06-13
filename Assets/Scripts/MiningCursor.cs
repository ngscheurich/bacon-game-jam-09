using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiningCursor : MonoBehaviour
{
	private GameManager gameManager;
  	private GridManager gridManager;

	void Awake()
	{
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
						int eventIndex = Random.Range(0, gameManager.miningEvents.Count);
						MiningEvent miningEvent = gameManager.miningEvents[eventIndex];
						int eventChance = Random.Range(0, 11) * gameManager.depth;
						if (eventChance <= miningEvent.Chance) {
							int minerIndex = Random.Range(0, gameManager.miners.Count);
							Miner miner = gameManager.miners[minerIndex];
							string description = PersonalizeDescription(miner, miningEvent.Description);
							description += string.Format(" -{0} morale.", miningEvent.Terror);
							miner.AdjustMorale(-miningEvent.Terror);
							Debug.Log(description);
						} else {
							Debug.Log("Nothing happens...");
						}
					}
				}
			}
		}
	}

	string PersonalizeDescription(Miner miner, string description)
	{
		description = description.Replace("{name}", miner.Name);
		if (miner.Gender == "male") {
			description = description.Replace("{he/she}", "he");
			description = description.Replace("{him/her}", "him");
			description = description.Replace("{himself/herself}", "himself");
			description = description.Replace("{his/her}", "his");
		} else if (miner.Gender == "female") {
			description = description.Replace("{he/she}", "she");
			description = description.Replace("{him/her}", "her");
			description = description.Replace("{himself/herself}", "herself");
			description = description.Replace("{his/her}", "her");
		}
		return description;
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
