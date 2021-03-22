using Godot;
using System;

public class Weapon : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    string icon_path = "";
    int weapon_selected = 0;
    int stat_edit = -1;
    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;

        database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = database_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;

        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary newWeaponDict = jsonDictionary["weapon" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("WeaponButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("WeaponButton").AddItem(newWeaponDict["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("WeaponButton").SetItemText(i, newWeaponDict["name"] as string);
            }
        }

        Godot.Collections.Dictionary newSystemDict = systemDictionary["elements"] as Godot.Collections.Dictionary;
        for (int i = 0; i < newSystemDict.Count; i++)
        {
            if (i > GetNode<OptionButton>("ElementLabel/ElementButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ElementLabel/ElementButton").AddItem(newSystemDict[i.ToString()] as string);
            }
            else
            {
                GetNode<OptionButton>("ElementLabel/ElementButton").SetItemText(i, newSystemDict[i.ToString()] as string);
            }
        }

        newSystemDict = systemDictionary["weapons"] as Godot.Collections.Dictionary;
        for (int i = 0; i < newSystemDict.Count; i++)
        {
            if (i > GetNode<OptionButton>("WTypeLabel/WTypeButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("WTypeLabel/WTypeButton").AddItem(newSystemDict[i.ToString()] as string);
            }
            else
            {
                GetNode<OptionButton>("WTypeLabel/WTypeButton").SetItemText(i, newSystemDict[i.ToString()] as string);
            }
        }
        database_editor.Close();
        _refresh_data(0);
    }

    private void _refresh_data(int id)
    {
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newWeaponDict = jsonDictionary["weapon" + id] as Godot.Collections.Dictionary;

        Godot.File system_editor = new Godot.File();
        system_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = system_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newSystemDict = systemDictionary["stats"] as Godot.Collections.Dictionary;

        GetNode<LineEdit>("NameLabel/NameText").Text = newWeaponDict["name"] as string;
        string icon = newWeaponDict["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(newWeaponDict["icon"] as string) as Godot.Texture;
        }
        GetNode<TextEdit>("DescLabel/DescText").Text = newWeaponDict["description"] as string;
        GetNode<OptionButton>("WTypeLabel/WTypeButton").Selected = Convert.ToInt32(newWeaponDict["weapon_type"]);
        GetNode<SpinBox>("PriceLabel/PriceSpin").Value = Convert.ToInt32(newWeaponDict["price"]);
        GetNode<OptionButton>("ElementLabel/ElementButton").Selected = Convert.ToInt32(newWeaponDict["element"]);

        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").Clear();
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").Clear();
        for (int i = 0; i < newSystemDict.Count; i++)
        {
            string statName = newSystemDict[i.ToString()] as string;
            Godot.Collections.Dictionary weaponStatFormula = newWeaponDict["stat_list"] as Godot.Collections.Dictionary;
            string statFormula = "";
            if (weaponStatFormula.Contains(statName))
            {
                statFormula = weaponStatFormula[statName as string] as string;
            }
            else
            {
                statFormula = "0";
            }
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").AddItem(statName);
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").AddItem(statFormula);
        }

        database_editor.Close();
        system_editor.Close();
    }

    private void _on_AddWeaponButton_pressed()
    {
        GetNode<OptionButton>("WeaponButton").AddItem("NewWeapon");
        int id = GetNode<OptionButton>("WeaponButton").GetItemCount() - 1;
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary weapon_data = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary weapon_stats_array = new Godot.Collections.Dictionary();
        weapon_data.Add("name", "NewWeapon");
        weapon_data.Add("icon", "");
        weapon_data.Add("description", "New created weapon");
        weapon_data.Add("weapon_type", 0);
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
        jsonDictionary.Add("weapon" + id, weapon_data);
		database_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Write);
		database_editor.StoreLine(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_WeaponSaveButton_pressed()
    {
        _save_weapon_data();
        _refresh_data(weapon_selected);
    }

    private void _on_Search_pressed()
    {
        GetNode<FileDialog>("IconLabel/IconSearch").PopupCentered();
    }

    private void _on_IconSearch_file_selected(string path)
    {
        icon_path = path;
        GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(path) as Godot.Texture;
    }

    private void _on_WeaponButton_item_selected(int id)
    {
        weapon_selected = id;
        _refresh_data(id);
    }

    private void _save_weapon_data()
    {
		Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary finalData = jsonDictionary["weapon"+weapon_selected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary weaponStatFormula = finalData["stat_list"] as Godot.Collections.Dictionary;
		finalData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		GetNode<OptionButton>("WeaponButton").SetItemText(weapon_selected, GetNode<LineEdit>("NameLabel/NameText").Text);
		finalData["icon"] = icon_path;
		finalData["description"] = GetNode<TextEdit>("DescLabel/DescText").Text;
        finalData["weapon_type"] = GetNode<OptionButton>("WTypeLabel/WTypeButton").Selected;
		finalData["price"] = GetNode<SpinBox>("PriceLabel/PriceSpin").Value;
		finalData["element"] = GetNode<OptionButton>("ElementLabel/ElementButton").Selected;
        int items = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").GetItemCount();
        for (int i = 0; i < items; i++)
        {
            string stat = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").GetItemText(i);
            string formula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").GetItemText(i);
            weaponStatFormula[stat] = formula;
        }
        database_editor.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_StatValueList_item_activated(int index)
    {
        string stat_name = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").GetItemText(index);
        string stat_formula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").GetItemText(index);
        GetNode<Label>("StatEditor/StatLabel").Text = stat_name;
        GetNode<LineEdit>("StatEditor/StatEdit").Text = stat_formula;
        stat_edit = index;
        GetNode<WindowDialog>("StatEditor").Show();
    }

    private void _on_OkButton_pressed()
    {
        string stat_formula = GetNode<LineEdit>("StatEditor/StatEdit").Text;
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").SetItemText(stat_edit, stat_formula);
        _save_weapon_data();
        stat_edit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }

    private void _on_CancelButton_pressed()
    {
        stat_edit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
