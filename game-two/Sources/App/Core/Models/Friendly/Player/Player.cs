using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

using Controls;

public class Player : KinematicBody2D
{
	// Const
	private const int POULPE_POS = 960;

	private const string MONSTRE = "Monstre";
	private const string VOID = "Void";
	private const string END = "End";

	private const string PORTAL = "Portal";
	private const string PORTAL_NODE_PATH = "../End/";
	private const string HEAD = "Head";

	
	// Player properties
	private const int PLAYER_SPEED = 400;
	private const int JUMP_SPEED = 7000;
	private const int GRAVITY = 1200;

	/// <summary> Velocity of player, used for player movements
	private Vector2 _velocity;

	public Vector2 Velocity
	{
		get
		{
			return this._velocity;
		}
		set
		{
			this._velocity = value;
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


	// Player status	
	private TouchScreenButton.Direction direction;

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

	private bool _isHit;
	public bool IsHit
	{
		get
		{
			return this._isHit;
		}
		set
		{
			this._isHit = value;
		}
	}
	

	// Signals
	[Signal]
	delegate void Flip(bool flip_dir);
	[Signal]
	delegate void Anim(string anim);


	// UI ellements
	private Label _hangingLabel;
	private Label _healthLabel;


	public override void _Ready()
	{
		// Signals connections
		this.Connect("Flip", GetNode("Head"), "Flip");
		this.Connect("Anim", GetNode("Head"), "Anim");

		// Set status to default value
		this.Velocity = new Vector2();
		this.direction = TouchScreenButton.Direction.Null;
		this.HangingStatus = false;
		this.HangStat = true;
		this.AllowedHanging = false;
		this.IsHit = false;
		this.Health = 50;

		// Get UI ellements
		_hangingLabel = ((Label) this.GetParent().GetNode("GUI").GetNode("Node2D").GetNode("Hanging"));
		_healthLabel = ((Label) this.GetParent().GetNode("GUI").GetNode("Node2D").GetNode("Health"));


		// Add new tentacule
		this.TentaculeArray = new List<Tentacule>();
		
		AddNewTentacule(true);
		AddNewTentacule(false);

		for (int i = 0; i <= this.TentaculeArray.Count - 1; i++)
		{
			TentaculeArray[i].AddNewPixBlock();
			TentaculeArray[i].AddNewPixBlock();
		}
	}

	// Signals methods 
	public void _OnTouchScreenButtonDirectionJoy(TouchScreenButton.Direction direction)
	{
		this.direction = direction;
	}

	public void _on_Player_body_entered(KinematicBody2D body)
	{
		if(body.Name.Contains(MONSTRE) && ((Monstre) body).IsAttack)
		{
			this.IsHit = true;
			this.Health -= 25;
		}
		if(body.Name == VOID)
		{
			this.Health -= this.Health;
		}
		if(body.Name == END)
		{
			this.Visible = false;
			((EndPortal) this.GetNode(PORTAL_NODE_PATH + PORTAL)).WinStatus = true;
		}
	}

	public void _on_Head_animation_finished()
	{
		if(((Head) this.GetNode(HEAD)).Animation.Contains(Animations.HeadAnimations.HitAnimation.ToString()))
		{
			this.IsHit = false;
		}
		if(((Head) this.GetNode(HEAD)).Animation.Contains(Animations.HeadAnimations.DeathAnimation.ToString()))
		{
			GetTree().ReloadCurrentScene();
		}
	}

	public void _on_Portal_animation_finished()
	{
		if(((EndPortal) this.GetNode(PORTAL_NODE_PATH + PORTAL)).Animation.Contains(Animations.PortalAnimations.PortalEndAnimation.ToString()))
		{
			GetTree().ReloadCurrentScene();
		}
	}

	public void GetJoystickStatus(bool state)
	{
		this.JoystickStatus = state;
	}


	// Godot Process methods
	public override void _Process(float delta){
		_hangingLabel.Text = "Hanging: " + this.HangStat;
		_healthLabel.Text = "Health: " + this.Health;

		// Button for activation/desactivation of hanging
		bool hanging = Input.IsActionPressed(UI.ui_hanging.ToString());

		if(hanging){
			this.HangStat = !this.HangStat;
		}

	}

	public override void _PhysicsProcess(float delta)
	{
		MoveIt(delta);
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

				if(!((finalPos.x >= 150 && finalPos.x <= 350) && (finalPos.y >= 775 && finalPos.y <= 975)) && !this.HangingStatus)
				{
					if(finalPos.x > POULPE_POS + 40){			
						if(this.TentaculeArray[i].IsPositionRight)	
						{
							Vector2 circleCenter = this.TentaculeArray[i].PixBlockArray[0].Position;
							float radius = this.TentaculeArray[i].PixBlockArray[this.TentaculeArray[i].PixBlockArray.Count-1].Position.x - this.TentaculeArray[i].PixBlockArray[0].Position.x;

							finalPos = finalPos - globalTentPos;
							if(this.HangStat)
							{
								HangUp(finalPos, lastFinalPos, this.TentaculeArray[i], circleCenter, radius);
							}
							finalPos = lastFinalPos;
						}
					}
					else if(finalPos.x < POULPE_POS - 40)
					{
						if(!this.TentaculeArray[i].IsPositionRight)
						{
							Vector2 circleCenter = this.TentaculeArray[i].PixBlockArray[0].Position;
							float radius = this.TentaculeArray[i].PixBlockArray[0].Position.x - this.TentaculeArray[i].PixBlockArray[this.TentaculeArray[i].PixBlockArray.Count-1].Position.x;
							
							finalPos = finalPos - globalTentPos;
							if(this.HangStat)
							{
								HangUp(finalPos, lastFinalPos, this.TentaculeArray[i], circleCenter, radius);
								finalPos = lastFinalPos;
							}
						}
					}
				}
			}
		}
	}
	
	public void AddNewTentacule(bool tentaculePosition)
	{
		var tentacule = new Tentacule(tentaculePosition);

		this.AddChild(tentacule);
		this.TentaculeArray.Add(tentacule);

		Vector2 pos = this.Position;
		
		if(tentaculePosition)
		{
			tentacule.Position = new Vector2(100, 50);
		}
		else if(!tentaculePosition)
		{
			tentacule.Position = new Vector2(-100, 50);
		}
		else
		{
			GD.Print("Error => Argument 1 invalid => Please use Right or Left");
		}
		
		tentacule.AddNewPixBlock();
	}
	

	/// <summary> Method used for player physic's movements operations
	public void MoveIt(float delta)
	{
		_velocity.x = 0;

		// Input directions
		bool right = Input.IsActionPressed(Controls.UI.ui_right.ToString());
		bool left = Input.IsActionPressed(Controls.UI.ui_left.ToString());
		bool jump = Input.IsActionPressed(Controls.UI.ui_space.ToString());
		
		// Modification of player movement depending on the input
		if (right || direction == TouchScreenButton.Direction.Droite)
		{
			EmitSignal(nameof(Flip), true);
			_velocity.x += PLAYER_SPEED;
		}
		if (left || direction == TouchScreenButton.Direction.Gauche)
		{
			EmitSignal(nameof(Flip), false);
			_velocity.x -= PLAYER_SPEED;
		}
		
		// Addition of gravity
		_velocity.y += delta * GRAVITY;

		// Move the player
		_velocity = MoveAndSlide(_velocity, new Vector2(0, -1));

		ChangeAnimation();
	}


	public void ChangeAnimation()
	{
		// Moddification of animations
		if(this.Health <= 0)
		{
			for(int i = 0; i <= this.TentaculeArray.Count - 1; i++)
			{
				this.TentaculeArray[i].Visible = false;
			}

			EmitSignal(nameof(Anim), Animations.HeadAnimations.DeathAnimation.ToString());
		}
		else
		{
			if(this.IsHit)
			{
				EmitSignal(nameof(Anim), Animations.HeadAnimations.HitAnimation.ToString());
				SetTentaculePosWhenFlip("idle");
			}
			else
			{
				if (_velocity.x > 0 || _velocity.x < 0)
				{
					EmitSignal(nameof(Anim), Animations.HeadAnimations.RunAnimation.ToString());
					SetTentaculePosWhenFlip("run");
				}
				else
				{
					EmitSignal(nameof(Anim), Animations.HeadAnimations.IdleAnimation.ToString());
					SetTentaculePosWhenFlip("idle");
				}
			}
		}
	}

	public void SetTentaculePosWhenFlip(string anim){
		for(int i = 0; i <= this.TentaculeArray.Count - 1; i++)
		{
			var tentacule = this.TentaculeArray[i];
			Vector2 tentPos = tentacule.Position;
			Vector2 pos = this.Position;
			Vector2 offSet = new Vector2(0, 0);

			if(anim == "run")
			{
				if(direction == TouchScreenButton.Direction.Gauche)
				{
					if(tentacule.IsPositionRight)
					{
						offSet = new Vector2(50, 50);
					}
					else if(!tentacule.IsPositionRight)
					{
						offSet = new Vector2(-100, 50);
					}
				}
				else if(direction == TouchScreenButton.Direction.Droite)
				{
					if(tentacule.IsPositionRight)
					{
						offSet = new Vector2(100, 50);
					}
					else if(!tentacule.IsPositionRight)
					{
						offSet = new Vector2(-50, 50);
					}
				}
			}
			else if(anim == "idle")
			{
				if(tentacule.IsPositionRight)
				{
					offSet = new Vector2(100, 50);
				}
				else if(!tentacule.IsPositionRight)
				{
					offSet = new Vector2(-100, 50);
				}
			}

			tentacule.Position = new Vector2(offSet);
		}
	}
	
	public void HangUp(Vector2 finalPos, Vector2 lastFinalPos, Tentacule tentacule, Vector2 circleCenter, float radius)
	{
		this.HangingStatus = true;
		// Calculation of the distance between the tentacle origin and the clicked point
		double exp = Math.Sqrt(Math.Pow(finalPos.x - circleCenter.x,2)+Math.Pow(finalPos.y-circleCenter.y,2));
		// GD.Print("Expression => " + exp);
		
		if(exp >= radius)
		{
			if(exp >= radius*3)
			{
				//If exp > 3*TentaculeLenght then we force final point to to the max distance (max distance = radius)
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

				finalPos.x = circleCenter.x + (float) dx;
				finalPos.y = circleCenter.y - (float) dy;
			}

			//Starting Hanging Animation
			animationController.Play(Animations.PlayerAnimations.HangingAnimation.ToString(), tentacule, finalPos);
		}
	}
}