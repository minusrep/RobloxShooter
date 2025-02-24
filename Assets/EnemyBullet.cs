using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public LayerMask playerLayer;
    public float damage;
    public float range;
    void Start()
    {
        Collider[] hitColliders =
            Physics.OverlapSphere(transform.position, range,playerLayer);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i].GetComponent<AIController>() != null)
                {
                    hitColliders[i].GetComponent<AIController>().SetDamage(damage);
                }
                else
                {
                    PlayerController.instance.SetDamage(damage);
                }
            }
        }
        Destroy(gameObject, 4f);
    }

   
}
