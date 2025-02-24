using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public GameObject healingExplosion;
    public float helingValue;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            float playerHealth = PlayerController.instance._health;
            if (playerHealth < 100)
            {
                if (playerHealth + helingValue > 100)
                {
                    PlayerController.instance.SetHealing(100-playerHealth);
                }
                else
                {
                    PlayerController.instance.SetHealing(helingValue);
                }

                GameObject explosion = Instantiate(healingExplosion, transform.position, Quaternion.identity);
                Destroy(explosion,3f);
                Destroy(gameObject);
            }
        }
    }
}
