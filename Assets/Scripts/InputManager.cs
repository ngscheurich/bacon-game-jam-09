using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	private Player player;

	void Awake()
	{
		player = GameManager.instance.player;
	}

	void Update()
	{
		player.Move(Input.GetAxisRaw("Horizontal"));
	}
}
