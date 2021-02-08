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
			class_array.Add("class0", class_data);
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
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
