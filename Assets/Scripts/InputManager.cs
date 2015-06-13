using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour
{
	public int hits = 0;

	void Update()
	{
		if (Input.GetButtonDown("Submit"))
			hits++;
	
		if (Input.GetButtonDown("Fire1"))
			Application.LoadLevel("Brain");
	}


}
