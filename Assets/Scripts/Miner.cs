using UnityEngine;
using System.Collections;

public class Miner : MonoBehaviour
{
	public static Miner instance = null;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		
		DontDestroyOnLoad(transform.gameObject);

		transform.position = GameManager.instance.levelSize;	

		StartCoroutine(Move());
	}
	
	IEnumerator Move()
	{
		
	}
}
