using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherSpawner : MonoBehaviour
{
    [SerializeField] private GameObject featherPrefab;
    [SerializeField] private AudioClip feathersFalling;

    [SerializeField] private float Xmax;
    [SerializeField] private float Xmin;
    [SerializeField] private float Zmax;
    [SerializeField] private float Zmin;
    [SerializeField] private float y;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(FeatherRoutine());
    }

    IEnumerator FeatherRoutine()
    {
        Instantiate(featherPrefab, RandomPosition(), Random.rotation);
        yield return new WaitForSeconds(0.3f);
    }

    private Vector3 RandomPosition()
    {
        float x = Random.Range(Xmin, Xmax);
        float z = Random.Range(Zmin, Zmax);
        return new Vector3(x, y, z);
    }
}
