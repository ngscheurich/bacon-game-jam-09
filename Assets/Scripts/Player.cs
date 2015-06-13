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
	public int speed = 10;
	
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

	public void Move(float x) {
		float newPositionX = transform.position.x + x * Time.deltaTime * speed;
		float newPositionY = transform.position.y;
		Vector2 newPosition = new Vector2(newPositionX, newPositionY);
		transform.position = newPosition;
	}
}
