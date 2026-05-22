using UnityEngine;

namespace Core
{
    public class LaboratoryFireCore : FireBase
    {
        public override void ProcessInteraction(GameObject tool)
        {
            if (tool.layer.Equals(7))
            {
                FinishTraining(true,"Thành công! Bạn đã dập tắt thành công đám cháy hóa chất bằng bình chữa cháy CO2.");
            }
        }

        // protected override void HandleTimeOut()
        // {
        //     FinishTraining(false, "It took too long, and the fire spread throughout the entire kitchen.!");
        // }
    }
}