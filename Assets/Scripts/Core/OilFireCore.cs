using UnityEngine;

namespace Core
{
    public class OilFireCore : FireBase
    {
        public override void ProcessInteraction(GameObject tool)
        {
            if (tool.layer.Equals(6))
            {
                FinishTraining(true, "Bạn đã dập tắt đám cháy đúng cách bằng cách sử dụng nắp nồi!");
            }
            else if (tool.layer.Equals(4)) // Nếu là nước
            {
                FinishTraining(false, "Thất bại! Việc đổ nước vào đám cháy dầu đã gây ra một vụ bùng phát nguy hiểm!");
            }
        }

        // protected override void HandleTimeOut()
        // {
        //     FinishTraining(false, "It took too long, and the fire spread throughout the entire kitchen.!");
        // }
    }
}