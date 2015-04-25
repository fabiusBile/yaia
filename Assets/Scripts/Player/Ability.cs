using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Ability {

	struct ability{
		//string reqClass;
		//int minLvl;
		//float cd;
		public AbilityDelegate call;
		public ability(AbilityDelegate call){
			this.call=call;
		}
	}

	private  delegate  void AbilityDelegate(GameObject o);

	public static void Use (GameObject target, string name){
		list[name].call(target);
	}

	static private  Dictionary <string,ability>  list = new Dictionary<string,ability> {
		{"Jump", new ability(Jump) },
	};

	static void Jump(GameObject o){
		o.GetComponent<Rigidbody2D>().AddForce(Vector3.up*500f);
	}
}
