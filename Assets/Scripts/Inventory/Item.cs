using UnityEngine;
using System.Collections;
using System.Xml;
public class Item {
	public  string itName;
	public  string type;
	public  string image;
	static XmlNode item;
	static XmlNode prefsNode;
	public Hashtable prefs;

	public Item (string name,string type, string image, Hashtable prefs){
		this.itName = name;
		this.type = type;
		this.image = image;
		this.prefs = prefs;
	}
	public Item (object[] data){
		this.itName=data[0].ToString();
		this.type=data[1].ToString();
		this.image=data[2].ToString();
		object[] keys = data [3] as object[];
		object[] values = data [4] as object[];

		this.prefs = new Hashtable();
		for (int i=0; i!=keys.Length;i++){
			this.prefs.Add(keys[i],values[i]);
		}
	}

	public Item(XmlDocument xmlDoc,XmlNode itmNode){
		XmlAttributeCollection atrs = itmNode.Attributes;
		XmlNode prefsNode = itmNode.FirstChild;
		Debug.Log (prefsNode.Name);
		this.itName = atrs["name"].Value;
		this.type = atrs["type"].Value;
		this.image = atrs["image"].Value;
		this.prefs = new Hashtable ();
		atrs = prefsNode.Attributes;
		foreach (XmlAttribute atr in atrs) {
			this.prefs.Add(atr.Name,atr.Value);
		}
	}


	public static XmlNode GetFields(XmlDocument doc){
		item = doc.CreateElement ("Item");
		AddAtr (doc,"name","");
		AddAtr (doc,"type","");
		AddAtr (doc,"image","");
		prefsNode = doc.CreateElement ("prefs");
		item.AppendChild (prefsNode);
		return item;
	}
	public XmlNode GetXml(XmlDocument doc){
		item = doc.CreateElement ("Item");
		AddAtr (doc,"name",itName);
		AddAtr (doc,"type",type);
		AddAtr (doc,"image",image);
		prefsNode = doc.CreateElement ("prefs");

		foreach (object key in prefs.Keys) {
			Debug.Log (key.ToString());
			XmlAttribute atr = doc.CreateAttribute (key.ToString());
			atr.Value = prefs[key].ToString();
			prefsNode.Attributes.Append (atr);
		}
		item.AppendChild (prefsNode);
		return item;
	}
	static void  AddAtr(XmlDocument doc,string atrName, string atrValue){
		XmlAttribute atr = doc.CreateAttribute (atrName);
		item.Attributes.Append (atr);
		item.Attributes [atrName].Value = atrValue;
	}
	public object[] serialize(){
		object[] data = new object[5];
		data[0]=itName;
		data[1]=type;
		data[2]=image;
		data[3] = (new ArrayList (prefs.Keys)).ToArray () as object;
		data[4] = (new ArrayList (prefs.Values)).ToArray () as object;
		return data;
	}
}
