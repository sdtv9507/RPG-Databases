using Godot;
using System;

[Tool]
public class Skill : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary newSkillDict = jsonDictionary["skill"+i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("SkillButton").GetItemCount())
            {
                GetNode<OptionButton>("SkillButton").AddItem(newSkillDict["name"] as string);
            }else{
                GetNode<OptionButton>("SkillButton").SetItemText(i, newSkillDict["name"] as string);
            }
        }
        database_editor.Close();
        _refresh_data(0);
    }

    private void _refresh_data(int id)
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newSkillDict = jsonDictionary["skill"+id] as Godot.Collections.Dictionary;
        GetNode<LineEdit>("NameLabel/NameText").Text = newSkillDict["name"] as string;
        database_editor.Close();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
