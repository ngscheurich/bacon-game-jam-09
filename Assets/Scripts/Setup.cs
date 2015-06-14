using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Setup : MonoBehaviour
{
	public List<Artifact> artifacts = new List<Artifact>();
	public List<Miner> allMiners = new List<Miner>();
	public List<Miner> miners = new List<Miner>();
	public List<MiningEvent> miningEvents = new List<MiningEvent>();

	private string dataPath = "Assets/Data";
}
