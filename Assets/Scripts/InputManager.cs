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
	}
}
