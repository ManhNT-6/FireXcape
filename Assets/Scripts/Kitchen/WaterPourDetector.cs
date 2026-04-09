using UnityEngine;

namespace Kitchen
{
    public class WaterTriggerPour : MonoBehaviour
    {
        private bool _hasPoured = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasPoured) return;

            if (other.CompareTag("Fire"))
            {
                OilFireController fire = other.GetComponentInParent<OilFireController>();

                if (fire == null) return;
                _hasPoured = true;

                Debug.Log("Water triggered by proximity!");

                fire.OnWaterPour();
            }
        }
    }
}