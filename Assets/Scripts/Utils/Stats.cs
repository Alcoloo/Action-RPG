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

        public const float atkSpeedFirstFactor = 1.1f;
        public const float atkSpeedSecondFactor = 1.15f;

        private List<float> statsStep = new List<float>();

        public void InitStats()
        {
            statsStep.Add(atkFirstStep);
            statsStep.Add(atkSecondStep);

            statsStep.Add(hpFirstStep);
            statsStep.Add(hpSecondStep);

            statsStep.Add(atkSpeedFirstFactor);
            statsStep.Add(atkSpeedSecondFactor);

        }

        public float GetBasicStatsStep(int index)
        {
            return statsStep[index];
        } 

    }
}
