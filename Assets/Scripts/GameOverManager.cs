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

		string outro = "GAME OVER\n=========\n\n";
		outro += string.Format("You reached a depth of {0}00 ft.\n\n", gameManager.depth.ToString());

		foreach (Miner miner in gameManager.miners) {
			outro += string.Format("{0} went mad on {1}.\n", miner.Name, miner.whenWentInsane.ToString("MMM d, yyyy"));
		}

		outro += "\nYou perished clasping the cache of artifacts that were recovered, muttering strange things about the return of Ancient Gods...";

		gameOverText.text = outro;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return))
			Application.LoadLevel("Title");
	}
}
