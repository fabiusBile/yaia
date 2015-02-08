using UnityEngine;
using System.Collections;
using System.Xml;

public class Spawn : MonoBehaviour
{
		public PlayersData pd;
		public NetworkPlayersData npd;
		public GameObject panel;

		void Start ()
		{
				GameObject ds = GameObject.Find ("PlayersData");
				pd = ds.GetComponent<PlayersData> ();
				npd = ds.GetComponent<NetworkPlayersData> ();
				GameObject myPlayer = PhotonNetwork.Instantiate ("Player", transform.position, transform.rotation, 0);
				PlayerControl pc = myPlayer.GetComponent<PlayerControl> ();
				pc.enabled = true;
				myPlayer.transform.FindChild ("CamMover").gameObject.SetActive (true);
				pc.checkpoint = transform;
		}
		
//		void OnJoinedLobby ()
//		{
//				PhotonNetwork.JoinRandomRoom ();
//		}
//
//		void OnPhotonRandomJoinFailed ()
//		{
//				PhotonNetwork.CreateRoom (null);
//		}
//
//		void OnJoinedRoom ()
//		{
//				GameObject myPlayer = PhotonNetwork.Instantiate ("Player", transform.position, transform.rotation, 0);
//				PlayerControl pc = myPlayer.GetComponent<PlayerControl> ();
//				pc.enabled = true;
//				myPlayer.transform.FindChild ("CamMover").gameObject.SetActive (true);
//				pc.checkpoint = transform;
//				int n = PlayerPrefs.GetInt ("CurentPlayer");
//				pc.playerName = characters [n].Attributes ["name"].Value;
//				pc.playerClass = characters [n].Attributes ["class"].Value;
//		}
}
