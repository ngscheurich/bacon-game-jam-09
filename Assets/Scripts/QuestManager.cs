using UnityEngine;
using System.Collections;

public class QuestManager : MonoBehaviour
{
	public static QuestManager instance = null;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);

		DontDestroyOnLoad(transform.gameObject);
	}
}
