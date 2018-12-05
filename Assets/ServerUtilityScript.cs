using UnityEngine;
using System.Collections;

public class ServerUtilityScript : MonoBehaviour {
	string typeName="WGEKieranSongGame";
	string gameName="WGEKieranSongRoom";
	HostData[]hostList;
	string defaultAddress="10.15.3.147";
//	string ipAddress="";
	//string ipAddress="10.15.1.89";
	public GameObject startServerButton;
	public GameObject joinServerButton;
	public GameObject voxelChunkPrefab;
	public GameObject networkCubePrefab;
	public GameObject mainCamera;
	string inputIP="";
	bool listTrigger=false;
	bool ipTrigger=false;
	bool clientIpTrigger=false;

	// Use this for initialization
	void Start () {
		//MasterServer.ipAddress = defaultAddress;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//when user click on start server button this methord will be call and trigger the ipTrigger which allow user to input a ip address************************************
	public void StartServer()
	{
		if (!Network.isServer && !Network.isClient) 
		{
			ipTrigger=true;
//			Network.InitializeServer(4,25000,!Network.HavePublicAddress());
//			MasterServer.RegisterHost (typeName,gameName);
		}
	}


	//disable the join and start server button and create the terrain on server initialized*********************************************************************************
	void OnServerInitialized()
	{
		Debug.Log ("Server Initializied");
		joinServerButton.gameObject.SetActive (false);
		startServerButton.gameObject.SetActive (false);
		Network.Instantiate (voxelChunkPrefab, Vector3.zero, Quaternion.identity, 0);
	}


	//when user click on join server button this methord will be call and trigger the clientIpTrigger which allow user to input a ip address to find a server****************
	public void RefreshHostList()
	{
		clientIpTrigger = true;

//			MasterServer.RequestHostList (typeName);
//			Debug.Log (typeName);

	}


	//when client received a host list, the listTrigger will allow a list to show up and allow user to choose from the list*************************************************
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if(msEvent==MasterServerEvent.HostListReceived)
		{
			hostList=MasterServer.PollHostList ();
			listTrigger=true;
//			foreach(HostData hd in hostList)
//			{

//				if(hd.gameName==gameName)
//				{
//					Network.Connect (hd);
//				}
//			}
		}
	}


	//when client connect to a game server, disable the join or start server button and maincamera and spawn a controlable cube*******************************************
	void OnConnectedToServer()
	{
		Debug.Log ("Server Joined");
		joinServerButton.gameObject.SetActive (false);
		startServerButton.gameObject.SetActive (false);
		//SpawnPlayer();
		mainCamera.SetActive (false);
		Network.Instantiate (networkCubePrefab, new Vector3 (8, 8, 8), Quaternion.identity, 0);
	}





	void OnGUI()
	{
	
	
	//allow client to type in the ip address*******************************************************************************************************************************
		if (clientIpTrigger == true)
		{
			joinServerButton.gameObject.SetActive (false);
			startServerButton.gameObject.SetActive (false);
			
			GUI.Label(new Rect(Screen.width/3,Screen.height/3,Screen.width/2,Screen.height/20),"Please enter a IP address, or leave it blank to use default address");
			inputIP=GUI.TextField(new Rect(Screen.width/3,Screen.height/2,300,40),inputIP,20);
			if(GUI.Button(new Rect(Screen.width/1.5f,Screen.height/1.5f,100,50),"Confirm"))
			{
				if(inputIP=="")
				{
					MasterServer.ipAddress=defaultAddress;
				}
				else
				{
					MasterServer.ipAddress=inputIP;
				}
				print(MasterServer.ipAddress);
				MasterServer.RequestHostList (typeName);
				clientIpTrigger=false;
			}
			
			if(GUI.Button (new Rect (Screen.width / 2.4f, Screen.height/1.15f, Screen.width / 6, Screen.height / 18), "Back"))
			{
				
				clientIpTrigger=false;
				joinServerButton.gameObject.SetActive (true);
				startServerButton.gameObject.SetActive (true);
			}
		}
	
	
	//allow host to type in the ip address*******************************************************************************************************************************
		if (ipTrigger == true)
		{
			joinServerButton.gameObject.SetActive (false);
			startServerButton.gameObject.SetActive (false);

			GUI.Label(new Rect(Screen.width/3,Screen.height/3,Screen.width/2,Screen.height/20),"Please enter a IP address, or leave it blank to use default address");
			inputIP=GUI.TextField(new Rect(Screen.width/3,Screen.height/2,300,40),inputIP,20);
			if(GUI.Button(new Rect(Screen.width/1.5f,Screen.height/1.5f,100,50),"Confirm"))
			{
				if(inputIP=="")
				{
				MasterServer.ipAddress=defaultAddress;
				}
				else
				{
				MasterServer.ipAddress=inputIP;
				}
				print(MasterServer.ipAddress);
				Network.InitializeServer(4,25000,!Network.HavePublicAddress());
				MasterServer.RegisterHost (typeName,gameName);
				ipTrigger=false;
			}

			if(GUI.Button (new Rect (Screen.width / 2.4f, Screen.height/1.15f, Screen.width / 6, Screen.height / 18), "Back"))
			{
				
				ipTrigger=false;
				joinServerButton.gameObject.SetActive (true);
				startServerButton.gameObject.SetActive (true);
			}
		}




	//display a list of server to allow client to choose**************************************************************************************************************
		if (listTrigger == true)
		{
			joinServerButton.gameObject.SetActive (false);
			startServerButton.gameObject.SetActive (false);
			GUI.Box(new Rect(Screen.width / 2.5f, 20, Screen.width / 5, Screen.height/1.1f),"");
			foreach(HostData hd in hostList)
			{
				int i=30;

				if(GUI.Button (new Rect (Screen.width / 2.4f, i, Screen.width / 6, Screen.height / 18), hd.gameName))
				{
					Network.Connect (hd);
					listTrigger=false;
				}
				i+=20;
			}
			if(GUI.Button (new Rect (Screen.width / 2.4f, Screen.height/1.15f, Screen.width / 6, Screen.height / 18), "Back"))
			{

				listTrigger=false;
				joinServerButton.gameObject.SetActive (true);
				startServerButton.gameObject.SetActive (true);
			}
			//MasterServer.ClearHostList ();
		}

	}

}
