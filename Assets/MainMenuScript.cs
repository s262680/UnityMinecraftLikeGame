using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (Screen.width / 2.4f, Screen.height / 3, Screen.width / 8, Screen.height / 12), "VoxelChunk")) 
		{
			Application.LoadLevel("VoxelChunk");
		}
		if (GUI.Button (new Rect (Screen.width / 2.4f, Screen.height / 2, Screen.width / 8, Screen.height / 12), "PathFinding")) 
		{
			Application.LoadLevel("PathFinding");
		}
		if (GUI.Button (new Rect (Screen.width / 2.4f, Screen.height / 1.5f, Screen.width / 8, Screen.height / 12), "Network")) 
		{
			Application.LoadLevel("Network");
		}
	}
}
