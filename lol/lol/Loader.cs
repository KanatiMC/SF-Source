namespace lol
{
    using System;
    using UnityEngine;

    public class Loader
    {
        public static GameObject Load;

        public static void Init()
        {
            Load = new GameObject();
            Load.AddComponent<Main>();
            UnityEngine.Object.DontDestroyOnLoad(Load);
        }
    }
}

