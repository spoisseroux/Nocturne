using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceInventory : MonoBehaviour
{
    [SerializeField] ItemData clownhatCopy;
    List<InventorySlot> replacement = new List<InventorySlot>();
    private InventorySlot clownHat;

    public static event PushNewInventory Push;
    public delegate void PushNewInventory(List<InventorySlot> slots);

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("ReplaceInventory::Start() --> called");
        clownHat = new InventorySlot(clownhatCopy, 1);
        replacement.Add(clownHat);
    }

    void OnEnable()
    {
        //Debug.Log("ReplaceInventory::OnEnable --> called");
        Push?.Invoke(replacement);
        //Debug.Log("ReplaceInventory::OnEnable --> Pushing new inventory...");
    }
}
