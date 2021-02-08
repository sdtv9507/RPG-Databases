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
			Godot.Collections.Dictionary newCharDict = jsonDictionary["chara"+i] as Godot.Collections.Dictionary;
			if (i == 0)
			{
				GetNode<OptionButton>("CharacterButton").SetItemText(0, newCharDict["name"] as string);
			}
            if (jsonDictionary.Count > 1 && i > 0)
            {
                GetNode<OptionButton>("CharacterButton").AddItem(newCharDict["name"] as string);
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
        Godot.Collections.Dictionary dataDictionary = jsonDictionary["chara"+id] as Godot.Collections.Dictionary;
		GetNode<OptionButton>("CharacterButton").SetItemText(0, dataDictionary["name"] as string);
		GetNode<LineEdit>("NameLabel/NameText").Text = dataDictionary["name"] as string;
		GetNode<SpinBox>("InitLevelLabel/InitLevelText").Value = Convert.ToInt32(dataDictionary["initialLevel"]);
		GetNode<SpinBox>("MaxLevelLabel/MaxLevelText").Value = Convert.ToInt32(dataDictionary["maxLevel"]);
        database_editor.Close();

		Godot.File class_editor = new Godot.File();
		class_editor.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
		string classAsText = class_editor.GetAsText();
		JSONParseResult classParsed = JSON.Parse(classAsText);
		Godot.Collections.Dictionary classDictionary = classParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary dataClassDictionary = classDictionary["class0"] as Godot.Collections.Dictionary;
        GetNode<OptionButton>("ClassLabel/ClassText").SetItemText(0, dataClassDictionary["name"] as string);
        class_editor.Close();
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
		character_data.Add("faceImage", "res://");
		character_data.Add("charaImage", "res://");
		character_data.Add("name", "NewCharacter");
		character_data.Add("class", 0);
		character_data.Add("initialLevel", 1);
		character_data.Add("maxLevel", 99);
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
        Godot.Collections.Dictionary finalData = jsonDictionary["chara"+character_selected] as Godot.Collections.Dictionary;
		finalData["faceImage"] = facePath;
		finalData["charaImage"] = "";
		finalData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		finalData["class"] = GetNode<OptionButton>("ClassLabel/ClassText").Selected;
		finalData["initialLevel"] = GetNode<SpinBox>("InitLevelLabel/InitLevelText").Value;
		finalData["maxLevel"] = GetNode<SpinBox>("MaxLevelLabel/MaxLevelText").Value;
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
