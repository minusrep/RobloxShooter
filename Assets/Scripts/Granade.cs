using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public GameObject Explosion;
    public float damage;
    private void OnCollisionEnter(Collision other)
    {
        GameObject explosion = Instantiate(Explosion, transform.position, Quaternion.identity);
        if (explosion.GetComponent<EnemyBullet>() != null)
        {
            explosion.GetComponent<EnemyBullet>().damage = damage;
        }

        Destroy(gameObject);
    }
}
