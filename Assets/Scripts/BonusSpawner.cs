using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusSpawner : MonoBehaviour
{
    private List<Transform> _spawnPoints = new List<Transform>();
    private List<int> _pIds = new List<int>();

    public GameObject healing, ammo, coins;
    public int healingCount, ammoCount, coinsCount;
    private List<GameObject> _bonusSpawned = new List<GameObject>();
    void Start()
    {
        foreach (Transform point in transform)
        {
            _spawnPoints.Add(point);
        }

        for (int i = 0; i < healingCount; i++)
        {
            Instantiate(healing, GetRandomPoint(), Quaternion.identity);
        }
        for (int i = 0; i < ammoCount; i++)
        {
            Instantiate(ammo, GetRandomPoint(), Quaternion.identity);
        }
        for (int i = 0; i < coinsCount; i++)
        {
            Instantiate(coins, GetRandomPoint(), Quaternion.identity);
        }
    }

    Vector3 GetRandomPoint()
    {
        if (_pIds.Count > 0)
        {
            int rId = Random.Range(0, _spawnPoints.Count);
            if (_pIds.Contains(rId))
            {
                return GetRandomPoint();
            }
            else
            {
                _pIds.Add(rId);
                return _spawnPoints[rId].position;
            }
        }
        else
        {
            int rId = Random.Range(0, _spawnPoints.Count);
            _pIds.Add(rId);
            return _spawnPoints[rId].position;
        }
    }
    

   
    
}
