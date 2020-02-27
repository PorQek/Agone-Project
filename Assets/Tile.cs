using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public float hoverAmount; //wartość zwiększania rozmiaru tila

    public LayerMask obstacleLayer;

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount; //zwiększa rozmiar tila po najechaniu myszką
    }
    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount; //zmniejsza rozmiar tila po wyjściu myszki 
    }

    public bool IsCleat()
    {
        Collider2D obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, obstacleLayer); //tworzy niewielkie kółko, które sprawdza czy koliduje z obstacle
        if (obstacle != null) //zwraca info, czy ma obstacle, czy nie
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
