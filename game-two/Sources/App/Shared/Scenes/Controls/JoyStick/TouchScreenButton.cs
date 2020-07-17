using Godot;
using System;

public class TouchScreenButton : Godot.TouchScreenButton
{
    private int _zone = 100;
    private Vector2 _radius = new Vector2(75, 75);

    private bool _isPressedArea;
    private float _tolerenceJoy = 1f;
    private float _distanceDuCentre = 0;
    // private Color _pasTransparent = new Color(1, 1, 1, 1);
    // private Color _presqueTransparent = new Color(1, 1, 1, 0.2f);

    private Color _transparency = new Color(1, 1, 1, 1);
    
    [Signal]
    delegate void DirectionJoy(string dir);
    [Signal]
    delegate void JoyState(bool state);

    private Timer _timerTransparence;
    private float _delaisTranparence = 5f;

    private Vector2 _positionParent = new Vector2(0, 0);

    private Vector2 _inputEventPosition;
    private Vector2 _positionRelativeToScene;

    private Vector2 _playerPosition;

    public Vector2 PlayerPosition
    {
        get
        {
            return this._playerPosition;
        }
        set
        {
            this._playerPosition = value;
        }
    }

    public enum Direction
    {
        Droite,
        Gauche,
        Haut,
        Bas,
        Null
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _isPressedArea = false;

        //gestion du timer
        //modifier la variable delaisTranparence pour gerer, en seconde, le temps avant mise en transparence du joy
        //Axe d'amélioration : repositioner le timer dans le parent, et gerer desactivation/activation via signaux completement.
        _timerTransparence = new Timer();
        _timerTransparence.WaitTime = _delaisTranparence;
        _timerTransparence.OneShot = true;
        _timerTransparence.Connect("timeout", this, nameof(StartTransparency));
        AddChild(_timerTransparence);

        //pour mettre une transparence a 0.2
        //les réglages de l'alpha sont au niveau des 2 variables pasTransparent et presqueTransparent qui sont de type Color. 
        StartTransparency();


        _positionParent = ((Sprite)GetParent()).Position;
        Position = new Vector2(0, 0) - _radius;

        this.Connect(nameof(JoyState), GetNode("../../../Player"), "GetJoystickStatus");
        this.Connect(nameof(DirectionJoy), GetNode("../../../Player"), "_OnTouchScreenButtonDirectionJoy");
    }

    public Vector2 getJoyPos()
    {
        return this.Position + _radius;
    }

    public void GetPlayerPosition(Vector2 position)
    {
        this.PlayerPosition = position;
    }


    public override void _Input(InputEvent inputEvent)
    {
        if ((inputEvent is InputEventScreenDrag))
        {
            StopTransparency();

            _inputEventPosition = ((Vector2) inputEvent.Get("position"));
			_positionRelativeToScene = _positionParent;
        }
        if ((inputEvent is InputEventScreenTouch) && (!inputEvent.IsPressed()))
        {
            //GD.Print("LACHÉ");
            Position = new Vector2(0, 0) - _radius;
            EmitSignal(nameof(DirectionJoy), Direction.Null);
            _isPressedArea = false;
            EmitSignal(nameof(JoyState), _isPressedArea);
            _timerTransparence.Start();
        }

        if (((inputEvent is InputEventScreenTouch && inputEvent.IsPressed()) || inputEvent is InputEventScreenDrag) && (((_inputEventPosition.x < _positionParent.x + (_zone * _tolerenceJoy)) && (_inputEventPosition.x > _positionParent.x + (_zone * _tolerenceJoy) * (-1))) && (_inputEventPosition.y < _positionParent.y + (_zone * _tolerenceJoy) && _inputEventPosition.y > _positionParent.y + (_zone * _tolerenceJoy) * (-1))))
        {
            // GD.Print("PRESSED");
            _isPressedArea = true;
            EmitSignal(nameof(JoyState), _isPressedArea);
        }


        if ((inputEvent is InputEventScreenDrag) && _isPressedArea)
        {
            GlobalPosition = (Vector2)inputEvent.Get("position") - (_radius * this.Scale);
			// GD.Print("Global  position = > " + GlobalPosition);

            float directionH = getJoyPos().x;
            float directionV = getJoyPos().y;
            // GD.Print("getJoyPos",getJoyPos());
            
			if (directionV < _zone/2*-1 )
			{
				// GD.Print("Direction : HAUT");
				EmitSignal(nameof(DirectionJoy), Direction.Haut);
			}
            if (directionV > _zone/2 )
			{
			 	// GD.Print("Direction : BAS");
				EmitSignal(nameof(DirectionJoy), Direction.Bas);
			}
            if (directionH > _zone / 2)
            {
                // GD.Print("Direction : DROITE");
                EmitSignal(nameof(DirectionJoy), Direction.Droite);
            }
            if (directionH < _zone / 2 * (-1))
            {
                // GD.Print("Direction : GAUCHE");
                EmitSignal(nameof(DirectionJoy), Direction.Gauche);
            }


            //pour pas que le joy parte de sa zone
            if (getJoyPos().Length() > _zone)
            {
                Position = new Vector2((getJoyPos().Normalized() * _zone) - _radius);
            }
        }
    }

    public void StartTransparency()
    {
        _transparency = new Color(1, 1, 1, 0.2f);

        ChangeTransparency(_transparency);
    }

    public void StopTransparency()
    {
        _transparency = new Color(1, 1, 1, 1);

        ChangeTransparency(_transparency);
    }

    public void ChangeTransparency(Color transparency)
    {
        GetParent().Set("modulate", transparency);
        GetNode("../../../GUI/btnB").Set("modulate", transparency);
    }
}