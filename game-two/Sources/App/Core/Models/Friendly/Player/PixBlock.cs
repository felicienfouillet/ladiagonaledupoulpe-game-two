using Godot;
using System;

public class PixBlock : Node2D
{
    private AnimatedSprite animSprite;

	[Export]
	private Vector2 PIXBLOCK_GRAVITY = new Vector2(0, 200);

    private Vector2 _velocity;

    private Tentacule _parent;

    private Player _player;

    public override void _Ready()
    {
        animSprite = ((AnimatedSprite) GetNode("AnimatedSprite"));

        _parent = ((Tentacule) this.GetParent());

        _player = ((Player) _parent.GetParent());
        
        _velocity = new Vector2();
    }

    public void _on_Pixblock_body_entered(KinematicBody2D body)
    {
        if(this.Name == "LastPixBlock")
        {
            if(body.Name.Contains("Monstre") && _player.HangingStatus)
            {
                ((Monstre) body).Health -= 25;
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
        
        // if(this.Name != "FirstPixBlock")
        // {
        //     //_velocity = (_velocity + PIXBLOCK_GRAVITY) * delta;

        //     //_velocity = _player.Velocity;
        //     for(int i = 1; i <= _parent.PixBlockArray.Count - 1; i++)
        //     {
        //         if(_parent.PixBlockArray[i].Position <= (_parent.PixBlockArray[i-1].Position + new Vector2(0, -150)))
        //         {
        //             //MoveAndCollide(_velocity);
        //             // _velocity = MoveAndSlide(_velocity, new Vector2(0, -1));
        //         }
        //     }
        // }else{
        //      _velocity = _player.Velocity;
        // }

        // _velocity = MoveAndSlide(_velocity, new Vector2(0, -1));

        animSprite.Play("pixBlockAnim");        
    }
}
