using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGameItem : MonoBehaviour
{
    private bool clicked;

    [SerializeField] private ParticleSystem ps;

    public bool wasClicked() { return clicked; }

    public void onClick()
    {
        clicked = true;
        ps.Play();
    }

}
