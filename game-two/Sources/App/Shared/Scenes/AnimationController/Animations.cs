namespace Animations
{
	public class PlayerAnimation
		{
			private PlayerAnimation(string value) {Value = value; }

			public string Value {get; set; }

			public static PlayerAnimation Jump {get {return new PlayerAnimation("JumpAnimation"); }}
			public static PlayerAnimation HangUp {get {return new PlayerAnimation("HangingAnimation"); }}
			public static PlayerAnimation Hit {get {return new PlayerAnimation("HitAnimation"); }}
			public static PlayerAnimation Death {get {return new PlayerAnimation("DeathAnimation"); }}
		}
	public class EnnemiesAnimation
		{
			private EnnemiesAnimation(string value) {Value = value; }

			public string Value {get; set; }

			public static EnnemiesAnimation Run {get {return new EnnemiesAnimation("MonstreRun"); }}
			public static EnnemiesAnimation Idle {get {return new EnnemiesAnimation("MonstreIdle"); }}
			public static EnnemiesAnimation Attack {get {return new EnnemiesAnimation("MonstreAttack"); }}
			public static EnnemiesAnimation Hit {get {return new EnnemiesAnimation("HitAnimation"); }}
			public static EnnemiesAnimation Death {get {return new EnnemiesAnimation("MonstreDeath"); }}
		}
}