using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeatherSpawner : MonoBehaviour
{
    [SerializeField] private GameObject featherPrefab;

    [SerializeField] private float Xmax;
    [SerializeField] private float Xmin;
    [SerializeField] private float Zmax;
    [SerializeField] private float Zmin;
    [SerializeField] private float y;

    public BoxCollider collectionBoxCollider;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(FeatherRoutine());
        StartCoroutine(CountToFourSecondsThenEnableBoxCollider());
    }

    IEnumerator FeatherRoutine()
    {
        while (true)
        {
            Instantiate(featherPrefab, RandomPosition(), Random.rotation);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator CountToFourSecondsThenEnableBoxCollider()
    {
        yield return new WaitForSeconds(8.0f); // Wait for 4 second
        collectionBoxCollider.enabled = true;
    }

    private Vector3 RandomPosition()
    {
        float x = Random.Range(Xmin, Xmax);
        float z = Random.Range(Zmin, Zmax);
        return new Vector3(x, y, z);
    }
}
