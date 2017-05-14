using Rpg;
using UnityEngine;

namespace Rpg.Controller
{

    /// <summary>
    /// 
    /// </summary>
    public class LevelController : BaseController
    {
        public AnimationCurve levelingCurve;

        public int expMultiplier = 1;
        public int currentLevel = 1;
        public int currentExp = 0;
        public int globalExp = 0;
        public int maxLevel = 100;
        private int compensator = 1000;
       

        protected override void InitController()
        {
            base.InitController();
            if (expMultiplier < 1) expMultiplier = 1;
            
        }
        
        public void gainExp(int expToEarn)
        {
            Debug.Log(name + " earn " + expToEarn + " exp");
            currentExp += expToEarn;
            globalExp += expToEarn;
            CheckExp();
        }
        private void CheckExp()
        {
            if (currentLevel >= maxLevel)
            {
                currentLevel = maxLevel;
                return;
            }

            int expToLevelUp = getExpToLevelUp();
            Debug.Log(expToLevelUp);
            if(currentExp > expToLevelUp)
            {
                currentExp -= expToLevelUp;
                currentLevel += 1;
                CheckExp();
                return;
            }
        }
        protected override void DoAcTion()
        {
            base.DoAcTion();
          
        }
        
        private int getExpToLevelUp()
        {
            if (currentLevel + 1 > maxLevel) return 0;

            float nextLevel = (float)(currentLevel + 1) / (float)maxLevel;
            return (int)((levelingCurve.Evaluate(nextLevel)-levelingCurve.Evaluate(0.01f)) * compensator * expMultiplier);
            
        }

    }
}