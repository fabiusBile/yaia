using UnityEngine;
using System.Collections;

public class NetworkPlayersData : MonoBehaviour
{
		public Hashtable npd;
		public string[] debug;
		void Awake ()
		{
				npd = new Hashtable (4);
				debug=new string[5];
		}

		public void Add (PlayersData pd)
		{
				npd.Add (pd.data["id"],pd);
				PlayersData debugData = pd;
				debug[0]=debugData.data["id"].ToString();
				debug[1]=debugData.data["name"].ToString();
				debug[2]=debugData.data["class"].ToString();
				debug[3]=debugData.data["weapon"].ToString();
				debug[4]=debugData.data["weaponSprite"].ToString();
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
