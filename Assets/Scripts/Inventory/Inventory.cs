using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class Inventory : MonoBehaviour {
	List<Item> inventory;
	XmlDocument invDoc;
	void Start(){
		invDoc = new XmlDocument();
		Load ();
	}
	public void Add (Item item){
		inventory.Add (item);
		Save ();
	}
	void Load(){
		inventory = new List<Item> ();
		try {
			invDoc.Load ("Data/inventory.xml");
		} catch (FileNotFoundException e) {
			if (e!=null){
				XmlNode root = invDoc.CreateElement("Inventory"); 
				invDoc.AppendChild(root);
				Save();
			}
		}
		foreach (XmlNode item in invDoc.FirstChild.ChildNodes){
			inventory.Add(new Item(item.Attributes["name"].ToString(),item.Attributes["type"].ToString(),item.Attributes["image"].ToString()));
		}
	}
	void Save(){
		foreach (Item item in inventory){
			XmlNode itemNode = invDoc.CreateElement("Item");
			itemNode.Attributes.Append(invDoc.CreateAttribute("name"));
			itemNode.Attributes.Append(invDoc.CreateAttribute("type"));
			itemNode.Attributes.Append(invDoc.CreateAttribute("image"));
			itemNode.Attributes["name"].Value=item.name;
			itemNode.Attributes["type"].Value=item.type;
			itemNode.Attributes["image"].Value=item.image.name;
		}
		invDoc.Save("Data/inventory.xml");
	}
}
