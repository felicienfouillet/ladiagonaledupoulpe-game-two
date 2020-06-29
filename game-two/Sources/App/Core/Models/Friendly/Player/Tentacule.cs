using Godot;
using System;
using System.Collections.Generic;
/// <summary>
/// Blabla
/// </summary>
public class Tentacule : KinematicBody2D
{
	private string _positionRelativeToPlayer;

	public string PositionRelativeToPlayer
	{
		get
		{
			return this._positionRelativeToPlayer;
		}

		set
		{
			this._positionRelativeToPlayer = value;
		}
	}

	private KinematicBody2D _pixBlock;
	
	private PackedScene _pixBlockScene;

	private List<PixBlock> _pixBlockArray;
	
	public List<PixBlock> PixBlockArray
	{
		get
		{
			return this._pixBlockArray;
		}
		set
		{
			this._pixBlockArray = value;
		}
	}

	private float _playerScale;

	public float PlayerScale
	{
		get
		{
			return this._playerScale;
		}
		set
		{
			this._playerScale = value;
		}
	}
	
	private SpriteFrames _aPix;
	
	public Tentacule(string positionRelativeToPlayer)
	{
		this.PositionRelativeToPlayer = positionRelativeToPlayer;
		this.PixBlockArray = new List<PixBlock>();
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pixBlockScene = ((PackedScene) ResourceLoader.Load("res://Sources/App/Core/Models/Friendly/Player/PixBlock.tscn"));
		Player player = (Player) this.GetParent();
		this.PlayerScale = player.Scale.x;
	}

	public override void _Draw()
	{
		// if(this.PositionRelativeToPlayer == "Right")
		// {
		// 	DrawCircle(new Vector2(this.PixBlockArray[0].Position.x, this.PixBlockArray[0].Position.y), (this.PixBlockArray[PixBlockArray.Count-1].Position.x - this.PixBlockArray[0].Position.x)+100, new Color(1, 0, 0, 1));
		// }
		// else
		// {
		// 	DrawCircle(new Vector2(this.PixBlockArray[0].Position.x, this.PixBlockArray[0].Position.y), (this.PixBlockArray[0].Position.x - this.PixBlockArray[PixBlockArray.Count-1].Position.x)+100, new Color(1, 0, 0, 1));
		// }
		
	} 

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		//ShowPixBlocks();
	}
	
	public void AddNewPixBlock(PixBlock pixBlock)
	{
		//_aPix = ResourceLoader.Load("res://Sources/App/Core/Models/Friendly/Player/PixBlock.tres") as SpriteFrames;

		pixBlock = ((PixBlock) _pixBlockScene.Instance());
		Vector2 pos = this.Position;
		
		//aPlayer.Frames = _aPix;
		float scaleX = 0.05f;
		float scaleY = 0.05f;
		pixBlock.Scale = new Vector2(scaleX, scaleY);
		
		//pixBlock.AddChild((AnimatedSprite) aPlayer);
		this.AddChild(pixBlock);
		this.PixBlockArray.Add(pixBlock);
		
		Vector2 pixPos = pixBlock.Position;
		
		var rng = new RandomNumberGenerator();
		rng.Randomize();
		float posY;
		
		if(this.PixBlockArray.IndexOf(pixBlock) == 0)
		{
			posY = 0;
		}
		else
		{
			posY = (rng.RandfRange(-5, 5));
		}
		
		if(this.PositionRelativeToPlayer == "Right")
		{
			pixBlock.Position = new Vector2(50*this.PixBlockArray.IndexOf(pixBlock), posY);
		}
		else if(this.PositionRelativeToPlayer == "Left")
		{
			pixBlock.Position = new Vector2(-(50*this.PixBlockArray.IndexOf(pixBlock)), posY);
		}
		else
		{
			GD.Print("Error => Argument 5 invalid => Right or Left");
		}
	}
}
