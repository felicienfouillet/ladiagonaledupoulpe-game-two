using Godot;
using System;

public class Head : AnimatedSprite
{	
	private bool _flip_direction;
	private string _animation;

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

		Play(_animation);
	}
	
	public void Flip(bool flip_dir)
	{
		this._flip_direction = flip_dir;
	}
	
	public void Anim(string anim)
	{
		this._animation = anim;
	}
}