using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PathForAI : MonoBehaviour
{
    public static PathForAI instance;
    public List<Transform> _points = new List<Transform>();

    private void Awake()
    {
        instance = this;
        foreach (Transform point in transform)
        {
            _points.Add(point);
        }
    }

    public Vector3 GetPointToMove()
    {
        int randomPoint = Random.Range(0, _points.Count);
        var vector3 =_points[randomPoint].position+ Random.insideUnitSphere.normalized * 3;
        Vector3 newPos = new Vector3(vector3.x, 0, vector3.z);
        return newPos;
    }
}
