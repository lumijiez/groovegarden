using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_Prefab;
    [SerializeField] private Transform pos;
    private float spawnTimer = 0f;
    private float spawnInterval = 1f;

    private void Update()
    {
        if (Vector2.Distance(PlayerController.Instance.transform.position, transform.position) < 20f)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnInterval)
            {
                spawnTimer = 0f;

                if (Random.value < 0.2)
                {
                    Instantiate(m_Prefab, pos.position, Quaternion.identity);
                }
            }
        }
    }
}
