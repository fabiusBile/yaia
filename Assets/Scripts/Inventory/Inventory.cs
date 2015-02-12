using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class Inventory : ScriptableObject {
	List<Item> inventory;
	XmlDocument invDoc;
	void Awake(){
		inventory = new List<Item>();
	}
	void Add (Item item){

	}
	List<Item> Load(){
		invDoc = new XmlDocument();
		try {
			invDoc.Load ("Data/inventory.xml");
		} catch (FileNotFoundException e) {
			if (e!=null){
				XmlNode root = invDoc.CreateElement("Inventory"); 
			}
		}
		foreach (Item item in inventory){
			XmlAttribute name = item.name;
			XmlAttribute type = item.type;
			XmlAttribute image = item.image;


		}
	}
	void Save(){
		invDoc.Save("Data/inventory.xml");
	}
}
