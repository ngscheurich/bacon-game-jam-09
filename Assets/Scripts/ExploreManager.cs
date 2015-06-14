using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class ExploreManager : MonoBehaviour
{
	public GameObject player;
	public GameObject wall;

	private string dataPath = "Assets/Data";
	private GameManager gameManager;
	private Dictionary<string, GameObject> charMap = new Dictionary<string, GameObject>();
	private List<string> newLineChars = new List<string>();

	void Awake()
	{
		gameManager = GameManager.instance;

		charMap.Add("#", wall);
		charMap.Add("P", player);

		newLineChars.Add("\n");
		newLineChars.Add("\r");
		newLineChars.Add("\r\n");

		GenerateLevel();
	}

	void GenerateLevel()
	{
		string text = "";
		try {
			text = File.ReadAllText(string.Format("{0}/{1}.txt", dataPath, "Level" + gameManager.depth));
			Vector2 currentPosition = new Vector2(0f, 0f);
			
			foreach(char c in text) {
				float nextX = currentPosition.x + 1;
				float nextY = currentPosition.y;
				
				string character = c.ToString();
				
				if (charMap.ContainsKey(character)) {
					Instantiate(charMap[character], currentPosition, Quaternion.identity);
				} else if (newLineChars.Contains(character)) {
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
