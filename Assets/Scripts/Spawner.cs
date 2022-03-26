using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public float minTime = 2.0f;
    public float maxTime = 4.0f;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));
    }
}
