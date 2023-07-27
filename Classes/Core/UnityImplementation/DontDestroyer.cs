using UnityEngine;

namespace SuperFramework.Classes.Core
{
    /// <summary>
    /// Set monobehaviour to not be destroyed on load (object will persist through the scene change)
    /// </summary>
    public sealed class DontDestroyer : MonoBehaviour
    {
        private void Start() => DontDestroyOnLoad(gameObject);
    }
}


