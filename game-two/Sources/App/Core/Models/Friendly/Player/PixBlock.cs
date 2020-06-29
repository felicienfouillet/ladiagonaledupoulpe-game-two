using Godot;
using System;

public class PixBlock : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private AnimatedSprite animSprite;

    public override void _Ready()
    {
        animSprite = ((AnimatedSprite) GetNode("AnimatedSprite"));
    }

    public void _on_Pixblock_body_entered(KinematicBody2D body)
    {
        if(body.Name == "Monstre")
        {
            ((Monstre) body).Health -= 25;
        }

        if(body.Name == "AllowedHanging")
        {
            ((Player) GetParent().GetParent()).AllowedHanging = true;
        }

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        animSprite.Play("pixBlockAnim");
    }
}
