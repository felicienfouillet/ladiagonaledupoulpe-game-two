using Godot;
using System;

public class PixBlock : Node2D
{
    private AnimatedSprite _animSprite;

	private Vector2 _PixBlockGravity = new Vector2(0, 200);

    private Vector2 _velocity;

    private Tentacule _parent;

    private Player _player;

    public override void _Ready()
    {
        _animSprite = ((AnimatedSprite) GetNode("AnimatedSprite"));

        _parent = ((Tentacule) this.GetParent());

        _player = ((Player) _parent.GetParent());
        
        _velocity = new Vector2();
    }

    public void _on_Pixblock_body_entered(KinematicBody2D body)
    {
        if(this.Name == "LastPixBlock"  && _player.HangingStatus)
        {
            if(body.Name.Contains("Monstre"))
            {

                ((Monstre) body).Health -= 25;
                //_player.AllowedHanging = true;
            }

            if(body.Name == "AllowedHanging")
            {
                _player.AllowedHanging = true;
            }
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        _animSprite.Play("pixBlockAnim");        
    }
}
