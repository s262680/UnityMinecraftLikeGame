using UnityEngine;
using System.Collections;

public class HomeButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (Screen.width / 1.15f, Screen.height / 20f, Screen.width / 8, Screen.height / 12), "Home")) {
			Application.LoadLevel ("MainMenu");
		}
	}
}
