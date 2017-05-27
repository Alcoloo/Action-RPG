using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rpg.Manager
{

    /// <summary>
    /// 
    /// </summary>
    public class SaveManager : BaseManager<SaveManager>
    {


        private PlayerCarac _player;

        public PlayerCarac player
        {
            get { return _player; }
        }

        protected void Start()
        {

        }

        protected void Update()
        {

        }

        #region Save functions


        public void SavePlayer(PlayerCarac pPlayer)
        {
            _player = pPlayer;
        }

        #endregion
        

    }
}