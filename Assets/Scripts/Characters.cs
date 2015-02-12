﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;

public class Characters : MonoBehaviour
{
		public Button[] CharButtons;
		XmlNodeList characters;
		XmlDocument charsDoc;
		public GameObject createCharacter;
		public GameObject characterInfo;
		PlayersData pd;
		Inventory inventory;

		// Use this for initialization
		public void Start ()
		{
				pd = GameObject.Find ("PlayersData").GetComponent<PlayersData> ();
				inventory = pd.gameObject.GetComponent<Inventory> ();
				charsDoc = new XmlDocument ();
				try {
						charsDoc.Load ("Data/characters.xml");
				} catch (FileNotFoundException e) {
						if (e != null) {
								XmlNode rootNode = charsDoc.CreateElement ("characters");
								charsDoc.AppendChild (rootNode);
								XmlNode charNode;
								for (int i=0; i!=4; i++) {
										charNode = charsDoc.CreateElement ("char");
										XmlAttribute name = charsDoc.CreateAttribute ("name");
										XmlAttribute cls = charsDoc.CreateAttribute ("class");
										XmlAttribute weapon = charsDoc.CreateAttribute ("weapon");	
										XmlAttribute id = charsDoc.CreateAttribute ("id");		
										charNode.Attributes.Append (id);
										charNode.Attributes.Append (name);
										charNode.Attributes.Append (cls);
										charNode.Attributes.Append (weapon);	
										rootNode.AppendChild (charNode);
								}
								save ();
						}
				}
				characters = charsDoc.GetElementsByTagName ("char");
				for (int i=0; i!=CharButtons.Length; i++) {
						string name = characters [i].Attributes ["name"].Value;
						if (name != "") {
								CharButtons [i].transform.GetChild (0).GetComponent<Text> ().text = name;
						} else {
								CharButtons [i].transform.GetChild (0).GetComponent<Text> ().text = "New Character";
						}
				
						int n = i;
						CharButtons [i].onClick.AddListener (delegate {
								SelectChar (n);
						});
				}
		}

		void SelectChar (int i)
		{
				bool empty = characters [i].Attributes ["name"].Value == "" ? true : false;
				if (!empty) {
						pd.data ["id"] = characters [i].Attributes ["id"].Value;
						pd.data ["name"] = characters [i].Attributes ["name"].Value;
						pd.data ["class"] = characters [i].Attributes ["class"].Value;
						pd.data ["weapon"] = characters [i].Attributes ["weapon"].Value;
						characterInfo.GetComponent<CharacterInfo> ().i = i;
						characterInfo.GetComponent<CharacterInfo> ().init (pd.data ["name"].ToString (), pd.data ["class"].ToString ());
						characterInfo.SetActive (true);
						gameObject.SetActive (false);
				} else {
						createCharacter.SetActive (true);
						createCharacter.GetComponent<CreateCharacter> ().i = i;
						gameObject.SetActive (false);
				}
						
		}

		public void submitCharacter (string id, string name, string cls, int i)
		{
				characters [i].Attributes ["id"].Value = id;
				characters [i].Attributes ["name"].Value = name;
				characters [i].Attributes ["class"].Value = cls;
				switch (cls) {
				case "gunslinger":
						characters [i].Attributes ["weapon"].Value = "mgun";
						inventory.Add(new Item("mgun","mgun","mgun"));
						break;
				case "warrior":
						characters [i].Attributes ["weapon"].Value = "sword";
						inventory.Add(new Item("sword","sword","sword"));
						break;
				default:
						characters [i].Attributes ["weapon"].Value = "sword";
						break;
				}
				save ();
		}

		public void delete (int i)
		{
				foreach (XmlAttribute atr in characters[i].Attributes)
						atr.Value = null;
				save ();
		}

		void save ()
		{
				charsDoc.Save ("Data/characters.xml");
		}
}
