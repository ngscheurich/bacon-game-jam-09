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
	public int depth = 0;
	public int morale = 100;

	public Text timeText;
	public Text dateText;
	public Text depthText;
	public Text moraleText;
	public Artifact baseArtifact;

	private Player player;
	private DateTime initialDateTime = new DateTime(1823, 12, 3, 8, 0, 0);
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
		player = Player.instance;
		LoadArtifacts();
		initializing = false;
	}

	void Update()
	{
		if (initializing) return;
		string depthString = depth.ToString();
		depthText.text = depthString;
		string moraleString = morale.ToString();
		moraleText.text = moraleString;
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
		string dataPath = "Assets/Data";
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
