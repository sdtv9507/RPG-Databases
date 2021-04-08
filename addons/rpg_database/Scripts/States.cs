using Godot;
using System;

[Tool]
public class States : Container
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    string iconPath = "";
    int stateSelected = 0;
    // Called when the node enters the scene tree for the first time.
    public void Start()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "State") as Godot.Collections.Dictionary;
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary stateData = jsonDictionary["state" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("StateButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("StateButton").AddItem(stateData["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("StateButton").SetItemText(i, stateData["name"] as string);
            }
        }
        RefreshData(stateSelected);
    }

    public void RefreshData(int id)
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "State") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary stateData = jsonDictionary["state" + id] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary eraseConditions = stateData["erase_conditions"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary messages = stateData["messages"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary customEraseConditions = stateData["custom_erase_conditions"] as Godot.Collections.Dictionary;
        GetNode<LineEdit>("NameLabel/NameLine").Text = stateData["name"] as string;
        string icon = stateData["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/Sprite").Texture = GD.Load(stateData["icon"] as string) as Godot.Texture;
        }
        GetNode<OptionButton>("RestrictionLabel/RestrictionOption").Selected = Convert.ToInt32(stateData["restriction"]);
        GetNode<SpinBox>("PriorityLabel/PriorityValue").Value = Convert.ToInt32(stateData["priority"]);
        GetNode<SpinBox>("EraseLabel/TurnsLabel/MinTurns").Value = Convert.ToInt32(eraseConditions["turns_min"]);
        GetNode<SpinBox>("EraseLabel/TurnsLabel/MaxTurns").Value = Convert.ToInt32(eraseConditions["turns_max"]);
        GetNode<SpinBox>("EraseLabel/DamageLabel/Damage").Value = Convert.ToInt32(eraseConditions["erase_damage"]);
        GetNode<SpinBox>("EraseLabel/SetpsLabel/SpinBox").Value = Convert.ToInt32(eraseConditions["erase_setps"]);
        foreach (String message in messages.Values)
        {
            GetNode<ItemList>("MessagesLabel/PanelContainer/VBoxContainer/MessageList").AddItem(message);
        }
        foreach (String condition in customEraseConditions.Values)
        {
            GetNode<ItemList>("CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions").AddItem(condition);
        }
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
