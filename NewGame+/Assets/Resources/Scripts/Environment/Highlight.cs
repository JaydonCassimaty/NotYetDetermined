using UnityEngine;
using System.Collections;
using System.Linq;
using System;

public class Highlight : MonoBehaviour
{
	public int maxArraySize = 2;
	public int arraySize = 1;
	private Color startcolor;
	public Material someMaterial;
	public Material someOtherMaterial;

	Material[] mats;

	void Start(){
		GetComponent<MeshRenderer>().materials = new Material[maxArraySize];
		Material[] mats = GetComponent<MeshRenderer>().materials;
		mats[0] = someMaterial;
		GetComponent<MeshRenderer>().materials = mats;
	}

	void OnMouseEnter()
	{
		startcolor = GetComponent<MeshRenderer>().material.color;
		GetComponent<MeshRenderer>().material.color = Color.yellow;
	}

	void OnMouseExit()
	{
		GetComponent<MeshRenderer>().material.color = startcolor;
	}

	void OnMouseDown()
	{
		while (arraySize < maxArraySize){
			arraySize ++;
		}

		//mats[0] = someMaterial;
		//mats[1] = someOtherMaterial;
		//GetComponent<MeshRenderer>().materials = mats;
	}

	void OnMouseUp()
	{
		GetComponent<MeshRenderer>().material = someMaterial;
	}
}
