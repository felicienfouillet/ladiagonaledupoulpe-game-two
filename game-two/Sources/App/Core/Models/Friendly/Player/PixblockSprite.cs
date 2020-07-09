using Godot;
using System;

public class PixblockSprite : AnimatedSprite
{
    private Player _player;
    private const string _playerNodePath = "../../../../Player";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = (Player) this.GetNode(_playerNodePath);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(_player.Border)
		{
            this.Play(Animations.PixBlockAnimations.WhitePixBlockAnimation.ToString());
		}
		else
		{
            this.Play(Animations.PixBlockAnimations.PixBlockAnimation.ToString());
		}   
    }
}
