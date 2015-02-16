using UnityEngine;
using System.Collections;

public class PlayersData : MonoBehaviour
{
		public int i;
		public Hashtable data;

		void Awake ()
		{
				data = new Hashtable ();
				data.Add ("id", "");
				data.Add ("name", "");
				data.Add ("class", "");
				data.Add ("weapon", "");
				data.Add ("weaponSprite", "");
				DontDestroyOnLoad (gameObject);
		}
		
		public PlayersData (string id, string name, string cls, string weapon, string weaponSprite)
		{
				data ["id"] = id;
				data ["name"] = name;
				data ["class"] = cls;
				data ["weapon"] = weapon;
				data ["weaponSprite"] = weaponSprite;
		}

		public PlayersData (object[] recievedData)
		{
				data = new Hashtable ();
				data.Add ("id", recievedData [0].ToString ());
				data.Add ("name", recievedData [1].ToString ());
				data.Add ("class", recievedData [2].ToString ());
				data.Add ("weapon", recievedData [3].ToString ());
				data.Add ("weaponSprite", recievedData [4].ToString ());
		}

		public object[] GetArray ()
		{
				object[] arr = new object[data.Count];
				arr [0] = data ["id"];
				arr [1] = data ["name"];
				arr [2] = data ["class"];
				arr [3] = data ["weapon"];
				arr [4] = data ["weaponSprite"];	
				return arr;
		}
}
