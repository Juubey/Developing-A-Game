using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace checkPointsManager.runtime
{
    public class CheckPoint : MonoBehaviour
    {
        public GameObject Player;
        private void OnTriggerEnter(Collider other)
        {         
            Player.GetComponent<Player_Checkpoint>().currentCheckpoint = this;                        
        }
    }
}
