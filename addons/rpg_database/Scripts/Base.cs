using Godot;
using System;

[Tool]
public class Base : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Godot.File database_editor = new Godot.File();
		if (!database_editor.FileExists("res://databases/editor_database.json"))
		{
			database_editor.Open("res://databases/editor_database.json", Godot.File.ModeFlags.Write);
			database_editor.Close();
		}
		if (!database_editor.FileExists("res://databases/Character.json"))
		{
			database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary character_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary character_data = new Godot.Collections.Dictionary();
			character_data.Add("faceImage", "");
			character_data.Add("charaImage", "");
			character_data.Add("name", "Kate");
			character_data.Add("class", 0);
			character_data.Add("initialLevel", 1);
			character_data.Add("maxLevel", 99);
			character_data.Add("startWeapon", 0);
			character_data.Add("startArmor", 0);
			character_data.Add("startAccesory", 0);
			character_array.Add("chara0", character_data);
			database_editor.StoreLine(JSON.Print(character_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Class.json"))
		{
			database_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary class_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary class_data = new Godot.Collections.Dictionary();
			class_data.Add("name", "Warrior");
			class_data.Add("icon", "");
			class_data.Add("experience", "level * 30");
			class_data.Add("hp", "level * 25 + 10");
			class_data.Add("mp", "level * 15 + 5");
			class_data.Add("atk", "level * 5 + 3");
			class_data.Add("def", "level * 5 + 3");
			class_data.Add("int", "level * 5 + 3");
			class_data.Add("res", "level * 5 + 3");
			class_data.Add("spd", "level * 5 + 3");
			class_data.Add("luk", "level * 5 + 3");
			class_data.Add("skill_list", "");
			class_array.Add("class0", class_data);
			database_editor.StoreLine(JSON.Print(class_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Skill.json"))
		{
			database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary class_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary class_data = new Godot.Collections.Dictionary();
			class_data.Add("name", "Double Attack");
			class_array.Add("skill0", class_data);
			database_editor.StoreLine(JSON.Print(class_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Item.json"))
		{
			database_editor.Open("res://databases/Item.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary class_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary class_data = new Godot.Collections.Dictionary();
			class_data.Add("name", "Potion");
			class_array.Add("item0", class_data);
			database_editor.StoreLine(JSON.Print(class_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Weapon.json"))
		{
			database_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary class_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary class_data = new Godot.Collections.Dictionary();
			class_data.Add("name", "Broad Sword");
			class_array.Add("weapon0", class_data);
			database_editor.StoreLine(JSON.Print(class_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Armor.json"))
		{
			database_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary class_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary class_data = new Godot.Collections.Dictionary();
			class_data.Add("name", "Clothes");
			class_array.Add("armor0", class_data);
			database_editor.StoreLine(JSON.Print(class_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Accesory.json"))
		{
			database_editor.Open("res://databases/Accesory.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary class_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary class_data = new Godot.Collections.Dictionary();
			class_data.Add("name", "Ring");
			class_array.Add("accesory0", class_data);
			database_editor.StoreLine(JSON.Print(class_array));
			database_editor.Close();
		}

		GetNode<Control>("Tabs/Character").Call("_Start");
	}

	private void _on_Tabs_tab_changed(int tab)
	{
		if (tab == 0) 
		{
			GetNode<Control>("Tabs/Character").Call("_Start");
		} else if (tab == 1) {
			GetNode<Control>("Tabs/Class").Call("_Start");
		} else if (tab == 2) {
			GetNode<Control>("Tabs/Skill").Call("_Start");
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
