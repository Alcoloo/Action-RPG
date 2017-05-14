
namespace Rpg
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour
        where T : Component
    {
        public enum SingletonMode { replaceExisting, destroySelfIfAlreadyExists };

        public static T manager;

        [Header("Singleton")]
        [SerializeField]
        private SingletonMode m_SingletonMode = SingletonMode.replaceExisting;
        [SerializeField]
        private bool m_dontDestroyGameObjectOnLoad = true;

        //
        protected Transform m_Transform;

        protected virtual void Awake()
        {
            switch (m_SingletonMode)
            {
                case SingletonMode.destroySelfIfAlreadyExists:
                    if (manager != null)
                        Destroy(gameObject);
                    else manager = this as T;
                    break;
                default:
                case SingletonMode.replaceExisting:
                    if (manager != null)
                        Destroy(manager.gameObject);
                    manager = this as T;
                    break;
            }

            if (m_dontDestroyGameObjectOnLoad) DontDestroyOnLoad(gameObject);

            m_Transform = transform;
        }
    }
}
