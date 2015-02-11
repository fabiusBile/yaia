using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;
using System;

public class CreateCharacter : MonoBehaviour
{
		public ToggleGroup classSelector;
		public InputField name;
		public Characters chr;
		public int i = 0;

		public void submit ()
		{
				if (name != null) {
						string id = "";

						for (int k=0; k!=6; k++)
								id += RandomChar ();	
						id+=DateTime.Now.Day.ToString()+DateTime.Now.Hour.ToString()+DateTime.Now.Minute.ToString();
						chr.submitCharacter (id,name.text, classSelector.ActiveToggles ().FirstOrDefault ().name, i);
						chr.Start ();
						chr.gameObject.SetActive (true);
						gameObject.SetActive (false);
				}
		}

		string RandomChar ()
		{
				int num = Mathf.RoundToInt (UnityEngine.Random.Range (48f, 122f));
				return char.ConvertFromUtf32 (num);
		}
}
