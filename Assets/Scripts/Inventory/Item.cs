using UnityEngine;
using System.Collections;

public class Item {
	public  string name;
	public  string type;
	public  Texture2D image;


	public Item (string name,string type, Texture2D image){
		this.name = name;
		this.type = type;
		this.image = image;
	}
}
