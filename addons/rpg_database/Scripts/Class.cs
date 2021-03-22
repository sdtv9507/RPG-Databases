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
    int stat_edit = -1;
    Godot.Collections.Array<String> skill_list_array = new Godot.Collections.Array<String>();
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
            Godot.Collections.Dictionary newClassDict = jsonDictionary["class" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ClassButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ClassButton").AddItem(newClassDict["name"] as string);
            }
            else
            {
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
        Godot.Collections.Dictionary newClassDict = jsonDictionary["class" + id] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newSkillList = newClassDict["skill_list"] as Godot.Collections.Dictionary;

        Godot.File system_editor = new Godot.File();
        system_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string systemAsText = system_editor.GetAsText();
        JSONParseResult systemParsed = JSON.Parse(systemAsText);
        Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newSystemDict = systemDictionary["stats"] as Godot.Collections.Dictionary;

        Godot.File skill_editor = new Godot.File();
        skill_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
        string skillAsText = skill_editor.GetAsText();
        JSONParseResult skillParsed = JSON.Parse(skillAsText);
        Godot.Collections.Dictionary skillDictionary = skillParsed.Result as Godot.Collections.Dictionary;

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
            }
            else
            {
                statFormula = "level * 5";
            }
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").AddItem(statName);
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").AddItem(statFormula);
        }

        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").Clear();
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").Clear();
        foreach (string element in newSkillList.Keys)
        {
            skill_list_array.Add(element);
            Godot.Collections.Dictionary skillData = skillDictionary["skill" + element] as Godot.Collections.Dictionary;
            string skillName = skillData["name"] as string;
            GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").AddItem(skillName);
            string level = Convert.ToString(newSkillList[element as string]);
            GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").AddItem(level);
        }

        foreach (string element in skillDictionary.Keys)
        {
            Godot.Collections.Dictionary skillData = skillDictionary[element] as Godot.Collections.Dictionary;
            string name = Convert.ToString(skillData["name"]);
            GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").AddItem(name);
            GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").Select(0);
        }

        database_editor.Close();
        system_editor.Close();
        skill_editor.Close();
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
        Godot.Collections.Dictionary finalData = jsonDictionary["class" + class_selected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classStatFormula = finalData["stat_list"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary skillList = finalData["skill_list"] as Godot.Collections.Dictionary;

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
        int skills_count = GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").GetItemCount();
        for (int i = 0; i < skills_count; i++)
        {
            string skill = Convert.ToString(skill_list_array[i]);
            int level = Convert.ToInt32(GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").GetItemText(i));
            skillList[skill] = level;
        }
        database_editor.Close();
        finalData["stat_list"] = classStatFormula;
        finalData["skill_list"] = skillList;
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
        Godot.Collections.Dictionary stat_data = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary skill_data = new Godot.Collections.Dictionary();
        class_data.Add("name", "NewClass");
        class_data.Add("icon", "");
        class_data.Add("experience", "level * 30");
        stat_data.Add("hp", "level * 25 + 10");
        stat_data.Add("mp", "level * 15 + 5");
        stat_data.Add("atk", "level * 5 + 3");
        stat_data.Add("def", "level * 5 + 3");
        stat_data.Add("int", "level * 5 + 3");
        stat_data.Add("res", "level * 5 + 3");
        stat_data.Add("spd", "level * 5 + 3");
        stat_data.Add("luk", "level * 5 + 3");
        class_data.Add("stat_list", stat_data);
        skill_data.Add(0, 5);
        class_data.Add("skill_list", skill_data);
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

    private void _on_AddSkillButton_pressed()
    {
        GetNode<WindowDialog>("SkillLabel/AddSkill").PopupCentered();
    }

    private void _on_RemoveButton_pressed()
    {
        int selected = GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").GetSelectedItems()[0];
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").RemoveItem(selected);
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").RemoveItem(selected);
        skill_list_array.RemoveAt(selected);
    }

    private void _on_OkButton_pressed()
    {
        Godot.File database_editor = new Godot.File();
        Godot.File skill_editor = new Godot.File();
        database_editor.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
        string skillAsText = database_editor.GetAsText();
        JSONParseResult skillParsed = JSON.Parse(skillAsText);
        Godot.Collections.Dictionary skillDictionary = skillParsed.Result as Godot.Collections.Dictionary;

        int skill = GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").GetSelectedId();
        int level = Convert.ToInt32(GetNode<SpinBox>("SkillLabel/AddSkill/LevelLabel/LevelSpin").Value);
        skill_list_array.Add(Convert.ToString(skill));
        Godot.Collections.Dictionary skillData = skillDictionary["skill" + skill] as Godot.Collections.Dictionary;
        string skillName = skillData["name"] as string;
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").AddItem(skillName);
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").AddItem(Convert.ToString(level));

        database_editor.Close();
        GetNode<WindowDialog>("SkillLabel/AddSkill").Hide();
    }

    private void _on_CancelButton_pressed()
    {
        GetNode<WindowDialog>("SkillLabel/AddSkill").Hide();
    }

    private void _on_StatsList_item_selected(int index)
    {
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").Select(index);
    }

    private void _on_FormulaList_item_selected(int index)
    {
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").Select(index);
    }

    private void _on_SkillList_item_selected(int index)
    {
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").Select(index);
    }

    private void _on_SkillLevelList_item_selected(int index)
    {
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").Select(index);
    }

    private void _on_FormulaList_item_activated(int index)
    {
        string stat_name = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").GetItemText(index);
        string stat_formula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").GetItemText(index);
        GetNode<Label>("StatEditor/StatLabel").Text = stat_name;
        GetNode<LineEdit>("StatEditor/StatEdit").Text = stat_formula;
        stat_edit = index;
        GetNode<WindowDialog>("StatEditor").Show();
    }

    private void _on_OkStatButton_pressed()
    {
        string stat_formula = GetNode<LineEdit>("StatEditor/StatEdit").Text;
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").SetItemText(stat_edit, stat_formula);
        _save_class_data();
        stat_edit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }

    private void _on_CancelStatButton_pressed()
    {
        stat_edit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}