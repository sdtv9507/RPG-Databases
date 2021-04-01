using Godot;
using System;

[Tool]
public class Item : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string iconPath = "";
    int itemSelected = 0;
    // Called when the node enters the scene tree for the first time.
    public void Start()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;
        
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary itemData = jsonDictionary["item"+i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ItemButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("ItemButton").AddItem(itemData["name"] as string);
            }else{
                GetNode<OptionButton>("ItemButton").SetItemText(i, itemData["name"] as string);
            }
        }
        
		jsonDictionary = this.GetParent().GetParent().Call("ReadData", "System") as Godot.Collections.Dictionary;
        
        Godot.Collections.Dictionary systemData = jsonDictionary["elements"] as Godot.Collections.Dictionary;
        for (int i = 0; i < systemData.Count; i++)
        {
            if (i > GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").AddItem(systemData[i.ToString()] as string);
            }else{
                GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").SetItemText(i, systemData[i.ToString()] as string);
            }
        }
        RefreshData(itemSelected);
    }

    private void RefreshData(int id)
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary itemData = jsonDictionary["item"+id] as Godot.Collections.Dictionary;
        GetNode<LineEdit>("NameLabel/NameText").Text = itemData["name"] as string;
        string icon = itemData["icon"] as string;
        if (icon != "")
        {
            GetNode<Sprite>("IconLabel/IconSprite").Texture = GD.Load(itemData["icon"] as string) as Godot.Texture;
        }
        GetNode<TextEdit>("DescLabel/DescText").Text = itemData["description"] as string;
        GetNode<OptionButton>("ItemTypeLabel/ItemTypeButton").Selected = Convert.ToInt32(itemData["item_type"]);
        GetNode<SpinBox>("PriceLabel/PriceBox").Value = Convert.ToInt32(itemData["price"]);
        GetNode<OptionButton>("ConsumableLabel/ConsumableButton").Selected = Convert.ToInt32(itemData["consumable"]);
        GetNode<OptionButton>("TargetLabel/TargetButton").Selected = Convert.ToInt32(itemData["target"]);
        GetNode<OptionButton>("UsableLabel/UsableButton").Selected = Convert.ToInt32(itemData["usable"]);
        GetNode<SpinBox>("HitLabel/HitBox").Value = Convert.ToInt32(itemData["success"]);
        GetNode<OptionButton>("TypeLabel/TypeButton").Selected = Convert.ToInt32(itemData["hit_type"]);
        GetNode<OptionButton>("DamageLabel/DTypeLabel/DTypeButton").Selected = Convert.ToInt32(itemData["damage_type"]);
        GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").Selected = Convert.ToInt32(itemData["element"]);
        GetNode<LineEdit>("DamageLabel/DFormulaLabel/FormulaText").Text = itemData["formula"] as string;
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
    private void _on_AddItem_pressed()
    {
        GetNode<OptionButton>("ItemButton").AddItem("NewItem");
        int id = GetNode<OptionButton>("ItemButton").GetItemCount() - 1;
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary itemData = new Godot.Collections.Dictionary();
		itemData.Add("name", "NewItem");
		itemData.Add("icon", "");
		itemData.Add("description", "New created item");
		itemData.Add("item_type", 0);
		itemData.Add("price", 10);
		itemData.Add("consumable", 0);
		itemData.Add("target", 3);
		itemData.Add("usable", 0);
		itemData.Add("success", 95);
		itemData.Add("hit_type", 1);
		itemData.Add("damage_type", 1);
		itemData.Add("element", 0);
		itemData.Add("formula", "10");
		jsonDictionary.Add("item" + id, itemData);
        this.GetParent().GetParent().Call("StoreData", "Item", jsonDictionary);
    }

    private void _on_RemoveItem_pressed()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;
        if (jsonDictionary.Keys.Count > 1)
        {
            int itemId = itemSelected;
            while (itemId < jsonDictionary.Keys.Count - 1)
            {
                jsonDictionary["item" + itemId] = jsonDictionary["item" + (itemId + 1)];
                itemId += 1;
            }
            jsonDictionary.Remove("item" + itemId);
            this.GetParent().GetParent().Call("StoreData", "Item", jsonDictionary);
            GetNode<OptionButton>("ItemButton").RemoveItem(itemSelected);
            if (itemSelected == 0)
            {
                GetNode<OptionButton>("ItemButton").Select(itemSelected + 1);
                itemSelected += 1;
            }
            else
            {
                GetNode<OptionButton>("ItemButton").Select(itemSelected - 1);
                itemSelected -= 1;
            }
            GetNode<OptionButton>("ItemButton").Select(itemSelected);
            RefreshData(itemSelected);
        }
    }

    private void _on_ItemSaveButton_pressed()
    {
        SaveItemData();
        RefreshData(itemSelected);
    }
    private void SaveItemData()
    {
		Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary itemData = jsonDictionary["item"+itemSelected] as Godot.Collections.Dictionary;
		itemData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
		GetNode<OptionButton>("ItemButton").SetItemText(itemSelected, GetNode<LineEdit>("NameLabel/NameText").Text);
		itemData["icon"] = iconPath;
		itemData["description"] = GetNode<TextEdit>("DescLabel/DescText").Text;
        itemData["item_type"] = GetNode<OptionButton>("ItemTypeLabel/ItemTypeButton").Selected;
		itemData["price"] = GetNode<SpinBox>("PriceLabel/PriceBox").Value;
		itemData["consumable"] = GetNode<OptionButton>("ConsumableLabel/ConsumableButton").Selected;
		itemData["target"] = GetNode<OptionButton>("TargetLabel/TargetButton").Selected;
		itemData["usable"] = GetNode<OptionButton>("UsableLabel/UsableButton").Selected;
		itemData["success"] = GetNode<SpinBox>("HitLabel/HitBox").Value;
		itemData["hit_type"] = GetNode<OptionButton>("TypeLabel/TypeButton").Selected;
		itemData["damage_type"] = GetNode<OptionButton>("DamageLabel/DTypeLabel/DTypeButton").Selected;
		itemData["element"] = GetNode<OptionButton>("DamageLabel/ElementLabel/ElementButton").Selected;
		itemData["formula"] = GetNode<LineEdit>("DamageLabel/DFormulaLabel/FormulaText").Text;
        this.GetParent().GetParent().Call("StoreData", "Item", jsonDictionary);
    }

    private void _on_ItemButton_item_selected(int id)
    {
        itemSelected = id;
        RefreshData(id);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
