using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiningCursor : MonoBehaviour
{
	public MineManager mineManager;

	void Awake()
	{
		Vector2 startPosition = new Vector2(0f, mineManager.gridSize.x - 1f);
		transform.position = startPosition;
		StartCoroutine(Move());
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
			Mine();
	}
	
	void Mine()
	{
		Vector2 key = transform.position;
		if (mineManager.grid.ContainsKey(key)) {
			List<GameObject> objects = mineManager.grid[key];
			foreach (GameObject obj in objects) {
				if (obj != null) {
					if (obj.tag == "Stone") {
						int minerIndex = Random.Range(0, GameManager.instance.saneMiners.Count);
						Miner miner = GameManager.instance.saneMiners[minerIndex];

						int eventIndex = Random.Range(0, GameManager.instance.miningEvents.Count);
						MiningEvent miningEvent = GameManager.instance.miningEvents[eventIndex];
						int eventChance = Random.Range(0, 11) * GameManager.instance.depth;

						Destroy(obj);

						double timeTookToMine = Random.Range(0.5f, 2.5f);
						GameManager.instance.currentDateTime =
							GameManager.instance.currentDateTime.AddDays(timeTookToMine);

						string outcome = "";

						if (objects.Contains(mineManager.entrance)) {
							GameManager.instance.entranceLocated = true;
							outcome = "{name} has located the entrance!";
							mineManager.entranceLocated = true;
							StartCoroutine(mineManager.FlashText(mineManager.enterText, 1f));
						} else if (eventChance <= miningEvent.Chance) {
							outcome = miningEvent.Description;
							miner.AdjustMorale(-miningEvent.Terror);
						}

						string message = PersonalizeMessage(miner, outcome);

						mineManager.eventText.text = message;
					}
				}
			}
		}
	}

	string PersonalizeMessage(Miner miner, string description)
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
			Vector2 currentPosition = transform.position;
			Vector2 newPosition = currentPosition;
			if (Input.GetKey(KeyCode.RightArrow))
				newPosition.x = newPosition.x + 1;
			else if (Input.GetKey(KeyCode.LeftArrow))
				newPosition.x = newPosition.x - 1;
			else if (Input.GetKey(KeyCode.UpArrow))
				newPosition.y = newPosition.y + 1;
			else if (Input.GetKey(KeyCode.DownArrow))
				newPosition.y = newPosition.y - 1;

			if (newPosition != currentPosition && mineManager.grid.ContainsKey(newPosition)) {
				mineManager.eventText.text = "";
				transform.position = newPosition;
			}

			yield return new WaitForSeconds(.1f);
		}
	}
}
