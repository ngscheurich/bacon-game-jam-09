using UnityEngine;
using System.Collections;

public abstract class Activatable : MonoBehaviour
{
	public virtual void Activate()
	{
		if (!gameObject.activeSelf)
			gameObject.SetActive(true);
	}
	
	public virtual void Deactivate()
	{
		if (gameObject.activeSelf)
			gameObject.SetActive(false);
	}
}
