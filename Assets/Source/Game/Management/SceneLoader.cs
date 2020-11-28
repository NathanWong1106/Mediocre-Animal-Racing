using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Racing.Game.Management
{
    public static class SceneLoader
    {
        public static Scene CurrentScene { get { return SceneManager.GetActiveScene(); } }

        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        public static void ReloadScene()
        {
            SceneManager.LoadSceneAsync(CurrentScene.name);
        }
    }
}
