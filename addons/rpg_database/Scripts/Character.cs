using Godot;
using System;

[Tool]
public class Character : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string facePath = "";
    string charaPath = "";
    int character_selected = 0;
    Godot.Collections.Array<int> equip_id_array = new Godot.Collections.Array<int>();
    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary newClassDict = jsonDictionary["chara" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("CharacterButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("CharacterButton").AddItem(newClassDict["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("CharacterButton").SetItemText(i, newClassDict["name"] as string);
            }
        }
        database_editor.Close();
        _refresh_Data(0);
    }
    public void _refresh_Data(int id)
    {
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary dataDictionary = jsonDictionary["chara" + id] as Godot.Collections.Dictionary;
        GetNode<OptionButton>("CharacterButton").SetItemText(id, dataDictionary["name"] as string);
        GetNode<LineEdit>("NameLabel/NameText").Text = dataDictionary["name"] as string;
        string face = dataDictionary["faceImage"] as string;
        if (face != "")
        {
            GetNode<Sprite>("FaceLabel/FaceSprite").Texture = GD.Load(dataDictionary["faceImage"] as string) as Godot.Texture;
        }
        GetNode<SpinBox>("InitLevelLabel/InitLevelText").Value = Convert.ToInt32(dataDictionary["initialLevel"]);
        GetNode<SpinBox>("MaxLevelLabel/MaxLevelText").Value = Convert.ToInt32(dataDictionary["maxLevel"]);

        Godot.File system_editor = new Godot.File();
        system_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = system_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary wtypeDictionary = systemDictionary["weapons"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary atypeDictionary = systemDictionary["armors"] as Godot.Collections.Dictionary;

        Godot.Collections.Dictionary etypeDictionary = dataDictionary["equip_types"] as Godot.Collections.Dictionary;
        GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").Clear();
        string equip_name;
        foreach (string equip in etypeDictionary.Keys)
        {
            string kind = equip[0].ToString();
			equip_id_array.Add(Convert.ToInt32(etypeDictionary[equip]));
            switch (kind)
            {
                case "w":
                    string w_id = equip.Remove(0, 1);
                    equip_name = "W: " + wtypeDictionary[w_id].ToString();
                    GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem(equip_name);
                    break;
                case "a":
                    string a_id = equip.Remove(0, 1);
                    equip_name = "A: " + atypeDictionary[a_id].ToString();
                    GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem(equip_name);
                    break;
            }

        }
        database_editor.Close();

        Godot.File class_editor = new Godot.File();
        class_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
        string classAsText = class_editor.GetAsText();
        JSONParseResult classParsed = JSON.Parse(classAsText);
        Godot.Collections.Dictionary classDictionary = classParsed.Result as Godot.Collections.Dictionary;
        for (int i = 0; i < classDictionary.Count; i++)
        {
            Godot.Collections.Dictionary newClassDict = classDictionary["class" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ClassLabel/ClassText").GetItemCount())
            {
                GetNode<OptionButton>("ClassLabel/ClassText").AddItem(newClassDict["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("ClassLabel/ClassText").SetItemText(i, newClassDict["name"] as string);
            }
        }
        class_editor.Close();

        Godot.File weapon_editor = new Godot.File();
        weapon_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
        string weaponAsText = weapon_editor.GetAsText();
        JSONParseResult weaponParsed = JSON.Parse(weaponAsText);
        Godot.Collections.Dictionary weaponDictionary = weaponParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary dataWeaponDictionary = weaponDictionary["weapon0"] as Godot.Collections.Dictionary;
        GetNode<OptionButton>("WeaponLabel/WeaponButton").SetItemText(0, dataWeaponDictionary["name"] as string);
        weapon_editor.Close();

        Godot.File armor_editor = new Godot.File();
        armor_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
        string armorAsText = armor_editor.GetAsText();
        JSONParseResult armorParsed = JSON.Parse(armorAsText);
        Godot.Collections.Dictionary armorDictionary = armorParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary dataArmorDictionary = armorDictionary["armor0"] as Godot.Collections.Dictionary;
        GetNode<OptionButton>("ArmorLabel/ArmorButton").SetItemText(0, dataArmorDictionary["name"] as string);
        armor_editor.Close();
    }

    public void _on_CharaSaveButton_pressed()
    {
        _save_character_values();
        _refresh_Data(character_selected);
    }

    private void _on_AddButton_pressed()
    {
        GetNode<OptionButton>("CharacterButton").AddItem("NewCharacter");
        int id = GetNode<OptionButton>("CharacterButton").GetItemCount() - 1;
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary character_data = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary etype_data = new Godot.Collections.Dictionary();
        character_data.Add("faceImage", "res://");
        character_data.Add("charaImage", "res://");
        character_data.Add("name", "NewCharacter");
        character_data.Add("class", 0);
        character_data.Add("initialLevel", 1);
        character_data.Add("maxLevel", 99);
        etype_data.Add("w0", 0);
        etype_data.Add("w1", 1);
        etype_data.Add("a2", 0);
        etype_data.Add("a3", 3);
        character_data.Add("startWeapon", 0);
        character_data.Add("startArmor", 0);
        character_data.Add("startAccesory", 0);
        jsonDictionary.Add("chara" + id, character_data);
        database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Write);
        database_editor.StoreString(JSON.Print(jsonDictionary));
        database_editor.Close();
    }
    private void _on_Search_pressed()
    {
        GetNode<FileDialog>("FaceLabel/FaceSearch").PopupCentered();
    }

    private void _on_FaceSearch_file_selected(string file)
    {
        facePath = file;
        _set_face_image(file);
    }

    private void _set_face_image(string path)
    {
        GetNode<Sprite>("FaceLabel/FaceSprite").Texture = GD.Load(path) as Godot.Texture;
    }
    public void _save_character_values()
    {
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary finalData = jsonDictionary["chara" + character_selected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary equip_type_Data = finalData["equip_types"] as Godot.Collections.Dictionary;

        finalData["faceImage"] = facePath;
        finalData["charaImage"] = "";
        finalData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
        GetNode<OptionButton>("CharacterButton").SetItemText(character_selected, GetNode<LineEdit>("NameLabel/NameText").Text);
        finalData["class"] = GetNode<OptionButton>("ClassLabel/ClassText").Selected;
        finalData["initialLevel"] = GetNode<SpinBox>("InitLevelLabel/InitLevelText").Value;
        finalData["maxLevel"] = GetNode<SpinBox>("MaxLevelLabel/MaxLevelText").Value;

        int equip_items = GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").GetItemCount();
        for (int i = 0; i < equip_items; i++)
        {
            string kind = GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").GetItemText(i)[0].ToString();
			switch (kind)
            {
                case "W":
					equip_type_Data["w" + i] = equip_id_array[i];
                    break;
                case "A":
                    equip_type_Data["a" + i] = equip_id_array[i];
                    break;
            }
        }
		
		finalData["equip_types"] = equip_type_Data;
        finalData["startWeapon"] = 0;
        finalData["startArmor"] = 0;
        finalData["startAccesory"] = 0;
        database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Write);
        database_editor.StoreString(JSON.Print(jsonDictionary));
        database_editor.Close();
    }

    private void _on_CharacterButton_item_selected(int id)
    {
        character_selected = id;
        _refresh_Data(id);
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
