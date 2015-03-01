using UnityEngine;
using System.Collections;

public class NetworkPlayer : MonoBehaviour {

	Vector3 realPosition;
	Quaternion realRotation;
	Vector3 realLookAngle;

	PlayerControl pl;
	float dir;
	hitpoints hp;
	string playerClass;
	string playerName;

	Animator anim;

	// Use this for initialization
	void Awake () {
		pl = transform.GetComponent<PlayerControl> ();
		anim = GetComponent<Animator>();
		hp= GetComponent<hitpoints>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		PhotonView pview = transform.GetComponent<PhotonView> ();
		float ping =   (float)PhotonNetwork.GetPing ()/1000f;
		dir = transform.localScale.x;
		if (!pview.isMine) { 
			transform.position=Vector3.Lerp(transform.position,realPosition,ping);    //Используем линейную интерполяцию
			transform.rotation=Quaternion.Lerp(transform.rotation,realRotation,ping); //для сглаживания движений
			pl.dir=Vector3.Lerp(pl.dir,realLookAngle,ping);
		}
	}
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

		if (stream.isWriting) { //Текущий игрок
			stream.SendNext(transform.position); 	//Положение
			stream.SendNext(transform.rotation); 	//Поворот
			stream.SendNext(pl.dir);			 	//Поворт рук
			stream.SendNext(dir);					//Направление 
			stream.SendNext(anim.GetFloat("Speed"));//Скорость для анимации
			stream.SendNext(pl.jump);				//Прыжок дл анимации
			stream.SendNext(hp.hp);					//хп
		}
		if (stream.isReading) { //Мультиплеерный игрок
			realPosition=(Vector3)stream.ReceiveNext(); 		//
			realRotation=(Quaternion)stream.ReceiveNext();		//
			realLookAngle=(Vector3)stream.ReceiveNext();		//
			dir = (float)stream.ReceiveNext();					//
			Vector3 localScale = transform.localScale;
			localScale.x=dir;
			transform.localScale=localScale;
			if (anim==null)anim=GetComponent<Animator>();
			anim.SetFloat("Speed",(float)stream.ReceiveNext());	//
			bool j = (bool)stream.ReceiveNext();				//
			anim.SetBool("Jump",j);
			if (hp==null)hp=GetComponent<hitpoints>();			//
			hp.hp=(float)stream.ReceiveNext();					//
		}
	}
}
