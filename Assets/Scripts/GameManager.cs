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

	public float timeFactor = 8;
	public int depth;
	public float morale;
	public int minerCount = 5;
	public float minerMorale = 10;
	public List<Artifact> artifacts = new List<Artifact>();
	public List<Miner> allMiners = new List<Miner>();
	public List<Miner> miners = new List<Miner>();
	public List<Miner> saneMiners = new List<Miner>();
	public List<MiningEvent> miningEvents = new List<MiningEvent>();
	public bool entranceLocated;
	public DateTime currentDateTime;
	
	private DateTime initialDateTime = new DateTime(1892, 12, 3, 8, 0, 0);
	private string dataPath = "Assets/Data";
	private Deserializer deserializer = new Deserializer(namingConvention: new UnderscoredNamingConvention());
	private bool initializing;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);

		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		currentDateTime = initialDateTime;
		StartCoroutine(AdvanceTime());
	}

	void Update()
	{
		if (initializing)
			return;
	}

	public void InitializeGame()
	{
		initializing = true;
		depth = 0;
		LoadArtifacts();
		LoadMiners();
		LoadMiningEvents();
		RecruitMiners();
		initializing = false;
	}

	void InstantiateObject(GameObject objekt, Vector2 position)
	{
		GameObject clone = (GameObject)Instantiate(objekt);
		clone.transform.parent = transform;
		clone.transform.position = position;
	}

	void RecruitMiners()
	{
		miners.Clear();
		int i = 0;
		while (i < minerCount) {
			int random = UnityEngine.Random.Range(0, allMiners.Count);
			Miner miner = allMiners[random];
			miners.Add(miner);
			saneMiners.Add(miner);
			miner.morale = minerMorale;
			morale = morale + minerMorale;
			allMiners.Remove(miner);
			i++;
		}
	}
	
	IEnumerator AdvanceTime()
	{
		while (true) {
			currentDateTime = currentDateTime.AddMinutes(1 * timeFactor);
			yield return new WaitForSeconds(1);
		}
	}

	StringReader GetDataInput(string filename)
	{
		string text = "";
		try {
			text = File.ReadAllText(string.Format("{0}/{1}.yml", dataPath, filename));
		} catch (Exception e) {
			Debug.LogException(e);
		}
		return new StringReader(text);
	}

	void LoadArtifacts()
	{
		artifacts.Clear();
		StringReader input = GetDataInput("Artifacts");

		try {
			Artifact[] artifactData = deserializer.Deserialize<Artifact[]>(input);
			foreach (Artifact artifact in artifactData) {
				artifacts.Add(artifact);
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	void LoadMiners()
	{
		allMiners.Clear();
		StringReader input = GetDataInput("Miners");
		
		try {
			Miner[] minerData = deserializer.Deserialize<Miner[]>(input);
			foreach (Miner miner in minerData) {
				allMiners.Add(miner);
			}
		} catch (Exception e) {
			Debug.LogException(e);
		}
	}

	void LoadMiningEvents()
	{
		miningEvents.Clear();
		StringReader input = GetDataInput("MiningEvents");
		
		try {
			MiningEvent[] miningEventData = deserializer.Deserialize<MiningEvent[]>(input);
			foreach (MiningEvent miningEvent in miningEventData) {
				miningEvents.Add(miningEvent);
			}
		} catch (Exception e) {
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
	public string Gender { get; set; }
	public float morale;
	public bool insane;
	public DateTime whenWentInsane;

	public void LoseMorale(float amount)
	{
		float loss = (amount > morale) ? morale : amount;
		morale -= loss;
		GameManager.instance.morale -= loss;
		if (morale == 0)
			BecomeInsane();
	}

	public void BecomeInsane()
	{
		Debug.Log(string.Format("{0} has gone mad.", Name));
		insane = true;
		GameManager.instance.saneMiners.Remove(this);
		whenWentInsane = GameManager.instance.currentDateTime;
	}
}

public class MiningEvent
{
	public string Description { get; set; }
	public int Terror { get; set; }
	public float Chance { get; set; }
}
