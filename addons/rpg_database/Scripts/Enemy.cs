using Godot;
using System;

public class Enemy : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string graphicsPath = "";
    int enemySelected = 0;
    int statEdit = -1;
    Godot.Collections.Array<String> dropIdArray = new Godot.Collections.Array<String>();
    // Called when the node enters the scene tree for the first time.
    public void Start()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Enemy") as Godot.Collections.Dictionary;
        for (int i = 0; i < jsonDictionary.Count; i++)
        {
            Godot.Collections.Dictionary enemyData = jsonDictionary["enemy" + i] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("EnemyButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("EnemyButton").AddItem(enemyData["name"] as string);
            }
            else
            {
                GetNode<OptionButton>("EnemyButton").SetItemText(i, enemyData["name"] as string);
            }
        }
        RefreshData(enemySelected);
    }

    public void RefreshData(int id)
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Enemy") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary enemyData = jsonDictionary["enemy" + id] as Godot.Collections.Dictionary;

        jsonDictionary = this.GetParent().GetParent().Call("ReadData", "System") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary systemStatsData = jsonDictionary["stats"] as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary itemList = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary weaponList = this.GetParent().GetParent().Call("ReadData", "Weapon") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armorList = this.GetParent().GetParent().Call("ReadData", "Armor") as Godot.Collections.Dictionary;

        GetNode<LineEdit>("NameLabel/NameLine").Text = enemyData["name"] as string;
        string graphic = enemyData["graphicImage"] as string;
        if (graphic != "")
        {
            GetNode<Sprite>("GraphicLabel/Graphic").Texture = GD.Load(enemyData["graphicImage"] as string) as Godot.Texture;
        }

        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatList").Clear();
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaList").Clear();
        for (int i = 0; i < systemStatsData.Count; i++)
        {
            string statName = systemStatsData[i.ToString()] as string;
            Godot.Collections.Dictionary enemyStatFormula = enemyData["stat_list"] as Godot.Collections.Dictionary;
            string statFormula = "";
            if (enemyStatFormula.Contains(statName))
            {
                statFormula = enemyStatFormula[statName].ToString();
            }
            else
            {
                statFormula = "level * 5";
            }
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatList").AddItem(statName);
            GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaList").AddItem(statFormula);
        }

        Godot.Collections.Dictionary dropList = enemyData["drop_list"] as Godot.Collections.Dictionary;

        GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").Clear();
        GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/ChanceList").Clear();
        dropIdArray.Clear();
        foreach (string drop in dropList.Keys)
        {
            string kind = drop[0].ToString();
            string kind_id = drop.Remove(0, 1);
            switch (kind)
            {
                case "i":
                    dropIdArray.Add(drop);
                    Godot.Collections.Dictionary itemData = itemList["item" + kind_id] as Godot.Collections.Dictionary;
                    GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").AddItem(itemData["name"].ToString());
                    break;
                case "w":
                    dropIdArray.Add(drop);
                    Godot.Collections.Dictionary weaponData = weaponList["weapon" + kind_id] as Godot.Collections.Dictionary;
                    GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").AddItem(weaponData["name"].ToString());
                    break;
                case "a":
                    dropIdArray.Add(drop);
                    Godot.Collections.Dictionary armorData = armorList["armor" + kind_id] as Godot.Collections.Dictionary;
                    GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").AddItem(armorData["name"].ToString());
                    break;
            }
            GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/ChanceList").AddItem(dropList[drop].ToString());
        }
        GetNode<SpinBox>("ExpLabel/ExpSpin").Value = Convert.ToInt32(enemyData["experience"]);
        GetNode<SpinBox>("GoldLabel/GoldSpin").Value = Convert.ToInt32(enemyData["money"]);
    }

    private void _on_AddEnemy_pressed()
    {
        GetNode<OptionButton>("EnemyButton").AddItem("NewEnemy");
        int id = GetNode<OptionButton>("EnemyButton").GetItemCount() - 1;
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Enemy") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary enemyData = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary statsData = new Godot.Collections.Dictionary();
        Godot.Collections.Dictionary dropData = new Godot.Collections.Dictionary();

        enemyData.Add("name", "Slime");
        enemyData.Add("graphicImage", "");
        statsData.Add("hp", "150");
        statsData.Add("mp", "50");
        statsData.Add("atk", "18");
        statsData.Add("def", "16");
        statsData.Add("int", "8");
        statsData.Add("res", "4");
        statsData.Add("spd", "12");
        statsData.Add("luk", "10");
        dropData.Add("i0", 80);
        enemyData.Add("experience", 6);
        enemyData.Add("money", 50);
        enemyData.Add("stat_list", statsData);
        enemyData.Add("drop_list", dropData);
        jsonDictionary.Add("enemy" + id, enemyData);
        this.GetParent().GetParent().Call("StoreData", "Enemy", jsonDictionary);
    }

    private void _on_RemoveEnemy_pressed()
    {
        Godot.Collections.Dictionary jsonDictionary = this.GetParent().GetParent().Call("ReadData", "Enemy") as Godot.Collections.Dictionary;
        if (jsonDictionary.Keys.Count > 1)
        {
            int enemyId = enemySelected;
            while (enemyId < jsonDictionary.Keys.Count - 1)
            {
                jsonDictionary["Enemy" + enemyId] = jsonDictionary["Enemy" + (enemyId + 1)];
                enemyId += 1;
            }
            jsonDictionary.Remove("Enemy" + enemyId);
            this.GetParent().GetParent().Call("StoreData", "Enemy", jsonDictionary);
            GetNode<OptionButton>("EnemyButton").RemoveItem(enemySelected);
            if (enemySelected == 0)
            {
                GetNode<OptionButton>("EnemyButton").Select(enemySelected + 1);
                enemySelected += 1;
            }
            else
            {
                GetNode<OptionButton>("EnemyButton").Select(enemySelected - 1);
                enemySelected -= 1;
            }
            GetNode<OptionButton>("EnemyButton").Select(enemySelected);
            RefreshData(enemySelected);
        }
    }

    private void _on_SearchGraphic_pressed()
    {
        GetNode<FileDialog>("EnemyGraphic").PopupCentered();
    }

    private void _on_EnemyGraphic_file_selected(String path)
    {
        graphicsPath = path;
        GetNode<Sprite>("GraphicLabel/Graphic").Texture = GD.Load(path) as Godot.Texture;
    }

    private void _on_FormulaList_item_activated(int index)
    {
        string statName = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/StatList").GetItemText(index);
        string statFormula = GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaList").GetItemText(index);
        GetNode<Label>("Stat").Text = statName;
        GetNode<LineEdit>("Formula").Text = statFormula;
        statEdit = index;
        GetNode<WindowDialog>("StatsEdit").PopupCentered();
    }

    private void _on_EnemyStatEditorOkButton_pressed()
    {
        string statFormula = GetNode<LineEdit>("Formula").Text;
        GetNode<ItemList>("StatsLabel/StatsContainer/DataContainer/FormulaList").SetItemText(statEdit, statFormula);
        //SaveEnemyData();
        statEdit = -1;
        GetNode<WindowDialog>("StatsEdit").Hide();
    }

    private void _on_EnemyStatEditorCancelButton_pressed()
    {
        statEdit = -1;
        GetNode<WindowDialog>("StatsEdit").Hide();
    }

    private void _on_AddDrop_pressed()
    {
        GetNode<OptionButton>("DropEdit/Type/OptionButton").Select(0);
        Godot.Collections.Dictionary itemData = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;

        for (int i = 0; i < itemData.Count; i++)
        {
            Godot.Collections.Dictionary itemName = itemData["item" + i.ToString()] as Godot.Collections.Dictionary;
            if (i > GetNode<OptionButton>("DropEdit/Drop/OptionButton").GetItemCount() - 1)
            {
                GetNode<OptionButton>("DropEdit/Drop/OptionButton").AddItem(itemName["name"].ToString());
            }
            else
            {
                GetNode<OptionButton>("DropEdit/Drop/OptionButton").SetItemText(i, itemName["name"].ToString());
            }
        }
        GetNode<WindowDialog>("DropEdit").PopupCentered();
    }

    private void _on_RemoveDrop_pressed()
    {

    }

    private void _on_DropType_item_selected(int index)
    {
        Godot.Collections.Dictionary itemData = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary weaponData = this.GetParent().GetParent().Call("ReadData", "Weapon") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armorData = this.GetParent().GetParent().Call("ReadData", "Armor") as Godot.Collections.Dictionary;
        GetNode<OptionButton>("DropEdit/Drop/OptionButton").Clear();
        switch (index)
        {
            case 0:
                for (int i = 0; i < itemData.Count; i++)
                {
                    Godot.Collections.Dictionary itemName = itemData["item" + i.ToString()] as Godot.Collections.Dictionary;
                    if (i > GetNode<OptionButton>("DropEdit/Drop/OptionButton").GetItemCount() - 1)
                    {
                        GetNode<OptionButton>("DropEdit/Drop/OptionButton").AddItem(itemName["name"].ToString());
                    }
                    else
                    {
                        GetNode<OptionButton>("DropEdit/Drop/OptionButton").SetItemText(i, itemName["name"].ToString());
                    }
                }
                break;
            case 1:
                for (int i = 0; i < weaponData.Count; i++)
                {
                    Godot.Collections.Dictionary weaponName = weaponData["weapon" + i.ToString()] as Godot.Collections.Dictionary;
                    if (i > GetNode<OptionButton>("DropEdit/Drop/OptionButton").GetItemCount() - 1)
                    {
                        GetNode<OptionButton>("DropEdit/Drop/OptionButton").AddItem(weaponName["name"].ToString());
                    }
                    else
                    {
                        GetNode<OptionButton>("DropEdit/Drop/OptionButton").SetItemText(i, weaponName["name"].ToString());
                    }
                }
                break;
            case 2:
                for (int i = 0; i < armorData.Count; i++)
                {
                    Godot.Collections.Dictionary armorName = armorData["armor" + i.ToString()] as Godot.Collections.Dictionary;
                    if (i > GetNode<OptionButton>("DropEdit/Drop/OptionButton").GetItemCount() - 1)
                    {
                        GetNode<OptionButton>("DropEdit/Drop/OptionButton").AddItem(armorName["name"].ToString());
                    }
                    else
                    {
                        GetNode<OptionButton>("DropEdit/Drop/OptionButton").SetItemText(i, armorName["name"].ToString());
                    }
                }
                break;
        }
    }

    private void _on_DropEditOkButton_pressed()
    {
        Godot.Collections.Dictionary itemList = this.GetParent().GetParent().Call("ReadData", "Item") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary weaponList = this.GetParent().GetParent().Call("ReadData", "Weapon") as Godot.Collections.Dictionary;
        Godot.Collections.Dictionary armorList = this.GetParent().GetParent().Call("ReadData", "Armor") as Godot.Collections.Dictionary;
        
        int id = GetNode<OptionButton>("DropEdit/Type/OptionButton").GetSelectedId();
        int selected_id = GetNode<OptionButton>("DropEdit/Drop/OptionButton").GetSelectedId();
        int chance = Convert.ToInt32(GetNode<SpinBox>("DropEdit/Chance/SpinBox").Value);

        switch (id)
        {
            case 0:
                dropIdArray.Add("i" + selected_id);
                Godot.Collections.Dictionary itemData = itemList["item" + selected_id] as Godot.Collections.Dictionary;
                GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").AddItem(itemData["name"].ToString());
                break;
            case 1:
                dropIdArray.Add("w" + selected_id);
                Godot.Collections.Dictionary weaponData = weaponList["weapon" + selected_id] as Godot.Collections.Dictionary;
                GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").AddItem(weaponData["name"].ToString());
                break;
            case 2:
                dropIdArray.Add("a" + selected_id);
                Godot.Collections.Dictionary armorData = armorList["armor" + selected_id] as Godot.Collections.Dictionary;
                GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").AddItem(armorData["name"].ToString());
                break;
        }
        GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/ChanceList").AddItem(chance.ToString());
        GetNode<WindowDialog>("DropEdit").Hide();
    }

    private void _on_DropEditCancelButton_pressed()
    {
        GetNode<WindowDialog>("DropEdit").Hide();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
