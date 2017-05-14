namespace Rpg
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.Events;
    using Rpg;
    using System.Collections.Generic;
    using UnityEngine.EventSystems;
    using System;

    public class MenuManager : BaseManager<MenuManager>
    {
        [SerializeField]

        protected override void Awake()
        {
            base.Awake();

        }

        protected override IEnumerator CoroutineStart()
        {
            throw new NotImplementedException();
        }

        //Apparition des bouttons de niveau et disparition du bouton Start
        protected override void Play()
        {

        }

        //Apparition du boutton Start et disparition des bouttons de niveau
        protected override void Menu()
        {

        }


    }
}