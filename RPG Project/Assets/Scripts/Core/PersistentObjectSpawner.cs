using UnityEngine;
using System.Collections;
using System;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab = null;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
          
            SpawnPersistantObjects();
        }

        private void SpawnPersistantObjects()
        {
            GameObject persistantObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistantObject);
            hasSpawned = true;
        }
    }
}