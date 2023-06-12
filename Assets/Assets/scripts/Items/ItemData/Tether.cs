using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tether")]
public class Tether : ItemData
{
    [SerializeField]
    GameObject CameraHolder;
    [SerializeField]
    GameObject Player;
    [SerializeField]
    public Vector3 spawnLocation;
    [SerializeField]
    Transform translatePlayerTo;

    public override bool Use()
    {
        // move player to spawn location
        Player.transform.position = translatePlayerTo.position;
        CameraHolder.transform.position = translatePlayerTo.position;
        Player.transform.rotation = translatePlayerTo.rotation;
        CameraHolder.transform.rotation = translatePlayerTo.rotation;
        return true;
    }
}
