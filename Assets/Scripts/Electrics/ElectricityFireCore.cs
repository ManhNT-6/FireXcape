using Core;
using UnityEngine;

namespace Electrics
{
    public class ElectricityFireCore : FireBase
    {
        private void OnEnable()
        {
            base.OnEnable();
            ShieldMetallController.OnMainKnobTurnedOff += HandlePowerOff;
        }

        private void OnDisable()
        {
            base.OnDisable();
            ShieldMetallController.OnMainKnobTurnedOff -= HandlePowerOff;
        }

        private void HandlePowerOff()
        {
            FinishTraining(true,"Điện đã bị ngắt. Có thể dập tắt đám cháy.");
        }
        
        public override void ProcessInteraction(GameObject tool)
        {
            if (tool.layer.Equals(4)) // Nếu là nước
            {
                //  logic fire outbreaks
                FinishTraining(false, "Thất bại! Việc đổ nước vào đám cháy điện đã gây ra một vụ bùng phát nguy hiểm!");
            }
        }

        // protected override void HandleTimeOut()
        // {
        //     FinishTraining(false, "It took too long, and the fire spread throughout the entire kitchen.!");
        // }
    }
}