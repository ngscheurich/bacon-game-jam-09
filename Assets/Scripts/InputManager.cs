using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;

	private GameManager gameManager;
	private Player player;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		
		DontDestroyOnLoad(transform.gameObject);

		gameManager = GameManager.instance;
		player = gameManager.player;
	}

	void Update()
	{
		if (gameManager.phase == GameManager.Phases.Mining) {
			//Miner.instance.Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		} else if (gameManager.phase == GameManager.Phases.Exploring) {
			player.Move(Input.GetAxisRaw("Horizontal"));
		}

		// if (Input.GetButtonDown("Submit"))
		// 	Application.LoadLevel("Mining");
	}
}
