using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	public InputManager instance = null;
	public int hits = 0;
	
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance = this)
			DestroyObject(this);
		DontDestroyOnLoad(transform.gameObject);
	}

	void Update()
	{
		if (Input.GetButtonDown("Submit"))
			hits++;
	
		if (Input.GetButtonDown("Fire1"))
			Application.LoadLevel("Brain");
	}


}
