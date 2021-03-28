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
		
		if (!database_editor.FileExists("res://databases/System.json"))
		{
			database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary system_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary stats_data = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary weapont_data = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary armort_data = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary element_data = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary slots_data = new Godot.Collections.Dictionary();
			stats_data.Add("0", "hp");
			stats_data.Add("1", "mp");
			stats_data.Add("2", "atk");
			stats_data.Add("3", "def");
			stats_data.Add("4", "int");
			stats_data.Add("5", "res");
			stats_data.Add("6", "spd");
			stats_data.Add("7", "luk");
			weapont_data.Add("0","Sword");
			weapont_data.Add("1","Spear");
			weapont_data.Add("2","Axe");
			weapont_data.Add("3","Staff");
			armort_data.Add("0","Armor");
			armort_data.Add("1","Robe");
			armort_data.Add("2","Shield");
			armort_data.Add("3","Hat");
			armort_data.Add("4","Accessory");
			element_data.Add("0","Physical");
			element_data.Add("1","Fire");
			element_data.Add("2","Ice");
			element_data.Add("3","Wind");
			slots_data.Add("w0","Weapon");
			slots_data.Add("a1","Head");
			slots_data.Add("a2","Body");
			slots_data.Add("a3","Accessory");
			system_array.Add("stats", stats_data);
			system_array.Add("weapons", weapont_data);
			system_array.Add("armors", armort_data);
			system_array.Add("elements", element_data);
			system_array.Add("slots", slots_data);
			database_editor.StoreLine(JSON.Print(system_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Character.json"))
		{
			database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary character_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary character_data = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary etype_data = new Godot.Collections.Dictionary();
			character_data.Add("faceImage", "");
			character_data.Add("charaImage", "");
			character_data.Add("name", "Kate");
			character_data.Add("class", 0);
			character_data.Add("initialLevel", 1);
			character_data.Add("maxLevel", 99);
			etype_data.Add("w0",0);
			etype_data.Add("w1",1);
			etype_data.Add("a2",0);
			etype_data.Add("a3",3);
			character_data.Add("startWeapon", 0);
			character_data.Add("startArmor", 0);
			character_data.Add("startAccesory", 0);
			character_data.Add("equip_types", etype_data);
			character_array.Add("chara0", character_data);
			database_editor.StoreLine(JSON.Print(character_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Skill.json"))
		{
			database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary skill_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary skill_data = new Godot.Collections.Dictionary();
			skill_data.Add("name", "Double Attack");
			skill_data.Add("icon", "");
			skill_data.Add("description", "Attacks an enemy twice");
			skill_data.Add("skill_type", 0);
			skill_data.Add("mp_cost", 4);
			skill_data.Add("tp_cost", 2);
			skill_data.Add("target", 1);
			skill_data.Add("usable", 1);
			skill_data.Add("success", 95);
			skill_data.Add("hit_type", 1);
			skill_data.Add("damage_type", 1);
			skill_data.Add("element", 0);
			skill_data.Add("formula", "atk * 4 - def * 2");
			skill_array.Add("skill0", skill_data);
			database_editor.StoreLine(JSON.Print(skill_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Class.json"))
		{
			database_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary class_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary class_data = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary class_stats_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary skill_list_array = new Godot.Collections.Dictionary();
			class_data.Add("name", "Warrior");
			class_data.Add("icon", "");
			class_data.Add("experience", "level * 30");
			class_stats_array.Add("hp", "level * 25 + 10");
			class_stats_array.Add("mp", "level * 15 + 5");
			class_stats_array.Add("atk", "level * 5 + 3");
			class_stats_array.Add("def", "level * 5 + 3");
			class_stats_array.Add("int", "level * 5 + 3");
			class_stats_array.Add("res", "level * 5 + 3");
			class_stats_array.Add("spd", "level * 5 + 3");
			class_stats_array.Add("luk", "level * 5 + 3");
			skill_list_array.Add(0, 5);
			class_data.Add("skill_list", skill_list_array);
			class_data.Add("stat_list", class_stats_array);
			class_array.Add("class0", class_data);
			database_editor.StoreLine(JSON.Print(class_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Item.json"))
		{
			database_editor.Open("res://databases/Item.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary item_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary item_data = new Godot.Collections.Dictionary();
			item_data.Add("name", "Potion");
			item_data.Add("icon", "");
			item_data.Add("description", "Heals 50HP to one ally");
			item_data.Add("item_type", 0);
			item_data.Add("price", 50);
			item_data.Add("consumable", 0);
			item_data.Add("target", 7);
			item_data.Add("usable", 0);
			item_data.Add("success", 100);
			item_data.Add("hit_type", 0);
			item_data.Add("damage_type", 3);
			item_data.Add("element", 0);
			item_data.Add("formula", "50");
			item_array.Add("item0", item_data);
			database_editor.StoreLine(JSON.Print(item_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Weapon.json"))
		{
			database_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary weapon_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary weapon_data = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary weapon_stats_array = new Godot.Collections.Dictionary();
			weapon_data.Add("name", "Broad Sword");
			weapon_data.Add("icon", "");
			weapon_data.Add("description", "Heals 50HP to one ally");
			weapon_data.Add("weapon_type", 0);
			weapon_data.Add("slot_type", 0);
			weapon_data.Add("price", 50);
			weapon_data.Add("element", 0);
			weapon_stats_array.Add("hp", "0");
			weapon_stats_array.Add("mp", "0");
			weapon_stats_array.Add("atk", "10");
			weapon_stats_array.Add("def", "2");
			weapon_stats_array.Add("int", "2");
			weapon_stats_array.Add("res", "1");
			weapon_stats_array.Add("spd", "0");
			weapon_stats_array.Add("luk", "0");
			weapon_data.Add("stat_list", weapon_stats_array);
			weapon_array.Add("weapon0", weapon_data);
			database_editor.StoreLine(JSON.Print(weapon_array));
			database_editor.Close();
		}

		if (!database_editor.FileExists("res://databases/Armor.json"))
		{
			database_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary armor_array = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary armor_data = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary armor_stats_array = new Godot.Collections.Dictionary();
			armor_data.Add("name", "Clothes");
			armor_data.Add("icon", "");
			armor_data.Add("description", "Heals 50HP to one ally");
			armor_data.Add("armor_type", 0);
			armor_data.Add("slot_type", 0);
			armor_data.Add("price", 50);
			armor_stats_array.Add("hp", "0");
			armor_stats_array.Add("mp", "0");
			armor_stats_array.Add("atk", "10");
			armor_stats_array.Add("def", "2");
			armor_stats_array.Add("int", "2");
			armor_stats_array.Add("res", "1");
			armor_stats_array.Add("spd", "0");
			armor_stats_array.Add("luk", "0");
			armor_data.Add("stat_list", armor_stats_array);
			armor_array.Add("armor0", armor_data);
			database_editor.StoreLine(JSON.Print(armor_array));
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
		} else if (tab == 3) {
			GetNode<Control>("Tabs/Item").Call("_Start");
		} else if (tab == 4) {
			GetNode<Control>("Tabs/Weapon").Call("_Start");
		} else if (tab == 5) {
			GetNode<Control>("Tabs/Armor").Call("_Start");
		} else if (tab == 6) {
			GetNode<Control>("Tabs/System").Call("_Start");
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
