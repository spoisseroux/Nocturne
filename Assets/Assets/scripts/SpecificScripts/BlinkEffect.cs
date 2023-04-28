using UnityEngine;
using System.Collections;

public class BlinkEffect : MonoBehaviour
{
    public Color startColor = Color.magenta;
    public Color endColor = Color.yellow;
    [Range(0, 10)]
    public float speed = 1;

    Renderer ren;

    void Start()
    {
        ren = GetComponent<Renderer>();
        StartCoroutine(Fading());
    }

    IEnumerator Fading()
    {
        while (true)
        {
            //lerpedColor = Color.Lerp(Color.white, Color.black, Time.time);
            ren.material.color = startColor;
            yield return new WaitForSeconds(speed);
            //lerpedColor = Color.Lerp(Color.black, Color.white, Time.time);
            ren.material.color = endColor;
            yield return new WaitForSeconds(speed);
        }
    }
}
 