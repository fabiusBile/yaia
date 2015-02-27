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
	public Item curentItem;
	void OnGUI(){
		if (GUI.Button(new Rect(0,0,200,20),"add")){
			Add(new Item("Sword","sword","Weapons/sword"));
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
	public Item Take(int i){
		Item it = inventory[i];
		inventory.RemoveAt (i);
		return it;
	}
	public void showInventory(){
		foreach (Item i in inventory){
			GameObject itm = Instantiate(item) as GameObject;
			itm.transform.SetParent(grid);
			Sprite[] image = Resources.LoadAll<Sprite>("Sprites/Weapons/player");
			itm.transform.GetChild(0).GetComponent<Image>().sprite=image[8];
			itm.transform.GetChild(1).GetComponent<Text>().text=i.itName;
			Debug.Log(itm.transform.GetChild(0).GetComponent<Image>().sprite);
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
			inventory.Add(new Item(item.Attributes["name"].Value.ToString(),item.Attributes["type"].Value.ToString(),item.Attributes["image"].Value.ToString()));
		}
	}
	public void Save(){
		foreach (Item item in inventory){
			XmlNode itemNode = invDoc.CreateElement("Item");
			itemNode=item.GetXml(invDoc);
			invDoc.FirstChild.AppendChild(itemNode);
		}
		invDoc.Save("Data/inventory.xml");
	}
}
