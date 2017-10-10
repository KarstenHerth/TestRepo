using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Interactable {

    // store a material for when the switch is activated / unactivated
    public Material activeMaterial;
    public Material inactiveMaterial;

    // stores is the switch is on (true) or off (false)
    private bool bIsActive = false;

    // a reference to the renderer for changing materials
    private Renderer rend;

    public override void Interact()
    {
        // toggle activeness of the switch
        if (bIsActive)
        {
            SetInactive();
        }
        else
        {
            SetActive();
        }
    }

    public void Start()
    {
        rend = GetComponent<Renderer> ();

        if (inactiveMaterial != null)
            rend.sharedMaterial = inactiveMaterial;
    }

    public void SetActive()
    {
        bIsActive = true;

        if (activeMaterial != null)
            rend.sharedMaterial = activeMaterial;
    }

    public void SetInactive()
    {
        bIsActive = false;

        if (inactiveMaterial != null)
            rend.sharedMaterial = inactiveMaterial;
    }

    public bool IsActive()
    {
        return bIsActive;
    }
}
