using UnityEngine;
using System.Collections;

public class Item {
	public  string name;
	public  string type;
	public  Sprite image;


	public Item (string name,string type, string image){
		this.name = name;
		this.type = type;
		this.image = Resources.Load ("image") as Sprite;
	}
}
