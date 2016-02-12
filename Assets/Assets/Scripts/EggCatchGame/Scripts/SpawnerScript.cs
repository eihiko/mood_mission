using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Globals;

namespace EggCatch
{
    public class SpawnerScript : MonoBehaviour
    {

        public Transform[] eggPrefabs;
        public PlayerScript playerScript;

        private float nextEggTime = 0.0f;
        private float spawnRate = 1.5f;
        private const string PREFAB_NAME_BASE = "EggPrefab";
        private string lastSceneCompleted;

        private void Awake()
        {
            lastSceneCompleted = Scenes.GetLastSceneCompleted();
        }

        private void Update()
        {
            if (!playerScript.shouldDropEggs) return;
            if (nextEggTime < Time.time)
            {
                SpawnEgg();
                nextEggTime = Time.time + spawnRate;

                //Speed up the spawnrate for the next egg
//            spawnRate *= 0.98f;
//            spawnRate = Mathf.Clamp(spawnRate, 0.3f, 99f);
            }
        }

        private void SpawnEgg()
        {
            float addXPos = Random.Range(-1.6f, 1.6f);
            Vector3 spawnPos = transform.position + new Vector3(addXPos, 0, 0);
            int index = chooseEggToDrop();
            Instantiate(eggPrefabs[index], spawnPos, Quaternion.identity);
        }

        private int chooseEggToDrop()
        {
            var list = new List<Transform>(eggPrefabs);
            int correctEggIndex = list.FindIndex(x => lastSceneCompleted.Contains(x.name.Replace(PREFAB_NAME_BASE, "")));

            // give more weight to correct egg
            if (correctEggIndex != -1 && new System.Random().NextDouble() > 0.5)
            {
                return correctEggIndex;
            }
            int chosenIndex = correctEggIndex;
            while (chosenIndex == correctEggIndex)
            {
                chosenIndex = Random.Range(0, eggPrefabs.Length);
            }
            return chosenIndex;
        }
    }
}