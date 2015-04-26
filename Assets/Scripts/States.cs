/**
*	Описывает состояния - различные положительные или отрицательные эффекты, наложение на персонажа,
*	т.е. баффы или дебаффы и прочее. 
**/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class States : MonoBehaviour {

	hitpoints hp;
	private delegate void StateMethod(GameObject O);
	struct state {
		StateMethod onEnable;   //Вызывается при наложении эффекта
		StateMethod onDisable;  //Вызывается при снятии эффекта
		public int times;		//Сколько раз наложен эффект состояния 
		public bool enabled;


		private state(StateMethod onEnable){
			this.onEnable = onEnable;
			this.times=0;
			this.enabled=false;
			this.onDisable=null;
		}
		private state(StateMethod onEnable, StateMethod onDisable){
			this.onEnable = onEnable;
			this.onDisable = onDisable;
			this.times=0;
			this.enabled=false;
		}

	}

	Dictionary<string,state> ActiveStates = new Dictionary<string,state>{ 			//Код этих состояний вызывается автоматически

	};
	Dictionary<string,state> ModifierStates = new Dictionary<string,state>{			//Временно изменяют определенные параметры 
		
	};
	Dictionary<string,state> onHitStates = new Dictionary<string,state>{			//Код вызывается при нанесении урона
		
	};
	// Use this for initialization
	void Start () {
		hp = gameObject.GetComponent<hitpoints> ();
	}



	public void onHit(GameObject attacker){
	}

	// Update is called once per frame
	void Update () {
	
	}





}
