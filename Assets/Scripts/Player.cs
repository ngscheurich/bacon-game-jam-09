using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
	public static Player instance = null;
	public int mentalHealth = 100;
	public Text mentalHealthText;
	public int damageSusceptibility = 1;
	public Quest currentQuest;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			DestroyObject(this);

		DontDestroyOnLoad(transform.gameObject);

		AdjustMentalHealth(0);
	}

	public void AdjustMentalHealth(int adj)
	{
		mentalHealth += adj;
		mentalHealthText.text = "Mental Health: " + mentalHealth;
	}

	public void TakeDamage(int damage)
	{
		AdjustMentalHealth(damage * damageSusceptibility);
	}
}
