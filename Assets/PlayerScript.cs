using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	//public VoxelChunk voxelChunk;
	public bool check;
	GameObject cube;
	bool keyDown=false;
	public VoxelChunk voxelChunk;
	public VoxelGenerator voxelGenerator;
	public Pathfinder pathfinder;
	//public Canvas canvasObject;
	public GameObject mainPanel;
	public string inputName="";
	public bool F1Trigger=false;
	public bool F2Trigger=false;
	public bool invTrigger = false;
	public Vector3 direction;
	Vector3 s,e;
	//public MouseLook lockRotationX;
	public MouseLook[] lockRotation;
	//public bool invOpen=false;
	public int selectBlockType=1;
	public Texture2D[] selectionTextures;
	float alphaLevel1=1f;
	float alphaLevel2=0.5f;
	float alphaLevel3=0.5f;
	float alphaLevel4=0.5f;

	public GameObject grassFab;
	public GameObject dirtFab;
	public GameObject stoneFab;
	public GameObject sandFab;
//	public Material grass;
//	public Material dirt;
//	public Material stone;
//	public Material sand;
	
	public delegate void EventCheckBlockType(Vector3 index, int blockType);
	public static event EventCheckBlockType OnEventCheck;

	public delegate void EventCheckCollectedBlock(int blockType);
	public static event EventCheckCollectedBlock OnCollect;


	


	// Use this for initialization
	void Start () {

//		alphaLevel [0] = 1f;
//		alphaLevel [1] = 0.5f;
//		alphaLevel [2] = 0.5f;
//		alphaLevel [3] = 0.5f;
//		Color colorHolder=GUI.color;
		//Screen.showCursor = false;
		//lockRotationX =GetComponent<MouseLook>();




		lockRotation=GetComponentsInChildren<MouseLook>();
	}



	
	void OnGUI()
	{
		GUI.Box(new Rect(0,Screen.height-30,250,30),"Press F1 to save, F2 to load");
		GUI.Box(new Rect(0,Screen.height-60,250,30),"Press i to open or close inventory");
		GUI.Box(new Rect(0,Screen.height-90,250,30),"Press 1-4 to select blocks");
		
		//just act as a crosshair**************************************************************************************************************************
		GUI.Label(new Rect(Screen.width/2-10,Screen.height/2-10,20,20),"+");




		//choose different blocks by pressing 1,2,3 or 4 key***********************************************************************************************
		//changing the alpha level based on which block is chosen, so other blocks will not be as visable as the chosen block******************************
		Color colorHolder=GUI.color;

		GUI.Box(new Rect(Screen.width/4,Screen.height/1.2f,Screen.width/2,Screen.height/8),"");

		GUI.color = new Color (1, 1, 1, alphaLevel1);
		GUI.DrawTexture (new Rect (Screen.width / 3.9f, Screen.height/1.2f, Screen.width / 10, Screen.height / 8), selectionTextures [0]);
		GUI.color = new Color (1, 1, 1, alphaLevel2);
		GUI.DrawTexture (new Rect (Screen.width / 2.65f, Screen.height/1.2f, Screen.width / 10, Screen.height / 8), selectionTextures [1]);
		GUI.color = new Color (1, 1, 1, alphaLevel3);
		GUI.DrawTexture (new Rect (Screen.width / 1.98f, Screen.height/1.2f, Screen.width / 10, Screen.height / 8), selectionTextures [2]);
		GUI.color = new Color (1, 1, 1, alphaLevel4);
		GUI.DrawTexture (new Rect (Screen.width / 1.58f, Screen.height/1.2f, Screen.width / 10, Screen.height / 8), selectionTextures [3]);

		GUI.color = colorHolder;

		if (Input.GetKeyDown ("1")) 
		{
			selectBlockType=1;
			alphaLevel1=1f;
			alphaLevel2=0.4f;
			alphaLevel3=0.4f;
			alphaLevel4=0.4f;
		}
		else if (Input.GetKeyDown ("2")) 
		{
			selectBlockType=2;
			alphaLevel1=0.4f;
			alphaLevel2=1f;
			alphaLevel3=0.4f;
			alphaLevel4=0.4f;
		}
		else if (Input.GetKeyDown ("3")) 
		{
			selectBlockType=3;
			alphaLevel1=0.4f;
			alphaLevel2=0.4f;
			alphaLevel3=1f;
			alphaLevel4=0.4f;
		}
		else if (Input.GetKeyDown ("4")) 
		{
			selectBlockType=4;
			alphaLevel1=0.4f;
			alphaLevel2=0.4f;
			alphaLevel3=0.4f;
			alphaLevel4=1f;
		}




		//On pressing the F1 key, a prompt will show up and ask user to type in a file name*************************************************************
		//and SAVE the terrain, player position and angle in to that xml file***************************************************************************
		//camera and controller rotation are disable during this process********************************************************************************
		if (Input.GetKeyDown (KeyCode.F1)) 
		{
			F1Trigger=true;
		}
		if(F1Trigger==true)
		{
			foreach(MouseLook n in lockRotation)
			{
				n.enabled=false;
			}
			
			GUI.Label(new Rect(Screen.width/3,Screen.height/2.5f,300,40),"Please type in a file name to save");
			
			inputName=GUI.TextField(new Rect(Screen.width/3,Screen.height/2,300,40),inputName,30);
			if(GUI.Button(new Rect(Screen.width/1.5f,Screen.height/1.5f,100,50),"Confirm"))
			{
				foreach(MouseLook n in lockRotation)
				{
					n.enabled=true;
				}
				float PosX =this.gameObject.transform.position.x;
				float PosY =this.gameObject.transform.position.y;
				float PosZ =this.gameObject.transform.position.z;
				float RotX =this.gameObject.transform.eulerAngles.x;
				float RotY =this.gameObject.transform.eulerAngles.y;
				float RotZ =this.gameObject.transform.eulerAngles.z;
				XMLVoxelFileWriter.SaveChunkToXMLFile(voxelChunk.terrainArray, PosX,PosY,PosZ,RotX,RotY,RotZ,inputName);
//				playerPosTrigger=true;
				F1Trigger=false;
			}
		}


		//On pressing the F2 key, a prompt will show up and ask user to type in a file name*************************************************************
		//and LOAD the terrain, player position and angle from that xml file****************************************************************************
		//camera and controller rotation are disable during this process********************************************************************************
		//Start and end position for pathfinding will also read from here if it exist(removed as its not necessary)*************************************
		if (Input.GetKeyDown (KeyCode.F2)) 
		{
			F2Trigger=true;
		}
		if(F2Trigger==true)
		{
			
			foreach(MouseLook n in lockRotation)
			{
			//lockRotationX.enabled=false;
				n.enabled=false;
			}
			
			GUI.Label(new Rect(Screen.width/3,Screen.height/2.5f,300,40),"Please type in a file name to load");
			
			inputName=GUI.TextField(new Rect(Screen.width/3,Screen.height/2,300,40),inputName,30);
			if(GUI.Button(new Rect(Screen.width/1.5f,Screen.height/1.5f,100,50),"Confirm"))
			{
				//lockRotationX.enabled=true;
				//lockRotationY.enabled=true;
				foreach(MouseLook n in lockRotation)
				{
					
					n.enabled=true;
				}
				
				
				if (System.IO.File.Exists(inputName+".xml"))
				{
					this.gameObject.transform.position=XMLVoxelFileWriter.LoadPosFromXMLFile(inputName);
					this.gameObject.transform.rotation=XMLVoxelFileWriter.LoadRotFromXMLFile(inputName);
				
//					XMLVoxelFileWriter.ReadStartAndEndPosition(out pathfinder.startPosition, out pathfinder.endPosition,inputName);
//					{
//						//Debug.Log (s);
//						pathfinder.startPosition=s;
//						pathfinder.endPosition=e;
//					}
					//get terrainArray from xml
					voxelChunk.terrainArray=XMLVoxelFileWriter.LoadChunkFromXMLFile(16,inputName);
					//draw the correct faces
					voxelChunk.CreateTerrain();
					//Update mesh info
					voxelGenerator.UpdateMesh();
				F2Trigger=false;
				}
			}
		
		
		}
	}
	// Update is called once per frame
	void Update () 
	{
	

		//Disable the time scale and set blocks when saving, loading or opening inventroy**************************************************************	
		if(F1Trigger||F2Trigger||invTrigger)
		{
			Time.timeScale=0;
			voxelChunk.CantSetBlocks();
		}
		else if(!F1Trigger||!F2Trigger||!invTrigger)
		{
			Time.timeScale=1;
			voxelChunk.CanSetBlocks();
		}




		//Call open and close inventroy method when press "i" key**************************************************************************************
		if(Input.GetKeyDown("i"))
		{
		openAndCloseInv();
		}







		
//		if(Input.GetKeyDown("i")&&invOpen==false)
//		{
//
//			Canvas inv=canvasObject.GetComponent<Canvas>();
//			inv.enabled=true;
//			invOpen=true;
//			
//		}
		

		





		//Destroy a block by clicking left mouse button and spawn that block as a collectable item by using delegate***********************************
		if (Input.GetButtonDown ("Fire1")) 
		{
			check=true;
			Vector3 v;
			if (PickThisBlock (out v, 4))
			 {			 
			 OnEventCheck(v,0);
				//voxelChunk.SetBlock (v, 0);
				//SpawnCube(v,1);
			 }
		} 


		//create a block by clicking right mouse button based on the select block type*****************************************************************
		else if (Input.GetButtonDown ("Fire2")) 
		{
			check=false;
			Vector3 v;
			if (PickThisBlock(out v, 4))
			{			
			OnEventCheck(v,selectBlockType);
				//voxelChunk.SetBlock(v,1);
			}
		}






//		if(voxelChunk.playerPosTrigger==true)
//		{
//			float PosX =this.gameObject.transform.position.x;
//			float PosY =this.gameObject.transform.position.y;
//			float PosZ =this.gameObject.transform.position.z;
//			float RotX =this.gameObject.transform.eulerAngles.x;
//			float RotY =this.gameObject.transform.eulerAngles.y;
//			float RotZ =this.gameObject.transform.eulerAngles.z;
//			XMLVoxelFileWriter.SaveChunkToXMLFile(voxelChunk.terrainArray, PosX,PosY,PosZ,RotX,RotY,RotZ,voxelChunk.inputName);
//			voxelChunk.playerPosTrigger=false;
//			//Debug.Log("save test");
//		}
		

//		if (Input.GetKeyDown (KeyCode.F2))
//		{
//			if (System.IO.File.Exists("PlayerPos.xml"))
//			{
//			//Debug.Log("abc");
//			this.gameObject.transform.position=XMLVoxelFileWriter.LoadPosFromXMLFile("PlayerPos");
//			this.gameObject.transform.rotation=XMLVoxelFileWriter.LoadRotFromXMLFile("PlayerPos");
//			}
//			
//		}

//
//		if (Input.GetKeyDown ("r")) 
//		{
//			keyDown = true;
//			PullInCubes();
//		}
		/*else if (Input.GetKeyUp ("r")) 
		{
			keyDown=false;
		}*/

	}











	//method that allow inventroy to be open when its closed, and allow it to be close when its opened by pressing the same key**************************
	//also disable the camera and controller rotation during the process*********************************************************************************
	void openAndCloseInv()
	{
		if (!mainPanel.activeInHierarchy) 
		{
			foreach(MouseLook n in lockRotation)
			{				
				n.enabled=false;
			}
			mainPanel.SetActive (true);
			invTrigger=true;
		}
		else
		{
			foreach(MouseLook n in lockRotation)
			{				
				n.enabled=true;				
			}
			mainPanel.SetActive (false);
			invTrigger=false;
		}







//		Canvas inv=canvasObject.GetComponent<Canvas>();
//		if(!inv.isActiveAndEnabled)
//		{
//			foreach(MouseLook n in lockRotation)
//			{
//
//				n.enabled=false;
//			}
//		inv.enabled=true;
//			invTrigger=true;
//		}
//		else
//		{
//			foreach(MouseLook n in lockRotation)
//			{
//				n.enabled=true;
//			}
//		inv.enabled=false;
//			invTrigger=false;
//		}
//		
	}

//	void PullInCubes()
//	{
//		Collider[] cArray = Physcis overlap sphere
//		 foreach(Collider c in cArray
//		 if c.name == drops
//		 c.gameObject.rigidbody.Add force
//	}









	//Method that allow create and destroy block depend on which button click*************************************************************************************************
	bool PickThisBlock(out Vector3 v, float dist)
	{
		v = new Vector3 ();
		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2,0));
		RaycastHit hit;

		if (check == true) 
		{
			if (Physics.Raycast (ray, out hit, dist)) 
			{
				//offset towards the centre of the block hit
				v = hit.point - hit.normal / 2;
				//round down to get the index of the block hit
				v.x = Mathf.Floor (v.x);
				v.y = Mathf.Floor (v.y);
				v.z = Mathf.Floor (v.z);
				
//				SpawnCube(v);
				return true;

			}
		}

		if (check == false) 
		{
			if (Physics.Raycast (ray, out hit, dist)) 
			{
				//offset towards the centre of the *neighbouring* block
				v = hit.point + hit.normal / 2;
				//round down to get the index of the empty
				v.x = Mathf.Floor (v.x);
				v.y = Mathf.Floor (v.y);
				v.z = Mathf.Floor (v.z);
				return true;
			}
		}
		return false;
	}



	//delegate that trigger in setblock method, check the position and type of the block that just destroy by the player and pass it to the spawn cube method****************
	void OnEnable()
	{
		VoxelChunk.OnDestroyCheck+=SpawnCube;		
	}
	
	void OnDisable()
	{
		
		VoxelChunk.OnDestroyCheck-=SpawnCube;
	}



	//Spawn different types of cube depend on the int value**********************************************************************************************************************
	void SpawnCube(Vector3 pos, int type)
	{
		if (type == 1) {
			GameObject grass = (GameObject)Instantiate (grassFab, pos + new Vector3 (0.5f, 0.5f, 0.5f), Quaternion.identity);
			grass.name = "grassDrop";
			grass.rigidbody.AddForce (transform.up * 300);
			grass.rigidbody.AddForce (transform.forward * 50);
		}
		if (type == 2) {
			GameObject dirt = (GameObject)Instantiate (dirtFab, pos + new Vector3 (0.5f, 0.5f, 0.5f), Quaternion.identity);
			dirt.name = "dirtDrop";
			dirt.rigidbody.AddForce (transform.up * 300);
			dirt.rigidbody.AddForce (transform.forward * 50);
		}
		if (type == 3) {
			GameObject stone = (GameObject)Instantiate (stoneFab, pos + new Vector3 (0.5f, 0.5f, 0.5f), Quaternion.identity);
			stone.name = "stoneDrop";
			stone.rigidbody.AddForce (transform.up * 300);
			stone.rigidbody.AddForce (transform.forward * 50);
		}
		if (type == 4) {
			GameObject sand = (GameObject)Instantiate (sandFab, pos + new Vector3 (0.5f, 0.5f, 0.5f), Quaternion.identity);
			sand.name = "sandDrop";
			sand.rigidbody.AddForce (transform.up * 300);
			sand.rigidbody.AddForce (transform.forward * 50);
		}
//		if (type > 0) {
//			cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
//			cube.name = "drops";
//			BoxCollider cubeBoxCollider = cube.AddComponent<BoxCollider> ();
//			//cubeBoxCollider.isTrigger=true;
//			cube.transform.position = new Vector3 (pos.x + 0.5f, pos.y + 0.5f, pos.z + 0.5f);
//			cube.transform.localScale -= new Vector3 (0.7f, 0.7f, 0.7f);
////		cube.renderer.material= (Material)Resources.Load ("grass",typeof(Material));
//			Debug.Log (type);
//			Material[] mats = cube.renderer.materials;
//			mats [0] = grass;
//			mats [1] = dirt;
//			mats [2] = stone;
//			mats [3] = sand;
//			cube.renderer.material = mats [type-1];
//
//			Rigidbody cubeRigidBody = cube.AddComponent<Rigidbody> ();
//			cubeRigidBody.mass = 1;
//			cubeRigidBody.drag = 0.5f;
//			cubeRigidBody.AddForce (transform.up * 300);
//			cubeRigidBody.AddForce (transform.forward * 50);
//		}

	}
	
	
	
	//when a drop object collect with the FPC, check the type, pass the result to inventoryManager script and then destroy the object***************************************
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.name== ("grassDrop")) 
		{
			OnCollect(0);
			Destroy (hit.gameObject);
		}
		if (hit.gameObject.name== ("dirtDrop")) 
		{
			OnCollect(1);
			Destroy (hit.gameObject);
		}
		if (hit.gameObject.name== ("stoneDrop")) 
		{
			OnCollect(2);
			Destroy (hit.gameObject);
		}
		if (hit.gameObject.name== ("sandDrop")) 
		{
			OnCollect(3);
			Destroy (hit.gameObject);
		}
	
	
//			if (hit.gameObject.name==("drops"))
//			{
//				if(keyDown==true)
//				{
//				//Debug.Log("abc");
//				
//				
//				direction=this.gameObject.transform.position-hit.gameObject.transform.position;
//				hit.rigidbody.AddForce(direction* 300.0f);
//				//Debug.Log("abc");
//				//keyDown=false;
//				}
//				
//			}
	}
	


//	bool PickEmptyBlock(out Vector3 v, float dist, bool check)
//	{
//		v = new Vector3 ();
//		Ray ray = Camera.main.ScreenPointToRay (new Vector3 (Screen.width / 2, Screen.height / 2,0));
//		RaycastHit hit;
//		
//		if (Physics.Raycast (ray, out hit, dist, check==false)) 
//		{
//			//offset towards the centre of the *neighbouring* block
//			v=hit.point+hit.normal/2;
//			//round down to get the index of the empty
//			v.x=Mathf.Floor(v.x);
//			v.y=Mathf.Floor(v.y);
//			v.z=Mathf.Floor(v.z);
//			return true;
//		}
//		return false;
//	}
}
