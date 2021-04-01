using Godot;
using System;

[Tool]
public class Armor : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    string iconPath = "";
    int armorSelected = 0;
    int statEdit = -1;
    // Called when the node enters the scene tree for the first time.
    public void Start()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Armor") as Godot.Collections.Dictionary;
        
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary armorData = jsonDictionary["armor" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ArmorButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ArmorButton").AddItem(armorData["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("ArmorButton").SetItemText(i, armorData["name"] as string);
            }
        }

        jsonDictionary = this.GetParent().GetParent().Call("ReadData", "System") as Godot.Collections.Dictionary;
        
        Godot.Collections.Dictionary systemData = jsonDictionary["armors"] as Godot.Collections.Dictionary;
        for (int i = 0; i < systemData.Count; i++)
        {
            if (i > GetNode<OptionButton>("ATypeLabel/ATypeButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ATypeLabel/ATypeButton").AddItem(systemData[i.ToString()] as string);
            }
            else
            {
                GetNode<OptionButton>("ATypeLabel/ATypeButton").SetItemText(i, systemData[i.ToString()] as string);
            }
        }
        
        systemData = jsonDictionary["slots"] as Godot.Collections.Dictionary;
        int final_id = 0;
        foreach (String str in systemData.Keys)
        {
            if (str[0] == 'a')
            {
                int id = Convert.ToInt32(str.Remove(0, 1)) - final_id;
                if (id > GetNode<OptionButton>("SlotLabel/SlotButton").GetItemCount() - 1)
                {
                    GetNode<OptionButton>("SlotLabel/SlotButton").AddItem(systemData[str] as string);
                }
                else
                {
                    GetNode<OptionButton>("SlotLabel/SlotButton").SetItemText(id, systemData[str] as string);
                }
            }
            else
            {
                final_id += 1;
            }
        }
        RefreshData(armorSelected);
    }

    private void RefreshData(int id)
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Armor") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armorData = jsonDictionary["armor" + id] as Godot.Collections.Dictionary;

        jsonDictionary = this.GetParent().GetParent().Call("ReadData", "System") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary systemData = jsonDictionary["stats"] as Godot.Collections.Dictionary;

        GetNode<LineEdit>("NameLabel/NameText").Text = armorData["name"] as string;
        string icon = armorData["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(armorData["icon"] as string) as Godot.Texture;
        }
        GetNode<TextEdit>("DescLabel/DescText").Text = armorData["description"] as string;
        GetNode<OptionButton>("ATypeLabel/ATypeButton").Selected = Convert.ToInt32(armorData["armor_type"]);
        GetNode<OptionButton>("SlotLabel/SlotButton").Selected = Convert.ToInt32(armorData["slot_type"]);
        GetNode<SpinBox>("PriceLabel/PriceSpin").Value = Convert.ToInt32(armorData["price"]);

        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").Clear();
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").Clear();
        for (int i = 0; i < systemData.Count; i++)
        {
            string statName = systemData[i.ToString()] as string;
            Godot.Collections.Dictionary armorStatFormula = armorData["stat_list"] as Godot.Collections.Dictionary;
            string statFormula = "";
            if (armorStatFormula.Contains(statName))
            {
                statFormula = armorStatFormula[statName as string] as string;
            }
            else
            {
                statFormula = "0";
            }
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").AddItem(statName);
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").AddItem(statFormula);
        }

    }

    private void _on_AddArmorButton_pressed()
    {
        GetNode<OptionButton>("ArmorButton").AddItem("NewArmor");
        int id = GetNode<OptionButton>("ArmorButton").GetItemCount() - 1;
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Armor") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armorData = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary armorStats = new Godot.Collections.Dictionary();
        armorData.Add("name", "NewArmor");
        armorData.Add("icon", "");
        armorData.Add("description", "New created armor");
        armorData.Add("armor_type", 0);
        armorData.Add("slot_type", 0);
        armorData.Add("price", 50);
        armorStats.Add("hp", "0");
        armorStats.Add("mp", "0");
        armorStats.Add("atk", "10");
        armorStats.Add("def", "2");
        armorStats.Add("int", "2");
        armorStats.Add("res", "1");
        armorStats.Add("spd", "0");
        armorStats.Add("luk", "0");
        armorData.Add("stat_list", armorStats);
        jsonDictionary.Add("armor" + id, armorData);
        this.GetParent().GetParent().Call("StoreData", "Armor", jsonDictionary);
    }

    private void _on_ArmorSaveButton_pressed()
    {
        SaveArmorData();
        RefreshData(armorSelected);
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

    private void _on_ArmorButton_item_selected(int id)
    {
        armorSelected = id;
        RefreshData(id);
    }

    private void SaveArmorData()
    {
		Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Armor") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armorData = jsonDictionary["armor"+armorSelected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armorStatFormula = armorData["stat_list"] as Godot.Collections.Dictionary;
		armorData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		GetNode<OptionButton>("ArmorButton").SetItemText(armorSelected, GetNode<LineEdit>("NameLabel/NameText").Text);
		armorData["icon"] = iconPath;
		armorData["description"] = GetNode<TextEdit>("DescLabel/DescText").Text;
        armorData["armor_type"] = GetNode<OptionButton>("ATypeLabel/ATypeButton").Selected;
        armorData["slot_type"] = GetNode<OptionButton>("SlotLabel/SlotButton").Selected;
		armorData["price"] = GetNode<SpinBox>("PriceLabel/PriceSpin").Value;
        int items = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").GetItemCount();
        for (int i = 0; i < items; i++)
        {
            string stat = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").GetItemText(i);
            string formula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").GetItemText(i);
            armorStatFormula[stat] = formula;
        }
        this.GetParent().GetParent().Call("StoreData", "Armor", jsonDictionary);
    }

    private void _on_StatValueList_item_activated(int index)
    {
        string statName = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatNameCont/StatNameList").GetItemText(index);
        string statFormula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").GetItemText(index);
        GetNode<Label>("StatEditor/StatLabel").Text = statName;
        GetNode<LineEdit>("StatEditor/StatEdit").Text = statFormula;
        statEdit = index;
        GetNode<WindowDialog>("StatEditor").Show();
    }

    private void _on_OkButton_pressed()
    {
        string statFormula = GetNode<LineEdit>("StatEditor/StatEdit").Text;
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatValueCont/StatValueList").SetItemText(statEdit, statFormula);
        SaveArmorData();
        statEdit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }

    private void _on_CancelButton_pressed()
    {
        statEdit = -1;
        GetNode<WindowDialog>("StatEditor").Hide();
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
