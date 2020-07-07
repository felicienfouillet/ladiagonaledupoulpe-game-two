using Godot;
using System;

public class TouchScreenButton : Godot.TouchScreenButton
{
    private int zone = 100;
    private Vector2 radius = new Vector2(75, 75);

    private bool isPressedArea;
    private float tolerenceJoy = 1f;
    private float distanceDuCentre = 0;
    private Color pasTransparent = new Color(1, 1, 1, 1);
    private Color presqueTransparent = new Color(1, 1, 1, 0.2f);
    
    [Signal]
    delegate void directionJoy(string dir);
    [Signal]
    delegate void JoyState(bool state);

    private Timer timerTransparence;
    private float delaisTranparence = 5f;

    private Vector2 positionParent = new Vector2(0, 0);

    private Vector2 inputEventPosition;
    private Vector2 positionRelativeToScene;

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
        isPressedArea = false;

        //gestion du timer
        //modifier la variable delaisTranparence pour gerer, en seconde, le temps avant mise en transparence du joy
        //Axe d'amélioration : repositioner le timer dans le parent, et gerer desactivation/activation via signaux completement.
        timerTransparence = new Timer();
        timerTransparence.WaitTime = delaisTranparence;
        //GD.Print("duree : ",timerTransparence.Get("wait_time"));
        timerTransparence.OneShot = true;
        timerTransparence.Connect("timeout", this, "activeTransparence");
        AddChild(timerTransparence);

        //pour mettre une transparence a 0.2
        //les réglages de l'alpha sont au niveau des 2 variables pasTransparent et presqueTransparent qui sont de type Color. 
        activeTransparence();


        positionParent = ((Sprite)GetParent()).Position;
        Position = new Vector2(0, 0) - radius;

        this.Connect("JoyState", GetNode("../../../Player"), "GetJoystickStatus");
        this.Connect("directionJoy", GetNode("../../../Player"), "_OnTouchScreenButtonDirectionJoy");
    }

    public override void _Input(InputEvent inputEvent)
    {
        if ((inputEvent is InputEventScreenDrag))
        {
            enleveTransparence();

            inputEventPosition = ((Vector2) inputEvent.Get("position"));
			positionRelativeToScene = positionParent;
        }
        if ((inputEvent is InputEventScreenTouch) && (!inputEvent.IsPressed()))
        {
            //GD.Print("LACHÉ");
            Position = new Vector2(0, 0) - radius;
            EmitSignal(nameof(directionJoy), Direction.Null);
            isPressedArea = false;
            EmitSignal(nameof(JoyState), isPressedArea);
            timerTransparence.Start();
        }

        if (((inputEvent is InputEventScreenTouch && inputEvent.IsPressed()) || inputEvent is InputEventScreenDrag) && (((inputEventPosition.x < positionParent.x + (zone * tolerenceJoy)) && (inputEventPosition.x > positionParent.x + (zone * tolerenceJoy) * (-1))) && (inputEventPosition.y < positionParent.y + (zone * tolerenceJoy) && inputEventPosition.y > positionParent.y + (zone * tolerenceJoy) * (-1))))
        {
            //GD.Print("PRESSED");
            isPressedArea = true;
            EmitSignal(nameof(JoyState), isPressedArea);
        }


        if ((inputEvent is InputEventScreenDrag) && isPressedArea)
        {
            GlobalPosition = (Vector2)inputEvent.Get("position") - (radius * this.Scale);
			GD.Print("Global  position = > " + GlobalPosition);

            float directionH = getJoyPos().x;
            float directionV = getJoyPos().y;
            //GD.Print("getJoyPos",getJoyPos());
            
			if (directionV < zone/2*-1 )
			{
				// GD.Print("Direction : HAUT");
				EmitSignal(nameof(directionJoy), Direction.Haut);
			}
            if (directionV > zone/2 )
			{
			 	// GD.Print("Direction : BAS");
				EmitSignal(nameof(directionJoy), Direction.Bas);
			}
            if (directionH > zone / 2)
            {
                // GD.Print("Direction : DROITE");
                EmitSignal(nameof(directionJoy), Direction.Droite);
            }
            if (directionH < zone / 2 * (-1))
            {
                // GD.Print("Direction : GAUCHE");
                EmitSignal(nameof(directionJoy), Direction.Gauche);
            }


            //pour pas que le joy parte de sa zone
            if (getJoyPos().Length() > zone)
            {
                Position = new Vector2((getJoyPos().Normalized() * zone) - radius);
            }
        }
    }

    public Vector2 getJoyPos()
    {
        return this.Position + radius;
    }


    public void GetPlayerPosition(Vector2 position)
    {
        this.PlayerPosition = position;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    public void activeTransparence()
    {
        GetParent().Set("modulate", presqueTransparent);
        GetNode("../../../GUI/btnA").Set("modulate", presqueTransparent);
        GetNode("../../../GUI/btnB").Set("modulate", presqueTransparent);
    }

    public void enleveTransparence()
    {
        GetParent().Set("modulate", pasTransparent);
        GetNode("../../../GUI/btnA").Set("modulate", pasTransparent);
        GetNode("../../../GUI/btnB").Set("modulate", pasTransparent);
    }
}
