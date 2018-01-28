using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// load the specified Unity level
	public void loadLevel(string leveltoLoad)
	{
		Debug.Log ("sdafasdfasdf");
		// load the specified level
		SceneManager.LoadScene (leveltoLoad);

	}

}
