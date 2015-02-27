using UnityEngine;
using System.Collections;

public class PlayersData : MonoBehaviour
{
		public int i;
		public Hashtable data;
		public Item CurentWeapon;
		
		public PlayersData (string id, string name, string cls, Item weapon)
		{
				data = new Hashtable();	
				Debug.Log(id);
				data.Add ("id",id);
				data.Add ("name", name);
				data.Add ("class", cls);
				data.Add ("weapon", weapon.serialize());
				CurentWeapon = weapon;
				Debug.Log(CurentWeapon.itName);
		}

		public PlayersData (object[] recievedData)
		{
				data = new Hashtable ();
				data.Add ("id", recievedData [0].ToString ());
				data.Add ("name", recievedData [1].ToString ());
				data.Add ("class", recievedData [2].ToString ());
				data.Add ("weapon", recievedData [3]);
				CurentWeapon = recievedData[3] as Item;
		}

		public object[] GetArray ()
		{
				object[] arr = new object[data.Count];
				arr [0] = data ["id"];
				arr [1] = data ["name"];
				arr [2] = data ["class"];
				arr [3] = data ["weapon"];
				return arr;
		}
}
