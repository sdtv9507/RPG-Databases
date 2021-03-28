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
    int initial_equip_edit = -1;
    Godot.Collections.Array<int> equip_id_array = new Godot.Collections.Array<int>();
    Godot.Collections.Array<String> equip_edit_array = new Godot.Collections.Array<String>();
    Godot.Collections.Array<int> initial_equip_id_array = new Godot.Collections.Array<int>();
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
        Godot.Collections.Dictionary eslotsDictionary = systemDictionary["slots"] as Godot.Collections.Dictionary;

        Godot.Collections.Dictionary etypeDictionary = dataDictionary["equip_types"] as Godot.Collections.Dictionary;
        GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").Clear();
        string equip_name;
        foreach (string equip in etypeDictionary.Keys)
        {
            string kind = equip[0].ToString();
            string type_id = etypeDictionary[equip].ToString();
            equip_id_array.Add(Convert.ToInt32(etypeDictionary[equip]));
            switch (kind)
            {
                case "w":
                    string w_id = equip.Remove(0, 1);
                    equip_name = "W: " + wtypeDictionary[type_id].ToString();
                    GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem(equip_name);
                    break;
                case "a":
                    string a_id = equip.Remove(0, 1);
                    equip_name = "A: " + atypeDictionary[type_id].ToString();
                    GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem(equip_name);
                    break;
            }

        }

        Godot.File weapon_editor = new Godot.File();
        weapon_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
        string weaponAsText = weapon_editor.GetAsText();
        JSONParseResult weaponParsed = JSON.Parse(weaponAsText);
        Godot.Collections.Dictionary weaponDictionary = weaponParsed.Result as Godot.Collections.Dictionary;

        Godot.File armor_editor = new Godot.File();
        armor_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
        string armorAsText = armor_editor.GetAsText();
        JSONParseResult armorParsed = JSON.Parse(armorAsText);
        Godot.Collections.Dictionary armorDictionary = armorParsed.Result as Godot.Collections.Dictionary;

        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/TypeList").Clear();
        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").Clear();
        Godot.Collections.Dictionary einitDictionary = dataDictionary["initial_equip"] as Godot.Collections.Dictionary;
        foreach (string equip in eslotsDictionary.Keys)
        {
            GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/TypeList").AddItem(eslotsDictionary[equip].ToString());
            string kind = equip[0].ToString();
            int kind_id = Convert.ToInt32(equip.Remove(0, 1));
            switch (kind)
            {
                case "w":
                    int w_id = Convert.ToInt32(einitDictionary[kind_id.ToString()]);
                    initial_equip_id_array.Add(w_id);
                    if (w_id >= 0)
                    {
                        Godot.Collections.Dictionary dataWeaponDictionary = weaponDictionary["weapon" + w_id] as Godot.Collections.Dictionary;
                        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").AddItem(dataWeaponDictionary["name"].ToString());
                    }
                    else
                    {
                        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").AddItem("None");
                    }
                    break;
                case "a":
                    int a_id = Convert.ToInt32(einitDictionary[kind_id.ToString()]);
                    initial_equip_id_array.Add(a_id);
                    if (a_id >= 0)
                    {
                        Godot.Collections.Dictionary dataArmorDictionary = armorDictionary["armor" + a_id] as Godot.Collections.Dictionary;
                        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").AddItem(dataArmorDictionary["name"].ToString());
                    }
                    else
                    {
                        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").AddItem("None");
                    }
                    break;
            }
        }
        system_editor.Close();
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
        weapon_editor.Close();
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
        Godot.Collections.Dictionary einit_data = new Godot.Collections.Dictionary();
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
        einit_data.Add("0", -1);
        einit_data.Add("1", -1);
        einit_data.Add("2", -1);
        einit_data.Add("3", -1);
        character_data.Add("initial_equip", einit_data);
        character_data.Add("equip_types", etype_data);
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
        Godot.Collections.Dictionary initial_equip_Data = finalData["initial_equip"] as Godot.Collections.Dictionary;

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

        int slot_items = GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").GetItemCount();
        for (int i = 0; i < slot_items; i++)
        {
            initial_equip_Data[i.ToString()] = Convert.ToInt32(initial_equip_id_array[i]);
        }

        finalData["initial_equip"] = initial_equip_Data;

        database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Write);
        database_editor.StoreString(JSON.Print(jsonDictionary));
        database_editor.Close();
    }

    private void _on_CharacterButton_item_selected(int id)
    {
        character_selected = id;
        _refresh_Data(id);
    }

    private void _on_CancelButton_pressed()
    {
        GetNode<WindowDialog>("EquipLabel/AddEquip").Hide();
    }

    private void _on_AddEquipTypeButton_pressed()
    {
        GetNode<WindowDialog>("EquipLabel/AddEquip").PopupCentered();

        Godot.File system_editor = new Godot.File();
        system_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = system_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary wtypeDictionary = systemDictionary["weapons"] as Godot.Collections.Dictionary;
        for (int i = 0; i < wtypeDictionary.Count; i++)
        {
            if (i > GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").AddItem(wtypeDictionary[i.ToString()].ToString());
            }
            else
            {
                GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").SetItemText(i, wtypeDictionary[i.ToString()].ToString());
            }
        }

        system_editor.Close();
    }

    private void _on_RemoveEquipTypeButton_pressed()
    {
        int selected = GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").GetSelectedItems()[0];
        GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").RemoveItem(selected);
    }

    private void _on_OkButton_pressed()
    {
        int kind = GetNode<OptionButton>("EquipLabel/AddEquip/TypeLabel/TypeButton").Selected;
        int item = GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").Selected;
        equip_id_array.Add(Convert.ToInt32(item));
        string item_text = GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").Text;
        switch (kind)
        {
            case 0:
                GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem("W: " + item_text);
                break;
            case 1:
                GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem("A: " + item_text);
                break;
        }
        GetNode<WindowDialog>("EquipLabel/AddEquip").Hide();
    }

    private void _on_TypeButton_item_selected(int index)
    {
        Godot.File system_editor = new Godot.File();
        system_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = system_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").Clear();
        switch (index)
        {
            case 0:
                Godot.Collections.Dictionary wtypeDictionary = systemDictionary["weapons"] as Godot.Collections.Dictionary;
                for (int i = 0; i < wtypeDictionary.Count; i++)
                {
                    if (i > GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").GetItemCount() - 1)
                    {
                        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").AddItem(wtypeDictionary[i.ToString()].ToString());
                    }
                    else
                    {
                        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").SetItemText(i, wtypeDictionary[i.ToString()].ToString());
                    }
                }
                break;
            case 1:
                Godot.Collections.Dictionary atypeDictionary = systemDictionary["armors"] as Godot.Collections.Dictionary;
                for (int i = 0; i < atypeDictionary.Count; i++)
                {
                    if (i > GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").GetItemCount() - 1)
                    {
                        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").AddItem(atypeDictionary[i.ToString()].ToString());
                    }
                    else
                    {
                        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").SetItemText(i, atypeDictionary[i.ToString()].ToString());
                    }
                }
                break;
        }
        system_editor.Close();
    }

    private void _on_EquipList_item_activated(int index)
    {
        initial_equip_edit = index;
        equip_edit_array.Add("-1");
        GetNode<Label>("InitialEquipLabel/InitialEquipChange/Label").Text = GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/TypeList").GetItemText(index);

        GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").Clear();
        GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").AddItem("None");
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary dataDictionary = jsonDictionary["chara" + character_selected] as Godot.Collections.Dictionary;

        Godot.File system_editor = new Godot.File();
        system_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = system_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary eslotsDictionary = systemDictionary["slots"] as Godot.Collections.Dictionary;

        Godot.Collections.Dictionary typesDictionary = dataDictionary["equip_types"] as Godot.Collections.Dictionary;
        if (eslotsDictionary.Contains("w" + index))
        {
            Godot.File weapon_editor = new Godot.File();
            weapon_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
            string weaponAsText = weapon_editor.GetAsText();
            JSONParseResult weaponParsed = JSON.Parse(weaponAsText);
            Godot.Collections.Dictionary weaponDictionary = weaponParsed.Result as Godot.Collections.Dictionary;

            Godot.Collections.Array<int> weapon_array = new Godot.Collections.Array<int>();
            foreach (string key in typesDictionary.Keys)
            {
                string kind = key[0].ToString();
                if (kind == "w")
                {
                    weapon_array.Add(Convert.ToInt32(typesDictionary[key]));
                }
            }

            foreach (string weapon in weaponDictionary.Keys)
            {
                Godot.Collections.Dictionary weapon_data = weaponDictionary[weapon] as Godot.Collections.Dictionary;
                if (weapon_array.Contains(Convert.ToInt32(weapon_data["weapon_type"])))
                {
                    equip_edit_array.Add(weapon.Remove(0,6));
                    GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").AddItem(weapon_data["name"].ToString());
                }
            }
            weapon_editor.Close();
        }
        else if (eslotsDictionary.Contains("a" + index))
        {
            Godot.File armor_editor = new Godot.File();
            armor_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
            string armorAsText = armor_editor.GetAsText();
            JSONParseResult armorParsed = JSON.Parse(armorAsText);
            Godot.Collections.Dictionary armorDictionary = armorParsed.Result as Godot.Collections.Dictionary;

            Godot.Collections.Array<int> armor_array = new Godot.Collections.Array<int>();
            foreach (string key in typesDictionary.Keys)
            {
                string kind = key[0].ToString();
                if (kind == "a")
                {
                    armor_array.Add(Convert.ToInt32(typesDictionary[key]));
                }
            }

            foreach (string armor in armorDictionary.Keys)
            {
                Godot.Collections.Dictionary armor_data = armorDictionary[armor] as Godot.Collections.Dictionary;
                if (armor_array.Contains(Convert.ToInt32(armor_data["armor_type"])))
                {
                    equip_edit_array.Add(armor.Remove(0,5));
                    GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").AddItem(armor_data["name"].ToString());
                }
            }
            armor_editor.Close();
        }
        database_editor.Close();
        system_editor.Close();
        GetNode<WindowDialog>("InitialEquipLabel/InitialEquipChange").PopupCentered();
    }

    private void _on_CancelInitialEquipButton_pressed()
    {
        initial_equip_edit = -1;
        equip_edit_array.Clear();
        GetNode<WindowDialog>("InitialEquipLabel/InitialEquipChange").Hide();
    }

    private void _on_OkInitialEquipButton_pressed()
    {
        string text = GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").Text;
        int selected = GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").Selected;
        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").SetItemText(initial_equip_edit, text);
        initial_equip_id_array[initial_equip_edit] = Convert.ToInt32(equip_edit_array[selected]);
        initial_equip_edit = -1;
        equip_edit_array.Clear();
        GetNode<WindowDialog>("InitialEquipLabel/InitialEquipChange").Hide();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
