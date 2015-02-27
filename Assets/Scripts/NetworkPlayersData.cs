﻿using UnityEngine;
using System.Collections;

public class NetworkPlayersData : MonoBehaviour
{
		public Hashtable npd;
		public string[] debug;
		void Awake ()
		{
				npd = new Hashtable (4);
		}

		public void Add (PlayersData pd)
		{
				npd.Add (pd.data["id"],pd);
		}
		
		public PlayersData Get (string name)
		{
				return npd [name] as PlayersData;
		}
		
		public void delete (string name){
			npd.Remove (name);
		}
		public Object[] GetArray(){
			ArrayList arr = new ArrayList ();
			foreach (PlayersData pd in npd)
						arr.Add (pd.data);
			return (arr.ToArray () as Object[]);
		}
}
