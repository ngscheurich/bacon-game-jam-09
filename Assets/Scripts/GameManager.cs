using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public Player player;
	public int minutesPerDay = 1440;
	public int day = 1;
	public int seconds = 0;
	public Text currentTimeText;

	public DateTime now;


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
			int newTime = (seconds == 1440) ? 0 : seconds + 1;
			if (newTime == 0) day++;
			seconds = newTime;
			//string
			currentTimeText.text = string.Format("Day {0} - {1}", day, seconds);
			yield return new WaitForSeconds(1);
		}
	}

}
