namespace Shared.Tools.ExtensionMethods.ChangePosition 
{
    using Godot;
    using System;
    using System.Collections.Generic;
    public static class PositionExtensions 
    {
        #region Public methods
        /// <summary> 
        /// Converts a method name to a dictionnary, from Godot, to give, for example, to a animationplayer
        /// </summary>
        public static void ChangeGlobalPosition(this KinematicBody2D pixBlock,/*this List<KinematicBody2D> pixBlockArray, this Tentacule tentacule,*/ int? i = null, params float[] args) 
        {
            float offsetX = args[0];
            float offsetY = args[1];

            /*Vector2 pos = tentacule.GetGlobalPosition();
            List<KinematicBody2D> pixBlockArray = tentacule.PixBlockArray;

            if(i.HasValue)
            {
                for(i = i.Value; i <= pixBlockArray.Count-1; i++)
                {
                    KinematicBody2D pixBlock = pixBlockArray[i.Value];
                    
                    if(tentacule.PositionRelativeToPlayer == "Right")
                    {
                        pixBlock.SetGlobalPosition(new Vector2(offsetX, offsetY));
                    }
                    else if(tentacule.PositionRelativeToPlayer == "Left")
                    {
                        pixBlock.SetGlobalPosition(new Vector2(offsetX, offsetY));
                    }
                }
            }*/

            pixBlock.Position = new Vector2(offsetX, offsetY);
        }
        #endregion
    }
}