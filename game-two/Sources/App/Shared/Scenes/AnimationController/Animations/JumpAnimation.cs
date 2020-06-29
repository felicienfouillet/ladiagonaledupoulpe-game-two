using Godot;
using System;
using System.Collections.Generic;

using Shared.Tools.ExtensionMethods.ChangePosition;

public class JumpAnimation : Node
{
	private Tentacule _tentacule;
	private String _tentaculePosition;
	
	public Tentacule Tentacule
	{
		get
		{
			return this._tentacule;
		}
		
		set
		{
			this._tentacule = value;
		}
	}
	
	public String TentaculePosition
	{
		get
		{
			return this._tentaculePosition;
		}
		
		set
		{
			this._tentaculePosition = value;
		}
	}
	
	private AnimationController _parent;

	public void GetTantacule(Tentacule tentacule)
	{
		this.Tentacule = tentacule;
		this.TentaculePosition = tentacule.PositionRelativeToPlayer;
	}
	
	public JumpAnimation()
	{
		_parent = (AnimationController) this.GetParent();
	}
	
	public void StartJumpAnimation()
	{
		GD.Print("[JumpAnimation] Start animation...");
		List<PixBlock> pixBlockArray = this.Tentacule.PixBlockArray;

		for(int i = 1; i <= pixBlockArray.Count-1; i++)
		{
			PixBlock pixBlock = pixBlockArray[i];

			if(this.TentaculePosition == "Left")
			{
				pixBlock.Position = new Vector2(-25*i, 50*i);
			}
			else if(this.TentaculePosition == "Right")
			{
				pixBlock.Position = new Vector2(25*i, 50*i);
			}

			pixBlock.Position = new Vector2(-25*i, 50*i);
		}
	}
	
	public void SetMiddleJumpAnimation()
	{
		GD.Print("[JumpAnimation] Start middle jump animation...");
		
		List<PixBlock> pixBlockArray = this.Tentacule.PixBlockArray;
		
		for(int i = 0; i <= pixBlockArray.Count-1; i++)
		{
			PixBlock pixBlock = pixBlockArray[i];

			var rng = new RandomNumberGenerator();
			rng.Randomize();

			if(this.TentaculePosition == "Left")
			{
				pixBlock.Position = new Vector2(rng.RandfRange(-5, 5), 50*i);
			}
			else if(this.TentaculePosition == "Right")
			{
				pixBlock.Position = new Vector2(-rng.RandfRange(-5, 5), 50*i);
			}

		}
	}
	
	public void SetLastJumpPosition()
	{
		GD.Print("[JumpAnimation] Start last jump position animation...");
		
		List<PixBlock> pixBlockArray = this.Tentacule.PixBlockArray;

		for(int i = 0; i <= pixBlockArray.Count-1; i++)
		{
			PixBlock pixBlock = pixBlockArray[i];
			
			var rng = new RandomNumberGenerator();
			rng.Randomize();
			if(this.Tentacule.PositionRelativeToPlayer == "Right")
			{
				pixBlock.Position = new Vector2(rng.RandfRange(-5, 5), 150 + 50*i);
			}
			else if(this.Tentacule.PositionRelativeToPlayer == "Left")
			{
				pixBlock.Position = new Vector2(-rng.RandfRange(-5, 5), 150 + 50*i);
			}
		}
	}

	public void SetFirstReturnJumpAnimation()
	{
		GD.Print("[JumpAnimation] Start first return jump animation...");
		
		List<PixBlock> pixBlockArray = this.Tentacule.PixBlockArray;
		
		for(int i = 0; i <= pixBlockArray.Count-1; i++)
		{
			PixBlock pixBlock = pixBlockArray[i];
			
			var rng = new RandomNumberGenerator();
			rng.Randomize();
			if(this.Tentacule.PositionRelativeToPlayer == "Right")
			{
				pixBlock.Position = new Vector2(-rng.RandfRange(-5, 5), 50*i);
			}
			else if(this.Tentacule.PositionRelativeToPlayer == "Left")
			{
				pixBlock.Position = new Vector2(rng.RandfRange(-5, 5), 50*i);
			}
		}
	}

	public void SetSecondReturnJumpAnimation()
	{
		
		List<PixBlock> pixBlockArray = this.Tentacule.PixBlockArray;
		
		GD.Print("[JumpAnimation] Start first return jump animation...");
		for(int i = 1; i <= pixBlockArray.Count-1; i++)
		{
			PixBlock pixBlock = pixBlockArray[i];
			
			var rng = new RandomNumberGenerator();
			rng.Randomize();
			if(this.TentaculePosition == "Left")
			{
				pixBlock.Position = new Vector2(-50*i, 50*i);
			}
			else if(this.TentaculePosition == "Right")
			{
				pixBlock.Position = new Vector2(50*i, 50*i);
			}
		}
	}
	
	public void SetJumpingOf()
	{
		GD.Print("[JumpAnimation] Set jumping off");
		var parent = (Player) this.Tentacule.GetParent();
		parent.Jumping = false;
			
		for(int i = 0; i <= this.Tentacule.PixBlockArray.Count-1; i++)
		{
			PixBlock pixBlock = this.Tentacule.PixBlockArray[i];
				
			var rng = new RandomNumberGenerator();
			rng.Randomize();

			if(this.TentaculePosition == "Right")
			{
				pixBlock.Position = new Vector2(50*i, rng.RandfRange(-5, 5));
			}
			else if(this.TentaculePosition == "Left")
			{
				pixBlock.Position = new Vector2(-50*i, rng.RandfRange(-5, 5));
			}
		}
	}
}
