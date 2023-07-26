using UnityEngine;

namespace SuperFramework.Classes.Core
{
    public sealed class DontDestroyer : MonoBehaviour
    {
        private void Start() => DontDestroyOnLoad(gameObject);
    }
}


