using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecorderWeapon : MonoBehaviour
{
    public List<Transform> _child = new List<Transform>();

    public int count;
    void Start()
    {
        foreach (Transform child in transform)
        {
            _child.Add(child);
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(count>0)
                _child[count-1].gameObject.SetActive(false);
            _child[count].gameObject.SetActive(true);
            count++;
        }
    }
}
