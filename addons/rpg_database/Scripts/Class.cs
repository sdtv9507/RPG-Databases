using Godot;
using System;

[Tool]
public class Class : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string icon_path = "";
    int class_selected = 0;
    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
		Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary newClassDict = jsonDictionary["class"+i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ClassButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ClassButton").AddItem(newClassDict["name"] as string);
            }else{
                GetNode<OptionButton>("ClassButton").SetItemText(i, newClassDict["name"] as string);
            }
        }
        database_editor.Close();
        _refresh_data(0);
    }

    public void _refresh_data(int id)
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newClassDict = jsonDictionary["class"+id] as Godot.Collections.Dictionary;

        Godot.File system_editor = new Godot.File();
		system_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
		string systemAsText = system_editor.GetAsText();
		JSONParseResult systemParsed = JSON.Parse(systemAsText);
		Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newSystemDict = systemDictionary["stats"] as Godot.Collections.Dictionary;
        
        GetNode<LineEdit>("NameLabel/NameText").Text = newClassDict["name"] as string;
        string icon = newClassDict["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(newClassDict["icon"] as string) as Godot.Texture;
        }
        GetNode<LineEdit>("ExpLabel/ExpText").Text = newClassDict["experience"] as string;
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").Clear();
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").Clear();
        for (int i = 0; i < newSystemDict.Count; i++)
        {
            string statName = newSystemDict[i.ToString()] as string;
            Godot.Collections.Dictionary classStatFormula = newClassDict["stat_list"] as Godot.Collections.Dictionary;
            string statFormula = "";
            if (classStatFormula.Contains(statName))
            {
                statFormula = classStatFormula[statName as string] as string;
            }else{
                statFormula = "level * 5";
            }
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").AddItem(statName);
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").AddItem(statFormula);
        }
        database_editor.Close();
        system_editor.Close();
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

    private void _on_ClassSaveButton_pressed()
    {
        _save_class_data();
        _refresh_data(class_selected);
    }

    private void _save_class_data()
    {
		Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary finalData = jsonDictionary["class"+class_selected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classStatFormula = finalData["stat_list"] as Godot.Collections.Dictionary;
        
		finalData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		GetNode<OptionButton>("ClassButton").SetItemText(class_selected, GetNode<LineEdit>("NameLabel/NameText").Text);
		finalData["icon"] = icon_path;
        finalData["experience"] = GetNode<LineEdit>("ExpLabel/ExpText").Text;
        int items = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").GetItemCount();
        for (int i = 0; i < items; i++)
        {
            string stat = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").GetItemText(i);
            string formula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").GetItemText(i);
            classStatFormula[stat] = formula;
        }
        database_editor.Close();
        finalData["skill_list"] = "";
        database_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }
    private void _on_AddClass_pressed()
    {
        GetNode<OptionButton>("ClassButton").AddItem("NewClass");
        int id = GetNode<OptionButton>("ClassButton").GetItemCount() - 1;
		Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
		Godot.Collections.Dictionary class_data = new Godot.Collections.Dictionary();
		class_data.Add("name", "NewClass");
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
		jsonDictionary.Add("class" + id, class_data);
		database_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_ClassButton_item_selected(int id)
    {
        class_selected = id;
        _refresh_data(id);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
