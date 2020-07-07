using Godot;
	using System;
	using System.Collections;
	using System.Collections.Generic;

	using Shared.Tools.ExtensionMethods.ToGodot;

public class AnimationController : Node2D
{
	private Tentacule _tentacule;

	private AnimationPlayer _animationPlayer;
	private JumpAnimation _jumpAnimation;
	private HangingAnimation _hangingAnimation;

	[Signal]
	delegate void JumpAnimationSignal(Tentacule tent);
	[Signal]
	delegate void HangingAnimationSignal(Tentacule tent, Vector2 finalPos);

	public AnimationController()
	{
		SetAnimationPlayer();
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InstanciateAnimations();

		this.Connect("JumpAnimationSignal", _jumpAnimation, "GetTantacule");
		this.Connect("HangingAnimationSignal", _hangingAnimation, "GetTantacule");
	}

	public void SetAnimationPlayer(){
		_animationPlayer = new AnimationPlayer();
		this.AddChild(_animationPlayer);
	}

	public void InstanciateAnimations(){
		_jumpAnimation = new JumpAnimation();
		this.AddChild(_jumpAnimation);

		_hangingAnimation = new HangingAnimation();
		this.AddChild(_hangingAnimation);
	}

	public void AddNewAnimationByMethods(Node anim, String name, int trackIdx, float lenght, bool loop)
	{
		Animation animation = new Animation();

		animation.Length = lenght;
		animation.Loop = loop;

		animation.AddTrack(Animation.TrackType.Method);
		animation.TrackSetPath(trackIdx, anim.GetPath());

		_animationPlayer.AddAnimation(name, animation);
	}
	
	public void SetAnimationTracks(string anim, int trackIdx, float timing, string methods)
	{
		Animation animation = _animationPlayer.GetAnimation(anim);
		animation.TrackInsertKey(trackIdx, timing, methods.ToDictionnary());
	}

	public void Play(string animation, Tentacule tentacule, params object[] args)
	{
		if (tentacule == null)
		{
			GD.Print("[Error] incorrect argument 2: a tentacule is required to start animation.");
		}
		else
		{
			GD.Print("[AnimationController] Loading animation...");
			switch(animation)
			{
				case "JumpAnimation":
					AddNewAnimationByMethods(_jumpAnimation, "JumpAnimation", 0, 0.5f, false);
					
					SetAnimationTracks("JumpAnimation", 0, 0f, "StartJumpAnimation");
					SetAnimationTracks("JumpAnimation", 0, 0.1f, "SetMiddleJumpAnimation");
					SetAnimationTracks("JumpAnimation", 0, 0.2f, "SetLastJumpPosition");
					SetAnimationTracks("JumpAnimation", 0, 0.3f, "SetFirstReturnJumpAnimation");
					SetAnimationTracks("JumpAnimation", 0, 0.4f, "SetSecondReturnJumpAnimation");
					SetAnimationTracks("JumpAnimation", 0, 0.5f, "SetJumpingOf");

					EmitSignal(nameof(JumpAnimationSignal), tentacule);
					_animationPlayer.Play("JumpAnimation");

					break;
				case "HangingAnimation":
					AddNewAnimationByMethods(_hangingAnimation, "HangingAnimation", 0, 0.3f, false);
					
					SetAnimationTracks("HangingAnimation", 0, 0f, "StartHangingAnimation");
					SetAnimationTracks("HangingAnimation", 0, 0.2f, "SetHangingAnimationOf");

					EmitSignal(nameof(HangingAnimationSignal), tentacule, (Vector2) args[0]);
					_animationPlayer.Play("HangingAnimation");

					break;
				case "null":
					_tentacule = null;
					GD.Print("[AnimationController] No animation to load.");
					break;
				default:
					GD.Print("[Error] Unable to find animation: " + animation);
					break;
			}
		}
	}
}