using UnityEngine;
using System.Collections;
using System.Xml;
public class Item {
	public  string name;
	public  string type;
	public  string image;
	static XmlNode item;

	public Item (string name,string type, string image){
		this.name = name;
		this.type = type;
		this.image = image;
	}
	public static XmlNode GetFields(XmlDocument doc){
		item = doc.CreateElement ("Item");
		AddAtr (doc,"name","");
		AddAtr (doc,"type","");
		AddAtr (doc,"image","");
		return item;
	}
	public XmlNode GetXml(XmlDocument doc){
		item = doc.CreateElement ("Item");
		AddAtr (doc,"name",name);
		AddAtr (doc,"type",type);
		AddAtr (doc,"image",image);
		return item;
	}
	static void  AddAtr(XmlDocument doc,string atrName, string atrValue){
		XmlAttribute atr = doc.CreateAttribute (atrName);
		item.Attributes.Append (atr);
		item.Attributes [atrName].Value = atrValue;
	}
}
