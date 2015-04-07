using UnityEngine;
using System.Collections;
using UniLua;
using System.IO;
using System.Collections.Generic;

 public abstract class LuaScript  {
	protected GameObject affectedObject;
	protected  List<NameFuncPair> define;

	protected static ILuaState  _lua; // через этот объект будет производится работа с Lua
	protected static ThreadStatus _status; // объект для работы с конкретным скриптом
	private string lua_script;
	protected string path;


	protected void init(){
		//Debug.Log (path);
//		StreamReader streamReader = new StreamReader(path);
//		this.lua_script = streamReader.ReadToEnd();
//		streamReader.Close();
		_lua = LuaAPI.NewState();	 // создаем 
		_lua.L_OpenLibs();
		_lua.L_RequireF("mylib", OpenLib, true);

//		string lua_script = @"
//    		local lib = require ""mylib""
//    		lib.trace(""Test output"")
//		";
		_status = _lua.L_LoadFile (path);

		if (_status != ThreadStatus.LUA_OK)
		{
			Debug.LogError("Error parsing lua code");
		}
	}

	private int OpenLib(ILuaState lua)
	{	
		lua.L_NewLib(define.ToArray());
		return 1;
	}

	public void execute(){
		_lua.L_LoadFile (path);
		_lua.Call(0, 0);
	}
	
}
