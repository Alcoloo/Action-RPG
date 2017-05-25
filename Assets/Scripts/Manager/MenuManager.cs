namespace Rpg
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.Events;
    using Rpg;
    using System.Collections.Generic;
    using UnityEngine.EventSystems;
    using System;
    using Assets.Scripts.Manager;

    public class MenuManager : BaseManager<MenuManager>
    {
        private static MenuManager m_Manager;

        protected override void Awake()
        {
            m_Manager = this;
            if (GameManager.manager != null) GameManager.manager.onPlay.AddListener(Play);

        }

        //Apparition des bouttons de niveau et disparition du bouton Start
        protected override void Play()
        {
            ScenesManager.instance.LoadNextScene("Intro");
        }

        //Apparition du boutton Start et disparition des bouttons de niveau
        protected override void Menu()
        {

        }


    }
}