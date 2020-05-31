using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public Unit selectedUnit; //zapisuje wybraną jednostkę

    public int playerTurn = 1; //zapisuje który gracz ma teraz ture: 1 lewo, 2 prawo

    public GameObject selectedUnitHighlight; //miejsce na podświetlenie wybranej jednostki

    public Image playerIndicator;
    public Sprite player1Indicator;
    public Sprite player2Indicator;
    
    public int player1Gold = 100;
    public int player2Gold = 100;

    public TextMeshProUGUI player1GoldText;
    public TextMeshProUGUI player2GoldText;

    private void Start()
    {
        GetGoldIncome(1);
    }

    public void UpdateGoldText()
    {
        player1GoldText.text = player1Gold.ToString();
        player2GoldText.text = player2Gold.ToString();
    }

    void GetGoldIncome(int playerTurn)
    {
        foreach (Totem totem in FindObjectsOfType<Totem>())
        {
            if (totem.playerNumber == playerTurn)
            {
                if (playerTurn == 1)
                {
                    player1Gold += totem.goldPerTurn;
                }
                else
                {
                    player2Gold += totem.goldPerTurn;
                }
            }
        }
        UpdateGoldText();
    }
    
    public void ResetTiles() //resetuje podświetlenie tilów
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            tile.Reset();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) //jeżeli gracz wcisnie wybrany przycisk = kończy swoją ture
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

    void EndTurn() //zmienia aktywnego gracza, kończy ture
    {
        if (playerTurn == 1)
        {
            playerTurn = 2;
            playerIndicator.sprite = player2Indicator;
        }else if (playerTurn == 2)
        {
            playerTurn = 1;
            playerIndicator.sprite = player1Indicator;
        }
        
        GetGoldIncome(playerTurn);

        if (selectedUnit != null) //gdyby poprzedni gracz zostawił jakąś jednostkę zaznaczoną - odznacza ją
        {
            selectedUnit.selected = false;
            selectedUnit = null;
        }

        ResetTiles();

        foreach (Unit unit in FindObjectsOfType<Unit>()) //dla każdej jednostki
        {
            unit.hasMoved = false;
            unit.attackIcon.SetActive(false);
            unit.hasAttacked = false;
        }
        
        GetComponent<UnitShop>().CloseMenus();
    }
}
