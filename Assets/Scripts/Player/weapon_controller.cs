/// <summary>
///Является  свзующем звеном между владельцем и оружием
///Принимает значения направления и инициализации выстрела с любого weapon-скрипта (от игрока или от любого скрипта бота)
///Любое оружие должно быть настроено так, что бы принимать эти значения с этого скрипта
/// Это позволяет использовать любое оружие любым ботом\игроком
/// </summary>

using UnityEngine;
using System.Collections;

public class weapon_controller : MonoBehaviour {
	public bool fire;
	public bool fire2;
	public bool facing;
}
