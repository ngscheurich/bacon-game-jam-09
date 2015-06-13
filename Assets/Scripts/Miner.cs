using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Miner : MonoBehaviour
{
	public static Miner instance = null;

	private GridManager gridManager;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		
		DontDestroyOnLoad(transform.gameObject);

		gridManager = GridManager.instance;
		transform.position = gridManager.gridSize;

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
				if (obj.tag == "Rock") Destroy(obj);
				for (int i = 0; i < 10; i++) {
					GameObject pebble = Instantiate(gridManager.pebblePrefab, key, Quaternion.identity) as GameObject;
					Vector2 pebblePosition = transform.position;
					pebblePosition.x = pebblePosition.x + Random.Range(0.1f, 1f);
					pebblePosition.y = pebblePosition.y - Random.Range(0.1f, 1f);
					pebble.transform.position = pebblePosition;
					pebble.GetComponent<Rigidbody2D>().AddForce(new Vector2(10f, 0));
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
