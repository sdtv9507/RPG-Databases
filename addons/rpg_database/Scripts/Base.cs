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
		Godot.File databaseFile = new Godot.File();

		if (!databaseFile.FileExists("res://databases/System.json"))
		{
			databaseFile.Open("res://databases/System.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary systemList = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary statsData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary weaponTypeData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary armorTypeData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary elementData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary slotsData = new Godot.Collections.Dictionary();
			statsData.Add("0", "hp");
			statsData.Add("1", "mp");
			statsData.Add("2", "atk");
			statsData.Add("3", "def");
			statsData.Add("4", "int");
			statsData.Add("5", "res");
			statsData.Add("6", "spd");
			statsData.Add("7", "luk");
			weaponTypeData.Add("0","Sword");
			weaponTypeData.Add("1","Spear");
			weaponTypeData.Add("2","Axe");
			weaponTypeData.Add("3","Staff");
			armorTypeData.Add("0","Armor");
			armorTypeData.Add("1","Robe");
			armorTypeData.Add("2","Shield");
			armorTypeData.Add("3","Hat");
			armorTypeData.Add("4","Accessory");
			elementData.Add("0","Physical");
			elementData.Add("1","Fire");
			elementData.Add("2","Ice");
			elementData.Add("3","Wind");
			slotsData.Add("w0","Weapon");
			slotsData.Add("a1","Head");
			slotsData.Add("a2","Body");
			slotsData.Add("a3","Accessory");
			systemList.Add("stats", statsData);
			systemList.Add("weapons", weaponTypeData);
			systemList.Add("armors", armorTypeData);
			systemList.Add("elements", elementData);
			systemList.Add("slots", slotsData);
			databaseFile.StoreLine(JSON.Print(systemList));
			databaseFile.Close();
		}

		if (!databaseFile.FileExists("res://databases/Character.json"))
		{
			databaseFile.Open("res://databases/Character.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary characterList = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary characterData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary equipTypeData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary initialEquipData = new Godot.Collections.Dictionary();
			characterData.Add("faceImage", "");
			characterData.Add("charaImage", "");
			characterData.Add("name", "Kate");
			characterData.Add("class", 0);
			characterData.Add("description", "");
			characterData.Add("initialLevel", 1);
			characterData.Add("maxLevel", 99);
			equipTypeData.Add("w0",0);
			equipTypeData.Add("w1",1);
			equipTypeData.Add("a2",0);
			equipTypeData.Add("a3",3);
			initialEquipData.Add("0",-1);
			initialEquipData.Add("1",-1);
			initialEquipData.Add("2",-1);
			initialEquipData.Add("3",-1);
			characterData.Add("initial_equip", initialEquipData);
			characterData.Add("equip_types", equipTypeData);
			characterList.Add("chara0", characterData);
			databaseFile.StoreLine(JSON.Print(characterList));
			databaseFile.Close();
		}

		if (!databaseFile.FileExists("res://databases/Skill.json"))
		{
			databaseFile.Open("res://databases/Skill.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary skillList = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary skillData = new Godot.Collections.Dictionary();
			skillData.Add("name", "Double Attack");
			skillData.Add("icon", "");
			skillData.Add("description", "Attacks an enemy twice");
			skillData.Add("skill_type", 0);
			skillData.Add("mp_cost", 4);
			skillData.Add("tp_cost", 2);
			skillData.Add("target", 1);
			skillData.Add("usable", 1);
			skillData.Add("success", 95);
			skillData.Add("hit_type", 1);
			skillData.Add("damage_type", 1);
			skillData.Add("element", 0);
			skillData.Add("formula", "atk * 4 - def * 2");
			skillList.Add("skill0", skillData);
			databaseFile.StoreLine(JSON.Print(skillList));
			databaseFile.Close();
		}

		if (!databaseFile.FileExists("res://databases/Class.json"))
		{
			databaseFile.Open("res://databases/Class.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary classList = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary classData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary classStats = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary skillList = new Godot.Collections.Dictionary();
			classData.Add("name", "Warrior");
			classData.Add("icon", "");
			classData.Add("experience", "level * 30");
			classStats.Add("hp", "level * 25 + 10");
			classStats.Add("mp", "level * 15 + 5");
			classStats.Add("atk", "level * 5 + 3");
			classStats.Add("def", "level * 5 + 3");
			classStats.Add("int", "level * 5 + 3");
			classStats.Add("res", "level * 5 + 3");
			classStats.Add("spd", "level * 5 + 3");
			classStats.Add("luk", "level * 5 + 3");
			skillList.Add(0, 5);
			classData.Add("skill_list", skillList);
			classData.Add("stat_list", classStats);
			classList.Add("class0", classData);
			databaseFile.StoreLine(JSON.Print(classList));
			databaseFile.Close();
		}

		if (!databaseFile.FileExists("res://databases/Item.json"))
		{
			databaseFile.Open("res://databases/Item.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary itemList = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary itemData = new Godot.Collections.Dictionary();
			itemData.Add("name", "Potion");
			itemData.Add("icon", "");
			itemData.Add("description", "Heals 50HP to one ally");
			itemData.Add("item_type", 0);
			itemData.Add("price", 50);
			itemData.Add("consumable", 0);
			itemData.Add("target", 7);
			itemData.Add("usable", 0);
			itemData.Add("success", 100);
			itemData.Add("hit_type", 0);
			itemData.Add("damage_type", 3);
			itemData.Add("element", 0);
			itemData.Add("formula", "50");
			itemList.Add("item0", itemData);
			databaseFile.StoreLine(JSON.Print(itemList));
			databaseFile.Close();
		}

		if (!databaseFile.FileExists("res://databases/Weapon.json"))
		{
			databaseFile.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary weaponList = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary weaponData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary weaponStats = new Godot.Collections.Dictionary();
			weaponData.Add("name", "Broad Sword");
			weaponData.Add("icon", "");
			weaponData.Add("description", "A light and easy to use sword");
			weaponData.Add("weapon_type", 0);
			weaponData.Add("slot_type", 0);
			weaponData.Add("price", 50);
			weaponData.Add("element", 0);
			weaponStats.Add("hp", "0");
			weaponStats.Add("mp", "0");
			weaponStats.Add("atk", "10");
			weaponStats.Add("def", "2");
			weaponStats.Add("int", "2");
			weaponStats.Add("res", "1");
			weaponStats.Add("spd", "0");
			weaponStats.Add("luk", "0");
			weaponData.Add("stat_list", weaponStats);
			weaponList.Add("weapon0", weaponData);
			databaseFile.StoreLine(JSON.Print(weaponList));
			databaseFile.Close();
		}

		if (!databaseFile.FileExists("res://databases/Armor.json"))
		{
			databaseFile.Open("res://databases/Armor.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary armorList = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary armorData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary armorStats = new Godot.Collections.Dictionary();
			armorData.Add("name", "Clothes");
			armorData.Add("icon", "");
			armorData.Add("description", "Light Clothes");
			armorData.Add("armor_type", 0);
			armorData.Add("slot_type", 0);
			armorData.Add("price", 50);
			armorStats.Add("hp", "0");
			armorStats.Add("mp", "0");
			armorStats.Add("atk", "10");
			armorStats.Add("def", "2");
			armorStats.Add("int", "2");
			armorStats.Add("res", "1");
			armorStats.Add("spd", "0");
			armorStats.Add("luk", "0");
			armorData.Add("stat_list", armorStats);
			armorList.Add("armor0", armorData);
			databaseFile.StoreLine(JSON.Print(armorList));
			databaseFile.Close();
		}

		if (!databaseFile.FileExists("res://databases/Enemy.json"))
		{
			databaseFile.Open("res://databases/Enemy.json", Godot.File.ModeFlags.Write);
			Godot.Collections.Dictionary enemyList = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary enemyData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary statsData = new Godot.Collections.Dictionary();
			Godot.Collections.Dictionary dropData = new Godot.Collections.Dictionary();
			enemyData.Add("name", "Slime");
			enemyData.Add("graphicImage", "");
			statsData.Add("hp", "150");
			statsData.Add("mp", "50");
			statsData.Add("atk", "18");
			statsData.Add("def", "16");
			statsData.Add("int", "8");
			statsData.Add("res", "4");
			statsData.Add("spd", "12");
			statsData.Add("luk", "10");
			dropData.Add("i0",80);
			enemyData.Add("experience", 6);
			enemyData.Add("money", 50);
			enemyData.Add("stat_list", statsData);
			enemyData.Add("drop_list", dropData);
			enemyList.Add("enemy0", enemyData);
			databaseFile.StoreLine(JSON.Print(enemyList));
			databaseFile.Close();
		}

		databaseFile.Close();
		GetNode<Control>("Tabs/Character").Call("Start");
	}

	public Godot.Collections.Dictionary ReadData(String file)
	{
		Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/"+file+".json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        databaseFile.Close();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		return jsonParsed.Result as Godot.Collections.Dictionary;
	}

	public void StoreData(String file, Godot.Collections.Dictionary save)
	{
		Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/"+file+".json", Godot.File.ModeFlags.Write);
        databaseFile.StoreString(JSON.Print(save));
        databaseFile.Close();
	}

	private void _on_Tabs_tab_changed(int tab)
	{
		if (tab == 0) 
		{
			GetNode<Control>("Tabs/Character").Call("Start");
		} else if (tab == 1) {
			GetNode<Control>("Tabs/Class").Call("Start");
		} else if (tab == 2) {
			GetNode<Control>("Tabs/Skill").Call("Start");
		} else if (tab == 3) {
			GetNode<Control>("Tabs/Item").Call("Start");
		} else if (tab == 4) {
			GetNode<Control>("Tabs/Weapon").Call("Start");
		} else if (tab == 5) {
			GetNode<Control>("Tabs/Armor").Call("Start");
		} else if (tab == 6) {
			GetNode<Control>("Tabs/Enemy").Call("Start");
		} else if (tab == 7) {
			GetNode<Control>("Tabs/States").Call("Start");
		} else if (tab == 8) {
			GetNode<Control>("Tabs/Effects").Call("Start");
		} else if (tab == 9) {
			GetNode<Control>("Tabs/System").Call("Start");
		}
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
