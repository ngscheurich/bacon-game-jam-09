using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public Player player;
	public int currentDay = 1;
	public float timeIncrementFactor = 8;
	public Text currentDateTimeText;

	private DateTime initialDateTime = new DateTime(1983, 12, 12, 23, 58, 0);
	private DateTime currentDateTime;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);

		DontDestroyOnLoad(transform.gameObject);

		player = Player.instance;

		GenerateTriggers();
	}

	void Start()
	{
		currentDateTime = initialDateTime;
		StartCoroutine(IncrementTime());
	}

	IEnumerator IncrementTime()
	{
		while (true) {
			currentDateTime = currentDateTime.AddMinutes(1 * timeIncrementFactor);
			TimeSpan dateTimeDelta = currentDateTime.Subtract(initialDateTime);
			int daysDelta = dateTimeDelta.Days;
			currentDay = daysDelta + 1;
			currentDateTimeText.text = string.Format("Day {0} - {1}", currentDay, currentDateTime.ToShortTimeString());
			yield return new WaitForSeconds(1);
		}
	}
	
	StringReader GetDataInput(string filename)
	{
		string dataPath = "Assets/Data";
		try {
			string text = File.ReadAllText(string.Format("{0}/{1}", dataPath, filename));
		} catch(Exception e) {
			Debug.Log(e.Message);
		}
		return new StringReader(text);
	}

	void GenerateTriggers()
	{
		StringReader input = GetDataInput("triggers.yml");
		Deserializer deserializer = new Deserializer(namingConvention: new CamelCaseNamingConvention());
		DataTrigger[] triggers = deserializer.Deserialize<DataTrigger>(input);
		foreach (DataTrigger trigger in triggers) {
			Debug.Log(trigger.Description);
		}
	}

	class DataTrigger
	{
		public string Description { get; set; }
		public int Severity { get; set; }
	}
}
