using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSpriteFadeManager : MonoBehaviour
{
    [SerializeField] Image charSprite;
    [SerializeField] public float FadeOutTime = 1f;
    [SerializeField] public float FadeInTime = 1f;
    [SerializeField] public float FadeInToAlpha = 0.5f;
    private bool FadeIninScript = false;
    private bool FadeOutinScript = false;
    private IEnumerator fadeinRoutine;



    public void FadeIn()
    {
        if (FadeIninScript == false)
        {
            fadeinRoutine = CharFadeIn(charSprite, FadeInTime, FadeInToAlpha);
            StartCoroutine(fadeinRoutine);
        }
    }

    public void FadeOut()
    {
        if (FadeOutinScript == false)
        {
            StartCoroutine(CharFadeOut(charSprite, FadeOutTime));
        }
    }

    private IEnumerator CharFadeIn(Image charSprite, float FadeInTime, float FadeInToAlpha)
    {
        FadeIninScript = true;
        charSprite.enabled = true;
        float currentTime = 0;
        float start = charSprite.color.a;
        float tempIn = start;
        var tempColorIn = charSprite.color;

        while (currentTime < FadeInTime)
        {
            currentTime += Time.deltaTime;
            tempIn = Mathf.Lerp(start, FadeInToAlpha, currentTime / FadeInTime);
            tempColorIn.a = tempIn;
            charSprite.color = tempColorIn;
            yield return null;
        }
        FadeIninScript = false;
        yield break;
    }

    private IEnumerator CharFadeOut(Image charSprite, float FadeOutTime)
    {
        FadeOutinScript = true;
        if (FadeIninScript)
        {
            StopCoroutine(fadeinRoutine);
            FadeIninScript = false;
        }
        float startAlpha = charSprite.color.a;
        float tempOut = startAlpha;
        var tempColor = charSprite.color;

        while (charSprite.color.a > 0)
        {
            tempOut -= startAlpha * Time.deltaTime / FadeOutTime;
            tempColor.a = tempOut;
            charSprite.color = tempColor;
            yield return null;
        }

        charSprite.enabled = false;
        FadeOutinScript = false;
    }
}
