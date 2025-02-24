using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDamageUI : MonoBehaviour
{
    public Transform uiParent;
    public GameObject textPrefab;

    public void SpawnDamageUI(string textDamage)
    {
        GameObject go = Instantiate(textPrefab, uiParent);
        go.GetComponent<Text>().text = textDamage;
        
    }


}
