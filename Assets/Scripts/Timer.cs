using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	private float seconds;
	private float startTime;
	private Text text;
	public bool stop = false;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!stop) {
			seconds = (int)Mathf.Round (Time.time - startTime);
			text.text = seconds.ToString ();
		}
	}


}
