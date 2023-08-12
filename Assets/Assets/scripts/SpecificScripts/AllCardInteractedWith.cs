using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllCardInteractedWith : MonoBehaviour
{

    [HideInInspector] public int collectedCards = 0;
    [SerializeField] int numOfCardsToCollect;
    [SerializeField] GameObject gameObjectToEnable;
    [SerializeField] DialogueTextPrinter textToPrint;
    private bool isDone = false;

    // Update is called once per frame
    void Update()
    {
        if ((collectedCards >= numOfCardsToCollect) && (isDone == false)) {
            gameObjectToEnable.SetActive(true);
            if (textToPrint) {
                textToPrint.printOnClick();
            }
            isDone = true;
        }
    }
}
