using Godot;
using System;

[Tool]
public class Effects : Container
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    bool addNewEffect = true;
    // Called when the node enters the scene tree for the first time.
    public void Start()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Effect") as Godot.Collections.Dictionary;
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary effectData = jsonDictionary["effect" + i] as Godot.Collections.Dictionary;
            Godot.Collections.Dictionary showList = effectData["show_list"] as Godot.Collections.Dictionary;
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
                GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").AddItem("Disabled");
            }
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
            GetNode<WindowDialog>("AddEffect").PopupCentered();
        }
    }

    private void _on_RemoveEffect_pressed()
    {
        int selectedEffect = GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetSelectedItems()[0];
        if (selectedEffect > -1)
        {
            GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").RemoveItem(selectedEffect);
        }
    }

    private void _on_ClearEffects_pressed()
    {
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").Clear();
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
        String dataType = "";
        if (GetNode<CheckButton>("AddEffect/VBoxContainer/ShowList/CheckButton").Disabled == false)
        {
            selected = GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").Selected;
            dataType = GetNode<OptionButton>("AddEffect/VBoxContainer/ShowList/OptionButton").GetItemText(selected);
        }
        selected = GetNode<OptionButton>("AddEffect/VBoxContainer/Value1/OptionButton").Selected;
        String value1 = GetNode<OptionButton>("AddEffect/VBoxContainer/Value1/OptionButton").GetItemText(selected);
        String value2 = "";
        if (GetNode<CheckButton>("AddEffect/VBoxContainer/Value2/CheckButton").Disabled == false)
        {
            selected = GetNode<OptionButton>("AddEffect/VBoxContainer/Value2/OptionButton").Selected;
            value2 = GetNode<OptionButton>("AddEffect/VBoxContainer/Value2/OptionButton").GetItemText(selected);
        }
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").AddItem(name);
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/DataTypes").AddItem(dataType);
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").AddItem(value1);
        GetNode<ItemList>("EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").AddItem(value2);
        GetNode<WindowDialog>("AddEffect").Hide();
    }

    private void _on_AddEffectCancel_pressed()
    {
        GetNode<WindowDialog>("AddEffect").Hide();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
