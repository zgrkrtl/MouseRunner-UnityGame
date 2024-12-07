using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
   public static SpawnManager Instance { get; private set; }     // Singleton instance

   [SerializeField] private GameObject roadPrefab;
   [SerializeField] private float roadSpawnInterval = 5f;    // Time interval between spawns
   [SerializeField] private Transform startRoad;
   [SerializeField] private Transform roadContainer;
   [SerializeField] private GameObject collectablePrefab;
   [SerializeField] private List<GameObject> obstaclePrefabList;
   [SerializeField] private List<GameObject> buildingPrefabList;
   [SerializeField] private int numberOfObstacles = 3;
   [SerializeField] private int numberOfCollectables = 4;

   private Vector3 spawnPosition;
   
   private void Awake()
   {
      if (Instance != null && Instance != this)
      {
         Destroy(gameObject);
         return;
      }
      Instance = this;
   }
   private void Start()
   {
      InitializeRoads();
      StartCoroutine(SpawnRoadInInterval());
   }

   private void Update()
   {
      DestroyIntstantiatedRoads();
   }

   private void InitializeRoads()
   {
      spawnPosition = startRoad.position + new Vector3(17.5f,0,0);
      Instantiate(roadPrefab, spawnPosition+ new Vector3(0,0,-10), Quaternion.identity,roadContainer);
      Instantiate(roadPrefab, spawnPosition+ new Vector3(0,0,0), Quaternion.identity,roadContainer);
      Instantiate(roadPrefab, spawnPosition+ new Vector3(0,0,10), Quaternion.identity,roadContainer);
      spawnPosition = spawnPosition + new Vector3(0, 0, 20);
   }
   
   private void DestroyIntstantiatedRoads()
   {
      for (int i = roadContainer.childCount - 1; i >= 0; i--)
      {
         if (roadContainer.GetChild(i).transform.position.z <-20)
         {
            Destroy(roadContainer.GetChild(i).gameObject);
         }
      }
      spawnPosition = roadContainer.GetChild(roadContainer.childCount - 1).position + new Vector3(0,0,10);
   }
   
   private IEnumerator SpawnRoadInInterval()
   {
      while (true)
      {
         if (Player.Instance.IsDead())
         {
            yield break;
         }
         // Instantiate road
         GameObject road = Instantiate(roadPrefab, spawnPosition, Quaternion.identity,roadContainer);
         
         // Handle Spawns of Collectable and Obstacles
         List<GameObject> occupiedPositions = new List<GameObject>();
         for (int i = 0; i < numberOfCollectables; i++)
         {
            GameObject randomPositionOnRoadForCollectable = GetRandomPositionOnRoad(road);
            while (occupiedPositions.Contains(randomPositionOnRoadForCollectable))
            {
               randomPositionOnRoadForCollectable = GetRandomPositionOnRoad(road);
            }
            occupiedPositions.Add(randomPositionOnRoadForCollectable);
            Instantiate(collectablePrefab, randomPositionOnRoadForCollectable.transform.position+new Vector3(0,0.5f,0), Quaternion.identity, randomPositionOnRoadForCollectable.transform);
         }
         for (int i = 0; i < numberOfObstacles; i++)
         {
            GameObject randomPositionOnRoadForObstacle = GetRandomPositionOnRoad(road);
            while (occupiedPositions.Contains(randomPositionOnRoadForObstacle))
            {
               randomPositionOnRoadForObstacle = GetRandomPositionOnRoad(road);
            }
            occupiedPositions.Add(randomPositionOnRoadForObstacle);
            Instantiate(obstaclePrefabList[Random.Range(0,obstaclePrefabList.Count)], randomPositionOnRoadForObstacle.transform.position, Quaternion.Euler(0, 90, 0),randomPositionOnRoadForObstacle.transform);

         }
         
        
         // Handle Spawns of Buildings Left-Right
         Instantiate(buildingPrefabList[Random.Range(0, buildingPrefabList.Count)],road.transform.GetChild(5).transform.position,Quaternion.Euler(0, 90, 0),road.transform.GetChild(5));
         Instantiate(buildingPrefabList[Random.Range(0, buildingPrefabList.Count)],road.transform.GetChild(6).transform.position,Quaternion.Euler(0, 270, 0),road.transform.GetChild(6));
         yield return new WaitForSeconds(roadSpawnInterval); 
      }
   } 

   public void StopAllRoads()
   {
      foreach (Transform child in roadContainer)
      {
         Road road = child.GetComponent<Road>();
         if (road != null)
         {
            road.setRoadMoveSpeed(0);
         }
      }
   }

   private GameObject GetRandomPositionOnRoad(GameObject road)
   {
      List<GameObject> spawnPoints = new List<GameObject>();
      
      for (int i = 0; i < 4; i++)
      {
         Transform child = road.transform.GetChild(i);
         foreach (Transform childOfChild in child)
         {
            spawnPoints.Add(childOfChild.transform.gameObject);
         } 
      } 
      
      return spawnPoints[Random.Range(0,spawnPoints.Count)];
   }
   
}
