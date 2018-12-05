using UnityEngine;
using System.Collections;
using System.Xml;


public class XMLVoxelFileWriter {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	//save chuck data, player location and rotation into a xml file********************************************************************************************
	public static void SaveChunkToXMLFile(int[ , , ] voxelArray,float PosX, float PosY, float PosZ, 
	                                      float RotX,float RotY,float RotZ, string fileName)
	{
		XmlWriterSettings writerSettings = new XmlWriterSettings ();
		writerSettings.Indent = true;
		
		//create a write instance
		XmlWriter xmlWriter = XmlWriter.Create (fileName + ".xml", writerSettings);
		//write the beginning of the document
		xmlWriter.WriteStartDocument ();
		
		//create the root element
		xmlWriter.WriteStartElement ("VoxelChunk");
		
		//iterate through all array elements
		for (int x=0; x<voxelArray.GetLength(0); x++) 
		{
			for (int y=0; y< voxelArray.GetLength(1); y++) 
			{
				for (int z=0; z< voxelArray.GetLength(2); z++) 
				{
					if (voxelArray [x, y, z] != 0) 
					{
						//create a single voxel element
						xmlWriter.WriteStartElement ("Voxel");
						//write an attribute to store the x y z index
						xmlWriter.WriteAttributeString("x", x.ToString());
						xmlWriter.WriteAttributeString("y", y.ToString());
						xmlWriter.WriteAttributeString("z", z.ToString());
						//store the voxel type
						xmlWriter.WriteString (voxelArray[x,y,z].ToString());
						//end the voxel element
						xmlWriter.WriteEndElement();
					}
				}
			}
		}
		
		xmlWriter.WriteStartElement ("CharPos");
		
		//		int x = PosX;
		//		int y = PosY;
		//		int z = PosZ;
		xmlWriter.WriteAttributeString("x", PosX.ToString());
		xmlWriter.WriteAttributeString("y", PosY.ToString());
		xmlWriter.WriteAttributeString("z", PosZ.ToString());
		xmlWriter.WriteEndElement ();
		
		xmlWriter.WriteStartElement ("CharRot");
		
		//		int a = RotX;
		//		int b = RotY;
		//		int c = RotZ;
		xmlWriter.WriteAttributeString("a", RotX.ToString());
		xmlWriter.WriteAttributeString("b", RotY.ToString());
		xmlWriter.WriteAttributeString("c", RotZ.ToString());
		xmlWriter.WriteEndElement ();
	
		
		xmlWriter.WriteEndElement ();
		xmlWriter.WriteEndDocument ();
		xmlWriter.Close ();
	}
	
	
//	public static void SaveChunkToXMLFile(int[ , , ] voxelArray, string fileName)
//	{
//		XmlWriterSettings writerSettings = new XmlWriterSettings ();
//		writerSettings.Indent = true;
//
//		//create a write instance
//		XmlWriter xmlWriter = XmlWriter.Create (fileName + ".xml", writerSettings);
//		//write the beginning of the document
//		xmlWriter.WriteStartDocument ();
//
//		//create the root element
//		xmlWriter.WriteStartElement ("VoxelChunk");
//
//		//iterate through all array elements
//		for (int x=0; x<voxelArray.GetLength(0); x++) 
//		{
//			for (int y=0; y< voxelArray.GetLength(1); y++) 
//			{
//				for (int z=0; z< voxelArray.GetLength(2); z++) 
//				{
//					if (voxelArray [x, y, z] != 0) 
//					{
//						//create a single voxel element
//						xmlWriter.WriteStartElement ("Voxel");
//						//write an attribute to store the x y z index
//						xmlWriter.WriteAttributeString("x", x.ToString());
//						xmlWriter.WriteAttributeString("y", y.ToString());
//						xmlWriter.WriteAttributeString("z", z.ToString());
//						//store the voxel type
//						xmlWriter.WriteString (voxelArray[x,y,z].ToString());
//						//end the voxel element
//						xmlWriter.WriteEndElement();
//					}
//				}
//			}
//		}
//
//		xmlWriter.WriteEndElement ();
//		xmlWriter.WriteEndDocument ();
//		xmlWriter.Close ();
//	}



	//load chuck data from xml file**********************************************************************************************************
	public static int[ , , ] LoadChunkFromXMLFile(int size, string fileName)
	{
	
		
		int[ , , ] voxelArray = new int[size, size, size];
		//create an xml reader with the file supplied
		XmlReader xmlReader = XmlReader.Create (fileName+".xml");
		//iterate through and read every line in the xml file
		while (xmlReader.Read ()) 
		{
			//check if this node is a voxel element
			if (xmlReader.IsStartElement("Voxel"))
			{
				//retrieve x y z attribute and store as int
				int x = int.Parse (xmlReader["x"]);
				int y = int.Parse (xmlReader["y"]);
				int z = int.Parse (xmlReader["z"]);
				//read the next value which will be the block type
				xmlReader.Read();
				//store the current value
				int value=int.Parse(xmlReader.Value);

				voxelArray[x, y ,z]=value;
			}
		}

		return voxelArray;
		
	}
	
	
	


//	public static void SavePosToXMLFile(float PosX, float PosY, float PosZ, 
//	                                    float RotX,float RotY,float RotZ, string fileName)
//	{
//		XmlWriterSettings writerSettings = new XmlWriterSettings ();
//		writerSettings.Indent = true;
//		
//		//create a write instance
//		XmlWriter xmlWriter = XmlWriter.Create (fileName + ".xml", writerSettings);
//		//write the beginning of the document
//		xmlWriter.WriteStartDocument ();
//		
//		//create the root element
//		xmlWriter.WriteStartElement ("CharLocation");
//
//
//		xmlWriter.WriteStartElement ("CharPos");
//
////		int x = PosX;
////		int y = PosY;
////		int z = PosZ;
//		xmlWriter.WriteAttributeString("x", PosX.ToString());
//		xmlWriter.WriteAttributeString("y", PosY.ToString());
//		xmlWriter.WriteAttributeString("z", PosZ.ToString());
//		xmlWriter.WriteEndElement ();
//
//		xmlWriter.WriteStartElement ("CharRot");
//		
////		int a = RotX;
////		int b = RotY;
////		int c = RotZ;
//		xmlWriter.WriteAttributeString("a", RotX.ToString());
//		xmlWriter.WriteAttributeString("b", RotY.ToString());
//		xmlWriter.WriteAttributeString("c", RotZ.ToString());
//		xmlWriter.WriteEndElement ();
//
//
//		xmlWriter.WriteEndElement ();
//		xmlWriter.WriteEndDocument ();
//		xmlWriter.Close ();
//	}


	//load player position from xml file********************************************************************************************************
	public static Vector3 LoadPosFromXMLFile( string fileName)
	{
		
		Vector3 playerPos=(new Vector3(1,1,1));


		//create an xml reader with the file supplied
		XmlReader xmlReader = XmlReader.Create (fileName+".xml");
		//iterate through and read every line in the xml file
		while (xmlReader.Read ())
		{
			//check if this node is a voxel element
			if (xmlReader.IsStartElement ("CharPos")) 
			{
				//retrieve x y z attribute and store as int
				float PosX = float.Parse (xmlReader ["x"]);
				float PosY = float.Parse (xmlReader ["y"]);
				float PosZ = float.Parse (xmlReader ["z"]);

				playerPos = new Vector3 (PosX, PosY, PosZ);
			}

		}		 
		xmlReader.Close ();
		return playerPos;
		
	}

	//load player rotation from xml file************************************************************************************************************
	public static Quaternion LoadRotFromXMLFile( string fileName)
	{
		
		
		Quaternion playerRot=Quaternion.Euler(new Vector3(1,1,1));
		
		//create an xml reader with the file supplied
		XmlReader xmlReader = XmlReader.Create (fileName+".xml");
		//iterate through and read every line in the xml file
		while (xmlReader.Read ()) 
		{
			
			if (xmlReader.IsStartElement("CharRot"))
			{
				//retrieve a b c attribute and store as int
				float RotX = float.Parse (xmlReader["a"]);
				float RotY = float.Parse (xmlReader["b"]);
				float RotZ = float.Parse (xmlReader["c"]);
				
				playerRot=Quaternion.Euler(new Vector3(RotX,RotY,RotZ));
			}
		}
		xmlReader.Close ();
		return playerRot;
		
	}


	//load start and end position from xml file************************************************************************************************************
	public static bool ReadStartAndEndPosition(out Vector3 start, out Vector3 end, string fileName)
	{
		bool foundStart = false;
		bool foundEnd = false;

		start = new Vector3 (-1, -1, -1);
		end = new Vector3 (-1, -1, -1);

		XmlReader xmlReader = XmlReader.Create (fileName + ".xml");

		while (xmlReader.Read()) 
		{
			if(xmlReader.IsStartElement("start"))
			{
				int x=int.Parse(xmlReader["x"]);
				int y=int.Parse(xmlReader["y"]);
				int z=int.Parse(xmlReader["z"]);

				start=new Vector3(x,y,z);
				foundStart=true;
			}
			if(xmlReader.IsStartElement("end"))
			{
				int x=int.Parse(xmlReader["x"]);
				int y=int.Parse(xmlReader["y"]);
				int z=int.Parse(xmlReader["z"]);
				
				end=new Vector3(x,y,z);
				foundEnd =true;
			}
		}

		return foundStart && foundEnd;

	}

}
