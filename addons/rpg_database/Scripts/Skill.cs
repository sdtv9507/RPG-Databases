using Godot;
using System;

[Tool]
public class Skill : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string iconPath = "";
    int skillSelected = 0;
    // Called when the node enters the scene tree for the first time.
    public void Start()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Skill") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary systemDictionary = this.GetParent().GetParent().Call("ReadData", "System") as Godot.Collections.Dictionary;
        GetNode<OptionButton>("SkillButton").Clear();
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary skillData = jsonDictionary["skill" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("SkillButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("SkillButton").AddItem(skillData["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("SkillButton").SetItemText(i, skillData["name"] as string);
            }
        }

        Godot.Collections.Dictionary systemData = systemDictionary["elements"] as Godot.Collections.Dictionary;
        for (int i = 0; i < systemData.Count; i++)
        {
            if (i > GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").AddItem(systemData[i.ToString()] as string);
            }
            else
            {
                GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").SetItemText(i, systemData[i.ToString()] as string);
            }
        }

        systemData = systemDictionary["skills"] as Godot.Collections.Dictionary;
        for (int i = 0; i < systemData.Count; i++)
        {
            if (i > GetNode<OptionButton>("SkillTypeLabel/SkillTypeButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("SkillTypeLabel/SkillTypeButton").AddItem(systemData[i.ToString()] as string);
            }
            else
            {
                GetNode<OptionButton>("SkillTypeLabel/SkillTypeButton").SetItemText(i, systemData[i.ToString()] as string);
            }
        }
        RefreshData(skillSelected);
    }

    private void RefreshData(int id)
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Skill") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary skillData = jsonDictionary["skill" + id] as Godot.Collections.Dictionary;
        GetNode<LineEdit>("NameLabel/NameText").Text = skillData["name"] as string;
        string icon = skillData["icon"] as string;
        if (icon != "")
        {
            iconPath = icon;
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(skillData["icon"] as string) as Godot.Texture;
        }
        GetNode<TextEdit>("DescLabel/DescText").Text = skillData["description"] as string;
        GetNode<OptionButton>("SkillTypeLabel/SkillTypeButton").Selected = Convert.ToInt32(skillData["skill_type"]);
        GetNode<SpinBox>("MPCostLabel/MPCostBox").Value = Convert.ToInt32(skillData["mp_cost"]);
        GetNode<SpinBox>("TPCostLabel/TPCostBox").Value = Convert.ToInt32(skillData["tp_cost"]);
        GetNode<OptionButton>("TargetLabel/TargetButton").Selected = Convert.ToInt32(skillData["target"]);
        GetNode<OptionButton>("UsableLabel/UsableButton").Selected = Convert.ToInt32(skillData["usable"]);
        GetNode<SpinBox>("HitLabel/HitBox").Value = Convert.ToInt32(skillData["success"]);
        GetNode<OptionButton>("TypeLabel/TypeButton").Selected = Convert.ToInt32(skillData["hit_type"]);
        GetNode<OptionButton>("DamageLabel/DTypeLabel/DTypeButton").Selected = Convert.ToInt32(skillData["damage_type"]);
        GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").Selected = Convert.ToInt32(skillData["element"]);
        GetNode<LineEdit>("DamageLabel/DFormulaLabel/FormulaText").Text = skillData["formula"] as string;
        if (skillData.Contains("effects") == true)
        {
            this.ClearEffectList();
            Godot.Collections.Array effectList = skillData["effects"] as Godot.Collections.Array;
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

    private void _on_AddSkill_pressed()
    {
        GetNode<OptionButton>("SkillButton").AddItem("NewSkill");
        int id = GetNode<OptionButton>("SkillButton").GetItemCount() - 1;
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Skill") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary skillData = new Godot.Collections.Dictionary();
        skillData.Add("name", "NewSkill");
        skillData.Add("icon", "");
        skillData.Add("description", "New created skill");
        skillData.Add("skill_type", 0);
        skillData.Add("mp_cost", 0);
        skillData.Add("tp_cost", 0);
        skillData.Add("target", 1);
        skillData.Add("usable", 1);
        skillData.Add("success", 95);
        skillData.Add("hit_type", 1);
        skillData.Add("damage_type", 1);
        skillData.Add("element", 0);
        skillData.Add("formula", "atk * 4 - def * 2");
        jsonDictionary.Add("skill" + id, skillData);
        this.GetParent().GetParent().Call("StoreData", "Skill", jsonDictionary);
    }

    private void _on_RemoveSkill_pressed()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Skill") as Godot.Collections.Dictionary;
        if (jsonDictionary.Keys.Count > 1)
        {
            int skillId = skillSelected;
            while (skillId < jsonDictionary.Keys.Count - 1)
            {
                jsonDictionary["skill" + skillId] = jsonDictionary["skill" + (skillId + 1)];
                skillId += 1;
            }
            jsonDictionary.Remove("skill" + skillId);
            this.GetParent().GetParent().Call("StoreData", "Skill", jsonDictionary);
            GetNode<OptionButton>("SkillButton").RemoveItem(skillSelected);
            if (skillSelected == 0)
            {
                GetNode<OptionButton>("SkillButton").Select(skillSelected + 1);
                skillSelected += 1;
            }
            else
            {
                GetNode<OptionButton>("SkillButton").Select(skillSelected - 1);
                skillSelected -= 1;
            }
            GetNode<OptionButton>("SkillButton").Select(skillSelected);
            RefreshData(skillSelected);
        }
    }

    private void _on_SkillSaveButton_pressed()
    {
        SaveSkillData();
        RefreshData(skillSelected);
    }
    private void SaveSkillData()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Skill") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary skillData = jsonDictionary["skill" + skillSelected] as Godot.Collections.Dictionary;
        Godot.Collections.Array effectList = new Godot.Collections.Array();
        
        skillData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
        GetNode<OptionButton>("SkillButton").SetItemText(skillSelected, GetNode<LineEdit>("NameLabel/NameText").Text);
        skillData["icon"] = iconPath;
        skillData["description"] = GetNode<TextEdit>("DescLabel/DescText").Text;
        skillData["skill_type"] = GetNode<OptionButton>("SkillTypeLabel/SkillTypeButton").Selected;
        skillData["mp_cost"] = GetNode<SpinBox>("MPCostLabel/MPCostBox").Value;
        skillData["tp_cost"] = GetNode<SpinBox>("TPCostLabel/TPCostBox").Value;
        skillData["target"] = GetNode<OptionButton>("TargetLabel/TargetButton").Selected;
        skillData["usable"] = GetNode<OptionButton>("UsableLabel/UsableButton").Selected;
        skillData["success"] = GetNode<SpinBox>("HitLabel/HitBox").Value;
        skillData["hit_type"] = GetNode<OptionButton>("TypeLabel/TypeButton").Selected;
        skillData["damage_type"] = GetNode<OptionButton>("DamageLabel/DTypeLabel/DTypeButton").Selected;
        skillData["element"] = GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").Selected;
        skillData["formula"] = GetNode<LineEdit>("DamageLabel/DFormulaLabel/FormulaText").Text;
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
        skillData["effects"] = effectList;
        this.GetParent().GetParent().Call("StoreData", "Skill", jsonDictionary);
    }

    private void _on_SkillButton_item_selected(int id)
    {
        skillSelected = id;
        RefreshData(id);
    }

    private void _on_AddSkillEffect_pressed()
    {
        this.GetParent().GetParent().Call("OpenEffectManager", "Skill");
    }

    private void _on_RemoveSkillEffect_pressed()
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
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
