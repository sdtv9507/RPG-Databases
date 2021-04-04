using Godot;
using System;

public class Enemy : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    string graphicsPath = "";
    int enemySelected = 0;
    Godot.Collections.Array<int> dropIdArray = new Godot.Collections.Array<int>();
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
                    int i_id = 0;
                    dropIdArray.Add(i_id);
                    Godot.Collections.Dictionary itemData = itemList["item" + i_id] as Godot.Collections.Dictionary;
                    GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").AddItem(itemData["name"].ToString());
                    break;
                case "w":
                    int w_id = 0;
                    dropIdArray.Add(w_id);
                    Godot.Collections.Dictionary weaponData = weaponList["weapon" + w_id] as Godot.Collections.Dictionary;
                    GetNode<ItemList>("DropsLabel/DropsContainer/VBoxContainer/HBoxContainer/DropsList").AddItem(weaponData["name"].ToString());
                    break;
                case "a":
                    int a_id = 0;
                    dropIdArray.Add(a_id);
                    Godot.Collections.Dictionary armorData = armorList["armor" + a_id] as Godot.Collections.Dictionary;
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
                jsonDictionary["Enemy"+enemyId] = jsonDictionary["Enemy"+(enemyId+1)];
                enemyId+= 1;
            }
            jsonDictionary.Remove("Enemy"+enemyId);
            this.GetParent().GetParent().Call("StoreData", "Enemy", jsonDictionary);
            GetNode<OptionButton>("EnemyButton").RemoveItem(enemySelected);
            if (enemySelected == 0)
            {
                GetNode<OptionButton>("EnemyButton").Select(enemySelected+1);
                enemySelected += 1;
            }
            else
            {
                GetNode<OptionButton>("EnemyButton").Select(enemySelected-1);
                enemySelected -= 1;
            }
            GetNode<OptionButton>("EnemyButton").Select(enemySelected);
            RefreshData(enemySelected);
        }
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
