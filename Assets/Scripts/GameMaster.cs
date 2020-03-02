using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit; //zapisuje wybraną jednostkę

    public int playerTurn = 1; //zapisuje który gracz ma teraz ture: 1 lewo, 2 prawo

    public GameObject selectedUnitHighlight; //miejsce na podświetlenie wybranej jednostki

    public void ResetTiles() //resetuje podświetlenie tilów
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //jeżeli gracz nadusi wybrany przycisk = kończy swoją ture
        {
            EndTurn();
        }

        if (selectedUnit != null) //po wybraniu jednostki aktywuje selectedUnitHighlight i porusza nim za jednostką
        {
            selectedUnitHighlight.SetActive(true);
            selectedUnitHighlight.transform.position = selectedUnit.transform.position;
        }
        else
        {
            selectedUnitHighlight.SetActive(false);
        }
    }

    void EndTurn() //zmienia aktywnego gracza
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
        }else if (playerTurn == 2)
        {
            playerTurn = 1;
        }

        if (selectedUnit != null) //gdyby poprzedni gracz zostawił jakąś jednostkę zaznaczoną - odznacza ją
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }

        ResetTiles();

        foreach (Unit unit in FindObjectsOfType<Unit>()) //resetuje "zużycie" jednostek
        {
            unit.hasMoved = false;
        }
    }
}
