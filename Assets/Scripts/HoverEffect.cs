using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    public float hoverAmount;
    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount; //zwiększa rozmiar tila po najechaniu myszką
    }
    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount; //zmniejsza rozmiar tila po wyjściu myszki 
    }
}
