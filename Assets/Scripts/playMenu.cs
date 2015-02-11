using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
public class playMenu : MonoBehaviour
{
		public InputField roomname;
		public Text conStatus;
		public GameObject popup;
		public GameObject lobby;
		XmlNodeList characters;
		XmlDocument charsDoc;
		public void createGame ()
		{
				if (PhotonNetwork.insideLobby)
						PhotonNetwork.CreateRoom (roomname.text);
				else
						Debug.Log ("Errr");
		}

		public void Init()
		{
				charsDoc = new XmlDocument ();
				charsDoc.Load ("Data/characters.xml");
				characters = charsDoc.GetElementsByTagName ("char");
				PhotonNetwork.ConnectUsingSettings ("0.001");
				
				int n = PlayerPrefs.GetInt ("CurentPlayer");
				PhotonNetwork.player.name=characters [n].Attributes ["id"].Value;
		}
		public void Disable(){
				if (PhotonNetwork.connected)
						PhotonNetwork.Disconnect ();
		}
		void Update ()
		{
				if (gameObject.GetActive ())
						conStatus.text = PhotonNetwork.connectionStateDetailed.ToString ();
		}
		void OnJoinedRoom(){
			popup.SetActive (false);
			gameObject.SetActive (false);
			lobby.SetActive (true);
		}
}
