using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour
{
	public GameObject titleCanvas;
	public GameObject introCanvas;
	
	private GameManager gameManager;
	private bool titleDone;
	
	void Awake()
	{
		gameManager = GameManager.instance;
		
		titleCanvas = GameObject.Find("TitleCanvas");
		introCanvas = GameObject.Find("IntroCanvas");

		introCanvas.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Return)) {
			if (!titleDone) {
				titleCanvas.SetActive(false);
				introCanvas.SetActive(true);
				titleDone = true;
			} else {


			}
		}
			Application.LoadLevel("Mine");
	}
}
