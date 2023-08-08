using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSpriteAnimate : MonoBehaviour
{
    public Image image;
    public Sprite[] animationIn;
    public Sprite[] animationIdle;
    public Sprite[] animationOut;
    public float m_Speed = .02f;
    private int spriteIndex = 0;
    [HideInInspector] public bool isActive = false;
    [HideInInspector] public bool idleIsDone = false;
    [SerializeField] GameObject sunSprite;
    [SerializeField] GameObject charSprite;
    [SerializeField] [Range(0, 255)] byte Alpha = 180;


    public void Start()
    {
        StartCoroutine(Prime());
    }

    IEnumerator Prime() {
        //PRIME TO GET RID OF WHITE ON FIRST ANIMATION
        image.color = new Color32(255, 255, 255, 0);
        image.enabled = true;
        while (spriteIndex < animationIn.Length)
        { //play in animation
            yield return new WaitForSeconds(m_Speed);
            image.sprite = animationIn[spriteIndex];
            spriteIndex += 1;
        }
        image.enabled = false;
        image.color = new Color32(255, 255, 255, Alpha);
    }

    public void Play() {
        if (isActive == false) {
            StartCoroutine(PlayRoutine());
        }
    }

    IEnumerator PlayRoutine() {

        isActive = true;

        if ((sunSprite != null) && (charSprite != null)) {
            sunSprite.SetActive(false);
            charSprite.SetActive(false);
        } 

        image.enabled = true;
        spriteIndex = 0;

        while (spriteIndex < animationIn.Length) { //play in animation
            image.sprite = animationIn[spriteIndex];
            spriteIndex += 1;
            yield return new WaitForSeconds(m_Speed);
        }
        spriteIndex = 0;
        while (!idleIsDone) { //play idle animation
            if (spriteIndex >= animationIdle.Length) {
                spriteIndex = 0;
            }
            image.sprite = animationIdle[spriteIndex];
            spriteIndex += 1;
            yield return new WaitForSeconds(m_Speed);
        }
        spriteIndex = 0;
        while (spriteIndex < animationOut.Length) { //play out animation   
            image.sprite = animationOut[spriteIndex];
            spriteIndex += 1;
            yield return new WaitForSeconds(m_Speed);
        }

        if ((sunSprite != null) && (charSprite != null))
        {
            sunSprite.SetActive(true);
            sunSprite.GetComponent<UISpriteAnimate>().Func_PlayUIAnim();
            charSprite.SetActive(true);
            charSprite.GetComponent<UISpriteAnimate>().Func_PlayUIAnim();
        }

        spriteIndex = 0;
        idleIsDone = false;
        isActive = false;
        image.enabled = false;
    }
}
