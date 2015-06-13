using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public Player player;
	public int minutesPerDay = 1440;
	public int day = 1;
	public int time = 0;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		DontDestroyOnLoad(transform.gameObject);
	}

	void Start()
	{
		StartCoroutine(IncrementTime());
	}

	IEnumerator IncrementTime()
	{
		while (true) {
			time++;
			ct.text = string.Format("Day {0} - {1}", day, time); 
			yield return WaitForSeconds(1);
		}
	}

}
