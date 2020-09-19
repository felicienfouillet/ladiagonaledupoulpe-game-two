using Godot;
using System;

public class HealthBar : TextureProgress
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    Player player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = ((Player) this.GetParent().GetParent().GetParent().GetNode("Player"));

        Set("max_value", player.Health);
		Value = (float)Get("max_value");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Value = player.Health;
    }
}
