using UnityEngine;

namespace Rpg
{

    /// <summary>
    /// 
    /// </summary>
    public class BaseController : MonoBehaviour
    {

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

    }
}