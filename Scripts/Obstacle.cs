using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
   {
      Player.Instance.OnDeath();
      SpawnManager.Instance.StopAllRoads();
      GameManager.Instance.RemoveLife();
   }
}