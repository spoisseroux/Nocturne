using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISpriteAnimate : MonoBehaviour
{
    public string nameForKeepingTrack;
    public Image m_Image;
    public Sprite[] m_SpriteArray;
    public float m_Speed = .02f;
    private int m_IndexSprite;
    Coroutine m_CorotineAnim;
    bool IsDone;
    public bool playOnAwake = false;
    public bool triggerNewOnOneShot = false;
    public UISpriteAnimate spriteAnimToTriggerOnFinish;
    private bool alreadyPlayed = false;

    public void Start()
    {
        if (playOnAwake) {
            //play anim on start
            Func_PlayUIAnim();
        }
        
    }

    public void Func_PlayUIAnim()
    {
        if (alreadyPlayed == false) {
            alreadyPlayed = true;
            IsDone = false;
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
        }
    }
    public void Func_StopUIAnim()
    {
        IsDone = true;
        StopCoroutine(m_CorotineAnim);

        if (spriteAnimToTriggerOnFinish != null)
        {
            spriteAnimToTriggerOnFinish.Func_PlayUIAnim();
        }
    }
    IEnumerator Func_PlayAnimUI()
    {
        yield return new WaitForSeconds(m_Speed);
        if (m_IndexSprite >= m_SpriteArray.Length)
        {
            //if we trigger new sprite anim on finish switch to that
            if ((triggerNewOnOneShot) && (spriteAnimToTriggerOnFinish != null)) {
                Func_StopUIAnim();
            }

            //else loop
            if (triggerNewOnOneShot == false) {
                m_IndexSprite = 0;
            }
            
        }
        m_Image.sprite = m_SpriteArray[m_IndexSprite];
        m_IndexSprite += 1;
        if (IsDone == false)
            m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }
}
