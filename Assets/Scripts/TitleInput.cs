using UnityEngine;
using System.Collections;

public class TitleInput : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
			Application.LoadLevel("Mine");
	}
}
