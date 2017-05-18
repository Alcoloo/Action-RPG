using UnityEngine;

namespace Rpg
{

    /// <summary>
    /// 
    /// </summary>
    public class BaseController : MonoBehaviour
    {
        protected void Awaken()
        {
            AwakeController();
        }
        protected void Start()
        {
            InitController();
        }

        protected void Update()
        {
            DoAcTion();
        }

        protected virtual void InitController()
        {

        }

        protected virtual void DoAcTion()
        {

        }
        protected virtual void AwakeController()
        {

        }

    }
}