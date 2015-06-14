using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class ExploreManager : MonoBehaviour
{
	public GameObject wallPrefab;

	private string dataPath = "Assets/Data";
	private Deserializer deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
	private GameManager gameManager;

	public struct LevelCharacters
	{
		public string wall;
		public List<string> newLines = new List<string>();

		public LevelCharacters()
		{
			wall = "#";
			newLines.Add("\n");
			newLines.Add("\r");
			newLines.Add("\r\n");
		}
	}

	private LevelCharacters levelCharacters = new LevelCharacters();

	void Awake()
	{
		gameManager = GameManager.instance;
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
				
				if (character == levelCharacters.wall) {
					InstantiateObject(wallPrefab, currentPosition);
				} else if (levelCharacters.newLines.Contains(character)) {
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
