using Core;
using UnityEngine;

namespace Electrics
{
    public class ElectricityFireCore : FireBase
    {
        private void OnEnable()
        {
            ShieldMetallController.OnMainKnobTurnedOff += HandlePowerOff;
        }

        private void OnDisable()
        {
            ShieldMetallController.OnMainKnobTurnedOff -= HandlePowerOff;
        }

        private void HandlePowerOff()
        {
            FinishTraining(true,"Power is off. Fire can now be extinguished.");
        }
        
        public override void ProcessInteraction(GameObject tool)
        {
            if (tool.layer.Equals(4)) // Nếu là nước
            {
                //  logic fire outbreaks
                Debug.Log("---Manh--- Check: Fire is interacting with water");
                FinishTraining(false, "Failure! Pouring water on an electricity fire caused a dangerous flare-up.!");
            }
        }

        protected override void HandleTimeOut()
        {
            FinishTraining(false, "It took too long, and the fire spread throughout the entire kitchen.!");
        }
    }
}