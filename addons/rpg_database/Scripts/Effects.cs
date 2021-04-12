using Godot;
using System;

[Tool]
public class Effects : Container
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    bool addNewEffect = true;
    bool databaseLoaded = false;
    // Called when the node enters the scene tree for the first time.
    public void Start()
    {
        if (databaseLoaded == false)
        {
            Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Effect") as Godot.Collections.Dictionary;
            for (int i = 0; i < jsonDictionary.Count; i++)
            {
                Godot.Collections.Dictionary effectData = jsonDictionary["effect" + i] as Godot.Collections.Dictionary;
                Godot.Collections.Dictionary showList = effectData["data_type"] as Godot.Collections.Dictionary;
                Godot.Collections.Dictionary value2 = effectData["value2"] as Godot.Collections.Dictionary;

                GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").AddItem(effectData["name"].ToString());
                if (showList["show"] as bool? == true)
                {
                    GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").AddItem(showList["data"].ToString());
                }
                else
                {
                    GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").AddItem("Disabled");
                }

                GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").AddItem(effectData["value1"].ToString());
                if (value2["show"] as bool? == true)
                {
                    GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").AddItem(value2["data"].ToString());
                }
                else
                {
                    GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").AddItem("-1");
                }
            }
            databaseLoaded = true;
        }

    }

    private void _on_AddEffect_pressed()
    {
        addNewEffect = true;
        GetNode<WindowDialog>("AddEffect").PopupCentered();
    }

    private void _on_EditEffect_pressed()
    {
        if (GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetSelectedItems()[0] > -1)
        {
            addNewEffect = false;
            int id = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetSelectedItems()[0];

            String name = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetItemText(id);
            String dataTypes = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").GetItemText(id);
            int value1 = Convert.ToInt32(GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").GetItemText(id));
            int value2 = Convert.ToInt32(GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").GetItemText(id));

            GetNode<LineEdit>("AddEffect/VBoxContainer/Name/LineEdit").Text = name;
            if (dataTypes == "Disabled")
            {
                GetNode<CheckButton>("AddEffect/VBoxContainer/ShowList/CheckButton").Pressed = false;
                GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Disabled = true;
            }
            else
            {
                GetNode<CheckButton>("AddEffect/VBoxContainer/ShowList/CheckButton").Pressed = true;
                GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Disabled = false;
                switch (dataTypes)
                {
                    case "States":
                        GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Select(0);
                        break;
                    case "Stats":
                        GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Select(1);
                        break;
                    case "Weapon Types":
                        GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Select(2);
                        break;
                    case "Armor Types":
                        GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Select(3);
                        break;
                    case "Elements":
                        GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Select(4);
                        break;
                    case "Skill Types":
                        GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Select(5);
                        break;
                }
            }

            GetNode<OptionButton>("AddEffect/VBoxContainer/Value1/OptionButton").Select(value1);
            if (value2 == -1)
            {
                GetNode<CheckButton>("AddEffect/VBoxContainer/Value2/CheckButton").Pressed = false;
                GetNode<OptionButton>("AddEffect/VBoxContainer/Value2/OptionButton").Disabled = true;
            }
            else
            {
                GetNode<CheckButton>("AddEffect/VBoxContainer/Value2/CheckButton").Pressed = true;
                GetNode<OptionButton>("AddEffect/VBoxContainer/Value2/OptionButton").Disabled = false;
                GetNode<OptionButton>("AddEffect/VBoxContainer/Value2/OptionButton").Select(value2);
            }
            GetNode<WindowDialog>("AddEffect").PopupCentered();
        }
    }

    private void _on_RemoveEffect_pressed()
    {
        int selectedEffect = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetSelectedItems()[0];
        if (selectedEffect > -1)
        {
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").RemoveItem(selectedEffect);
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").RemoveItem(selectedEffect);
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").RemoveItem(selectedEffect);
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").RemoveItem(selectedEffect);
        }
    }

    private void _on_ClearEffects_pressed()
    {
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").Clear();
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").Clear();
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").Clear();
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").Clear();
    }

    private void _on_ShowListCheckButton_pressed()
    {
        if (GetNode<CheckButton>("AddEffect/VBoxContainer/ShowList/CheckButton").Pressed == true)
        {
            GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Disabled = false;
        }
        else
        {
            GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Disabled = true;
        }
    }

    private void _on_Value2CheckButton_pressed()
    {
        if (GetNode<CheckButton>("AddEffect/VBoxContainer/Value2/CheckButton").Pressed == true)
        {
            GetNode<OptionButton>("AddEffect/VBoxContainer/Value2/OptionButton").Disabled = false;
        }
        else
        {
            GetNode<OptionButton>("AddEffect/VBoxContainer/Value2/OptionButton").Disabled = true;
        }
    }

    private void _on_AddEffectConfirm_pressed()
    {
        String name = GetNode<LineEdit>("AddEffect/VBoxContainer/Name/LineEdit").Text;
        int selected = 0;
        String dataType = "Disabled";
        if (GetNode<CheckButton>("AddEffect/VBoxContainer/ShowList/CheckButton").Pressed == true)
        {
            selected = GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Selected;
            dataType = GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").GetItemText(selected);
        }
        int value1 = GetNode<OptionButton>("AddEffect/VBoxContainer/Value1/OptionButton").Selected;
        int value2 = -1;
        if (GetNode<CheckButton>("AddEffect/VBoxContainer/Value2/CheckButton").Pressed == true)
        {
            value2 = GetNode<OptionButton>("AddEffect/VBoxContainer/Value2/OptionButton").Selected;
        }
        if (addNewEffect == false)
        {
            int id = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetSelectedItems()[0];
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").SetItemText(id, name);
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").SetItemText(id, dataType);
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").SetItemText(id, value1.ToString());
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").SetItemText(id, value2.ToString());
        }
        else
        {
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").AddItem(name);
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").AddItem(dataType);
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").AddItem(value1.ToString());
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").AddItem(value2.ToString());
        }

        GetNode<WindowDialog>("AddEffect").Hide();
    }

    private void _on_AddEffectCancel_pressed()
    {
        GetNode<WindowDialog>("AddEffect").Hide();
    }

    private void _on_SaveEffects_pressed()
    {
        int size = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetItemCount();
        Godot.Collections.Dictionary effectList = new Godot.Collections.Dictionary();
        for (int i = 0; i < size; i++)
        {
            Godot.Collections.Dictionary effectData = new Godot.Collections.Dictionary();
            Godot.Collections.Dictionary showList = new Godot.Collections.Dictionary();
            Godot.Collections.Dictionary value2 = new Godot.Collections.Dictionary();
            effectData["name"] = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetItemText(i);
            String dataType = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").GetItemText(i);
            if (dataType == "Disabled")
            {
                showList["show"] = false;
                showList["data"] = "";
            }
            else
            {
                showList["show"] = true;
                showList["data"] = dataType;
            }
            effectData["data_type"] = showList;
            effectData["value1"] = Convert.ToInt32(GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").GetItemText(i));
            int value2Val = Convert.ToInt32(GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").GetItemText(i));
            if (value2Val == -1)
            {
                value2["show"] = false;
                value2["data"] = "";
            }
            else
            {
                value2["show"] = true;
                value2["data"] = value2Val;
            }
            effectData["value2"] = value2;
            effectList["effect" + i] = effectData;
        }

        this.GetParent().GetParent().Call("StoreData", "Effect", effectList);
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
