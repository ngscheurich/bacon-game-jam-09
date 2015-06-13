using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	public int currentDay = 1;
	public float timeFactor = 8;
	public int depth = 1;
	public int morale = 100;

	public Player player;	
	public Text dateText;
	public Text timeText;
	public Text depthText;
	public Text moraleText;
	public GameObject wallPrefab;
	public Artifact baseArtifact;

	private string dataPath = "Assets/Data";
	private DateTime initialDateTime = new DateTime(1892, 12, 3, 8, 0, 0);
	private DateTime currentDateTime;
	private List<Artifact> artifacts = new List<Artifact>();
	private bool initializing;
	private Deserializer deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);

		DontDestroyOnLoad(transform.gameObject);

		InitializeGame();
	}

	void Start()
	{

		currentDateTime = initialDateTime;
		StartCoroutine(AdvanceTime());
	}

	void InitializeGame()
	{
		initializing = true;
		GenerateLevel();
		LoadArtifacts();
		initializing = false;
	}

	void Update()
	{
		if (initializing) return;
		string depthString = string.Format("Depth: {0}", depth.ToString());
		depthText.text = depthString;
		string moraleString = string.Format("Morale: {0}", morale.ToString());
		moraleText.text = moraleString;
  	}

	void GenerateLevel()
	{
		SpriteRenderer renderer = wallPrefab.GetComponent<SpriteRenderer>();
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
					InstantiateObject(wallPrefab, spritePosition);
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

	void InstantiateObject(GameObject objekt, Vector2 position)
	{
		GameObject clone = (GameObject)Instantiate(objekt);
		clone.transform.parent = transform;
		clone.transform.position = position;
	}

	IEnumerator AdvanceTime()
	{
		while (true) {
			currentDateTime = currentDateTime.AddMinutes(1 * timeFactor);

			TimeSpan dateTimeDelta = currentDateTime.Subtract(initialDateTime);
			int daysDelta = dateTimeDelta.Days;

			currentDay = daysDelta + 1;

			dateText.text = string.Format(currentDateTime.ToString("MMM d, yyyy"));
			timeText.text = currentDateTime.ToShortTimeString();

			yield return new WaitForSeconds(1);
		}
	}

	StringReader GetDataInput(string filename)
	{
		string text = "";
		try {
			text = File.ReadAllText(string.Format("{0}/{1}.yml", dataPath, filename));
		} catch(Exception e) {
			Debug.LogException(e);
		}
		return new StringReader(text);
	}

	void LoadArtifacts()
	{
		StringReader input = GetDataInput("Artifacts");

		try {
			ArtifactData[] artifactData = deserializer.Deserialize<ArtifactData[]>(input);

			foreach (ArtifactData artifact in artifactData) {
				Artifact clone = (Artifact)Instantiate(baseArtifact);

				clone.transform.parent = transform;
				clone.description = artifact.Description;
				clone.minDepth = artifact.MinDepth;

				artifacts.Add(clone);
			}
		} catch(Exception e) {
			Debug.LogException(e);
		}
	}
}

class ArtifactData
{
	public string Description { get; set; }
	public int MinDepth { get; set; }
}
