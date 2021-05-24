namespace Utilities.Wireplug.Actions
{
    public class QuitAction
    {
        public void Quit()
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                        Application.Quit();
            #endif
        }
    }
}