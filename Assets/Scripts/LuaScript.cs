using UnityEngine;
using System.Collections;
using UniLua;
using System.IO;


 public abstract class LuaScript : MonoBehaviour {

	protected NameFuncPair[] define;

	protected static ILuaState  _lua; // через этот объект будет производится работа с Lua
	protected static ThreadStatus _status; // объект для работы с конкретным скриптом
	private string lua_script;
	protected string path;

	public LuaScript(){
		StreamReader streamReader = new StreamReader(path);
		this.lua_script = streamReader.ReadToEnd();
		streamReader.Close();

		_lua = LuaAPI.NewState();	 // создаем 
		_lua.L_OpenLibs();
		_lua.L_RequireF("mylib", OpenLib, true);
		
		_status = _lua.L_LoadString(lua_script); // загружаем скрипт
		
		if (_status != ThreadStatus.LUA_OK)
		{
			Debug.LogError("Error parsing lua code");
		}
	}

	private int OpenLib(ILuaState lua)
	{	
		lua.L_NewLib(define);
		return 1;
	}

	public void execute(){
		_lua.Call(0, 0); // запускаем Lua-скрипт
	}

}
