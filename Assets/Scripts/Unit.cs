using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected; //sprawdza czy ta jednostka jest wybrana
    GameMaster gm; //daje dostęp do skryptu GameMastera

    public int tileSpeed; //ile tilesów ta jednostka może się poruszyć
    public bool hasMoved; //sprawdza czy ta jednostka poruszyła się już w tej turze

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    private void OnMouseDown() //po kliknięciu na jednostkę
    {
        if (selected == true) //jeżeli jednostka jest już wybrana - odwybiera ją
        {
            selected = false;
            gm.selectedUnit = null;
        }
        else //jeżeli ta jednostka nie jest wybrana
        {
            if (gm.selectedUnit != null) //inna jednostka jest już wybrana - odwybiera ją
            {
                gm.selectedUnit.selected = false;
            }

            selected = true; //wybiera tą jednostkę
            gm.selectedUnit = this;
            GetWalkableTiles();
        }
    }
    void GetWalkableTiles() //pokazuje tile na, które ta jednostka może się poruszyć
    {
        if (hasMoved == true) //jeżeli ta jednostka już wcześniej się poruszyła - wyjście  funkcji
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x) + Mathf.Abs(transform.position.y - tile.transform.position.y) <= tileSpeed)
            {
                if(tile.IsClear() == true)
                {
                    tile.Highlights();
                }

            }
        }
    }
}
