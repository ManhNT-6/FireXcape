using UnityEngine;

namespace Core
{
    public class LaboratoryFireCore : FireBase
    {
        public override void ProcessInteraction(GameObject tool)
        {
            if (tool.layer.Equals(7))
            {
                FinishTraining(true,"Success! You have successfully contained the chemical fire using a CO2 extinguisher.");
            }
        }

        protected override void HandleTimeOut()
        {
            FinishTraining(false, "It took too long, and the fire spread throughout the entire kitchen.!");
        }
    }
}