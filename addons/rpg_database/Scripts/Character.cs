using Godot;
using System;

[Tool]
public class Character : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string facePath = "";
    string charaPath = "";
    int characterSelected = 0;
    int initialEquipEdit = -1;
    Godot.Collections.Array<int> equipIdArray = new Godot.Collections.Array<int>();
    Godot.Collections.Array<String> equipEditArray = new Godot.Collections.Array<String>();
    Godot.Collections.Array<int> initialEquipIdArray = new Godot.Collections.Array<int>();
    // Called when the node enters the scene tree for the first time.
    public void Start()
    {
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary charaData = jsonDictionary["chara" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("CharacterButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("CharacterButton").AddItem(charaData["name"].ToString());
            }
            else
            {
                GetNode<OptionButton>("CharacterButton").SetItemText(i, charaData["name"].ToString());
            }
        }
        databaseFile.Close();
        RefreshData(0);
    }
    public void RefreshData(int id)
    {
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary charaData = jsonDictionary["chara" + id] as Godot.Collections.Dictionary;
        GetNode<OptionButton>("CharacterButton").SetItemText(id, charaData["name"].ToString());
        GetNode<LineEdit>("NameLabel/NameText").Text = charaData["name"].ToString();
        string face = charaData["faceImage"].ToString();
        if (face != "")
        {
            GetNode<Sprite>("FaceLabel/FaceSprite").Texture = GD.Load(charaData["faceImage"].ToString()) as Godot.Texture;
        }
        GetNode<SpinBox>("InitLevelLabel/InitLevelText").Value = Convert.ToInt32(charaData["initialLevel"]);
        GetNode<SpinBox>("MaxLevelLabel/MaxLevelText").Value = Convert.ToInt32(charaData["maxLevel"]);

        databaseFile.Close();
        databaseFile.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        jsonAsText = databaseFile.GetAsText();
        jsonParsed = JSON.Parse(jsonAsText);
        jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary wtypeDictionary = jsonDictionary["weapons"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary atypeDictionary = jsonDictionary["armors"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary equipSlotsDictionary = jsonDictionary["slots"] as Godot.Collections.Dictionary;

        Godot.Collections.Dictionary etypeDictionary = charaData["equip_types"] as Godot.Collections.Dictionary;
        GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").Clear();
        equipIdArray.Clear();
        string equip_name;
        foreach (string equip in etypeDictionary.Keys)
        {
            string kind = equip[0].ToString();
            string type_id = etypeDictionary[equip].ToString();
            equipIdArray.Add(Convert.ToInt32(etypeDictionary[equip]));
            switch (kind)
            {
                case "w":
                    string w_id = equip.Remove(0, 1);
                    equip_name = "W: " + wtypeDictionary[type_id].ToString();
                    GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem(equip_name);
                    break;
                case "a":
                    string a_id = equip.Remove(0, 1);
                    equip_name = "A: " + atypeDictionary[type_id].ToString();
                    GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem(equip_name);
                    break;
            }

        }

        databaseFile.Close();
        databaseFile.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
        jsonAsText = databaseFile.GetAsText();
        jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary weaponList = jsonParsed.Result as Godot.Collections.Dictionary;

        databaseFile.Close();
        databaseFile.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
        jsonAsText = databaseFile.GetAsText();
        jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary armorList = jsonParsed.Result as Godot.Collections.Dictionary;

        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/TypeList").Clear();
        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").Clear();
        Godot.Collections.Dictionary initialEquipData = charaData["initial_equip"] as Godot.Collections.Dictionary;
        initialEquipIdArray.Clear();
        foreach (string equip in equipSlotsDictionary.Keys)
        {
            GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/TypeList").AddItem(equipSlotsDictionary[equip].ToString());
            string kind = equip[0].ToString();
            int kind_id = Convert.ToInt32(equip.Remove(0, 1));
            
            switch (kind)
            {
                case "w":
                    int w_id = -1;
                    if (kind_id < initialEquipData.Keys.Count)
                    {
                        w_id = Convert.ToInt32(initialEquipData[kind_id.ToString()]);
                    }
                    
                    initialEquipIdArray.Add(w_id);
                    if (w_id >= 0)
                    {
                        Godot.Collections.Dictionary weaponData = weaponList["weapon" + w_id] as Godot.Collections.Dictionary;
                        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").AddItem(weaponData["name"].ToString());
                    }
                    else
                    {
                        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").AddItem("None");
                    }
                    break;
                case "a":
                    int a_id = -1;
                    if (kind_id < initialEquipData.Keys.Count)
                    {
                        a_id = Convert.ToInt32(initialEquipData[kind_id.ToString()]);
                    }
                    
                    initialEquipIdArray.Add(a_id);
                    if (a_id >= 0)
                    {
                        Godot.Collections.Dictionary armorData = armorList["armor" + a_id] as Godot.Collections.Dictionary;
                        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").AddItem(armorData["name"].ToString());
                    }
                    else
                    {
                        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").AddItem("None");
                    }
                    break;
            }
        }
        
        databaseFile.Close();
        databaseFile.Open("res://databases/Class.json", Godot.File.ModeFlags.Read);
        jsonAsText = databaseFile.GetAsText();
        jsonParsed = JSON.Parse(jsonAsText);
        jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary classData = jsonDictionary["class" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("ClassLabel/ClassText").GetItemCount())
            {
                GetNode<OptionButton>("ClassLabel/ClassText").AddItem(classData["name"].ToString());
            }
            else
            {
                GetNode<OptionButton>("ClassLabel/ClassText").SetItemText(i, classData["name"].ToString());
            }
        }

        databaseFile.Close();
    }

    public void _on_CharaSaveButton_pressed()
    {
        SaveCharacterData();
        RefreshData(characterSelected);
    }

    private void _on_AddButton_pressed()
    {
        GetNode<OptionButton>("CharacterButton").AddItem("NewCharacter");
        int id = GetNode<OptionButton>("CharacterButton").GetItemCount() - 1;
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        databaseFile.Close();
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary characterData = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary etypeData = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary einitData = new Godot.Collections.Dictionary();
        characterData.Add("faceImage", "res://");
        characterData.Add("charaImage", "res://");
        characterData.Add("name", "NewCharacter");
        characterData.Add("class", 0);
        characterData.Add("initialLevel", 1);
        characterData.Add("maxLevel", 99);
        etypeData.Add("w0", 0);
        etypeData.Add("w1", 1);
        etypeData.Add("a2", 0);
        etypeData.Add("a3", 3);
        einitData.Add("0", -1);
        einitData.Add("1", -1);
        einitData.Add("2", -1);
        einitData.Add("3", -1);
        characterData.Add("initial_equip", einitData);
        characterData.Add("equip_types", etypeData);
        jsonDictionary.Add("chara" + id, characterData);
        databaseFile.Open("res://databases/Character.json", Godot.File.ModeFlags.Write);
        databaseFile.StoreString(JSON.Print(jsonDictionary));
        databaseFile.Close();
    }
    private void _on_Search_pressed()
    {
        GetNode<FileDialog>("FaceLabel/FaceSearch").PopupCentered();
    }

    private void _on_FaceSearch_file_selected(string file)
    {
        facePath = file;
        SetFaceImage(file);
    }

    private void SetFaceImage(string path)
    {
        GetNode<Sprite>("FaceLabel/FaceSprite").Texture = GD.Load(path) as Godot.Texture;
    }
    public void SaveCharacterData()
    {
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        databaseFile.Close();
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary charaData = jsonDictionary["chara" + characterSelected] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary equipTypeData = charaData["equip_types"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary initialEquipData = charaData["initial_equip"] as Godot.Collections.Dictionary;

        charaData["faceImage"] = facePath;
        charaData["charaImage"] = "";
        charaData["name"] = GetNode<LineEdit>("NameLabel/NameText").Text;
        GetNode<OptionButton>("CharacterButton").SetItemText(characterSelected, GetNode<LineEdit>("NameLabel/NameText").Text);
        charaData["class"] = GetNode<OptionButton>("ClassLabel/ClassText").Selected;
        charaData["initialLevel"] = GetNode<SpinBox>("InitLevelLabel/InitLevelText").Value;
        charaData["maxLevel"] = GetNode<SpinBox>("MaxLevelLabel/MaxLevelText").Value;

        equipTypeData.Clear();
        int equip_items = GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").GetItemCount();
        for (int i = 0; i < equip_items; i++)
        {
            string kind = GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").GetItemText(i)[0].ToString();
            switch (kind)
            {
                case "W":
                    equipTypeData["w" + i] = equipIdArray[i];
                    break;
                case "A":
                    equipTypeData["a" + i] = equipIdArray[i];
                    break;
            }
        }

        charaData["equip_types"] = equipTypeData;

        int slot_items = GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").GetItemCount();
        for (int i = 0; i < slot_items; i++)
        {
            initialEquipData[i.ToString()] = Convert.ToInt32(initialEquipIdArray[i]);
        }

        charaData["initial_equip"] = initialEquipData;

        databaseFile.Open("res://databases/Character.json", Godot.File.ModeFlags.Write);
        databaseFile.StoreString(JSON.Print(jsonDictionary));
        databaseFile.Close();
    }

    private void _on_CharacterButton_item_selected(int id)
    {
        characterSelected = id;
        RefreshData(id);
    }

    private void _on_CancelButton_pressed()
    {
        GetNode<WindowDialog>("EquipLabel/AddEquip").Hide();
    }

    private void _on_AddEquipTypeButton_pressed()
    {
        GetNode<WindowDialog>("EquipLabel/AddEquip").PopupCentered();

        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary wtypeData = jsonDictionary["weapons"] as Godot.Collections.Dictionary;
        for (int i = 0; i < wtypeData .Count; i++)
        {
            if (i > GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").AddItem(wtypeData[i.ToString()].ToString());
            }
            else
            {
                GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").SetItemText(i, wtypeData[i.ToString()].ToString());
            }
        }

        databaseFile.Close();
    }

    private void _on_RemoveEquipTypeButton_pressed()
    {
        int selected = GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").GetSelectedItems()[0];
        equipIdArray.RemoveAt(selected);
        GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").RemoveItem(selected);
    }

    private void _on_OkButton_pressed()
    {
        int kind = GetNode<OptionButton>("EquipLabel/AddEquip/TypeLabel/TypeButton").Selected;
        int item = GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").Selected;
        equipIdArray.Add(Convert.ToInt32(item));
        string itemText = GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").Text;
        switch (kind)
        {
            case 0:
                GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem("W: " + itemText);
                break;
            case 1:
                GetNode<ItemList>("EquipLabel/EquipContainer/EquipContainer/EquipList").AddItem("A: " + itemText);
                break;
        }
        GetNode<WindowDialog>("EquipLabel/AddEquip").Hide();
    }

    private void _on_TypeButton_item_selected(int index)
    {
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").Clear();
        switch (index)
        {
            case 0:
                Godot.Collections.Dictionary wTypeData = jsonDictionary["weapons"] as Godot.Collections.Dictionary;
                for (int i = 0; i < wTypeData.Count; i++)
                {
                    if (i > GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").GetItemCount() - 1)
                    {
                        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").AddItem(wTypeData[i.ToString()].ToString());
                    }
                    else
                    {
                        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").SetItemText(i, wTypeData[i.ToString()].ToString());
                    }
                }
                break;
            case 1:
                Godot.Collections.Dictionary aTypeData = jsonDictionary["armors"] as Godot.Collections.Dictionary;
                for (int i = 0; i < aTypeData.Count; i++)
                {
                    if (i > GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").GetItemCount() - 1)
                    {
                        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").AddItem(aTypeData[i.ToString()].ToString());
                    }
                    else
                    {
                        GetNode<OptionButton>("EquipLabel/AddEquip/EquipLabel/EquipButton").SetItemText(i, aTypeData[i.ToString()].ToString());
                    }
                }
                break;
        }
        databaseFile.Close();
    }

    private void _on_EquipList_item_activated(int index)
    {
        initialEquipEdit = index;
        equipEditArray.Add("-1");
        GetNode<Label>("InitialEquipLabel/InitialEquipChange/Label").Text = GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/TypeList").GetItemText(index);

        GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").Clear();
        GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").AddItem("None");
        Godot.File databaseFile = new Godot.File();
        databaseFile.Open("res://databases/Character.json", Godot.File.ModeFlags.Read);
        string jsonAsText = databaseFile.GetAsText();
        JSONParseResult jsonParsed = JSON.Parse(jsonAsText);
        Godot.Collections.Dictionary jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary charaData = jsonDictionary["chara" + characterSelected] as Godot.Collections.Dictionary;

        databaseFile.Close();
        databaseFile.Open("res://databases/System.json", Godot.File.ModeFlags.Read);
        jsonAsText = databaseFile.GetAsText();
        jsonParsed = JSON.Parse(jsonAsText);
        jsonDictionary = jsonParsed.Result as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary slotsData = jsonDictionary["slots"] as Godot.Collections.Dictionary;

        Godot.Collections.Dictionary equipTypesData = charaData["equip_types"] as Godot.Collections.Dictionary;
        if (slotsData.Contains("w" + index))
        {
            databaseFile.Close();
            databaseFile.Open("res://databases/Weapon.json", Godot.File.ModeFlags.Read);
            jsonAsText = databaseFile.GetAsText();
            jsonParsed = JSON.Parse(jsonAsText);
            Godot.Collections.Dictionary weaponList = jsonParsed.Result as Godot.Collections.Dictionary;

            Godot.Collections.Array<int> weaponArray = new Godot.Collections.Array<int>();
            foreach (string key in equipTypesData.Keys)
            {
                string kind = key[0].ToString();
                if (kind == "w")
                {
                    weaponArray.Add(Convert.ToInt32(equipTypesData[key]));
                }
            }

            foreach (string weapon in weaponList.Keys)
            {
                Godot.Collections.Dictionary weaponData = weaponList[weapon] as Godot.Collections.Dictionary;
                if (weaponArray.Contains(Convert.ToInt32(weaponData["weapon_type"])))
                {
                    equipEditArray.Add(weapon.Remove(0,6));
                    GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").AddItem(weaponData["name"].ToString());
                }
            }
            databaseFile.Close();
        }
        else if (slotsData.Contains("a" + index))
        {
            databaseFile.Close();
            databaseFile.Open("res://databases/Armor.json", Godot.File.ModeFlags.Read);
            jsonAsText = databaseFile.GetAsText();
            jsonParsed = JSON.Parse(jsonAsText);
            Godot.Collections.Dictionary armorList = jsonParsed.Result as Godot.Collections.Dictionary;

            Godot.Collections.Array<int> armorArray = new Godot.Collections.Array<int>();
            foreach (string key in equipTypesData.Keys)
            {
                string kind = key[0].ToString();
                if (kind == "a")
                {
                    armorArray.Add(Convert.ToInt32(equipTypesData[key]));
                }
            }

            foreach (string armor in armorList.Keys)
            {
                Godot.Collections.Dictionary armorData = armorList[armor] as Godot.Collections.Dictionary;
                if (armorArray.Contains(Convert.ToInt32(armorData["armor_type"])))
                {
                    equipEditArray.Add(armor.Remove(0,5));
                    GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").AddItem(armorData["name"].ToString());
                }
            }
            databaseFile.Close();
        }
        GetNode<WindowDialog>("InitialEquipLabel/InitialEquipChange").PopupCentered();
    }

    private void _on_CancelInitialEquipButton_pressed()
    {
        initialEquipEdit = -1;
        equipEditArray.Clear();
        GetNode<WindowDialog>("InitialEquipLabel/InitialEquipChange").Hide();
    }

    private void _on_OkInitialEquipButton_pressed()
    {
        string text = GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").Text;
        int selected = GetNode<OptionButton>("InitialEquipLabel/InitialEquipChange/Label/OptionButton").Selected;
        GetNode<ItemList>("InitialEquipLabel/PanelContainer/TypeContainer/EquipList").SetItemText(initialEquipEdit, text);
        initialEquipIdArray[initialEquipEdit] = Convert.ToInt32(equipEditArray[selected]);
        initialEquipEdit = -1;
        equipEditArray.Clear();
        GetNode<WindowDialog>("InitialEquipLabel/InitialEquipChange").Hide();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
