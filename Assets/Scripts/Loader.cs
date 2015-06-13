using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
	public GameManager gameManager;
	public GridManager gridManager;
	public InputManager inputManager;

	void Awake()
	{
		if (GameManager.instance == null) Instantiate(gameManager);
		if (GridManager.instance == null) Instantiate(gridManager);
		if (InputManager.instance == null) Instantiate(inputManager);
	}
}
