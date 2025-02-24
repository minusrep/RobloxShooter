using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public GameObject ammoExplosion;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            WeaponController.instance.AddAmmo();
            GameObject explosion = Instantiate(ammoExplosion, transform.position, Quaternion.identity);
            Destroy(explosion,3f);
            Destroy(gameObject);
        }
    }
}
