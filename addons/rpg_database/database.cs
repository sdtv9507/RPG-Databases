#if TOOLS
using Godot;
using System;

[Tool]
public class database : EditorPlugin
{
    PackedScene MainPanel = (PackedScene)ResourceLoader.Load("res://addons/rpg_database/Scenes/Base.tscn");
    Control instanced_scene;
    Button button = new Button();
    public override void _Ready()
    {
    }

    public override void _EnterTree()
    {   
        instanced_scene = (Control)MainPanel.Instance();
        button.Text = "Database";
        button.Connect("pressed",this,"_on_button_pressed");
        AddControlToContainer(0, button);
        AddChild(instanced_scene);
    }
    public override void _ExitTree()
    {
        RemoveChild(instanced_scene);
        RemoveControlFromContainer(0, button);
        if (instanced_scene != null) {
            instanced_scene.QueueFree();
        }
    }

    public void _on_button_pressed()
    {
        if (instanced_scene.Visible == false)
        {
            Vector2 pos = new Vector2(150, 150);
            instanced_scene.SetPosition(pos);
            instanced_scene.Show();
        }
        else
        {
            instanced_scene.Hide();
        }
    }
}
#endif