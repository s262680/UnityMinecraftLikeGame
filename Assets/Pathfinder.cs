using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour {

	public VoxelChunk voxelChunk;
	public VoxelGenerator voxelGenerator;
	GameObject cube;
	bool traversing=false;

	public Vector3 startPosition=new Vector3(0,4,1);
	public Vector3 endPosition=new Vector3(15,4,3);
	Vector3 offset=new Vector3(0.5f,0.5f,0.5f);

	public delegate void EventCheckTraversal();

	public static event EventCheckTraversal traversalBegin;
	public static event EventCheckTraversal traversalEnd;

	// Use this for initialization
	void Start () {
	
	
	//load the second assessment start and end position by default****************************************************************************************************
	XMLVoxelFileWriter.ReadStartAndEndPosition(out startPosition, out endPosition,"AssessmentChunk2");

	}


	//buttons that allow user to changing the map, start and end position or start the pathfinding********************************************************************* 
	void OnGUI()
	{
		
		if(GUI.Button(new Rect(50,50,150,50), "AssessmentChunk1"))
		   {

			XMLVoxelFileWriter.ReadStartAndEndPosition(out startPosition, out endPosition,"AssessmentChunk1");
			//get terrainArray from xml
			voxelChunk.terrainArray=XMLVoxelFileWriter.LoadChunkFromXMLFile(16,"AssessmentChunk1");
			//draw the correct faces
			voxelChunk.CreateTerrain();
			//Update mesh info
			voxelGenerator.UpdateMesh();
			
			}

		if(GUI.Button(new Rect(50,120,150,50), "AssessmentChunk2"))
		{
			
			XMLVoxelFileWriter.ReadStartAndEndPosition(out startPosition, out endPosition,"AssessmentChunk2");
			//get terrainArray from xml
			voxelChunk.terrainArray=XMLVoxelFileWriter.LoadChunkFromXMLFile(16,"AssessmentChunk2");
			//draw the correct faces
			voxelChunk.CreateTerrain();
			//Update mesh info
			voxelGenerator.UpdateMesh();
			
		}

		if (!traversing) 
		{
			if (GUI.Button (new Rect (50, 190, 150, 50), "Start")) 
			{
			
				Stack<Vector3> path = Dijkstra (startPosition, endPosition, voxelChunk);
				if (path.Count > 0) 
				{
					//Debug.Log (path.Count);
					StartCoroutine (LerpAlongPath (path));
				}
			}
		}

		
	
}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown ("t")) 
		{

			foreach (Vector3 n in voxelChunk.GetTerrainList()) 
			{

				Debug.Log (n);
			}
		}


	//another way to start the pathfinding by pressing f4*********************************************************************************************
	if (!traversing) 
		{
			if (Input.GetKeyDown (KeyCode.F4)) 
			{
			
				//Debug.Log ("a");
				//Stack<Vector3> path = BreadthFirstSearch (startPosition, endPosition, voxelChunk);
				Stack<Vector3> path =  Dijkstra(startPosition, endPosition, voxelChunk);
				if (path.Count > 0) 
				{
					//Debug.Log (path.Count);
					StartCoroutine(LerpAlongPath(path));
				}
			}

//			if(Input.GetKeyDown(KeyCode.F2))
//			{
//				Vector3 s, e;
//				if (XMLVoxelFileWriter.ReadStartAndEndPosition(out s, out e, "VoxelChunk"))
//				{
//					startPosition=s;
//					endPosition=e;
//				}
//			}
		}
	}

	Stack<Vector3> BreadthFirstSearch
		(Vector3 start, Vector3 end, VoxelChunk vc)
	{
		//stack not usually used with BFS
		//now we use this to build a waypoint list by traver through parents
		Stack<Vector3> waypoints = new Stack<Vector3> ();
		Dictionary<Vector3, Vector3> visitedParent = new Dictionary<Vector3,Vector3> ();
		Queue<Vector3> q = new Queue<Vector3> ();
		bool found = false;
		Vector3 current = start;

		q.Enqueue (start);

		while (q.Count>0 && !found) 
		{
			current=q.Dequeue();

			if(current!=end)
			{
				//our adjacent nodes are x+1, x-1, z+1 and z-1
				List<Vector3> neighbourList=new List<Vector3>();
				neighbourList.Add (current+new Vector3(1,0,0));//x+1
				neighbourList.Add (current+new Vector3(-1,0,0));//x-1
				neighbourList.Add (current+new Vector3(0,0,1));//z+1
				neighbourList.Add (current+new Vector3(0,0,-1));//z-1

				foreach(Vector3 n in neighbourList)
				{
					//check if n is within range
					if((n.x>=0&&n.x<vc.GetChunkSize())
					   &&n.z>=0&&n.z<vc.GetChunkSize())
					{
						//check if we can traverse over this
						if(vc.IsTraversable(n))
						{
							//check if node is alread processed
							if(!visitedParent.ContainsKey(n))
							{
								visitedParent[n]=current;
								q.Enqueue(n);
							}
						}
					}
				}
			}
			else
			{
				found=true;
			}
		}

		if(found)
		{
			while (current!=start)
				{
					//put the current location into the waypoint
					waypoints.Push(current+offset);
					//find the parent of the current and replace the current location
					current=visitedParent[current];

				}
				waypoints.Push (start+offset);
		}
		//Debug.Log (waypoints.Count);
		return waypoints;

	}

	Stack<Vector3> DepthFirstSearch
		(Vector3 start, Vector3 end, VoxelChunk vc)
	{
		//stack not usually used with BFS
		//now we use this to build a waypoint list by traver through parents
		Stack<Vector3> waypoints = new Stack<Vector3> ();
		Dictionary<Vector3, Vector3> visitedParent = new Dictionary<Vector3,Vector3> ();
		Stack<Vector3> q = new Stack<Vector3> ();
		bool found = false;
		Vector3 current = start;
		
		q.Push (start);
		
		while (q.Count>0 && !found) 
		{
			current=q.Pop();
			
			if(current!=end)
			{
				//our adjacent nodes are x+1, x-1, z+1 and z-1
				List<Vector3> neighbourList=new List<Vector3>();
				neighbourList.Add (current+new Vector3(1,0,0));//x+1
				neighbourList.Add (current+new Vector3(-1,0,0));//x-1
				neighbourList.Add (current+new Vector3(0,0,1));//z+1
				neighbourList.Add (current+new Vector3(0,0,-1));//z-1
				
				foreach(Vector3 n in neighbourList)
				{
					//check if n is within range
					if((n.x>=0&&n.x<vc.GetChunkSize())
					   &&n.z>=0&&n.z<vc.GetChunkSize())
					{
						//check if we can traverse over this
						if(vc.IsTraversable(n))
						{
							//check if node is alread processed
							if(!visitedParent.ContainsKey(n))
							{
								visitedParent[n]=current;
								q.Push(n);
							}
						}
					}
				}
			}
			else
			{
				found=true;
			}
		}
		
		if(found)
		{
			while (current!=start)
			{
				//put the current location into the waypoint
				waypoints.Push(current+offset);
				//find the parent of the current and replace the current location
				current=visitedParent[current];
				
			}
			waypoints.Push (start+offset);
		}
		//Debug.Log (waypoints.Count);
		return waypoints;
		
	}
	
	
	
	
	
		
	//dijkstra method that allow finding the shortest path depending on the cost of the blocks*******************************************************
	Stack<Vector3> Dijkstra( Vector3 start, Vector3 end,VoxelChunk vc)
	{
		Stack<Vector3> waypoints = new Stack<Vector3> ();
		Dictionary<Vector3, int> distance = new Dictionary<Vector3,int> ();
		Dictionary<Vector3, Vector3> visitedParent = new Dictionary<Vector3,Vector3> ();
		bool found = false;
		Stack<Vector3> q = new Stack<Vector3> ();
		List<Vector3> l=new List<Vector3>();
		int newDist=0;
		int length =0;
		int tempHolder;
		//Vector3 tempDKHolder;
		start += new Vector3 (0, -1, 0);
		end += new Vector3 (0, -1, 0);
		Vector3 current =start;

		foreach (Vector3 n in vc.GetTerrainList()) 
		{
			distance[n]=int.MaxValue;
	
				l.Add (n);
			//Debug.Log (n);
		}
		distance [start] = 0;
		while (l.Count>0 && !found) 
		{
			//set tempHolder to inf*
			tempHolder=int.MaxValue;

			//loop through each of the items in the list to find the smallest value
			foreach(Vector3 v in l)
			{
				//if the item is in the distance dictionary
				if(distance.ContainsKey(v))
				{
					//the tempholder will be always greater than the first value because its inf
					//but after it will equal to the first value
					//which will campare to each value of the list to find the smallest value again
					if(tempHolder>distance[v])
					{
						tempHolder=distance[v];
						//once it found the smaller value, set the current equal to that vector
						current=v;

					}
				}

			}
			//once the smallest value confirmed, remove the item from the list
			l.Remove(current);



			if(current!=end)
			{
				//our adjacent nodes are x+1, x-1, z+1 and z-1
				List<Vector3> neighbourList=new List<Vector3>();
				neighbourList.Add (current+new Vector3(1,0,0));//x+1
				neighbourList.Add (current+new Vector3(-1,0,0));//x-1
				neighbourList.Add (current+new Vector3(0,0,1));//z+1
				neighbourList.Add (current+new Vector3(0,0,-1));//z-1
				
				foreach(Vector3 n in neighbourList)
				{
					if (l.Contains(n))
					{
						//check if n is within range
						if((n.x>=0&&n.x<vc.GetChunkSize())
						   &&n.z>=0&&n.z<vc.GetChunkSize())
						{
							//check if we can traverse over this
							if(vc.IsTraversableForDijkstra(n))
							{
								if(vc.isStone(n))
								{
									length=1;
								}
								else 
								{
									length=3;							
								}
								
								newDist=distance[current]+length;
								
								if(newDist<distance[n]||!distance.ContainsKey(n))
								{
//									//check if node is alread processed
//									if(!visitedParent.ContainsKey(n))
//									{
										distance[n]=newDist;
										visitedParent[n]=current;

//									}
								}
							 }
						  }
					  }
				 }
			}
			else
			{
				found=true;
			}
		}
		if(found)
		{
			while (current!=start)
			{
				//put the current location into the waypoint
				waypoints.Push(current+offset+new Vector3(0,1,0));
				//find the parent of the current and replace the current location
				current=visitedParent[current];
				
			}
			waypoints.Push (start+offset+new Vector3(0,1,0));
		}
		//Debug.Log (waypoints.Count);
		return waypoints;

	}


	
	
	//allow a cube to lerp from start to end position by using the result and cost from the dijkstra******************************************************************
	IEnumerator LerpAlongPath(Stack<Vector3> path)
	{
		traversing = true;
		traversalBegin ();
		float lerpTime = 1.0f;

		if(cube!=null)
		{
			DestroyObject(cube);
		}

		cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		//pop first waypoint off as the starting point
		Vector3 current = path.Pop ();
		cube.transform.position = current;

		while (path.Count>0) 
		{
		
			Vector3 target=path.Pop ();
			if(voxelChunk.isStone(target-offset+new Vector3(0,-1,0)))
			{
				lerpTime=1.0f;
			}
			if(voxelChunk.isDirt(target-offset+new Vector3(0,-1,0)))
			{
				lerpTime=3.0f;
			}

			float currentTime=0.0f;

			while(currentTime<lerpTime)
			{
				currentTime+=Time.deltaTime;
				cube.transform.position=Vector3.Lerp(current,target,currentTime/lerpTime);
				yield return 0;
			}
			//set to exact position when lerp time is over
			cube.transform.position=target;
			current=target;
		}
		traversing = false;
		traversalEnd ();
	}
}
