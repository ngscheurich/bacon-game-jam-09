using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleManager : MonoBehaviour
{
	private Text titleText;
	private Text introText;
	private bool titleDone;
	
	void Awake()
	{
		GameManager.instance.InitializeGame();
		titleText = GameObject.Find("TitleText").GetComponent<Text>();
		introText = GameObject.Find("IntroText").GetComponent<Text>();
		introText.enabled = false;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return)) {
			if (!titleDone) {
				titleText.enabled = false;
				introText.enabled = true;
				titleDone = true;
			} else {
				Application.LoadLevel("Mine");
			}
		}
	}
}
