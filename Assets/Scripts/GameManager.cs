using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public Player player;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		DontDestroyOnLoad(transform.gameObject);
	}

}
