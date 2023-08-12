using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginSpaceScreenScript : MonoBehaviour
{
    [SerializeField] UIDissolveHandler uIDissolveHandler;
    [SerializeField] DialogueTextPrinter textPrinter;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(beinningCoroutine());
    }

    IEnumerator beinningCoroutine() {
        yield return new WaitForSecondsRealtime(1);
        yield return new WaitUntil(() => uIDissolveHandler.inScript == false);
        textPrinter.printOnClick();
    }
}
