using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer),typeof(MeshCollider))]


public class VoxelGenerator : MonoBehaviour {

	Mesh mesh;
	MeshCollider meshCollider;
	List<Vector3> vertexList;
	List<int> triIndexList;
	List<Vector2> UVList;


	public List<string> texNames;
	public List<Vector2> texCoords;
	public float texSize;
	Dictionary<string, Vector2> texNameCoordDictionary;


	GameObject cube;
	bool lerping=false;

	List<Vector3> wayPointList;

	int numQuads = 0;

	public void Initialise()
	{
		CreateTextureNameCoordDictionary ();

		mesh = GetComponent<MeshFilter> ().mesh;
		meshCollider = GetComponent<MeshCollider> ();
		vertexList=new List<Vector3>();
		triIndexList=new List<int>();
		UVList = new List<Vector2> ();
	}

	public void UpdateMesh()
	{
		mesh.Clear ();

		mesh.vertices = vertexList.ToArray ();
		mesh.triangles = triIndexList.ToArray ();
		mesh.uv = UVList.ToArray ();
		mesh.RecalculateNormals ();
		
		meshCollider.sharedMesh = null;
		meshCollider.sharedMesh = mesh;

		ClearPreviousData ();
	}

	// Use this for initialization
	void Start () {

		//Initialise ();



//		for (int x=0; x<10; x++) 
//		{
//			for (int z=0; z<10; z++)
//			{
//			CreateVoxel (x, 0, z, "Sand");
//			}
//		}

//		CreateVoxel (0, 0, 0, "Dirt");
//		CreateVoxel (1, 0, 0, "Grass");
//		CreateVoxel (2, 0, 0, "Grass");
//		CreateVoxel (3, 0, 0, "Grass");
//		CreateVoxel (3, 0, 1, "Grass");
//		CreateVoxel (3, 0, 2, "Grass");
//		CreateVoxel (3, 0, 2, "Grass");
//		CreateVoxel (4, 0, 2, "Grass");
//		CreateVoxel (4, 0, 3, "Dirt");


		//UpdateMesh ();


//		wayPointList = new List<Vector3> ();
//		wayPointList.Add(new Vector3(0.5f, 1.5f, 0.5f));
//		wayPointList.Add(new Vector3(1.5f, 1.5f, 0.5f));
//		wayPointList.Add(new Vector3(2.5f, 1.5f, 0.5f));
//		wayPointList.Add(new Vector3(3.5f, 1.5f, 0.5f));
//		wayPointList.Add(new Vector3(3.5f, 1.5f, 1.5f));
//		wayPointList.Add(new Vector3(3.5f, 1.5f, 2.5f));
//		wayPointList.Add(new Vector3(4.5f, 1.5f, 2.5f));
//		wayPointList.Add(new Vector3(4.5f, 1.5f, 3.5f));



	}


	
	// Update is called once per frame
	void Update () 
	{
//	if (Input.GetKeyDown ("space")) 
//		{
//			StartCoroutine (Lerp());
//		}
	}

	public void CreateVoxel(int x, int y, int z, string texture)
	{
		//Vector2 uvCoords = texNameCoordDictionary [texture];

		CreateNegtiveXFace (x, y, z, texture);
		CreatePositiveXFace (x, y, z, texture);

		CreateNegtiveYFace (x, y, z, texture);
		CreatePositiveYFace (x, y, z, texture);

		CreateNegtiveZFace (x, y, z, texture);
		CreatePositiveZFace (x, y, z, texture);
	}

	public void CreateNegtiveZFace(int x, int y, int z, string texture)
	{
		Vector2 uvCoords = texNameCoordDictionary [texture];
		vertexList.Add (new Vector3 (x, y, z));
		vertexList.Add (new Vector3 (x, y + 1, z));
		vertexList.Add (new Vector3 (x + 1, y+1, z));
		vertexList.Add (new Vector3 (x+1, y, z));
		AddTriangleIndices ();
		AddUVCoords (uvCoords);
	}

	public void CreatePositiveZFace(int x, int y, int z, string texture)
	{
		Vector2 uvCoords = texNameCoordDictionary [texture];
		vertexList.Add (new Vector3 (x+1, y, z+1));
		vertexList.Add (new Vector3 (x + 1, y + 1, z+1));
		vertexList.Add (new Vector3 (x , y+1, z+1));
		vertexList.Add (new Vector3 (x, y, z+1));
		AddTriangleIndices ();
		AddUVCoords (uvCoords);
	}

	public void CreateNegtiveXFace(int x, int y, int z, string texture)
	{
		Vector2 uvCoords = texNameCoordDictionary [texture];
		vertexList.Add (new Vector3 (x, y, z+1));
		vertexList.Add (new Vector3 (x, y + 1, z+1));
		vertexList.Add (new Vector3 (x , y+1, z));
		vertexList.Add (new Vector3 (x, y, z));
		AddTriangleIndices ();
		AddUVCoords (uvCoords);
	}

	public void CreatePositiveXFace(int x, int y, int z, string texture)
	{
		Vector2 uvCoords = texNameCoordDictionary [texture];
		vertexList.Add (new Vector3 (x+1, y, z));
		vertexList.Add (new Vector3 (x+1, y + 1, z));
		vertexList.Add (new Vector3 (x+1 , y+1, z+1));
		vertexList.Add (new Vector3 (x+1, y, z+1));
		AddTriangleIndices ();
		AddUVCoords (uvCoords);
	}

	public void CreateNegtiveYFace(int x, int y, int z, string texture)
	{
		Vector2 uvCoords = texNameCoordDictionary [texture];
		vertexList.Add (new Vector3 (x, y, z+1));
		vertexList.Add (new Vector3 (x, y , z));
		vertexList.Add (new Vector3 (x+1 , y, z));
		vertexList.Add (new Vector3 (x+1, y, z+1));
		AddTriangleIndices ();
		AddUVCoords (uvCoords);
	}

	public void CreatePositiveYFace(int x, int y, int z, string texture)
	{
		Vector2 uvCoords = texNameCoordDictionary [texture];
		vertexList.Add (new Vector3 (x, y+1, z));
		vertexList.Add (new Vector3 (x, y + 1, z+1));
		vertexList.Add (new Vector3 (x+1 , y+1, z+1));
		vertexList.Add (new Vector3 (x+1, y+1, z));
		AddTriangleIndices ();
		AddUVCoords (uvCoords);
	}

	void AddTriangleIndices()
	{
		triIndexList.Add (numQuads * 4);
		triIndexList.Add ((numQuads * 4)+1);
		triIndexList.Add ((numQuads * 4)+3);
		triIndexList.Add ((numQuads * 4)+1);
		triIndexList.Add ((numQuads * 4)+2);
		triIndexList.Add ((numQuads * 4)+3);
		numQuads++;
	}

	void AddUVCoords(Vector2 uvCoords)
	{
		UVList.Add(new Vector2(uvCoords.x, uvCoords.y+0.5f));
		UVList.Add(new Vector2(uvCoords.x+0.5f, uvCoords.y+0.5f));
		UVList.Add(new Vector2(uvCoords.x+0.5f, uvCoords.y));
		UVList.Add(new Vector2(uvCoords.x, uvCoords.y));
	}

	void CreateTextureNameCoordDictionary()
	{
		texNameCoordDictionary = new Dictionary<string,Vector2> ();

		if (texNames.Count == texCoords.Count) 
		{
			for (int i=0; i<texNames.Count; i++) 
			{
				texNameCoordDictionary.Add (texNames [i], texCoords [i]);
			}
		} 
		else 
		{
			Debug.Log ("texNames and texCoords count mismatch");
		}
	}

	void ClearPreviousData()
	{
		vertexList.Clear ();
		triIndexList.Clear ();
		UVList.Clear ();
		numQuads = 0;
	}



//	void StartLerping()
//	{
//
//
//		//StartCoroutine (Lerp(new Vector3(0.5f,1.5f,0.5f),new Vector3(3.5f,1.5f,0.5f)));
//	}

//	IEnumerator Lerp()
//	{
//		if (cube) 
//		{
//			Destroy(cube);
//		}
//		cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
//		for (int i =1; i<wayPointList.Count; i++) 
//		{
//			float ct = 0;
//			float tt = 1f;
//			while (ct<tt) 
//			{
//				ct += Time.deltaTime;
//				cube.transform.position = Vector3.Lerp (wayPointList[i-1], wayPointList[i], ct / tt);
//				yield return 0;
//			}
//		}
//	}
}
