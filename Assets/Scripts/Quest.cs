using UnityEngine;
using System;
using System.Collections;

public class Quest : MonoBehaviour
{
	public string title;
	public string description;
	public string startTime;
	public int duration;
	public TimeSpan startTimeSpan;
	[Serializable]
	public struct ValidDays {
		public bool sunday;
		public bool monday;
		public bool tuesday;
		public bool wednesday;
		public bool thursday;
		public bool friday;
		public bool saturday;
	}
	public ValidDays validDays;

	void Awake()
	{
		startTime = startTimeSpan.ToString();
	}
}
