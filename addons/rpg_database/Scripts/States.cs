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
    int add_string = -1;
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
            iconPath = stateData["icon"].ToString();
            GetNode<Sprite>("IconLabel/Sprite").Texture = GD.Load(stateData["icon"] as string) as Godot.Texture;
        }
        GetNode<OptionButton>("RestrictionLabel/RestrictionOption").Selected = Convert.ToInt32(stateData["restriction"]);
        GetNode<SpinBox>("PriorityLabel/PriorityValue").Value = Convert.ToInt32(stateData["priority"]);
        GetNode<SpinBox>("EraseLabel/TurnsLabel/MinTurns").Value = Convert.ToInt32(eraseConditions["turns_min"]);
        GetNode<SpinBox>("EraseLabel/TurnsLabel/MaxTurns").Value = Convert.ToInt32(eraseConditions["turns_max"]);
        GetNode<SpinBox>("EraseLabel/DamageLabel/Damage").Value = Convert.ToInt32(eraseConditions["erase_damage"]);
        GetNode<SpinBox>("EraseLabel/SetpsLabel/SpinBox").Value = Convert.ToInt32(eraseConditions["erase_setps"]);
        GetNode<ItemList>("MessagesLabel/PanelContainer/VBoxContainer/MessageList").Clear();
        foreach (String message in messages.Values)
        {
            GetNode<ItemList>("MessagesLabel/PanelContainer/VBoxContainer/MessageList").AddItem(message);
        }
        GetNode<ItemList>("CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions").Clear();
        foreach (String condition in customEraseConditions.Values)
        {
            GetNode<ItemList>("CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions").AddItem(condition);
        }
    }

    private void _on_SearchState_pressed()
    {
        GetNode<FileDialog>("IconLabel/SearchDialog").PopupCentered();
    }

    private void _on_SearchStateIconDialog_file_selected(String path)
    {
        iconPath = path;
        GetNode<Sprite>("IconLabel/Sprite").Texture = GD.Load(path) as Godot.Texture;
    }

    private void _on_AddCustomStateCondition_pressed()
    {
        add_string = 0;
        GetNode<WindowDialog>("AddString").WindowTitle = "Custom State Erase Formula";
        GetNode<WindowDialog>("AddString").PopupCentered();
    }

    private void _on_RemoveCustomStateCondition_pressed()
    {
        int selected = GetNode<ItemList>("CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions").GetSelectedItems()[0];
        if (selected > -1)
        {
            GetNode<ItemList>("CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions").RemoveItem(selected);
        }
    }

    private void _on_AddStateMessage_pressed()
    {
        add_string = 1;
        GetNode<WindowDialog>("AddString").WindowTitle = "State Message";
        GetNode<WindowDialog>("AddString").PopupCentered();
    }

    private void _on_RemoveStateMessage_pressed()
    {
        int selected = GetNode<ItemList>("MessagesLabel/PanelContainer/VBoxContainer/MessageList").GetSelectedItems()[0];
        if (selected > -1)
        {
            GetNode<ItemList>("MessagesLabel/PanelContainer/VBoxContainer/MessageList").RemoveItem(selected);
        }
    }

    private void _on_ConfirmAddString_pressed()
    {
        String text = GetNode<LineEdit>("AddString/LineEdit").Text;
        if (add_string == 0)
        {
            GetNode<ItemList>("CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions").AddItem(text);
        }
        else if (add_string == 1)
        {
            GetNode<ItemList>("MessagesLabel/PanelContainer/VBoxContainer/MessageList").AddItem(text);
        }
        add_string = -1;
        GetNode<WindowDialog>("AddString").Hide();
    }

    private void _on_CancelAddString_pressed()
    {
        add_string = -1;
        GetNode<WindowDialog>("AddString").Hide();
    }

    private void _on_StateButton_item_selected(int id)
    {
        stateSelected = id;
        RefreshData(id);
    }

    private void _on_AddState_pressed()
    {
        GetNode<OptionButton>("StateButton").AddItem("NewState");
        int id = GetNode<OptionButton>("StateButton").GetItemCount() - 1;
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "State") as Godot.Collections.Dictionary;

        Godot.Collections.Dictionary stateData = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary eraseCondition = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary messages = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary customEraseConditions = new Godot.Collections.Dictionary();
        stateData.Add("name", "NewState");
        stateData.Add("icon", "");
        stateData.Add("restriction", 4);
        stateData.Add("priority", 100);
        eraseCondition.Add("turns_min", 0);
        eraseCondition.Add("turns_max", 0);
        eraseCondition.Add("erase_damage", 0);
        eraseCondition.Add("erase_setps", 0);
        stateData.Add("erase_conditions", eraseCondition);
        messages.Add("0", "Insert a custom message");
        stateData.Add("messages", messages);
        customEraseConditions.Add("0", "Insert a custom condition");
        stateData.Add("custom_erase_conditions", customEraseConditions);

        jsonDictionary.Add("state" + id, stateData);
        this.GetParent().GetParent().Call("StoreData", "State", jsonDictionary);
    }

    private void _on_RemoveState_pressed()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "State") as Godot.Collections.Dictionary;
        if (jsonDictionary.Keys.Count > 1)
        {
            int stateId = stateSelected;
            while (stateId < jsonDictionary.Keys.Count - 1)
            {
                jsonDictionary["state" + stateId] = jsonDictionary["state" + (stateId + 1)];
                stateId += 1;
            }
            jsonDictionary.Remove("state" + stateId);
            this.GetParent().GetParent().Call("StoreData", "State", jsonDictionary);
            GetNode<OptionButton>("StateButton").RemoveItem(stateSelected);
            if (stateSelected == 0)
            {
                GetNode<OptionButton>("StateButton").Select(stateSelected + 1);
                stateSelected += 1;
            }
            else
            {
                GetNode<OptionButton>("StateButton").Select(stateSelected - 1);
                stateSelected -= 1;
            }
            GetNode<OptionButton>("StateButton").Select(stateSelected);
            RefreshData(stateSelected);
        }
    }

    private void _on_SaveStates_pressed()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "State") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary stateData = jsonDictionary["state" + stateSelected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary eraseCondition = stateData["erase_conditions"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary messages = stateData["messages"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary customEraseConditions = stateData["custom_erase_conditions"] as Godot.Collections.Dictionary;
        stateData["name"] = GetNode<LineEdit>("NameLabel/NameLine").Text;
        stateData["icon"] = iconPath;
        stateData["restriction"] = GetNode<OptionButton>("RestrictionLabel/RestrictionOption").Selected;
        stateData["priority"] = GetNode<SpinBox>("PriorityLabel/PriorityValue").Value;
        eraseCondition["turns_min"] = GetNode<SpinBox>("EraseLabel/TurnsLabel/MinTurns").Value;
        eraseCondition["turns_max"] = GetNode<SpinBox>("EraseLabel/TurnsLabel/MaxTurns").Value;
        eraseCondition["erase_damage"] = GetNode<SpinBox>("EraseLabel/DamageLabel/Damage").Value;
        eraseCondition["erase_setps"] = GetNode<SpinBox>("EraseLabel/SetpsLabel/SpinBox").Value;
        int items = GetNode<ItemList>("MessagesLabel/PanelContainer/VBoxContainer/MessageList").GetItemCount();
        for (int i = 0; i < items; i++)
        {
            messages[i.ToString()] = GetNode<ItemList>("MessagesLabel/PanelContainer/VBoxContainer/MessageList").GetItemText(i);
        }
        items = GetNode<ItemList>("CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions").GetItemCount();
        for (int i = 0; i < items; i++)
        {
            messages[i.ToString()] = GetNode<ItemList>("CustomEraseLabel/PanelContainer/VBoxContainer/EraseConditions").GetItemText(i);
        }

        this.GetParent().GetParent().Call("StoreData", "State", jsonDictionary);
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
