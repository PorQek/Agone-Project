using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite[] tileGraphics; //zbiera sprity tilów

    public float hoverAmount; //wartość zwiększania rozmiaru tila

    public LayerMask obstacleLayer;

    public Color HighlightedColor;
    public bool isWalkable;
    GameMaster gm;

    public Color creatableColor;
    public bool isCreatable;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        int randTile = Random.Range(0, tileGraphics.Length); //losuje sprite tila
        rend.sprite = tileGraphics[randTile]; //renderuje losowy tile

        gm = FindObjectOfType<GameMaster>();
    }

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * hoverAmount; //zwiększa rozmiar tila po najechaniu myszką
    }
    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * hoverAmount; //zmniejsza rozmiar tila po wyjściu myszki 
    }

    public bool IsClear()
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

    public void Highlights() //podświetla tile
    {
        rend.color = HighlightedColor;
        isWalkable = true;
    }

    public void Reset() //reset podświetlenia tilów
    {
        rend.color = Color.white;
        isWalkable = false;
        isCreatable = false;
    }

    public void SetCreatable()
    {
        rend.color = creatableColor;
        isCreatable = true;
    }

    private void OnMouseDown()
    {
        if (isWalkable && gm.selectedUnit != null)
        {
            gm.selectedUnit.Move(this.transform.position);
        }
        else if (isCreatable == true)
        {
            ShopItem item = Instantiate(gm.purchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            gm.ResetTiles();
            Unit unit = item.GetComponent<Unit>();
            if(unit != null)
            {
                unit.hasMoved = true;
                unit.hasAttacked = true;
            }
        }
    }
}
