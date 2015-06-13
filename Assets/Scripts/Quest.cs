using UnityEngine;
using System;
using System.Collections;

public class Quest : MonoBehaviour
{
	public string title;
	public string description;
	public string dueBy;
	public int duration;
	public TimeSpan dueByTimeSpan;
	public bool validOnWeekend;

	void Awake()
	{
		dueBy = dueByTimeSpan.ToString();
	}
}
