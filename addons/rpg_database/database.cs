#if TOOLS
using Godot;
using System;

[Tool]
public class database : EditorPlugin
{
    PackedScene MainPanel = (PackedScene)ResourceLoader.Load("res://addons/rpg_database/Scenes/Base.tscn");
    Control instanced_scene;
    public override void _Ready()
    {
        instanced_scene = (Control)MainPanel.Instance();
        GetEditorInterface().GetEditorViewport().AddChild(instanced_scene);
        MakeVisible(false);
    }

    public override void _ExitTree()
    {
        if (instanced_scene != null) {
            instanced_scene.QueueFree();
        }
    }

    public override bool HasMainScreen()
    {
        return true;
    }

    public override void MakeVisible(bool visible)
    {
        if (instanced_scene != null) {
            instanced_scene.Visible = visible;
        }
    }

    public override string GetPluginName() 
    {
        return "Database";
    }

    public override Texture GetPluginIcon()
    {
        return (Godot.Texture)ResourceLoader.Load("res://addons/rpg_database/icon.png");
    }
}
#endif