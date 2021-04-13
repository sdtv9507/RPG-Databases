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
    public void Start()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Class") as Godot.Collections.Dictionary;

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
        RefreshData(classSelected);
    }

    public void RefreshData(int id)
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Class") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classData = jsonDictionary["class" + id] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classSkillList = classData["skill_list"] as Godot.Collections.Dictionary;

        jsonDictionary = this.GetParent().GetParent().Call("ReadData", "System") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary systemStatsData = jsonDictionary["stats"] as Godot.Collections.Dictionary;

        jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Skill") as Godot.Collections.Dictionary;

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
            Godot.Collections.Dictionary classStatFormula = classData["stat_list"] as Godot.Collections.Dictionary;
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
        skillListArray.Clear();
        foreach (string element in classSkillList.Keys)
        {
            skillListArray.Add(element);
            Godot.Collections.Dictionary skillData = jsonDictionary["skill" + element] as Godot.Collections.Dictionary;
            string skillName = skillData["name"] as string;
            GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").AddItem(skillName);
            string level = Convert.ToString(classSkillList[element as string]);
            GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").AddItem(level);
        }

        GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").Clear();
        foreach (string element in jsonDictionary.Keys)
        {
            Godot.Collections.Dictionary skillData = jsonDictionary[element] as Godot.Collections.Dictionary;
            string name = Convert.ToString(skillData["name"]);
            GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").AddItem(name);
            GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").Select(0);
        }
        if (classData.Contains("effects") == true)
        {
            this.ClearEffectList();
            Godot.Collections.Array effectList = classData["effects"] as Godot.Collections.Array;
            foreach (Godot.Collections.Dictionary effect in effectList)
            {
                this.AddEffectList(effect["name"].ToString(), Convert.ToInt32(effect["data_id"]), effect["value1"].ToString(), effect["value2"].ToString());
            }
        }
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
        SaveClassData();
        RefreshData(classSelected);
    }

    private void SaveClassData()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Class") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classData = jsonDictionary["class" + classSelected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classStatFormula = classData["stat_list"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary classSkillList = classData["skill_list"] as Godot.Collections.Dictionary;
        Godot.Collections.Array effectList = new Godot.Collections.Array();

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
        classData["stat_list"] = classStatFormula;
        classData["skill_list"] = classSkillList;

        int effectSize = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames").GetItemCount();
        for (int i = 0; i < effectSize; i++)
        {
            Godot.Collections.Dictionary effectData = new Godot.Collections.Dictionary();
            effectData["name"] = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames").GetItemText(i);
            effectData["data_id"] = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType").GetItemText(i);
            effectData["value1"] = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1").GetItemText(i);
            effectData["value2"] = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2").GetItemText(i);
            effectList.Add(effectData);
        }
        classData["effects"] = effectList;

        this.GetParent().GetParent().Call("StoreData", "Class", jsonDictionary);
    }
    private void _on_AddClass_pressed()
    {
        GetNode<OptionButton>("ClassButton").AddItem("NewClass");
        int id = GetNode<OptionButton>("ClassButton").GetItemCount() - 1;
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Class") as Godot.Collections.Dictionary;
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
        this.GetParent().GetParent().Call("StoreData", "Class", jsonDictionary);
    }

    private void _on_RemoveClass_pressed()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Class") as Godot.Collections.Dictionary;
        if (jsonDictionary.Keys.Count > 1)
        {
            int classId = classSelected;
            while (classId < jsonDictionary.Keys.Count - 1)
            {
                jsonDictionary["class" + classId] = jsonDictionary["class" + (classId + 1)];
                classId += 1;
            }
            jsonDictionary.Remove("class" + classId);
            this.GetParent().GetParent().Call("StoreData", "Class", jsonDictionary);
            GetNode<OptionButton>("ClassButton").RemoveItem(classSelected);
            if (classSelected == 0)
            {
                GetNode<OptionButton>("ClassButton").Select(classSelected + 1);
                classSelected += 1;
            }
            else
            {
                GetNode<OptionButton>("ClassButton").Select(classSelected - 1);
                classSelected -= 1;
            }
            GetNode<OptionButton>("ClassButton").Select(classSelected);
            RefreshData(classSelected);
        }
    }
    private void _on_ClassButton_item_selected(int id)
    {
        classSelected = id;
        RefreshData(id);
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
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Skill") as Godot.Collections.Dictionary;

        int skill = GetNode<OptionButton>("SkillLabel/AddSkill/SkillLabel/OptionButton").GetSelectedId();
        int level = Convert.ToInt32(GetNode<SpinBox>("SkillLabel/AddSkill/LevelLabel/LevelSpin").Value);
        skillListArray.Add(Convert.ToString(skill));
        Godot.Collections.Dictionary skillData = jsonDictionary["skill" + skill] as Godot.Collections.Dictionary;
        string skillName = skillData["name"] as string;
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillListContainer/SkillList").AddItem(skillName);
        GetNode<ItemList>("SkillLabel/SkillContainer/HBoxContainer/SkillLevelContainer/SkillLevelList").AddItem(Convert.ToString(level));

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
        SaveClassData();
        statEdit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }

    private void _on_CancelStatButton_pressed()
    {
        statEdit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }

    private void _on_AddClassEffect_pressed()
    {
        this.GetParent().GetParent().Call("OpenEffectManager", "Class");
    }

    private void _on_RemoveClassEffect_pressed()
    {
        int id = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames").GetSelectedItems()[0];
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames").RemoveItem(id);
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType").RemoveItem(id);
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1").RemoveItem(id);
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2").RemoveItem(id);
    }

    public void AddEffectList(String name, int dataId, String value1, String value2)
    {
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames").AddItem(name);
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType").AddItem(dataId.ToString());
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1").AddItem(value1);
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2").AddItem(value2);
    }

    public void ClearEffectList()
    {
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectNames").Clear();
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/DataType").Clear();
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue1").Clear();
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/HBoxContainer/EffectValue2").Clear();
    }
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}