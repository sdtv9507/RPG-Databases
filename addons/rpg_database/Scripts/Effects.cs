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

    }

    private void _on_AddEffect_pressed()
    {
        addNewEffect = true;
        GetNode<WindowDialog>("AddEffect").PopupCentered();
    }

    private void _on_EditEffect_pressed()
    {
        if (GetNode<ItemList>("Tabs/Effects/EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetSelectedItems()[0] > -1)
        {
            addNewEffect = false;
            GetNode<WindowDialog>("AddEffect").PopupCentered();
        }
    }

    private void _on_RemoveEffect_pressed()
    {
        int selectedEffect = GetNode<ItemList>("Tabs/Effects/EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").GetSelectedItems()[0];
        if (selectedEffect > -1)
        {
            GetNode<ItemList>("Tabs/Effects/EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").RemoveItem(selectedEffect);
        }
    }

    private void _on_ClearEffects_pressed()
    {
        GetNode<ItemList>("Tabs/Effects/EffectLabel/PanelContainer/VBoxContainer/Effects/EffectNames").Clear();
        GetNode<ItemList>("Tabs/Effects/EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue1").Clear();
        GetNode<ItemList>("Tabs/Effects/EffectLabel/PanelContainer/VBoxContainer/Effects/EffectValue2").Clear();
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
