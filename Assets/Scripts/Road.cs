using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Road : MonoBehaviour
{
    [SerializeField] private float roadMoveSpeed = 10f;
    void Update()
    {
        transform.Translate(Vector3.back * (Time.deltaTime * roadMoveSpeed));
    }

    public void setRoadMoveSpeed(float speed)
    {
        roadMoveSpeed = speed;
    }
    
}
