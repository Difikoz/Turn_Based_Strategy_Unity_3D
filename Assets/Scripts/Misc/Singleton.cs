using UnityEngine;

namespace WinterUniverse
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        public static T StaticInstance { get; private set; }

        [SerializeField] private bool _dontDestroyOnLoad = true;

        protected virtual void Awake()
        {
            if (StaticInstance == null)
            {
                StaticInstance = (T)this;
            }
            else if (StaticInstance != (T)this)
            {
                Destroy(gameObject);
                return;
            }
            if (_dontDestroyOnLoad && transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
            Initialize();
        }

        protected virtual void Initialize()
        {

        }
    }
}