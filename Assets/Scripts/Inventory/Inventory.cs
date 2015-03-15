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
	public NetworkPlayersData npd;
	public Transform curItemSlot;
	public Characters characters;
	public Transform infoPanel;

	void OnGUI(){
		if (GUI.Button(new Rect(0,0,200,20),"add")){
			Hashtable prefs = new Hashtable();
			prefs.Add ("swingDamage", 200);
			prefs.Add ("stubDamage", 100);
			prefs.Add ("minMultiplayer", 0.1);
			prefs.Add ("maxMultiplayer", 2);
			Add(new Item("Sword","sword","Weapons/sword",prefs));
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
		grid.GetComponent<InventoryCellsScaler> ().OnEnable ();
	}
	void ShowItem(Item i,int n){
		GameObject itm = Instantiate(item) as GameObject;
		itm.transform.SetParent(grid);
		Sprite[] image = Resources.LoadAll<Sprite>("Sprites/Weapons/player");
		itm.transform.GetChild(0).GetComponent<Image>().sprite=image[8];
		itm.transform.GetChild(1).GetComponent<Text>().text=i.itName;

		ItemCell itmcl = itm.GetComponent<ItemCell> ();
		itmcl.panel = infoPanel.gameObject;
		itmcl.panelText = infoPanel.GetChild (0).GetComponent<Text> ();
		foreach (object key in i.prefs.Keys) {
			itmcl.info+=key.ToString()+": "+i.prefs[key].ToString()+"\n";
		}

		itm.GetComponent<Button>().onClick.AddListener(delegate{
			inventory.Add(npd.localPd.CurentWeapon);
			setCurItem(Take(n));
			ShowItem(inventory[inventory.Count-1],inventory.Count-1);
			npd.localPd.CurentWeapon=i;
			itm.transform.localScale=Vector3.one;
			itm.GetComponent<ItemCell>().ShowInfo(false);
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
			Hashtable prefs = new Hashtable();
			foreach (XmlAttribute atr in item.FirstChild.Attributes){
				prefs.Add (atr.Name,atr.Value);
			}
			inventory.Add(new Item(item.Attributes["name"].Value.ToString(),item.Attributes["type"].Value.ToString(),item.Attributes["image"].Value.ToString(),prefs));
		}
	}
	public void Save(){
		invDoc = new XmlDocument ();
		XmlNode root = invDoc.CreateElement("Inventory"); 
		invDoc.AppendChild(root);
		Debug.Log (npd.localPd.CurentWeapon.itName);
		characters.ChangeCurWeapon (npd.localPd.i, npd.localPd.CurentWeapon);
		foreach (Item item in inventory){
			XmlNode itemNode = invDoc.CreateElement("Item");
			itemNode=item.GetXml(invDoc);
			invDoc.FirstChild.AppendChild(itemNode);
		}
		invDoc.Save("Data/inventory.xml");
		for (int i=0; i!=grid.childCount; i++) {
			GameObject.Destroy(grid.GetChild(i).gameObject);
		}
	}
}
