
namespace Rpg
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour
        where T : Component
    {
        public enum SingletonMode { destroySelfIfAlreadyExists, replaceExisting };

        public static T manager;

        [Header("Singleton")]
        [SerializeField]
        private SingletonMode m_SingletonMode = SingletonMode.replaceExisting;
        [SerializeField]
        private bool m_dontDestroyGameObjectOnLoad = true;

        //
        protected Transform m_Transform;

        protected bool IsDestroyed;

        protected virtual void Awake()
        {
            if ( m_dontDestroyGameObjectOnLoad && manager == null) DontDestroyOnLoad(transform.root.gameObject);
            m_Transform = transform;

            switch (m_SingletonMode)
            {
                case SingletonMode.destroySelfIfAlreadyExists:
                    if (manager != null)
                    {
                        if(gameObject != manager.gameObject)
                        {
                            DestroyImmediate(gameObject);
                            IsDestroyed = true;
                            break;
                        }
                    }
                    else manager = this as T;
                    break;
                default:
                case SingletonMode.replaceExisting:
                    if (manager != null)
                    {
                        DestroyImmediate(manager.gameObject);
                        IsDestroyed = true;
                    }
                    manager = this as T;
                    break;
            }
        }
    }
}
