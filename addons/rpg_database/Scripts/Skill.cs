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
        skillData.Add("mp_cost", 1);
        skillData.Add("tp_cost", 1);
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

    private void _on_SkillSaveButton_pressed()
    {
        SaveSkillData();
        RefreshData(skillSelected);
    }
    private void SaveSkillData()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Skill") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary skillData = jsonDictionary["skill" + skillSelected] as Godot.Collections.Dictionary;
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
        this.GetParent().GetParent().Call("StoreData", "Skill", jsonDictionary);
    }

    private void _on_SkillButton_item_selected(int id)
    {
        skillSelected = id;
        RefreshData(id);
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
