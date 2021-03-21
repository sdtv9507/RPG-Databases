using Godot;
using System;

[Tool]
public class Skill : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string icon_path = "";
    int skill_selected = 0;
    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        
		database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
		string systemAsText = database_editor.GetAsText();
		JSONParseResult systemParsed = JSON.Parse(systemAsText);
		Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary newSkillDict = jsonDictionary["skill"+i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("SkillButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("SkillButton").AddItem(newSkillDict["name"] as string);
            }else{
                GetNode<OptionButton>("SkillButton").SetItemText(i, newSkillDict["name"] as string);
            }
        }

        Godot.Collections.Dictionary newSystemDict = systemDictionary["elements"] as Godot.Collections.Dictionary;
        for (int i = 0; i < newSystemDict.Count; i++)
        {
            if (i > GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").AddItem(newSystemDict[i.ToString()] as string);
            }else{
                GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").SetItemText(i, newSystemDict[i.ToString()] as string);
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
        string icon = newSkillDict["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(newSkillDict["icon"] as string) as Godot.Texture;
        }
        GetNode<TextEdit>("DescLabel/DescText").Text = newSkillDict["description"] as string;
        GetNode<OptionButton>("SkillTypeLabel/SkillTypeButton").Selected = Convert.ToInt32(newSkillDict["skill_type"]);
        GetNode<SpinBox>("MPCostLabel/MPCostBox").Value = Convert.ToInt32(newSkillDict["mp_cost"]);
        GetNode<SpinBox>("TPCostLabel/TPCostBox").Value = Convert.ToInt32(newSkillDict["tp_cost"]);
        GetNode<OptionButton>("TargetLabel/TargetButton").Selected = Convert.ToInt32(newSkillDict["target"]);
        GetNode<OptionButton>("UsableLabel/UsableButton").Selected = Convert.ToInt32(newSkillDict["usable"]);
        GetNode<SpinBox>("HitLabel/HitBox").Value = Convert.ToInt32(newSkillDict["success"]);
        GetNode<OptionButton>("TypeLabel/TypeButton").Selected = Convert.ToInt32(newSkillDict["hit_type"]);
        GetNode<OptionButton>("DamageLabel/DTypeLabel/DTypeButton").Selected = Convert.ToInt32(newSkillDict["damage_type"]);
        GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").Selected = Convert.ToInt32(newSkillDict["element"]);
        GetNode<LineEdit>("DamageLabel/DFormulaLabel/FormulaText").Text = newSkillDict["formula"] as string;
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

    private void _on_AddSkill_pressed()
    {
        GetNode<OptionButton>("SkillButton").AddItem("NewSkill");
        int id = GetNode<OptionButton>("SkillButton").GetItemCount() - 1;
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
		Godot.Collections.Dictionary skill_data = new Godot.Collections.Dictionary();
		skill_data.Add("name", "NewSkill");
		skill_data.Add("icon", "");
		skill_data.Add("description", "New created skill");
		skill_data.Add("skill_type", 0);
		skill_data.Add("mp_cost", 1);
		skill_data.Add("tp_cost", 1);
		skill_data.Add("target", 1);
		skill_data.Add("usable", 1);
		skill_data.Add("success", 95);
		skill_data.Add("hit_type", 1);
		skill_data.Add("damage_type", 1);
		skill_data.Add("element", 0);
		skill_data.Add("formula", "atk * 4 - def * 2");
		jsonDictionary.Add("skill" + id, skill_data);
		database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Write);
		database_editor.StoreLine(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_SkillSaveButton_pressed()
    {
        _save_skill_data();
        _refresh_data(skill_selected);
    }
    private void _save_skill_data()
    {
		Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary finalData = jsonDictionary["skill"+skill_selected] as Godot.Collections.Dictionary;
		finalData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		GetNode<OptionButton>("SkillButton").SetItemText(skill_selected, GetNode<LineEdit>("NameLabel/NameText").Text);
		finalData["icon"] = icon_path;
		finalData["description"] = GetNode<TextEdit>("DescLabel/DescText").Text;
        finalData["skill_type"] = GetNode<OptionButton>("SkillTypeLabel/SkillTypeButton").Selected;
		finalData["mp_cost"] = GetNode<SpinBox>("MPCostLabel/MPCostBox").Value;
		finalData["tp_cost"] = GetNode<SpinBox>("TPCostLabel/TPCostBox").Value;
		finalData["target"] = GetNode<OptionButton>("TargetLabel/TargetButton").Selected;
		finalData["usable"] = GetNode<OptionButton>("UsableLabel/UsableButton").Selected;
		finalData["success"] = GetNode<SpinBox>("HitLabel/HitBox").Value;
		finalData["hit_type"] = GetNode<OptionButton>("TypeLabel/TypeButton").Selected;
		finalData["damage_type"] = GetNode<OptionButton>("DamageLabel/DTypeLabel/DTypeButton").Selected;
		finalData["element"] = GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").Selected;
		finalData["formula"] = GetNode<LineEdit>("DamageLabel/DFormulaLabel/FormulaText").Text;
        database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_SkillButton_item_selected(int id)
    {
        skill_selected = id;
        _refresh_data(id);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
