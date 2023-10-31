using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace checkPointsManager.runtime
{
    public class Player_Checkpoint : MonoBehaviour
    {
        public CheckPoint currentCheckpoint;
        public void teleportToCheckpoint(CheckPoint checkpoint = null)
        {
            if(checkpoint != null && currentCheckpoint != null){
                transform.position = currentCheckpoint.transform.position;
                transform.rotation = currentCheckpoint.transform.rotation;
            }else if (checkpoint != null){
                transform.position = checkpoint.transform.position;
                transform.rotation = checkpoint.transform.rotation;
            }
            else{
                throw new System.InvalidOperationException("There is no 'currentCheckpoint' assign in the 'Player_Checkpoint' component on "+gameObject.name);
            }
        }
    }
}
