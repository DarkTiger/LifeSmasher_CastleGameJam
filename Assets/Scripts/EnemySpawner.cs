using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] Enemy[] enemies = null;

    private void Awake()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    public void SpawnEnemy()
    {
        Instantiate(enemies[Random.Range(0, enemies.Length)], transform.position, transform.rotation);
    }
}
