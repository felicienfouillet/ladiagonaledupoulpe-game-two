using Godot;
using System;

public class Poulpix : AnimatedSprite
{
	private int SPEED = 250;
	private Vector2 velocity;
	private string direction;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		moveIt(delta);
	}
	
	public void _on_TouchScreenButton_directionJoy(string direction)
	{
		GD.Print(direction);
		this.direction = direction;
	}
	
	public void moveIt(float delta)
	{
		velocity = new Vector2();
		
		if ((Input.IsActionPressed("ui_left")) || (direction == "gauche"))
		{
			velocity.x -= 1;
			FlipH = false;
		}
		if ((Input.IsActionPressed("ui_right")) || (direction == "droite"))
		{
			velocity.x += 1;
			FlipH = true;
		}
		if ((Input.IsActionPressed("ui_up")) || (direction == "haut"))
		{
			velocity.y -= 1;
		}
		if ((Input.IsActionPressed("ui_down")) || (direction == "bas"))
		{
			velocity.y += 1;
		}

		if (velocity.Length()>0)
		{
			velocity = velocity.Normalized() * SPEED;
		}

		Position += velocity * delta;
		
		if(velocity.Length() > 0)
		{
			Play("run");
		}else
		{
			Play("idle");
		}
	}
}
