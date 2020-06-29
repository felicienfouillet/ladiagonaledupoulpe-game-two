using Godot;
using System;

public class Monstre : KinematicBody2D
{

    private AnimatedSprite _monstre;

    private const int MONSTRE_SPEED = 250;
    private const int MONSTRE_GRAVITY = 500;

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

    private bool _attackStatus;

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

    private Tween _tween;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _tween = new Tween();
        _tween.Repeat = true;
        this.AddChild(_tween);
        _tween.InterpolateCallback(this, 2f, nameof(ChangeDirection));
        _tween.Start();

        this.Direction = false;
        
        _monstre = ((AnimatedSprite) this.GetChild(0));
        this.Health = 25;
        this._attackStatus = false;
        _isOnGround = false;
    }

    public void _on_Monstre_body_entered(KinematicBody2D body)
    {
        if(body.Name == "Player")
        {
            this._attackStatus = true;
            this.Player = ((Player) body);
            this.Player.Health -= 25;
        }
    }

    public void _on_Monstre_body_exited(KinematicBody2D body)
    {
        if(body.Name == "Player")
        {
            this._attackStatus = false;
        }
    }

    public void _on_AnimatedSprite_animation_finished()
    {
        if(_monstre.Animation == "MonstreDeath")
        {
            this.QueueFree();
        }
        // if(_monstre.Animation == "MonstreAttack")
        // {
        //     this._attackStatus = false;
        // }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(float delta)
    {
        _velocity = new Vector2();
        _velocity.x = 0;

        _velocity.y += (MONSTRE_GRAVITY * 100) * delta;
        
        if(!this._attackStatus)
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
            _tween.Stop(this);
            _monstre.Play("MonstreDeath");
        }
        else
        {
            if((_velocity.x > 0 || _velocity.x < 0) && !this._attackStatus)
            {
                _monstre.Offset = new Vector2(0, 0);
                _monstre.Play("MonstreRun");
            }
            else if(this._attackStatus)
            {
                _monstre.Offset = new Vector2(0, 25);
                _monstre.Play("MonstreAttack");
            }
            else
            {
                _monstre.Offset = new Vector2(0, 50);
                _monstre.Play("MonstreIdle");
            }

            _velocity = MoveAndSlide(_velocity, new Vector2(0, -1));
        }
    }

    public void ChangeDirection()
    {
        this.Direction = !this.Direction;
    }
}
