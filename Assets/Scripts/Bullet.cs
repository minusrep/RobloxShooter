using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletLife = 3;
    public int damage;
    public string parentName;

    public bool isExplosion;
    public GameObject Explosion;
    private void Awake()
    {
        Destroy(gameObject, bulletLife);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isExplosion)
        {
            if (collision.collider.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyController>().SetDamage(damage, parentName);
            }
        }
        else
        {
           GameObject go = Instantiate(Explosion, transform.position, Quaternion.identity);
           go.GetComponent<ExplosionGrenade>().parentname = parentName;
           go.GetComponent<ExplosionGrenade>().damage = damage;
        }

        Destroy(gameObject);
    }
}
