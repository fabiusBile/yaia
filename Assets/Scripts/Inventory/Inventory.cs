﻿using UnityEngine;
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
	public NetworkPlayersData npd;
	public Transform curItemSlot;

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
		inventory.TrimExcess ();
		return it;
	}
	void setCurItem (Item itm){
			Sprite[] image = Resources.LoadAll<Sprite>("Sprites/Weapons/player");
			curItemSlot.transform.GetChild(0).GetComponent<Image>().sprite=image[8];
			curItemSlot.transform.GetChild(1).GetComponent<Text>().text=itm.itName;
	}
	public void showInventory(){
		setCurItem (npd.localPd.CurentWeapon);
		for (int i=0;i!=inventory.Count;i++){
			ShowItem(inventory[i],i);
		}
	}
	void ShowItem(Item i,int n){
		GameObject itm = Instantiate(item) as GameObject;
		itm.transform.SetParent(grid);
		Sprite[] image = Resources.LoadAll<Sprite>("Sprites/Weapons/player");
		itm.transform.GetChild(0).GetComponent<Image>().sprite=image[8];
		itm.transform.GetChild(1).GetComponent<Text>().text=i.itName;
		itm.GetComponent<Button>().onClick.AddListener(delegate{
			inventory.Add(npd.localPd.CurentWeapon);
			setCurItem(Take(n));
			ShowItem(inventory[inventory.Count-1],inventory.Count-1);
			npd.localPd.CurentWeapon=i;
			GameObject.Destroy(itm);
		});
	}
	void Load(){
		inventory = new List<Item> ();
		try { 
			invDoc.Load ("Data/inventory.xml");
		} catch (FileNotFoundException e) {
			if (e!=null){

				Save();
			}
		}
		foreach (XmlNode item in invDoc.FirstChild.ChildNodes){
			inventory.Add(new Item(item.Attributes["name"].Value.ToString(),item.Attributes["type"].Value.ToString(),item.Attributes["image"].Value.ToString()));
		}
	}
	public void Save(){
		invDoc = new XmlDocument ();
		XmlNode root = invDoc.CreateElement("Inventory"); 
		invDoc.AppendChild(root);
		foreach (Item item in inventory){
			XmlNode itemNode = invDoc.CreateElement("Item");
			itemNode=item.GetXml(invDoc);
			invDoc.FirstChild.AppendChild(itemNode);
		}
		invDoc.Save("Data/inventory.xml");
	}
}
