using Godot;
using System;

public class PixBlock : Node2D
{
    private const string ANIMATED_SPRITE = "AnimatedSprite";
    private const string LAST_PIX_BLOCK = "LastPixBlock";
    private const string MONSTRE = "Monstre";
    private const string ALLOWED_HANGING = "AllowedHanging";

    private AnimatedSprite _animSprite;

    private Tentacule _parent;

    private Player _player;

    public override void _Ready()
    {
        _animSprite = ((AnimatedSprite) GetNode(ANIMATED_SPRITE));

        _parent = ((Tentacule) this.GetParent());

        _player = ((Player) _parent.GetParent());
    }

    public void _on_Pixblock_body_entered(KinematicBody2D body)
    {
        if(this.Name == LAST_PIX_BLOCK  && _player.HangingStatus)
        {
            if(body.Name.Contains(MONSTRE))
            {
                ((Monstre) body).Health -= 25;
                ((Monstre) body).IsHit = true;
            }

            if(body.Name == ALLOWED_HANGING)
            {
                _player.AllowedHanging = true;
            }
        }
    }
}