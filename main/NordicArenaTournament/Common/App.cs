namespace NordicArenaTournament.Common
{
    /// <summary>
    /// Global statics container
    /// </summary>
    public class App
    {
        public static bool IsCompiledDebug
        {
            get
            {

#if DEBUG
                return true;
#else
                return false;
#endif           
            }
        }
    }
}