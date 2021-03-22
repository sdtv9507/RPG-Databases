using Godot;
using System;

public class Armor : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    string icon_path = "";
    int armor_selected = 0;
    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;

        database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = database_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;

        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary newArmorDict = jsonDictionary["armor" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ArmorButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ArmorButton").AddItem(newArmorDict["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("ArmorButton").SetItemText(i, newArmorDict["name"] as string);
            }
        }

        Godot.Collections.Dictionary newSystemDict = systemDictionary["armors"] as Godot.Collections.Dictionary;
        for (int i = 0; i < newSystemDict.Count; i++)
        {
            if (i > GetNode<OptionButton>("ATypeLabel/ATypeButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ATypeLabel/ATypeButton").AddItem(newSystemDict[i.ToString()] as string);
            }
            else
            {
                GetNode<OptionButton>("ATypeLabel/ATypeButton").SetItemText(i, newSystemDict[i.ToString()] as string);
            }
        }
        database_editor.Close();
        _refresh_data(0);
    }

    private void _refresh_data(int id)
    {
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
        string jsonAsText = database_editor.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newArmorDict = jsonDictionary["armor" + id] as Godot.Collections.Dictionary;

        Godot.File system_editor = new Godot.File();
        system_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = system_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newSystemDict = systemDictionary["stats"] as Godot.Collections.Dictionary;

        GetNode<LineEdit>("NameLabel/NameText").Text = newArmorDict["name"] as string;
        string icon = newArmorDict["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(newArmorDict["icon"] as string) as Godot.Texture;
        }
        GetNode<TextEdit>("DescLabel/DescText").Text = newArmorDict["description"] as string;
        GetNode<OptionButton>("ATypeLabel/ATypeButton").Selected = Convert.ToInt32(newArmorDict["armor_type"]);
        GetNode<SpinBox>("PriceLabel/PriceSpin").Value = Convert.ToInt32(newArmorDict["price"]);

        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").Clear();
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").Clear();
        for (int i = 0; i < newSystemDict.Count; i++)
        {
            string statName = newSystemDict[i.ToString()] as string;
            Godot.Collections.Dictionary armorStatFormula = newArmorDict["stat_list"] as Godot.Collections.Dictionary;
            string statFormula = "";
            if (armorStatFormula.Contains(statName))
            {
                statFormula = armorStatFormula[statName as string] as string;
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

    private void _on_AddArmorButton_pressed()
    {
        GetNode<OptionButton>("ArmorButton").AddItem("NewArmor");
        int id = GetNode<OptionButton>("ArmorButton").GetItemCount() - 1;
        Godot.File database_editor = new Godot.File();
        database_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armor_data = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary armor_stats_array = new Godot.Collections.Dictionary();
        armor_data.Add("name", "NewArmor");
        armor_data.Add("icon", "");
        armor_data.Add("description", "New created armor");
        armor_data.Add("armor_type", 0);
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
        jsonDictionary.Add("armor" + id, armor_data);
		database_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Write);
		database_editor.StoreLine(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_ArmorSaveButton_pressed()
    {
        _save_armor_data();
        _refresh_data(armor_selected);
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

    private void _on_ArmorButton_item_selected(int id)
    {
        armor_selected = id;
        _refresh_data(id);
    }

    private void _save_armor_data()
    {
		Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary finalData = jsonDictionary["armor"+armor_selected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armorStatFormula = finalData["stat_list"] as Godot.Collections.Dictionary;
		finalData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		GetNode<OptionButton>("ArmorButton").SetItemText(armor_selected, GetNode<LineEdit>("NameLabel/NameText").Text);
		finalData["icon"] = icon_path;
		finalData["description"] = GetNode<TextEdit>("DescLabel/DescText").Text;
        finalData["armor_type"] = GetNode<OptionButton>("ATypeLabel/ATypeButton").Selected;
		finalData["price"] = GetNode<SpinBox>("PriceLabel/PriceSpin").Value;
        int items = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").GetItemCount();
        for (int i = 0; i < items; i++)
        {
            string stat = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").GetItemText(i);
            string formula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").GetItemText(i);
            armorStatFormula[stat] = formula;
        }
        database_editor.Open("res://databases/Armor.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
