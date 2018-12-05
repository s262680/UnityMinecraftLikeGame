using UnityEngine;
using System.Collections;

public class NetworkFPCScript : MonoBehaviour {
	float lastSyncTime=0f;
	float syncDelay=0f;
	float syncTime=0f;
	Vector3 startPosition=Vector3.zero;
	Vector3 endPosition=Vector3.zero;
	// Use this for initialization
	void Start () {
	if (networkView.isMine) 
		{
			MonoBehaviour[] components = GetComponents<MonoBehaviour> ();
			foreach (MonoBehaviour m in components)
			{
				m.enabled = true;
			}
			foreach (Transform t in transform) 
			{
				t.gameObject.SetActive (true);
			}
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
	if (!networkView.isMine) 
		{
			syncTime+=Time.deltaTime;
			if(syncTime<syncDelay)
			{
				transform.position=Vector3.Lerp(startPosition,endPosition,syncTime/syncDelay);
			}
		}
		else 
		{
			if(Input.GetButtonDown("Fire1"))
			{

				Vector3 v;
				VoxelChunk vcs;
				if(PickThisBlock(out v, out vcs, 4))
				{
					//Debug.Log ("abc");
					NetworkView nv=vcs.GetComponent<NetworkView>();
					if(nv!=null)
					{
						nv.RPC ("SetBlock", RPCMode.All,v,0);
						
					}
				}
			}
		}
	}


	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting) 
		{
			syncPosition = transform.position;
			stream.Serialize (ref syncPosition);
		} 
		else 
		{
			stream.Serialize(ref syncPosition);
			syncTime =0f;
			syncDelay=Time.time-lastSyncTime;
			lastSyncTime=Time.time;
			startPosition=transform.position;
			endPosition=syncPosition;

		}
	}

	bool PickThisBlock(out Vector3 v, out VoxelChunk voxelChunkScript, float dist)
	{
		v = new Vector3 ();
		voxelChunkScript = null;
		Ray ray=Camera.main.ScreenPointToRay(new Vector3(Screen.width/2,Screen.height/2,0));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, dist)) 
		{
			voxelChunkScript = hit.collider.gameObject.GetComponent<VoxelChunk> ();
			if (voxelChunkScript != null) 
			{
				v = hit.point - hit.normal / 2;

				v.x = Mathf.Floor (v.x);
				v.y = Mathf.Floor (v.y);
				v.z = Mathf.Floor (v.z);
				return true;
			}
		}
		return false;
	}


}
