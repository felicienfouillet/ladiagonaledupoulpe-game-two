using Godot;
using System;

public class Head : AnimatedSprite
{	
	private bool _flip_direction;
	private string _animation;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		FlipIt();
	}
	
	public void FlipIt()
	{
		if (_flip_direction)
		{
			FlipH = false;
		}
		else
		{
			FlipH = true;
		}

		Play("White" + _animation);
	}
	
	public void Flip(bool flip_dir)
	{
		//GD.Print(flip_dir);
		this._flip_direction = flip_dir;
	}
	
	public void Anim(string anim)
	{
		//GD.Print(anim);
		this._animation = anim;
	}
}
