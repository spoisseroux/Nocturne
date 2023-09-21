using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WhiteFadeScript : MonoBehaviour
{

    [SerializeField] GameObject whiteScreen;
    [SerializeField] string animationName;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] string sceneName;
    [SerializeField] Image sunSprite;
    [SerializeField] Image charSprite;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] UIDissolveHandler crossfadeDissolve;
    [SerializeField] PlayerInteractionStatus canPlayerInteract;
    [SerializeField] AudioFadeManager audioToFade;
    private bool isInCollider = false;
    private Animator whiteScreenAnimator;
    private bool inScript = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (isInCollider && canPlayerInteract.CheckPlayerInteractionAvailability())
        {
            //Make sure pause menu is not on to activate
            if (Input.GetKeyDown(KeyCode.E) && (inScript == false) && (PauseMenu.activeSelf == false) && (crossfadeDissolve.inScript == false))
            {
                Debug.Log("In fade to white");
                StartCoroutine(playFadeToWhite());
            }
        }
    }


    public void FadeToWhite()
    {
        StartCoroutine(playFadeToWhite());
    }


    IEnumerator playFadeToWhite() {
        inScript = true;
        whiteScreen.SetActive(true);
        whiteScreenAnimator = whiteScreen.GetComponent<Animator>();
        whiteScreenAnimator.Play(animationName);
        audioToFade.FadeOut();
        yield return new WaitForSeconds(whiteScreenAnimator.GetCurrentAnimatorStateInfo(0).length + whiteScreenAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime);
        //go to next scene
        if (sceneName != "")
        {
            SceneManager.LoadScene(sceneName);
        }
        inScript = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        isInCollider = true;

        //switch to charSprite when in collider
        if (sunSprite && charSprite)
        {
            sunSprite.enabled = false;
            charSprite.enabled = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;

        if (sunSprite && charSprite)
        {
            charSprite.enabled = false;
            sunSprite.enabled = true;
        }
        //switch to charSprite when in collider

    }
}
