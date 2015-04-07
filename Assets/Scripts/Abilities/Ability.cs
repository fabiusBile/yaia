using UnityEngine;
using System.Collections;
using UniLua;


public class Ability : LuaScript {

	void Start () {
		define = new NameFuncPair[]{
			new NameFuncPair("move",move),
			new NameFuncPair("trace",L_Trace),
		};
	}

	public Ability(string path){
		this.path="abilities/"+path;
	}
	//Описываем функции, вызываемые из Lua 
	private int move(ILuaState s){
		float f = (float)s.L_CheckNumber(1);
		rigidbody2D.AddForce(Vector3.right*f);
		return 1;
	}

	private int L_Trace(ILuaState s)
	{
		Debug.Log("Lua trace: " + s.L_CheckString(1)); // читаем первый параметр
		return 1; // так надо
	}
}
