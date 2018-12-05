using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {

	//parent object inventory item
	public Transform parentPanel;

	//item info to build inventory items
	public List<Sprite> itemSprites;
	public List<string> itemNames;
	public List<int> itemAmounts;

	//Starting template item
	public GameObject startItem;

	List<InventoryItemScript> inventoryList;


	// Use this for initialization
	void Start () {
	
		inventoryList=new List<InventoryItemScript>();
		for (int i =0; i< itemNames.Count; i++) 
		{
			//create a duplicate of the starter item
			GameObject inventoryItem=(GameObject)Instantiate (startItem);
			//UI items need to parented by the canvas or an object within the canvas
			inventoryItem.transform.SetParent (parentPanel);
			//Original start item is disabled - so the duplicate must be enabled
			inventoryItem.SetActive(true);
			//get inventoryItemScript component so we can set the data
			InventoryItemScript iis= inventoryItem.GetComponent<InventoryItemScript>();
			iis.itemSprite.sprite=itemSprites[i];
			iis.itemNameText.text=itemNames[i];
			iis.itemName=itemNames[i];
			iis.itemAmountText.text=itemAmounts[i].ToString();
			iis.itemAmount=itemAmounts[i];
			//keep a list of the inventory items
			inventoryList.Add(iis);
		}
		DisplayListInOrder ();
	}


	//delegate that pass the type of block that player collided with to AmountAdd method***************************************************************
	void OnEnable()
	{
		PlayerScript.OnCollect += AmountAdd;
	}
	
	void OnDisable()
	{

		PlayerScript.OnCollect -= AmountAdd;
	}




	//Increase the amount of item in inventory depending on the value************************************************************************************
	void AmountAdd(int i)
	{
//		Debug.Log (inventoryList [0].itemAmount);
//		Debug.Log (inventoryList [2].itemAmount);
		if (i == 0) 
		{
			inventoryList[i].itemAmount+=1;
			inventoryList[i].itemAmountText.text=inventoryList[i].itemAmount.ToString();
		}
		else if (i == 1) 
		{
			inventoryList[i].itemAmount+=1;
			inventoryList[i].itemAmountText.text=inventoryList[i].itemAmount.ToString();
		}
		else if (i == 2) 
		{
			inventoryList[i].itemAmount+=1;
			inventoryList[i].itemAmountText.text=inventoryList[i].itemAmount.ToString();
		}
		else if (i == 3) 
		{
			inventoryList[i].itemAmount+=1;
			inventoryList[i].itemAmountText.text=inventoryList[i].itemAmount.ToString();
		}
		DisplayListInOrder ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DisplayListInOrder()
	{
		//Height of item plus space between each
		float yOffset = 100f;
		//use the start position for the first item
		Vector3 startPosition = startItem.transform.position;
		foreach (InventoryItemScript iis in inventoryList) 
		{
			iis.transform.position=startPosition;
			//set position of next item using offset
			startPosition.y-=yOffset;
		}
	}

	public void SelectionSortInventory()
	{
		//iterate through every item in the list except last
		for (int i =0; i< inventoryList.Count-1;i++)
		{
			int minIndex=i;

			//iterate through unsorted portion of the list
			for (int j=i; j<inventoryList.Count;j++)
			{
				if(inventoryList[j].itemAmount<inventoryList[minIndex].itemAmount)
				{
					minIndex=j;
				}
			}
			//swap the minimum item into position
			if (minIndex!=i)
			{
				InventoryItemScript iis=inventoryList[i];
				inventoryList[i]=inventoryList[minIndex];
				inventoryList[minIndex]=iis;
			}
      	 }
		//display the list in the new correct order
		DisplayListInOrder();
	}

	List<InventoryItemScript> QuickSort(List<InventoryItemScript>listIn)
	{
		if (listIn.Count <= 1) 
		{
			return listIn;
		}

		//naive pivot selection
		int pivotIndex = 0;

		//left and right lists
		List<InventoryItemScript> leftList = new List<InventoryItemScript> ();
		List<InventoryItemScript> rightList = new List<InventoryItemScript> ();

		for (int i=1; i <listIn.Count; i++) 
		{
			//compare amounts to determine list to add to
			if (listIn [i].itemAmount > listIn [pivotIndex].itemAmount) 
			{
				//greater than piovt to left list
				leftList.Add (listIn [i]);
			} 
			else 
			{
				//smaller than pivot to right list
				rightList.Add (listIn [i]);
			}
		}

		//recurese left list
		leftList = QuickSort (leftList);
		//recurese right list
		rightList = QuickSort (rightList);

		//concatenate lists: left+pivot+right
		leftList.Add (listIn [pivotIndex]);
		leftList.AddRange (rightList);
				              
		return leftList;
	}

	public void StartQuickSort()
	{
		inventoryList = QuickSort (inventoryList);
		DisplayListInOrder ();
	}

	public void BubbleSort()
	{
		//List<InventoryItemScript> tempHolder;
		bool keepGoing=true;
		while (keepGoing == true)
		{
			keepGoing=false;
			for (int i =0; i<inventoryList.Count-1; i++) 
			{
				if (inventoryList [i + 1].itemAmount < inventoryList [i].itemAmount) 
				{
					InventoryItemScript iis = inventoryList [i];
					inventoryList [i] = inventoryList [i + 1];
					inventoryList [i + 1] = iis;
					keepGoing=true;
				}
			}
		}
		DisplayListInOrder ();
	}
	
	
	
	
	
	
	
	//Merge Sort from low to high*******************************************************************************************************
	List<InventoryItemScript> MergeSortLowToHigh(List<InventoryItemScript>listIn)
	{
		if (listIn.Count <= 1) 
		{
			return listIn;
		}

		List<InventoryItemScript> leftList = new List<InventoryItemScript> ();
		List<InventoryItemScript> rightList = new List<InventoryItemScript> ();


		for (int i =0; i<(listIn.Count/2); i++) 
		{

			leftList.Add (listIn[i]);
			//Debug.Log(i);
		}

		for(int j =(listIn.Count/2); j<listIn.Count;j++)
		{
			rightList.Add(listIn[j]);
			//Debug.Log (j);
		}

		leftList = MergeSortLowToHigh (leftList);
		rightList = MergeSortLowToHigh (rightList);
		
		//leftList.AddRange(rightList);

		int a = 0;
		int b = 0;
		//int m=0;
		List<InventoryItemScript> holder = new List<InventoryItemScript> ();
		
		while (a<leftList.Count && b<rightList.Count) 
		{
			if(leftList[a].itemAmount<=rightList[b].itemAmount)
			{
				//m.Add(leftList[a]);
				holder.Add(leftList[a]);
				a++;
			}
			else
			{
				//m.Add (rightList[b]);
				holder.Add(rightList[b]);
				b++;
			}
		}

		while (a < leftList.Count) 
		{
		
			//m.AddRange (leftList);
			holder.Add(leftList[a]);
			a++;
			//m++;
		} 
		while(b<rightList.Count) 
		{
			//m.AddRange(rightList);
			holder.Add(rightList[b]);
			b++;
			//m++;
		}

		return holder;
//
//		leftList.AddRange (rightList);
//
//return leftList;
	}


	public void StartMergeSortLowToHigh()
	{
	//Debug.Log ("abc");
		inventoryList = MergeSortLowToHigh (inventoryList);
		DisplayListInOrder();
	}






	//merge sort from high to low****************************************************************************************************************
	List<InventoryItemScript> MergeSortHighToLow(List<InventoryItemScript>listIn)
	{
		if (listIn.Count <= 1) {
			return listIn;
		}
		
		List<InventoryItemScript> leftList = new List<InventoryItemScript> ();
		List<InventoryItemScript> rightList = new List<InventoryItemScript> ();
		
		
		for (int i =0; i<(listIn.Count/2); i++) {
			
			leftList.Add (listIn [i]);
			//Debug.Log(i);
		}
		
		for (int j =(listIn.Count/2); j<listIn.Count; j++) {
			rightList.Add (listIn [j]);
			//Debug.Log (j);
		}
		
		leftList = MergeSortHighToLow (leftList);
		rightList = MergeSortHighToLow (rightList);
		
		//leftList.AddRange(rightList);
		
		int a = 0;
		int b = 0;
		//int m=0;
		List<InventoryItemScript> holder = new List<InventoryItemScript> ();
		
		while (a<leftList.Count && b<rightList.Count) {
			if (leftList [a].itemAmount >= rightList [b].itemAmount) {
				//m.Add(leftList[a]);
				holder.Add (leftList [a]);
				a++;
			} else {
				//m.Add (rightList[b]);
				holder.Add (rightList [b]);
				b++;
			}
		}
		
		while (a < leftList.Count) {
			
			//m.AddRange (leftList);
			holder.Add (leftList [a]);
			a++;
			//m++;
		} 
		while (b<rightList.Count) {
			//m.AddRange(rightList);
			holder.Add (rightList [b]);
			b++;
			//m++;
		}
		
		return holder;
	}

	public void StartMergeSortHighToLow()
	{
		//Debug.Log ("abc");
		inventoryList = MergeSortHighToLow (inventoryList);
		DisplayListInOrder();
	}

}



