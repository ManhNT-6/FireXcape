using UnityEngine;

namespace Core
{
    public class OilFireCore : FireBase
    {
        public override void ProcessInteraction(GameObject tool)
        {
            if (tool.CompareTag("Lid"))
            {
                FinishTraining(true, "You extinguished the oil fire correctly using the pot lid!");
            }
            else if (tool.CompareTag("Water")) // Nếu là nước
            {
                //  logic fire outbreaks
                FinishTraining(false, "Failure! Pouring water on an oil fire caused a dangerous flare-up.!");
            }
        }

        protected override void HandleTimeOut()
        {
            FinishTraining(false, "It took too long, and the fire spread throughout the entire kitchen.!");
        }
    }
}