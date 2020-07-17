using Godot;
using System;

public class PixblockSprite : AnimatedSprite
{
    public override void _Ready()
    {

    }
    public override void _Process(float delta)
    {
        this.Play(Animations.PixBlockAnimations.PixBlockAnimation.ToString());  
    }
}