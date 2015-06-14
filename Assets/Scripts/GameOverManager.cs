using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour{	
	private GameManager gameManager;
	private Text gameOverText;
	
	void Awake()
	{
		gameManager = GameManager.instance;
		gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();

		string outro = "GAME OVER\n=========";

		foreach (Miner miner in gameManager.miners) {

		}
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return)) {

		}
	}
}
