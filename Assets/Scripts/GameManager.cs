using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public GameManager instance = null;
	
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance = this)
			DestroyObject(this);
	}
}
