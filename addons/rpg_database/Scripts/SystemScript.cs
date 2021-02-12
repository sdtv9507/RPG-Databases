using Godot;
using System;

[Tool]
public class SystemScript : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary finalData = jsonDictionary["stats"] as Godot.Collections.Dictionary;

        for (int i = 0; i < finalData.Count; i++)
        {
            ItemList item = GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList");
            item.AddItem((finalData[i.ToString()]).ToString());
        }
    }

    public void _save_Stats()
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary finalData = jsonDictionary["stats"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary stats_data = new Godot.Collections.Dictionary();

        int statSize = GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").GetItemCount();
        for (int i = 0; i < statSize; i++)
        {
            String text = GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").GetItemText(i);
            //stats_data[i.ToString()] = text;
            stats_data.Add(i.ToString(), text);
        }
        jsonDictionary["stats"] = stats_data;
        database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    public void _on_AddStat_pressed()
    {
        GetNode<WindowDialog>("StatsLabel/AddStat").PopupCentered();
    }

    public void _on_RemoveStat_pressed()
    {
        int index = GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").GetSelectedItems()[0];
        if (index > -1)
        {
            GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").RemoveItem(index);
            _save_Stats();
        }
    }
    public void _on_OKButton_pressed()
    {
        string statName = GetNode<LineEdit>("StatsLabel/AddStat/StatName").Text;
        if (statName != "")
        {
            GetNode<ItemList>("StatsLabel/StatsContainer/StatsBoxContainer/StatsList").AddItem(statName);
            _save_Stats();
        }
        GetNode<WindowDialog>("StatsLabel/AddStat").Hide();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
