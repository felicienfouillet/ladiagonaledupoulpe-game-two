using Godot;
using System;

public class EndPortal : AnimatedSprite
{
    private bool _winStatus;
    public bool WinStatus
    {
        get
        {
            return this._winStatus;
        }
        set
        {
            this._winStatus = value;
        }
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.WinStatus = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(!this.WinStatus)
        {
            Play(Animations.PortalAnimations.PortalIdleAnimation.ToString());
        }
        else
        {
            Play(Animations.PortalAnimations.PortalEndAnimation.ToString());
        }
    }
}
