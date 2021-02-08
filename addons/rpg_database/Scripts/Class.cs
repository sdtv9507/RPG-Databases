using Godot;
using System;

[Tool]
public class Class : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

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
            if (i == 0)
            {
                GetNode<OptionButton>("ClassButton").SetItemText(0, newClassDict["name"] as string);
            }
            if (jsonDictionary.Count > 1 && i > 0)
            {
                
                GetNode<OptionButton>("ClassButton").AddItem(newClassDict["name"] as string);
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
        GetNode<LineEdit>("NameLabel/NameText").Text = newClassDict["name"] as string;
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
