using Godot;
using System;
using System.Collections.Generic;

public class HangingAnimation : Node
{
    private Tentacule _tentacule;
	private Vector2 _finalPos;
	
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

	public Vector2 FinalPos
	{
		get
		{
			return this._finalPos;
		}
		
		set
		{
			this._finalPos = value;
		}
	}
	
	private AnimationController _parent;

	public void GetTantacule(Tentacule tentacule, Vector2 finalPos)
	{
		this.Tentacule = tentacule;
		this.FinalPos = finalPos;
	}
	
	public HangingAnimation()
	{
		_parent = (AnimationController) this.GetParent();
	}

	 public void StartHangingAnimation()
    {
        GD.Print("[HangUpAnimation] Start animation...");
		Vector2 pos = this.Tentacule.Position;
		List<PixBlock> pixBlockArray = this.Tentacule.PixBlockArray;

		pixBlockArray[pixBlockArray.Count - 1].Position = FinalPos;

        for(int i = 1; i <= pixBlockArray.Count-2; i++)
		{
			PixBlock pixBlock = pixBlockArray[i];

            var rng = new RandomNumberGenerator();
			rng.Randomize();

            float posX = 0;
            float posY = 0;
			
			if(i==pixBlockArray.Count-1)
			{
				posX = FinalPos.x;
				posY = FinalPos.y;
			}
			else
			{
				posX = (i+1/pixBlockArray.Count+0.75f)*((FinalPos.x-pixBlock.Position.x)/(pixBlockArray.Count));
				posY = (i+1/pixBlockArray.Count+0.5f)*((FinalPos.y-pixBlock.Position.y)/(pixBlockArray.Count));
			}

            pixBlock.Position = new Vector2(posX, posY);
		}
    }

    public void SetHangingAnimationOf()
	{
		GD.Print("[HangUpAnimation] Set hanging off");

		Player parent = (Player) this.Tentacule.GetParent();

		if(parent.AllowedHanging)
		{
			parent.Position = parent.Position + this.Tentacule.Position + this.FinalPos;
			parent.AllowedHanging = false;
		}

		if(this.Tentacule.IsPositionRight)
		{
			Vector2 pos = this.Tentacule.Position;
			
			for(int i = 1; i <= this.Tentacule.PixBlockArray.Count-1; i++)
			{
				PixBlock pixBlock = this.Tentacule.PixBlockArray[i];
				
				var rng = new RandomNumberGenerator();
				rng.Randomize();
				pixBlock.Position = new Vector2((50*i), rng.RandfRange(-5, 5));
			}
		}
		else if(!this.Tentacule.IsPositionRight)
		{
			Vector2 pos = this.Tentacule.Position;
			
			for(int i = 1; i <= this.Tentacule.PixBlockArray.Count-1; i++)
			{
				PixBlock pixBlock = this.Tentacule.PixBlockArray[i];
				
				var rng = new RandomNumberGenerator();
				rng.Randomize();
				pixBlock.Position =new Vector2(-50*i, rng.RandfRange(-5, 5));
			}
		}

		parent.HangingStatus = false;
	}
}
