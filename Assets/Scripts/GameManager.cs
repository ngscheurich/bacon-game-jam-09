using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public Player player;
	public int currentDay = 1;
	public float timeIncrementFactor = 8;
	public Text currentDateTimeText;

	private DateTime initialDateTime = new DateTime(1983, 12, 12, 23, 58, 0);
	private DateTime currentDateTime;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);
		DontDestroyOnLoad(transform.gameObject);

		player = Player.instance;
	}

	void Start()
	{
		currentDateTime = initialDateTime;
		StartCoroutine(IncrementTime());
	}

	IEnumerator IncrementTime()
	{
		while (true) {
			currentDateTime = currentDateTime.AddMinutes(1 * timeIncrementFactor);
			TimeSpan dateTimeDelta = currentDateTime.Subtract(initialDateTime);
			int daysDelta = dateTimeDelta.Days;
			currentDay = daysDelta + 1;
			currentDateTimeText.text = string.Format("Day {0} - {1}", currentDay, currentDateTime.ToShortTimeString());
			yield return new WaitForSeconds(1);
		}
	}

}
