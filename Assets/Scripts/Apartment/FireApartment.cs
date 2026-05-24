using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Apartment
{
    public class FireApartment : FireBase
    {
        public GameObject firePrefab;
        public List<Transform> firePoints = new List<Transform>();
        
        private void Start()
        {
            SpawnFire();
        }

        private void SpawnFire()
        {
            foreach (var firePoint in firePoints)
            {
                Instantiate(firePrefab, firePoint.position, firePoint.rotation);
            }
        }
        
        public override void ProcessInteraction(GameObject tool)
        {
            
        }
        
        // lam lua chay
        // huong dan: nen chay xuong duoi, chay nhanh neu khong lua chay qua to => that bai
    }
}