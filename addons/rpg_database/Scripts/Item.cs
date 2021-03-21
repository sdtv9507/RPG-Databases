using Godot;
using System;

[Tool]
public class Item : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string icon_path = "";
    int item_selected = 0;
    // Called when the node enters the scene tree for the first time.
    public void _Start()
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Item.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        
		database_editor.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
		string systemAsText = database_editor.GetAsText();
		JSONParseResult systemParsed = JSON.Parse(systemAsText);
		Godot.Collections.Dictionary systemDictionary = systemParsed.Result as Godot.Collections.Dictionary;
        
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary newItemDict = jsonDictionary["item"+i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ItemButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ItemButton").AddItem(newItemDict["name"] as string);
            }else{
                GetNode<OptionButton>("ItemButton").SetItemText(i, newItemDict["name"] as string);
            }
        }
        
        Godot.Collections.Dictionary newSystemDict = systemDictionary["elements"] as Godot.Collections.Dictionary;
        for (int i = 0; i < newSystemDict.Count; i++)
        {
            if (i > GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").AddItem(newSystemDict[i.ToString()] as string);
            }else{
                GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").SetItemText(i, newSystemDict[i.ToString()] as string);
            }
        }
        database_editor.Close();
        _refresh_data(0);
    }

    private void _refresh_data(int id)
    {
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Item.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary newItemDict = jsonDictionary["item"+id] as Godot.Collections.Dictionary;
        GetNode<LineEdit>("NameLabel/NameText").Text = newItemDict["name"] as string;
        string icon = newItemDict["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(newItemDict["icon"] as string) as Godot.Texture;
        }
        GetNode<TextEdit>("DescLabel/DescText").Text = newItemDict["description"] as string;
        GetNode<OptionButton>("ItemTypeLabel/ItemTypeButton").Selected = Convert.ToInt32(newItemDict["item_type"]);
        GetNode<SpinBox>("PriceLabel/PriceBox").Value = Convert.ToInt32(newItemDict["price"]);
        GetNode<OptionButton>("ConsumableLabel/ConsumableButton").Selected = Convert.ToInt32(newItemDict["consumable"]);
        GetNode<OptionButton>("TargetLabel/TargetButton").Selected = Convert.ToInt32(newItemDict["target"]);
        GetNode<OptionButton>("UsableLabel/UsableButton").Selected = Convert.ToInt32(newItemDict["usable"]);
        GetNode<SpinBox>("HitLabel/HitBox").Value = Convert.ToInt32(newItemDict["success"]);
        GetNode<OptionButton>("TypeLabel/TypeButton").Selected = Convert.ToInt32(newItemDict["hit_type"]);
        GetNode<OptionButton>("DamageLabel/DTypeLabel/DTypeButton").Selected = Convert.ToInt32(newItemDict["damage_type"]);
        GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").Selected = Convert.ToInt32(newItemDict["element"]);
        GetNode<LineEdit>("DamageLabel/DFormulaLabel/FormulaText").Text = newItemDict["formula"] as string;
        database_editor.Close();
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
    private void _on_AddItem_pressed()
    {
        GetNode<OptionButton>("ItemButton").AddItem("NewItem");
        int id = GetNode<OptionButton>("ItemButton").GetItemCount() - 1;
        Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Item.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
		Godot.Collections.Dictionary item_data = new Godot.Collections.Dictionary();
		item_data.Add("name", "NewItem");
		item_data.Add("icon", "");
		item_data.Add("description", "New created item");
		item_data.Add("item_type", 0);
		item_data.Add("price", 10);
		item_data.Add("consumable", 0);
		item_data.Add("target", 3);
		item_data.Add("usable", 0);
		item_data.Add("success", 95);
		item_data.Add("hit_type", 1);
		item_data.Add("damage_type", 1);
		item_data.Add("element", 0);
		item_data.Add("formula", "10");
		jsonDictionary.Add("item" + id, item_data);
		database_editor.Open("res://databases/Item.json", Godot.File.ModeFlags.Write);
		database_editor.StoreLine(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_ItemSaveButton_pressed()
    {
        _save_item_data();
        _refresh_data(item_selected);
    }
    private void _save_item_data()
    {
		Godot.File database_editor = new Godot.File();
		database_editor.Open("res://databases/Item.json", Godot.File.ModeFlags.Read);
		string jsonAsText = database_editor.GetAsText();
		JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        database_editor.Close();
		Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary finalData = jsonDictionary["item"+item_selected] as Godot.Collections.Dictionary;
		finalData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		GetNode<OptionButton>("ItemButton").SetItemText(item_selected, GetNode<LineEdit>("NameLabel/NameText").Text);
		finalData["icon"] = icon_path;
		finalData["description"] = GetNode<TextEdit>("DescLabel/DescText").Text;
        finalData["item_type"] = GetNode<OptionButton>("ItemTypeLabel/ItemTypeButton").Selected;
		finalData["price"] = GetNode<SpinBox>("PriceLabel/PriceBox").Value;
		finalData["consumable"] = GetNode<OptionButton>("ConsumableLabel/ConsumableButton").Selected;
		finalData["target"] = GetNode<OptionButton>("TargetLabel/TargetButton").Selected;
		finalData["usable"] = GetNode<OptionButton>("UsableLabel/UsableButton").Selected;
		finalData["success"] = GetNode<SpinBox>("HitLabel/HitBox").Value;
		finalData["hit_type"] = GetNode<OptionButton>("TypeLabel/TypeButton").Selected;
		finalData["damage_type"] = GetNode<OptionButton>("DamageLabel/DTypeLabel/DTypeButton").Selected;
		finalData["element"] = GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").Selected;
		finalData["formula"] = GetNode<LineEdit>("DamageLabel/DFormulaLabel/FormulaText").Text;
        database_editor.Open("res://databases/Item.json", Godot.File.ModeFlags.Write);
		database_editor.StoreString(JSON.Print(jsonDictionary));
		database_editor.Close();
    }

    private void _on_ItemButton_item_selected(int id)
    {
        item_selected = id;
        _refresh_data(id);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
