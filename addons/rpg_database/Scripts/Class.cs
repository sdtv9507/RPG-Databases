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
            if (i > GetNode<OptionButton>("ClassButton").GetItemCount())
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
        GetNode<LineEdit>("NameLabel/NameText").Text = newClassDict["name"] as string;
        string icon = newClassDict["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(newClassDict["icon"] as string) as Godot.Texture;
        }
        GetNode<LineEdit>("ExpLabel/ExpText").Text = newClassDict["experience"] as string;
        GetNode<LineEdit>("HPLabel/HPText").Text = newClassDict["hp"] as string;
        GetNode<LineEdit>("MPLabel/MPText").Text = newClassDict["mp"] as string;
        GetNode<LineEdit>("ATKLabel/ATKText").Text = newClassDict["atk"] as string;
        GetNode<LineEdit>("DEFLabel/DEFText").Text = newClassDict["def"] as string;
        GetNode<LineEdit>("INTLabel/INTText").Text = newClassDict["int"] as string;
        GetNode<LineEdit>("RESLabel/RESText").Text = newClassDict["res"] as string;
        GetNode<LineEdit>("SPDLabel/SPDText").Text = newClassDict["spd"] as string;
        GetNode<LineEdit>("LUKLabel/LUKText").Text = newClassDict["luk"] as string;
        database_editor.Close();
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
		finalData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		GetNode<OptionButton>("ClassButton").SetItemText(class_selected, GetNode<LineEdit>("NameLabel/NameText").Text);
		finalData["icon"] = icon_path;
        finalData["experience"] = GetNode<LineEdit>("ExpLabel/ExpText").Text;
        finalData["hp"] = GetNode<LineEdit>("HPLabel/HPText").Text;
        finalData["mp"] = GetNode<LineEdit>("MPLabel/MPText").Text;
        finalData["atk"] = GetNode<LineEdit>("ATKLabel/ATKText").Text;
        finalData["def"] = GetNode<LineEdit>("DEFLabel/DEFText").Text;
        finalData["int"] = GetNode<LineEdit>("INTLabel/INTText").Text;
        finalData["res"] = GetNode<LineEdit>("RESLabel/RESText").Text;
        finalData["res"] = GetNode<LineEdit>("SPDLabel/SPDText").Text;
        finalData["res"] = GetNode<LineEdit>("LUKLabel/LUKText").Text;
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
