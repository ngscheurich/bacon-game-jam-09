using UnityEngine;
using System.Collections;

public abstract class Singleton : MonoBehaviour {
	public Singleton instance = null;

	protected virtual void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		
		DontDestroyOnLoad(transform.gameObject);
	}

	public virtual void Activate()
	{
		gameObject.SetActive(true);
	}
	
	public virtual void Deactivate()
	{
		gameObject.SetActive(false);
	}
}
