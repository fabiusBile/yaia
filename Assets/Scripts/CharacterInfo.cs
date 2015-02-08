using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CharacterInfo : MonoBehaviour {
	public Text name;
	public Text cls;
	public int i;
	public Characters chr;

	public void init(string nm,string cl){
		name.text = nm;
		cls.text = cl;
		PlayerPrefs.SetInt ("CurentPlayer", i);
	}
	public void delete(){
		chr.delete (i);
		chr.Start ();
	}
	public void startSingle(){
		PlayerPrefs.SetInt ("Single", 1);
	}
	
}
