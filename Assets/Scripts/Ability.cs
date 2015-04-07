using UnityEngine;
using System.Collections;
using UniLua;
using System.Collections.Generic;

public class Ability : LuaScript {



	public Ability(string path, GameObject affObj){
		this.path="Abilities/"+path+".lua";
		this.affectedObject = affObj;

		define = new List<NameFuncPair>{
			new NameFuncPair("move",move),
			new NameFuncPair("trace",L_Trace),
		};

		init ();

	}
	//Описываем функции, вызываемые из Lua 
	private int move(ILuaState s){
		float f = (float)s.L_CheckNumber(1);
		affectedObject.rigidbody2D.AddForce(Vector3.right*f);
		return 1;
	}
	
	private int L_Trace(ILuaState s)
	{
		Debug.Log("Lua trace: " + s.L_CheckString(1)); // читаем первый параметр
		return 1; // так надо
	}
}
