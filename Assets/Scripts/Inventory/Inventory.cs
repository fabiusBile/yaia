using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
	List<Item> inventory;
	XmlDocument invDoc;
	public RectTransform grid;
	public RectTransform lobby;
	public GameObject item;
	void OnGUI(){
		if (GUI.Button(new Rect(0,0,200,20),"add")){
			Add(new Item("Mgun","mgun","Weapons/mgun"));
			Save();
		}
	}
	public void ToggleLobby(){
		lobby.localScale = lobby.localScale == Vector3.one ? lobby.localScale = Vector3.zero : Vector3.one;
	}

	void Start(){
		invDoc = new XmlDocument();
		Load ();
		//GameObject weapon = Instantiate (Resources.Load ("Entity/Weapons/"+itType, typeof(GameObject))) as GameObject;
		//weapon.GetComponent<SpriteRenderer>().sprite=Resources.Load("Sprites/Weapons/"+itSprite,typeof(Sprite)) as Sprite;
	}
	public void Add (Item item){
		inventory.Add (item);
	}
	public void Remove(int i){
		inventory.RemoveAt (i);
	}
	public void showInventory(){
		foreach (Item i in inventory){
			GameObject itm = Instantiate(item) as GameObject;
			itm.transform.SetParent(grid);
		}
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
	public void Save(){
		foreach (Item item in inventory){
			XmlNode itemNode = invDoc.CreateElement("Item");
//			itemNode.Attributes.Append(invDoc.CreateAttribute("name"));
//			itemNode.Attributes.Append(invDoc.CreateAttribute("type"));
//			itemNode.Attributes.Append(invDoc.CreateAttribute("image"));
//			itemNode.Attributes["name"].Value=item.name;
//			itemNode.Attributes["type"].Value=item.type;
//			itemNode.Attributes["image"].Value=item.image;
			itemNode=item.GetXml(invDoc);
			invDoc.FirstChild.AppendChild(itemNode);
		}
		invDoc.Save("Data/inventory.xml");
	}
}
