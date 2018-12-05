using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public AudioClip destroyBlockSound;
	public AudioClip grass;
	public AudioClip dirt;

	public AudioClip stone;

	public AudioClip sand;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//play destroy sound if value is 0*******************************************************************************
	void PlayDestroyBlockSound(int i)
	{
		if (i == 0) 
		{
			audio.PlayOneShot (destroyBlockSound);
		}
	}


	//play defferent sound for different type of block place on the map**********************************************
	void PlayPlaceBlockSound(int i)
	{
		if (i ==1) 
		{
			audio.PlayOneShot (grass);
		}
		if (i ==2) 
		{
			audio.PlayOneShot (dirt);
		}
		if (i ==3) 
		{
			audio.PlayOneShot (stone);
		}
		if (i ==4) 
		{
			audio.PlayOneShot (sand);
		}
	}
	
	//delegate that allow blocks to play different sound depend on the value*****************************************
	void OnEnable()
	{
		VoxelChunk.OnEventBlockChanged +=PlayDestroyBlockSound;
		VoxelChunk.OnEventBlockChanged +=PlayPlaceBlockSound;
	
	}
	
	void OnDisable()
	{
		VoxelChunk.OnEventBlockChanged-=PlayDestroyBlockSound;
		VoxelChunk.OnEventBlockChanged-=PlayPlaceBlockSound;

	}
}
