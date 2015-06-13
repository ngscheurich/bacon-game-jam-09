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

	public Player player;
	public int currentDay = 1;
	public float timeFactor = 8;
	public int depth = 0;

	public Text currentDateText;
	public Text currentTimeText;
	public Artifact baseArtifact;

	private DateTime initialDateTime = new DateTime(1983, 12, 12, 23, 58, 0);
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
	}

	IEnumerator AdvanceTime()
	{
		while (true) {
			currentDateTime = currentDateTime.AddMinutes(1 * timeFactor);

			TimeSpan dateTimeDelta = currentDateTime.Subtract(initialDateTime);
			int daysDelta = dateTimeDelta.Days;

			currentDay = daysDelta + 1;

			currentDateText.text = string.Format("Day {0} - {1}", currentDay, currentDateTime.DayOfWeek);
			currentTimeText.text = currentDateTime.ToShortTimeString();

			yield return new WaitForSeconds(1);
		}
	}

	StringReader GetDataInput(string filename)
	{
		string text = "";
		string dataPath = "Assets/Data";
		try {
			text = File.ReadAllText(string.Format("{0}/{1}", dataPath, filename));
		} catch(Exception e) {
			Debug.LogException(e);
		}
		return new StringReader(text);
	}

	void LoadArtifacts()
	{
		StringReader input = GetDataInput("enemies.yml");

		try {
			Artifact[] artifactData = deserializer.Deserialize<Artifact[]>(input);

			foreach (Artifact artifact in artifactData) {
				Enemy enemyClone = (Enemy)Instantiate(baseEnemy);

				enemyClone.transform.parent = transform;
				enemyClone.description = enemy.Description;
				enemyClone.severity = enemy.Severity;

				enemies.Add(enemyClone);
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
