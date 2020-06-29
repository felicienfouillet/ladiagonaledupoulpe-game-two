using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : KinematicBody2D
{
	private List<Tentacule> _tentaculeArray;
	
	public List<Tentacule> TentaculeArray
	{
		get
		{
			return this._tentaculeArray;
		}
		set
		{
			this._tentaculeArray = value;
		}
	}

	private int _health;
    public int Health
    {
        get
        {
            return this._health;
        }
        set
        {
            this._health = value;
        }
    }
	
	private const int SPEED = 750;
	private const int JUMP_SPEED = 7000;
	private const int GRAVITY = 500;
	
	private Vector2 velocity;
	private bool jumping;
	public bool Jumping
	{
		get
		{
			return jumping;
		}
		set
		{
			this.jumping = value;
		}
	}
	private string direction;

	private AnimationController animationController;

	private bool _joystickStatus;
	public bool JoystickStatus
	{
		get
		{
			return _joystickStatus;
		}
		set
		{
			this._joystickStatus = value;
		}
	}
	private bool _hangingStatus;
	public bool HangingStatus
	{
		get
		{
			return _hangingStatus;
		}
		set
		{
			this._hangingStatus = value;
		}
	}

	private bool _hangStat;
	public bool HangStat
	{
		get
		{
			return _hangStat;
		}
		set
		{
			this._hangStat = value;
		}
	}

	private bool _allowedHanging;

	public bool AllowedHanging
	{
		get
		{
			return _allowedHanging;
		}
		set
		{
			this._allowedHanging = value;
		}
	}

	private Vector2 _initialPosition;
	public Vector2 InitialPosition
	{
		get
		{
			return _initialPosition;
		}
		set
		{
			this._initialPosition = value;
		}
	}

	private Label _hangingLabel;
	private Label _healthLabel;
	
	[Signal]
	delegate void Flip(bool flip_dir);
	[Signal]
	delegate void Anim(string anim);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Jumping = false;
		this.HangingStatus = false;
		this.HangStat = true;
		this.AllowedHanging = false;

		this.Health = 50;

		_hangingLabel = ((Label) this.GetParent().GetNode("GUI").GetNode("Node2D").GetNode("Hanging"));
		_healthLabel = ((Label) this.GetParent().GetNode("GUI").GetNode("Node2D").GetNode("Health"));

		this.TentaculeArray = new List<Tentacule>();
		
		AddNewTentacule("Right");
		AddNewTentacule("Left");
		
		this.Connect("Flip", GetNode("Head"), "Flip");
		this.Connect("Anim", GetNode("Head"), "Anim");
		
		for (int i = 0; i <= this.TentaculeArray.Count - 1; i++)
		{
			TentaculeArray[i].AddNewPixBlock(new PixBlock());
			TentaculeArray[i].AddNewPixBlock(new PixBlock());
		}

		this.Position = this.GetViewport().Size/2;
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if(this.Health <= 0)
		{
			this.QueueFree();
			GetTree().ReloadCurrentScene();
		}
		_hangingLabel.Text = "Hanging: " + this.HangStat;
		_healthLabel.Text = "Health: " + this.Health;
		MoveIt(delta);
	}
	
	public void _OnTouchScreenButtonDirectionJoy(string direction)
	{
		GD.Print(direction);
		this.direction = direction;
	}
	public void GetJoystickStatus(bool state)
	{
		GD.Print(state);
		this._joystickStatus = state;
	}

	public override void _Input(InputEvent @event){
		if ((@event is InputEventScreenTouch) && @event.IsPressed()) 
		{
			var ctrans = GetCanvasTransform();
			var view_size = GetViewportRect().Size / ctrans.Scale;
			Vector2 offSet = new Vector2(0, -250);
			Vector2 camPos = (this.Position - offSet) - (view_size/2);
			Vector2 finalPos = new Vector2(((Vector2)@event.Get("position")).x,((Vector2)@event.Get("position")).y);
			Vector2 lastFinalPos = finalPos;
			
			for(int i = 0; i <= this.TentaculeArray.Count - 1; i++)
			{
				animationController = new AnimationController();
				AddChild(animationController);
					
				Vector2 tentPos = this.TentaculeArray[i].Position;
				Vector2 PixblockPos = this.TentaculeArray[i].PixBlockArray[this.TentaculeArray[i].PixBlockArray.Count-1].Position;
				Vector2 globalTentPos = ((view_size/2) + this.TentaculeArray[i].Position) - offSet;

				if((finalPos.x >= 150 && finalPos.x <= 350) && (finalPos.y >= 775 && finalPos.y <= 975) || (finalPos.x >= 1600 && finalPos.x <= 1800) && (finalPos.y >= 750 && finalPos.y <= 950))
				{
					GD.Print("Joystick clicked");
				}
				else if(!this.HangingStatus)
				{
					this.HangingStatus = true;
					if(finalPos.x > 1000){						
						if(this.TentaculeArray[i].PositionRelativeToPlayer == "Right")	
						{
							Vector2 circleCenter = this.TentaculeArray[i].PixBlockArray[0].Position;
							float radius = this.TentaculeArray[i].PixBlockArray[this.TentaculeArray[i].PixBlockArray.Count-1].Position.x - this.TentaculeArray[i].PixBlockArray[0].Position.x;

							finalPos = finalPos - globalTentPos;
							if(this.HangStat)
							{
								Hanging(finalPos, lastFinalPos, this.TentaculeArray[i], circleCenter, radius);
							}
							finalPos = lastFinalPos;
						}
					}
					else if(finalPos.x < 920)
					{
						if(this.TentaculeArray[i].PositionRelativeToPlayer == "Left")
						{
							Vector2 circleCenter = this.TentaculeArray[i].PixBlockArray[0].Position;
							float radius = this.TentaculeArray[i].PixBlockArray[0].Position.x - this.TentaculeArray[i].PixBlockArray[this.TentaculeArray[i].PixBlockArray.Count-1].Position.x;
							
							finalPos = finalPos - globalTentPos;
							if(this.HangStat)
							{
								Hanging(finalPos, lastFinalPos, this.TentaculeArray[i], circleCenter, radius);
								finalPos = lastFinalPos;
							}
						}
					}
					this.HangingStatus = false;
				}
			}
		}
	}

	public void Hanging(Vector2 finalPos, Vector2 lastFinalPos, Tentacule tentacule, Vector2 circleCenter, float radius)
	{
		double exp = Math.Sqrt(Math.Pow(finalPos.x - circleCenter.x,2)+Math.Pow(finalPos.y-circleCenter.y,2));
		// GD.Print("Expression => " + exp);
		
		if(exp >= radius)
		{
			if(exp >= radius*3)
			{
				double rX = radius*3;
				double d1 = finalPos.x - circleCenter.x;
				double d2 = finalPos.y - circleCenter.y;
								
				double d3 = Math.Sqrt((Math.Pow(d1,2) + Math.Pow(d2,2)));

				double dx = (d1*rX)/d3;
				double dy = Math.Sqrt(Math.Pow(rX,2) - Math.Pow(dx,2));

				if(lastFinalPos.y > 850)
				{
					dy = -Math.Sqrt(Math.Pow(rX,2) - Math.Pow(dx,2));
				}

				// GD.Print("dx => " + (circleCenter.x + dx));
				// GD.Print("dy => " + (circleCenter.y - dy));

				finalPos.x = circleCenter.x + (float) dx;
				finalPos.y = circleCenter.y - (float) dy;
			}
			// GD.Print("Final position 1 = > " + finalPos);
			animationController.Play("HangingAnimation", tentacule, finalPos);
		}
	}
	
	public void MoveIt(float delta)
	{
		velocity = new Vector2();

		velocity.x = 0;

		bool right = Input.IsActionPressed("ui_right");
		bool left = Input.IsActionPressed("ui_left");
		bool jump = Input.IsActionPressed("ui_space");
		bool hanging = Input.IsActionPressed("ui_hanging");
		
		if (right || direction == "droite")
		{
			EmitSignal(nameof(Flip), true);
			velocity.x += SPEED;
		}
		if (left || direction == "gauche")
		{
			EmitSignal(nameof(Flip), false);
			velocity.x -= SPEED;
		}

		// if (jump && IsOnFloor())
		// {
		// 	Jump();
		// }

		if(hanging){
			this.HangStat = !this.HangStat;
		}
		
		// if(!this.IsOnFloor()){
		velocity.y += delta * (GRAVITY*100);
		// }

		velocity = MoveAndSlide(velocity, new Vector2(0, -1));

		if (velocity.x > 0 || velocity.x < 0)
		{
			EmitSignal(nameof(Anim), "run");
			SetTentaculePosWhenFlip("run");
		}
		else
		{
			EmitSignal(nameof(Anim), "idle");
			SetTentaculePosWhenFlip("idle");
		}
		
		if (this.Jumping && IsOnFloor())
		{
			this.Jumping = false;
		}
	}

	public void Jump(){
		if(!this.Jumping)
		{
			this.Jumping = true;
			var tentaculeArrayLenght = this.TentaculeArray.Count;

			for(int i = 0; i <= tentaculeArrayLenght - 1; i++)
			{
				animationController = new AnimationController();
				AddChild(animationController);
				animationController.Play("JumpAnimation", this.TentaculeArray[i]);
			}
				var factor = this.TentaculeArray[0].PixBlockArray.Count;
				velocity.y -= JUMP_SPEED * factor;
		}
	}

	public void SetTentaculePosWhenFlip(string anim){
		for(int i = 0; i <= this.TentaculeArray.Count - 1; i++)
		{
			var tentacule = this.TentaculeArray[i];
			Vector2 tentPos = tentacule.Position;
			Vector2 pos = this.Position;
			if(anim == "run")
			{
				if(direction == "gauche")
				{
					if(tentacule.PositionRelativeToPlayer == "Right")
					{
						tentacule.Position = new Vector2(50, 50);
					}
					else if(tentacule.PositionRelativeToPlayer == "Left")
					{
						tentacule.Position = new Vector2(-100, 50);
					}
				}
				else if(direction == "droite")
				{
					if(tentacule.PositionRelativeToPlayer == "Right")
					{
						tentacule.Position = new Vector2(100, 50);
					}
					else if(tentacule.PositionRelativeToPlayer == "Left")
					{
						tentacule.Position = new Vector2(-50, 50);
					}
				}
			}
			else if(anim == "idle")
			{
				if(tentacule.PositionRelativeToPlayer == "Right")
				{
					tentacule.Position = new Vector2(100, 50);
				}
				else if(tentacule.PositionRelativeToPlayer == "Left")
				{
					tentacule.Position = new Vector2(-100, 50);
				}
			}
		}
	}
	
	public void AddNewTentacule(String tentaculePosition){
		var tentacule = new Tentacule(tentaculePosition);

		this.AddChild(tentacule);
		this.TentaculeArray.Add(tentacule);

		Vector2 pos = this.Position;
		
		if(tentaculePosition == "Right"){
			tentacule.Position = new Vector2(100, 50);
			// tentacule.Position = new Vector2(pos.x-800, pos.y-725);
		}else if(tentaculePosition == "Left"){
			tentacule.Position = new Vector2(-100, 50);
		}else{
			GD.Print("Error => Argument 1 invalid => Please use Right or Left");
		}
		
		tentacule.AddNewPixBlock(new PixBlock());
	}
}
