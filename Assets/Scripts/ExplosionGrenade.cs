using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionGrenade : MonoBehaviour
{
    public LayerMask enemyLayer;
    public AudioSource aud;
    public int damage;
    public string parentname;
    public float range;
    void Start()
    {
        aud.volume = UIController.instance.SettingsManager.sfxSlider.value;
        Collider[] hitColliders =
            Physics.OverlapSphere(transform.position, range,enemyLayer);
        if (hitColliders.Length > 0)
        {
            for (int i = 0; i < hitColliders.Length; i++)
            {
                hitColliders[i].GetComponent<EnemyController>().SetDamage(damage,parentname);
            }
        }
        Destroy(gameObject, 2f);
    }

    
}
