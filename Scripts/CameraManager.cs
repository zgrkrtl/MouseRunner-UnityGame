using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    
    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Player").transform)
          GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        if(GameObject.FindGameObjectWithTag("Player").transform)
            GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow =GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        if(GameObject.FindGameObjectWithTag("Player").transform)
            GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>().Follow = GameObject.FindGameObjectWithTag("Player").transform;

    }
}
