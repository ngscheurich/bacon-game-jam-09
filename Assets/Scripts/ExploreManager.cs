using UnityEngine;
using System.Collections;

public class ExploreManager : MonoBehaviour
{
	public GameObject wallPrefab;

	void GenerateLevel()
	{
		SpriteRenderer renderer = rockPrefab.GetComponent<SpriteRenderer>();
		Vector2 spriteSize = new Vector2(renderer.bounds.extents.x * 2, renderer.bounds.extents.y * 2);
		string text = "";
		try {
			text = File.ReadAllText(string.Format("{0}/{1}.txt", dataPath, "Level" + depth));
			Vector2 currentPosition = new Vector2(0f, 0f);
			
			foreach(char c in text) {
				float nextX = currentPosition.x + 1;
				float nextY = currentPosition.y;
				
				string character = c.ToString();
				
				if (character == "#") {
					Vector2 spritePosition = new Vector2(currentPosition.x * spriteSize.x, currentPosition.y * spriteSize.y);
					InstantiateObject(rockPrefab, spritePosition);
				} else if (character == "\n" || character == "\r" || character == "\r\n") {
					nextX = 0;
					nextY = currentPosition.y - 1;
				}
				
				Vector2 nextPosition = new Vector2(nextX, nextY);
				currentPosition = nextPosition;
			}
		} catch(Exception e) {
			Debug.LogException(e);
		}
	}

}
