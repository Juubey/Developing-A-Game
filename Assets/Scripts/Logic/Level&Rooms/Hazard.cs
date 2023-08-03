using SoulSpliter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null && collision.gameObject.tag == "Player")
        {
            damageable.TakeDamage(20);
            //collision.gameObject.GetComponent<TimeFreezer>().FreezeTime(20.0f);
            collision.gameObject.GetComponent<TimeStop>().StopTime(0.05f, 10, 0.1f);
            collision.gameObject.GetComponent<CameraShaker>().BasicShake(10.0f,10.0f);
        }
        /*
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<TimeStop>().StopTime(0.05f, 10, 0.1f);
            collision.gameObject.GetComponent<Player>().TakeDamage(20);
        }*/
    }
}
