using Godot;
using System;

using Animations;
public class Monstre : KinematicBody2D
{
    private AnimatedSprite _monstre;
	private const int MONSTRE_SPEED = 400;
    private const int MONSTRE_GRAVITY = 1200;

    private const string LAST_PIX_BLOCK = "LastPixBlock";
    private const string PLAYER = "Player";

    private Vector2 _velocity;

    private bool _direction;

    public bool Direction
    {
        get
        {
            return this._direction;
        }
        set
        {
            this._direction = value;
        }
    }

    private bool _isAttack;

    public bool IsAttack
    {
        get
        {
            return this._isAttack;
        }
        set
        {
            this._isAttack = value;
        }
    }


    private bool _isOnGround;

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

    private Player _player;
    public Player Player
    {
        get
        {
            return this._player;
        }
        set
        {
            this._player = value;
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

    private Tween _tween;

    public override void _Ready()
    {
        _tween = new Tween();
        _tween.Repeat = true;
        this.AddChild(_tween);
        _tween.InterpolateCallback(this, 2f, nameof(ChangeDirection));
        _tween.Start();

        this.IsHit = false;
        this.Direction = false;
        
        _monstre = ((AnimatedSprite) this.GetChild(0));
        this.Health = 50;
        this.IsAttack = false;
        _isOnGround = false;
    }

    public void _on_Monstre_body_entered(KinematicBody2D body)
    {
        if(body.Name == PLAYER)
        {
            if(this.Health > 0)
            {
                this.IsAttack = true;
            }
        }
        // if(body.Name == LAST_PIX_BLOCK)
        // {
        //     GD.Print("Monstre body entered => " + body.Name);
        //     this.IsHit = true;
        //     this.Health -= 25;
        // }
    }

    public void _on_Monstre_body_exited(KinematicBody2D body)
    {
        if(body.Name == PLAYER)
        {
            this.IsAttack = false;
        }
    }

    public void _on_AnimatedSprite_animation_finished()
    {
        if(_monstre.Animation == EnnemiesAnimations.MonstreDeath.ToString())
        {
            this.QueueFree();
        }
        if(_monstre.Animation == EnnemiesAnimations.MonstreAttack.ToString())
        {
            this.IsAttack = false;
        }
        if(_monstre.Animation == EnnemiesAnimations.MonstreHurt.ToString())
        {
            this.IsHit = false;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity = new Vector2();
        _velocity.x = 0;

        _velocity.y += (MONSTRE_GRAVITY * 100) * delta;
        
        if(!this.IsAttack)
        {
            if(!Direction)
            {
                _monstre.FlipH = false;
                _velocity.x -=MONSTRE_SPEED;
            }
            else
            {
                _monstre.FlipH = true;
                _velocity.x += MONSTRE_SPEED;
            }
        }
        
        if(this.Health <= 0)
        {
            if(this.GetChildren().Count >= 3)
            {
                this.GetChild(1).QueueFree();
                this.GetChild(2).QueueFree();
            }

            _tween.Stop(this);
            _monstre.Play(EnnemiesAnimations.MonstreDeath.ToString());
        }
        else
        {
            if(this.IsHit)
            {
                this._velocity.x = 0;
                _monstre.Offset = new Vector2(0, 25);
                _monstre.Play(EnnemiesAnimations.MonstreHurt.ToString());
            }
            else if((_velocity.x > 0 || _velocity.x < 0) && !this.IsAttack)
            {
                _monstre.Offset = new Vector2(0, 0);
                _monstre.Play(EnnemiesAnimations.MonstreRun.ToString());
            }
            else if(this.IsAttack)
            {
                _monstre.Offset = new Vector2(0, 25);
                _monstre.Play(EnnemiesAnimations.MonstreAttack.ToString());
            }
            else
            {
                _monstre.Offset = new Vector2(0, 50);
                _monstre.Play(EnnemiesAnimations.MonstreIdle.ToString());
            }

            _velocity = MoveAndSlide(_velocity, new Vector2(0, -1));
        }
    }

    public void ChangeDirection()
    {
        this.Direction = !this.Direction;
    }
}