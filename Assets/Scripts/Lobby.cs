using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
		public GameObject lobbylist;
		public Text lobbyname;
		public Text playersList;
		public GameObject startBtn;
		public PlayersData pd;
		public NetworkPlayersData npd;
		public PhotonView pv;
		public ArrayList players;
		public Transform[] Seats;
		public Text[] Labels;
		GameObject myPlayer;
		GameObject ds;

//		void OnGUI ()
//		{
//				if (GUI.Button (new Rect (0f, 0f, 200f, 50f), "debug")) {
//						foreach (object playersData in npd.npd){
//								Debug.Log ((playersData as PlayersData).data ["name"]);
//								Debug.Log(PhotonNetwork.player.name);
//						}
//						showNames ();
//				}
//		}
		
		void Awake ()
		{
				ds = GameObject.Find ("PlayersData");
				pv = gameObject.GetComponent<PhotonView> ();
				PlayerPrefs.SetInt ("Single", 0);
		}

		void OnEnable ()
		{
				npd = ds.GetComponent<NetworkPlayersData> ();
				pd = npd.localPd;
				PhotonNetwork.offlineMode = PlayerPrefs.GetInt ("Single") == 1 ? true : false;
				if (PhotonNetwork.offlineMode)
						PhotonNetwork.CreateRoom (null);
				PhotonNetwork.automaticallySyncScene = true;
				if (PhotonNetwork.connected) {
						lobbyname.text = PhotonNetwork.room.name;
						startBtn.SetActive (PhotonNetwork.isMasterClient);
						myPlayer = PhotonNetwork.Instantiate ("Entity/PLayers/TavernPlayer", Vector3.zero, transform.rotation, 0);
						DontDestroyOnLoad (myPlayer);
						if (PhotonNetwork.isMasterClient) {
								npd.Add (pd);
								PlaceTavernPlayers ();
						} else {
								sendDataToMaster ();
						}
				} else {
						lobbyname.text = PhotonNetwork.connectionStateDetailed.ToString ();
						startBtn.SetActive (false);
				}
				showNames ();
		}

		void IncreaseSortingLayer (Transform tr)
		{
				if (tr.GetComponent<Canvas> ())
						tr.GetComponent<Canvas> ().sortingOrder += 5;
				if (tr.childCount != 0)
						for (int i=0; i!=tr.childCount; i++)
								IncreaseSortingLayer (tr.GetChild (i));
		}

		public void StartGame ()
		{
				if (!PhotonNetwork.offlineMode) {
						PhotonNetwork.room.visible = false;
						PhotonNetwork.room.open = false;

				}
				if (PhotonNetwork.isMasterClient)
						foreach (GameObject tvPlayer in GameObject.FindGameObjectsWithTag("Player")) {
								PhotonNetwork.Destroy (tvPlayer);
						}
				PhotonNetwork.LoadLevel (1);
		}

		public void disconnect ()
		{					
				PhotonNetwork.Destroy (myPlayer);
				PhotonNetwork.LeaveRoom ();
				ClearLobby ();
		}

		void ClearLobby ()
		{
				ResetNPD ();
				foreach (Transform s in Seats) {
						if (s.childCount > 0)
								Destroy (s.GetChild (0).gameObject);
				}
				foreach (GameObject playerName in GameObject.FindGameObjectsWithTag("PlayerName"))
						playerName.SetActive (false);
		}

		[RPC]
		void PlaceTavernPlayers ()
		{
				foreach (GameObject playerName in GameObject.FindGameObjectsWithTag("PlayerName"))
						playerName.SetActive (false);
				int i = 0;
				foreach (GameObject tvPlayer in GameObject.FindGameObjectsWithTag("Player")) {
						tvPlayer.transform.SetParent (Seats [i]);
						Labels [i].gameObject.SetActive (true);
						if (!PhotonNetwork.offlineMode)
								Labels [i].text = npd.Get (tvPlayer.GetPhotonView ().owner.name).data ["name"].ToString ();
						else
								Labels [i].text = pd.data ["name"].ToString ();
						tvPlayer.transform.localPosition = Vector3.zero;
						if (i == 0 || i == 1) {
								if (tvPlayer.transform.localScale.x > 0)
										tvPlayer.transform.localScale = new Vector3 (-tvPlayer.transform.localScale.x, tvPlayer.transform.localScale.y, tvPlayer.transform.localScale.z);
						}
						if (i != 3) {
								IncreaseSortingLayer (tvPlayer.transform);
						}
						i++;
				}
				
		}

		void sendDataToMaster ()
		{
				//object[] data = pd.GetArray ();
				object data = pd.GetArray ();
				pv.RPC ("AddPlayersData", PhotonTargets.MasterClient, data);
		}

		void OnMasterClientSwitched ()
		{
				disconnect ();
				gameObject.SetActive (false);
				lobbylist.SetActive (true);
		}

		void OnPhotonPlayerConnected ()
		{
				showNames ();
		}

		void OnPhotonPlayerDisconnected ()
		{
				if (PhotonNetwork.isMasterClient) {
						foreach (string p in players) {
								bool contain = false;
								foreach (PhotonPlayer p2 in PhotonNetwork.playerList)
										if (p == p2.name)
												contain = true;
								if (!contain)
										npd.delete (p);
						}
				}
				showNames ();
				PlaceTavernPlayers ();
		}

		[RPC]
		void showNames ()
		{
				players = new ArrayList ();
				string playerNames = "";
				foreach (object id in npd.npd.Keys) {
						string name = (npd.npd [id] as PlayersData).data ["name"].ToString ();
						playerNames += (name + "\n");
						players.Add (name);
				}
				playersList.text = playerNames;
		}

		[RPC]
		void ResetNPD ()
		{
				npd.npd = new Hashtable (4);
		}
		
		void UpdatePlayersData ()
		{
				pv.RPC ("ResetNPD", PhotonTargets.Others, null);
				foreach (PlayersData p in npd.npd.Values) {
						object[] d = p.GetArray ();
						pv.RPC ("AddPlayersData", PhotonTargets.Others, d);
				}
				pv.RPC ("showNames", PhotonTargets.AllBuffered, null);
				gameObject.GetPhotonView ().RPC ("PlaceTavernPlayers", PhotonTargets.AllBuffered, null);
		}
		
		[RPC]
		void AddPlayersData (object[] data)
		{
				PlayersData loadedPd = new PlayersData (data);
				npd.Add (loadedPd);
				if (PhotonNetwork.isMasterClient) {
						UpdatePlayersData ();
				}
		}
}
