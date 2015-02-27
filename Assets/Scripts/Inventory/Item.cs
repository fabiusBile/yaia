using UnityEngine;
using System.Collections;
using System.Xml;
public class Item {
	public  string itName;
	public  string type;
	public  string image;
	static XmlNode item;

	public Item (string name,string type, string image){
		this.itName = name;
		this.type = type;
		this.image = image;
	}
	public Item (object[] data){
		this.itName=data[0].ToString();
		this.type=data[1].ToString();
		this.image=data[2].ToString();
	}

	public Item(XmlDocument xmlDoc,XmlNode itmNode){
		XmlAttributeCollection atrs = itmNode.Attributes;
		this.itName = atrs["name"].Value;
		this.type = atrs["type"].Value;
		this.image=atrs["image"].Value;
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
		AddAtr (doc,"name",itName);
		AddAtr (doc,"type",type);
		AddAtr (doc,"image",image);
		return item;
	}
	static void  AddAtr(XmlDocument doc,string atrName, string atrValue){
		XmlAttribute atr = doc.CreateAttribute (atrName);
		item.Attributes.Append (atr);
		item.Attributes [atrName].Value = atrValue;
	}
	public object[] serialize(){
		object[] data = new object[3];
		data[0]=itName;
		data[1]=type;
		data[2]=image;
		return data;
	}
}
