using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public GameObject ammoExplosion;
    public int coinsAdd;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UIController.instance.coinsCollectedInMap += coinsAdd;
            GameObject explosion = Instantiate(ammoExplosion, transform.position, Quaternion.identity);
            Destroy(explosion,3f);
            Destroy(gameObject);
        }
    }
}
