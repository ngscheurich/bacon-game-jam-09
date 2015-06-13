﻿using UnityEngine;
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
	
	public enum Phases { Mining, Exploring }
	public Phases phase = Phases.Mining;

	public Player player;	
	public Text dateText;
	public Text timeText;
	public Text depthText;
	public Text moraleText;
	public List<Artifact> artifacts = new List<Artifact>();
	public List<Miner> miners = new List<Miner>();
	public List<MiningEvent> miningEvents = new List<MiningEvent>();

	private string dataPath = "Assets/Data";
	private DateTime initialDateTime = new DateTime(1892, 12, 3, 8, 0, 0);
	private DateTime currentDateTime;

	private Deserializer deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
	private bool initializing;

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
		player.gameObject.SetActive(false);
		LoadArtifacts();
		initializing = false;
	}

	void Update()
	{
		if (initializing) return;
		string depthString = string.Format("Depth: {0}00 ft", depth.ToString());
		depthText.text = depthString;
		string moraleString = string.Format("Morale: {0}", morale.ToString());
		moraleText.text = moraleString;
  	}

	void InstantiateObject(GameObject objekt, Vector2 position)
	{
		GameObject clone = (GameObject)Instantiate(objekt);
		clone.transform.parent = transform;
		clone.transform.position = position;
	}

	IEnumerator Mining()
	{
		while (phase == Phases.Mining) {
			yield return null;
		}
	}

	IEnumerator Exploring()
	{
		while (phase == Phases.Exploring) {
			yield return null;
		}
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
			Artifact[] artifactData = deserializer.Deserialize<Artifact[]>(input);
			foreach (Artifact artifact in artifactData) {
				artifacts.Add(artifact);
			}
		} catch(Exception e) {
			Debug.LogException(e);
		}
	}

	void LoadMiners()
	{
		StringReader input = GetDataInput("Miners");
		
		try {
			Miner[] minerData = deserializer.Deserialize<Miner[]>(input);
			foreach (Miner miner in minerData) {
				miners.Add(miner);
			}
		} catch(Exception e) {
			Debug.LogException(e);
		}
	}

	void LoadMiningEvents()
	{
		StringReader input = GetDataInput("MiningEvents");
		
		try {
			MiningEvent[] miningEventData = deserializer.Deserialize<MiningEvent[]>(input);
			foreach (MiningEvent miningEvent in miningEventData) {
				miningEvents.Add(miningEvent);
			}
		} catch(Exception e) {
			Debug.LogException(e);
    	}
	}
}

public class Artifact
{
  public string Description { get; set; }
  public int MinDepth { get; set; }
}

public class Miner
{
  public string Name { get; set; }
}

public class MiningEvent
{
  public string Description { get; set; }
  public float Chance { get; set; }
}
