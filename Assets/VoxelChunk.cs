using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class VoxelChunk : MonoBehaviour {

	VoxelGenerator voxelGenerator;
	public int[, ,] terrainArray;
	int chunkSize=16;
	bool cantSetBlock=false;
//	public string inputName="";
//	public bool F1Trigger=false;
	GameObject cube;
	List<Vector3> wayPointList;
	public bool playerPosTrigger=false;
	
	//delegat signature
	//public delegate void EventBlockChanged();
	public delegate void EventBlockChangedWithType(int blockType);
	
	//event instances for EventBlockChanged
//	public static event EventBlockChanged OnEventBlockDestroyed;
//	public static event EventBlockChanged OnEventBlockPlaced;
	public static event EventBlockChangedWithType OnEventBlockChanged;


	public delegate void EventCheckDestroyType(Vector3 index, int destroyType);
	public static event EventCheckDestroyType OnDestroyCheck;



	//delegate that prevent and changes to blocks ****************************************************************************************************************************
	public void CantSetBlocks()
	{
		cantSetBlock = true;
	}

	public void CanSetBlocks()
	{
		cantSetBlock = false;
	}
	
	
	
	
	//Initialise the terrain*****************************************************************************************************************************************************
	void Start () {

		Pathfinder.traversalBegin += CantSetBlocks;
		Pathfinder.traversalEnd += CanSetBlocks;

		voxelGenerator = GetComponent<VoxelGenerator>();
		terrainArray=new int[chunkSize, chunkSize, chunkSize];

		voxelGenerator.Initialise ();

		InitialiseTerrain ();

		//get terrainArray from xml
		terrainArray=XMLVoxelFileWriter.LoadChunkFromXMLFile(16,"AssessmentChunk2");

		CreateTerrain ();

		voxelGenerator.UpdateMesh ();


		wayPointList=new List<Vector3>();


		for (int x=0; x<terrainArray.GetLength (0); x++) 		
		{
			for (int y=0; y<terrainArray.GetLength(1);y++)
			{
				for(int z=0;z<terrainArray.GetLength(2);z++)
				{
					if (terrainArray[x,y,z]==3)
					{

						wayPointList.Add(new Vector3(x+0.5f,y+1.5f,z+0.5f) );
					}
				}
			}
		}



	}

	
	//obtain the traversalible terrain list by check if the block on the the surface are dirt or stone***************************************************************
	public List<Vector3> GetTerrainList()
	{
		List<Vector3> terrainHolder = new List<Vector3> ();

		int y = 3;

		for (int x=0; x<terrainArray.GetLength (0); x++) 		
		{

				for(int z=0;z<terrainArray.GetLength(2);z++)
				{
					if (terrainArray[x,y,z]==3||terrainArray[x,y,z]==2)
					{
						terrainHolder.Add(new Vector3(x,y,z));
                    }
				}

		}

		return terrainHolder;
	}


	//old terrain******
	void InitialiseTerrain()
	{
		terrainArray=new int[chunkSize, chunkSize, chunkSize];

		for (int x=0; x<terrainArray.GetLength (0); x++) 		
		{
			for (int y=0; y<terrainArray.GetLength(1);y++)
			{
				for(int z=0;z<terrainArray.GetLength(2);z++)
				{
					if (y==3)
					{

						terrainArray[x,y,z]=1;

					}
					else if (y<3)
					{

						terrainArray[x,y,z]=2;
					}

					terrainArray[0, 3, 1] = 3; 
					terrainArray[0, 3, 2] = 3; 
					terrainArray[0, 3, 3] = 3; 
					terrainArray[1, 3, 3] = 3; 
					terrainArray[1, 3, 4] = 3; 
					terrainArray[2, 3, 4] = 3; 
					terrainArray[3, 3, 4] = 3; 
					terrainArray[4, 3, 4] = 3; 
					terrainArray[5, 3, 4] = 3; 
					terrainArray[5, 3, 3] = 3; 
					terrainArray[5, 3, 2] = 3; 
					terrainArray[6, 3, 2] = 3; 
					terrainArray[7, 3, 2] = 3; 
					terrainArray[8, 3, 2] = 3;  
					terrainArray[9, 3, 2] = 3; 
					terrainArray[10, 3, 2] = 3; 
					terrainArray[11, 3, 2] = 3; 
					terrainArray[12, 3, 2] = 3; 
					terrainArray[13, 3, 2] = 3; 
					terrainArray[13, 3, 3] = 3; 
					terrainArray[14, 3, 3] = 3; 
					terrainArray[15, 3, 3] = 3;
				}
			}
		}
	}

//	void OnGUI()
//	{
//		if (Input.GetKeyDown (KeyCode.F1)) 
//		{
//			 F1Trigger=true;
//		}
//			 if(F1Trigger==true)
//				{
//					
//					GUI.Label(new Rect(Screen.width/3,Screen.height/2.5f,300,40),"Please type in a file name to save");
//					
//					inputName=GUI.TextField(new Rect(Screen.width/3,Screen.height/2,300,40),inputName,30);
//					if(GUI.Button(new Rect(Screen.width/1.5f,Screen.height/1.5f,100,50),"Confirm"))
//						{
//							//XMLVoxelFileWriter.SaveChunkToXMLFile(terrainArray,inputName);
//							playerPosTrigger=true;
//							F1Trigger=false;
//						}
//				}
//			
//			
//	}
	void Update () 
	{
	
//	if(F1Trigger==true)
//	{
//	Time.timeScale=0;
//	CantSetBlocks();
//	}
//	else if(F1Trigger==false)
//	{
//	Time.timeScale=1;
//	CanSetBlocks();
//	}
	
//			if (Input.GetKeyDown ("space")) 
//				{
//					StartCoroutine (Lerp());
//				}

//		if (Input.GetKeyDown (KeyCode.F1)) 
//		{
//			if (System.IO.File.Exists("VoxelChunk.xml"))
//			{
//			Debug.Log("abc");
//			GUI.Label(new Rect(Screen.width/3,Screen.height/2.5f,300,40),"Please type in a file name to save");
//			fileName=GUI.TextField(new Rect(Screen.width/3,Screen.height/2,300,40),fileName,30);
//			Debug.Log("efg");
//			if(GUI.Button(new Rect(Screen.width/1.5f,Screen.height/1.5f,100,50),"Confirm"))
//			{
//			XMLVoxelFileWriter.SaveChunkToXMLFile(terrainArray,"VoxelChunk");
//			}
//			}
//		}

//		if (Input.GetKeyDown (KeyCode.F2))
//		{
//			//get terrainArray from xml
//			terrainArray=XMLVoxelFileWriter.LoadChunkFromXMLFile(16,"VoxelChunk");
//			//draw the correct faces
//			CreateTerrain();
//			//Update mesh info
//			voxelGenerator.UpdateMesh();
//		}
	}




	//create the terrain and reduce the polygon by removing the faces inside the chuck******************************************************************
	public void CreateTerrain()
	{
		
		for (int x=0; x<terrainArray.GetLength (0); x++) 		
		{
			for (int y=0; y<terrainArray.GetLength(1);y++)
			{
				for(int z=0;z<terrainArray.GetLength(2);z++)
				{
					if (terrainArray[x,y,z]!=0)
					{
						string tex;

						switch(terrainArray[x,y,z])
						{
						case 1:
							tex="Grass";
							break;
						case 2:
							tex="Dirt";
							break;
						case 3:
							tex="Stone";
							break;
						case 4:
							tex="Sand";
							break;
						default:
							tex="Grass";
							break;
						}

						//voxelGenerator.CreateVoxel(x,y,z,tex);

						//check if we need to draw that face
						if (x==0||terrainArray[x-1,y,z]==0)
						{
							voxelGenerator.CreateNegtiveXFace(x,y,z,tex);
						}
						if (x==terrainArray.GetLength (0) - 1 || terrainArray[x+1,y,z]==0)
						{
							voxelGenerator.CreatePositiveXFace(x,y,z,tex);
						}

						if (y==0||terrainArray[x,y-1,z]==0)
						{
							voxelGenerator.CreateNegtiveYFace(x,y,z,tex);
						}
						if (y==terrainArray.GetLength (0) - 1 || terrainArray[x,y+1,z]==0)
						{
							voxelGenerator.CreatePositiveYFace(x,y,z,tex);
						}

						if (z==0||terrainArray[x,y,z-1]==0)
						{
							voxelGenerator.CreateNegtiveZFace(x,y,z,tex);
						}
						if (z==terrainArray.GetLength (0) - 1 || terrainArray[x,y,z+1]==0)
						{
							voxelGenerator.CreatePositiveZFace(x,y,z,tex);
						}

						//print ("Create"+tex+"block,");
					}
				}
			}
		}
	}
	
	
	//delegate that pass the position and block type that player click on and pass it to set block**************************************************************
	void OnEnable()
	{
		PlayerScript.OnEventCheck+=SetBlock;
				
	}
	
	void OnDisable()
	{
		PlayerScript.OnEventCheck-=SetBlock;
				
	}


	//create of destroy blocks depend on the block type value*****************************************************************************************************
	[RPC] public void SetBlock(Vector3 index, int blockType)
	{
		if (!cantSetBlock) 
		{
			int checktype;
			if ((index.x > 0 
				&& index.x < terrainArray.GetLength (0))
				&& (index.y > 0
				&& index.y < terrainArray.GetLength (1))
				&& (index.z > 0
				&& index.y < terrainArray.GetLength (2))) 
			{
				checktype=terrainArray [(int)index.x, (int)index.y, (int)index.z];
				if(OnDestroyCheck!=null)
				{
				OnDestroyCheck(index, checktype);
				}

				terrainArray [(int)index.x, (int)index.y, (int)index.z] = blockType;
				CreateTerrain ();
				voxelGenerator.UpdateMesh ();
		
				//PlayerScript.OnEventCheck+=blockType;
				if(OnEventBlockChanged!=null)
				{
					OnEventBlockChanged (blockType);
				}
//		if (blockType==0)
//			{
//				OnEventBlockDestroyed();
//			}
//		else
//			{
//				OnEventBlockPlaced();
//			}
			}
		}
	}

	//check if the block is traversable for pathfinding scene********************************************************************************************
	public bool IsTraversable(Vector3 voxel)
	{
		//is block empty
		bool isEmpty = terrainArray [(int)voxel.x, (int)voxel.y, (int)voxel.z] == 0;
		//is block below stone
		bool isBelowStone= terrainArray [(int)voxel.x, (int)voxel.y-1, (int)voxel.z] == 3;
		bool isBelowDirt= terrainArray [(int)voxel.x, (int)voxel.y-1, (int)voxel.z] == 2;
		return isEmpty && (isBelowStone||isBelowDirt);
	
	}
	//check if the block is traversable for dijkstra only*************************************************************************************************
	public bool IsTraversableForDijkstra(Vector3 voxel)
	{
		//is block empty
		bool isEmpty = terrainArray [(int)voxel.x, (int)voxel.y+1, (int)voxel.z] == 0;
		//is block below stone
		bool isBelowStone= terrainArray [(int)voxel.x, (int)voxel.y, (int)voxel.z] == 3;
		bool isBelowDirt= terrainArray [(int)voxel.x, (int)voxel.y, (int)voxel.z] == 2;
		return isEmpty && (isBelowStone||isBelowDirt);
		
	}
	
	
	//check if the block is stone or not and return the result**********************************************************************************************
	public bool isStone(Vector3 voxel)
	{
		bool isBelowStone= terrainArray [(int)voxel.x, (int)voxel.y, (int)voxel.z] == 3;
		return isBelowStone;
	}
	
	//check if the dirt is stone or not and return the result**********************************************************************************************
	public bool isDirt(Vector3 voxel)
	{
		bool isBelowDirt= terrainArray [(int)voxel.x, (int)voxel.y, (int)voxel.z] == 2;
		return isBelowDirt;
	}
	
	public int GetChunkSize()
	{
		return chunkSize;
	}
	
	
//	IEnumerator LerpAlongPath(Stack<Vector3> path)
//			{
//
//				if (cube) 
//				{
//					Destroy(cube);
//				}
//				cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//				for (int i =1; i<wayPointList.Count; i++) 
//				{
//					float ct = 0;
//					float tt = 1f;
//					while (ct<tt) 
//					{
//						ct += Time.deltaTime;
//						cube.transform.position = Vector3.Lerp (wayPointList[i-1], wayPointList[i], ct / tt);
//						yield return 0;
//					}
//				}
//			}


}