using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class Player : MonoBehaviour
{
	public static Player instance = null;

	public int sanity = 100;
	public int health = 100;
	public int fatigue = 0;

	public int strength = 10;
	public int dexterity = 10;
	public int charisma = 10;
	
	public int fatigueFactor = 1;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);

		DontDestroyOnLoad(transform.gameObject);
	}

	public void AdjustSanity(int amount)
	{
		sanity += amount;
	}

	public void AdjustHealth(int amount)
	{
		health += amount;
  	}

	public void AdjustFatigue(int amount)
	{
		fatigue += amount;
  	}
}
