using Godot;
using System;

[Tool]
public class Class : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string iconPath = "";
    int classSelected = 0;
    int statEdit = -1;
    Godot.Collections.Array<String> skillListArray = new Godot.Collections.Array<String>();
    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;

        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary classData = jsonDictionary["class" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ClassButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ClassButton").AddItem(classData["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("ClassButton").SetItemText(i, classData["name"] as string);
            }
        }
        databaseFile.Close();
        _refresh_data(0);
    }

    public void _refresh_data(int id)
    {
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classData = jsonDictionary["class" + id] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classSkillList = classData["skill_list"] as Godot.Collections.Dictionary;

        databaseFile.Close();
        databaseFile.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        jsonAsText = databaseFile.GetAsText();
        jsonParsed = JSON.Parse(jsonAsText);
        jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary systemStatsData = jsonDictionary["stats"] as Godot.Collections.Dictionary;

        databaseFile.Close();
        databaseFile.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
        jsonAsText = databaseFile.GetAsText();
        jsonParsed = JSON.Parse(jsonAsText);
        jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;

        GetNode<LineEdit>("NameLabel/NameText").Text = classData["name"] as string;
        string icon = classData["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(classData["icon"] as string) as Godot.Texture;
        }
        GetNode<LineEdit>("ExpLabel/ExpText").Text = classData["experience"] as string;

        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").Clear();
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").Clear();
        for (int i = 0; i < systemStatsData.Count; i++)
        {
            string statName = systemStatsData[i.ToString()] as string;
            Godot.Collections.Dictionary classStatFormula = systemStatsData["stat_list"] as Godot.Collections.Dictionary;
            string statFormula = "";
            if (classStatFormula.Contains(statName))
            {
                statFormula = classStatFormula[statName].ToString();
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
        foreach (string element in classSkillList.Keys)
        {
            skillListArray.Add(element);
            Godot.Collections.Dictionary skillData = jsonDictionary["skill" + element] as Godot.Collections.Dictionary;
            string skillName = skillData["name"] as string;
            GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").AddItem(skillName);
            string level = Convert.ToString(classSkillList[element as string]);
            GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").AddItem(level);
        }

        foreach (string element in jsonDictionary.Keys)
        {
            Godot.Collections.Dictionary skillData = jsonDictionary[element] as Godot.Collections.Dictionary;
            string name = Convert.ToString(skillData["name"]);
            GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").AddItem(name);
            GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").Select(0);
        }

        databaseFile.Close();
    }

    private void _on_Search_pressed()
    {
        GetNode<FileDialog>("IconLabel/IconSearch").PopupCentered();
    }

    private void _on_IconSearch_file_selected(string path)
    {
        iconPath = path;
        GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(path) as Godot.Texture;
    }

    private void _on_ClassSaveButton_pressed()
    {
        _save_class_data();
        _refresh_data(classSelected);
    }

    private void _save_class_data()
    {
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        databaseFile.Close();
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classData = jsonDictionary["class" + classSelected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classStatFormula = classData["stat_list"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classSkillList = classData["skill_list"] as Godot.Collections.Dictionary;

        classData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
        GetNode<OptionButton>("ClassButton").SetItemText(classSelected, GetNode<LineEdit>("NameLabel/NameText").Text);
        classData["icon"] = iconPath;
        classData["experience"] = GetNode<LineEdit>("ExpLabel/ExpText").Text;
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
            string skill = Convert.ToString(skillListArray[i]);
            int level = Convert.ToInt32(GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").GetItemText(i));
            classSkillList[skill] = level;
        }
        databaseFile.Close();
        classData["stat_list"] = classStatFormula;
        classData["skill_list"] = classSkillList;
        databaseFile.Open("res://databases/Class.json", Godot.File.ModeFlags.Write);
        databaseFile.StoreString(JSON.Print(jsonDictionary));
        databaseFile.Close();
    }
    private void _on_AddClass_pressed()
    {
        GetNode<OptionButton>("ClassButton").AddItem("NewClass");
        int id = GetNode<OptionButton>("ClassButton").GetItemCount() - 1;
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        databaseFile.Close();
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classData = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary statData = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary skillData = new Godot.Collections.Dictionary();
        classData.Add("name", "NewClass");
        classData.Add("icon", "");
        classData.Add("experience", "level * 30");
        statData.Add("hp", "level * 25 + 10");
        statData.Add("mp", "level * 15 + 5");
        statData.Add("atk", "level * 5 + 3");
        statData.Add("def", "level * 5 + 3");
        statData.Add("int", "level * 5 + 3");
        statData.Add("res", "level * 5 + 3");
        statData.Add("spd", "level * 5 + 3");
        statData.Add("luk", "level * 5 + 3");
        classData.Add("stat_list", statData);
        skillData.Add(0, 5);
        classData.Add("skill_list", skillData);
        jsonDictionary.Add("class" + id, classData);
        databaseFile.Open("res://databases/Class.json", Godot.File.ModeFlags.Write);
        databaseFile.StoreString(JSON.Print(jsonDictionary));
        databaseFile.Close();
    }

    private void _on_ClassButton_item_selected(int id)
    {
        classSelected = id;
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
        skillListArray.RemoveAt(selected);
    }

    private void _on_OkButton_pressed()
    {
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Skill.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;

        int skill = GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").GetSelectedId();
        int level = Convert.ToInt32(GetNode<SpinBox>("SkillLabel/AddSkill/LevelLabel/LevelSpin").Value);
        skillListArray.Add(Convert.ToString(skill));
        Godot.Collections.Dictionary skillData = jsonDictionary["skill" + skill] as Godot.Collections.Dictionary;
        string skillName = skillData["name"] as string;
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").AddItem(skillName);
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").AddItem(Convert.ToString(level));

        databaseFile.Close();
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
        string statName = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatsListContainer/StatsList").GetItemText(index);
        string statFormula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").GetItemText(index);
        GetNode<Label>("StatEditor/StatLabel").Text = statName;
        GetNode<LineEdit>("StatEditor/StatEdit").Text = statFormula;
        statEdit = index;
        GetNode<WindowDialog>("StatEditor").Show();
    }

    private void _on_OkStatButton_pressed()
    {
        string statFormula = GetNode<LineEdit>("StatEditor/StatEdit").Text;
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaListContainer/FormulaList").SetItemText(statEdit, statFormula);
        _save_class_data();
        statEdit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }

    private void _on_CancelStatButton_pressed()
    {
        statEdit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}