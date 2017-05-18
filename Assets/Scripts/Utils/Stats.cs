using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rpg
{
    class Stats
    {
        public const int atkFirstStep = 10;
        public const int atkSecondStep = 20;

        public const int hpFirstStep = 50;
        public const int hpSecondStep = 100;

        public const int armorFirstStep = 10;
        public const int armorSecondStep = 15;

        private List<float> atkStep = new List<float>();
        private List<float> hpStep = new List<float>();
        private List<float> armorStep = new List<float>();

        private Dictionary<string, List<float>> statsStep = new Dictionary<string, List<float>>();

        public void InitStats()
        {
            atkStep.Add(atkFirstStep);
            atkStep.Add(atkSecondStep);

            hpStep.Add(hpFirstStep);
            hpStep.Add(hpSecondStep);

            armorStep.Add(armorFirstStep);
            armorStep.Add(armorSecondStep);

            statsStep.Add("AtkUpgrade", atkStep);
            statsStep.Add("HPUpgrade", hpStep);
            statsStep.Add("ArmorUpgrade", armorStep);

        }

        public float GetBasicStatsStep(string upgrade, int index)
        {
            return statsStep[upgrade][index];
        } 

    }
}
