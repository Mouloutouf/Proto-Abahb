using UnityEditor;
using UnityEditor.SceneManagement;

namespace Utilities.Editor
{
    public static class MenuScenes
    {
        public const string MainTab = "Open Scene/";
        public const string MapsTab = MainTab + "Maps/";
        public const string FolderMain = "Main";
        public const string FolderMaps = "Maps";
        public const string FolderArt = "Artbox";

        [MenuItem(MainTab + nameof(MainMenu), priority = 0)]
        public static void MainMenu() => OpenScene(nameof(MainMenu), FolderMain);
        


        // ==============================================

        static void OpenScene(string name, string subfolder)
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene("Assets/Scenes/" + subfolder + "/" + name + " Scene.unity");
            }
        }
    }
}