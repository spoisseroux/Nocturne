using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamppostScript : MonoBehaviour
{

    //public GameObject objectToSpawn1;
    public GameObject objectToSpawn2;
    [SerializeField] public GameObject door;
    [SerializeField] public AudioSource doorAudio;
    [SerializeField] public GameObject doorVideo;

    private bool doorSpawned = false;

    public TextPrinterMultiLinePages textPrinter;

    // Start is called before the first frame update
    void Start()
    {
        //door = GameObject.Find("whiteDoor");
        //doorAudio = door.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {

        if ((doorSpawned == false) && (textPrinter.isFinished == true)) {
            //Instantiate(objectToSpawn1);
            door.SetActive(true);
            doorAudio.Play();
            Instantiate(objectToSpawn2);
            doorVideo.SetActive(true);
            doorSpawned = true;
        }
        
    }
}
