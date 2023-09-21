using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Clown Hat")]
public class ClownHat : ItemData
{
    public override bool Use()
    {
        WhiteFadeScript lighthouse = FindClownCousin(GameObject.Find("PlayerObj").transform);

        if (lighthouse != null)
        {
            // trigger dialogue ???
            lighthouse.FadeToWhite();
            return true;
        }
        return false;
    }

    private WhiteFadeScript FindClownCousin(Transform player)
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, 2.0f);

        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<WhiteFadeScript>(out WhiteFadeScript lighthouse))
            {
                return lighthouse;
            }
        }
        return null;
    }
}
