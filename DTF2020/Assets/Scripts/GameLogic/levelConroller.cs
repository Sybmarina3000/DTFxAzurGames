using DefaultNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelConroller : MonoBehaviour
{
    EdgeCollider2D map;
    public GameObject objectPoint = null;
    List<GameObject> objectPoints = new List<GameObject>();

    [Serializable]
    public struct laserData
    {
        public float x;
        public float y;
        public float rotate;
    }

    void Start()
    {
        List<Vector2> points = new List<Vector2>();
        List<laserData> lasers = new List<laserData>();
        map = GetComponent<EdgeCollider2D>();
        if (map != default)
        {
            foreach (var point in map.points)
            {
                points.Add(point);
            }
            for (int n = 0; n < transform.childCount; n++)
            {
                var child = transform.GetChild(n);
                laserData data;
                data.x = child.transform.position.x;
                data.y = child.transform.position.y;
                data.rotate = child.transform.rotation.eulerAngles.z;
                lasers.Add(data);
            }
            Destroy(map);
        }
        if (objectPoint != default) 
        {
            foreach (var point in points)
            {
                var pointCopy = point;
                var pointSmeh = transform.position;
                pointCopy.x += pointSmeh.x;
                pointCopy.y += pointSmeh.y;
                var clone = Instantiate(objectPoint);
                clone.transform.position = pointCopy;
                objectPoints.Add(clone);
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getEndPoint()
    {
        return objectPoints[objectPoints.Count - 1];
    }

    public GameObject getCurrentPoint()
    {
        var x = RealizeBox.instance.player.transform.position.x;
        foreach (var point in objectPoints)
        {
            if (point.transform.position.x < x)
            {
                continue;
            }
            return point;
        }
        return getEndPoint();
    }

}
