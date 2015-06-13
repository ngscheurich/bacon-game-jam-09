using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
	public static Player instance = null;
	public int mentalHealth = 100;
	public int fatigue = 10;
	public int damageSusceptibility = 1;
	public int fatigueFactor = 1;
	public int sleepQuality = 1;
	public Quest currentQuest;
	public Text mentalHealthText;
	public Text fatigueText;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);

		DontDestroyOnLoad(transform.gameObject);

		AdjustMentalHealth(0);
		AdjustFatigue(0);
	}

	public void AdjustMentalHealth(int amount)
	{
		mentalHealth += amount;
		mentalHealthText.text = "Mental Health: " + mentalHealth;
	}

	public void AdjustFatigue(int amount)
	{
		fatigue += amount;
		fatigueText.text = "Fatigue: " + fatigue;
	}

	public void TakeDamage(int amount)
	{
		AdjustMentalHealth(amount * damageSusceptibility);
	}

	public void Sleep(int minutes)
	{
		fatigue += minutes * sleepQuality;
	}
}
