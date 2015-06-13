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
	public float timeScale = 8;
	public Text currentDateText;
	public Text currentTimeText;
	public Enemy baseEnemy;
	public Quest baseQuest;
	public bool isWeekday;

	private DateTime initialDateTime = new DateTime(1983, 12, 12, 23, 58, 0);
	private DateTime currentDateTime;
	private List<Enemy> enemies = new List<Enemy>();
	private List<Quest> quests = new List<Quest>();
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
		StartCoroutine(IncrementTime());
	}

	void InitializeGame()
	{
		initializing = true;
		player = Player.instance;
		LoadEnemies();
		LoadQuests();
		initializing = false;
	}

	void Update()
	{
		if (initializing) return;
	}

	IEnumerator IncrementTime()
	{
		while (true) {
			currentDateTime = currentDateTime.AddMinutes(1 * timeScale);

			if (currentDateTime.DayOfWeek == DayOfWeek.Saturday || currentDateTime.DayOfWeek == DayOfWeek.Sunday)
				isWeekday = false;
			else
				isWeekday = true;

			TimeSpan dateTimeDelta = currentDateTime.Subtract(initialDateTime);
			int daysDelta = dateTimeDelta.Days;

			currentDay = daysDelta + 1;

			currentDateText.text = string.Format("Day {0} - {1}", currentDay, currentDateTime.DayOfWeek);
			currentTimeText.text = string.Format(currentDateTime.ToShortTimeString());

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

	void LoadEnemies()
	{
		StringReader input = GetDataInput("enemies.yml");
		EnemyData[] enemyData = deserializer.Deserialize<EnemyData[]>(input);
		foreach (EnemyData enemy in enemyData) {
			Enemy enemyClone = (Enemy)Instantiate(baseEnemy);
			enemyClone.transform.parent = transform;
			enemyClone.description = enemy.Description;
			enemyClone.severity = enemy.Severity;
			enemies.Add(enemyClone);
		}
	}

	void LoadQuests()
	{
		StringReader input = GetDataInput("quests.yml");
		QuestData[] questData = deserializer.Deserialize<QuestData[]>(input);
		foreach (QuestData quest in questData) {
			Quest questClone = (Quest)Instantiate(baseQuest);
			questClone.transform.parent = transform;
			questClone.title = quest.Title;
			questClone.description = quest.Description;
			questClone.dueBy = quest.DueBy;
			quests.Add(questClone);
		}
	}
}

class EnemyData
{
	public string Description { get; set; }
	public int Severity { get; set; }
}


class QuestData
{
	public string Title { get; set; }
	public string Description { get; set; }
	public int DueBy { get; set; }
}
