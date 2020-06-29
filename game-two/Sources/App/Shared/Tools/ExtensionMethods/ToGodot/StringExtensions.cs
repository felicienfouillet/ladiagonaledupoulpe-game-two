namespace Shared.Tools.ExtensionMethods.ToGodot 
{
    public static class StringExtensions 
    {
        #region Public methods
        /// <summary> 
        /// Converts a method name to a dictionnary, from Godot, to give, for example, to a animationplayer
        /// </summary>
        public static Godot.Collections.Dictionary ToDictionnary(this string methodName, params object[] args) 
        {
            var dic = new Godot.Collections.Dictionary();

            dic.Add("method", methodName);
            dic.Add("args", args);

            return dic;
        }
        #endregion
    }
}