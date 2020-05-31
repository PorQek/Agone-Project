using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitShop : MonoBehaviour
{
    public Button player1ToogleButton;
    public Button player2ToogleButton;

    public GameObject player1Menu;
    public GameObject player2Menu;

    private GameMaster gm;

    private void Start()
    {
        gm = GetComponent<GameMaster>();
    }

    private void Update()
    {
        if (gm.playerTurn == 1)
        {
            player1ToogleButton.interactable = true;
            player2ToogleButton.interactable = false;
        }
        else
        {
            player1ToogleButton.interactable = false;
            player2ToogleButton.interactable = true;
        }
    }

    public void ToogleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void CloseMenus()
    {
        player1Menu.SetActive((false));
        player2Menu.SetActive((false));

    }

    public void BuyItem(ShopItem item)
    {
        if (gm.playerTurn == 1 && item.cost <= gm.player1Gold)
        {
            gm.player1Gold -= item.cost;
            player1Menu.SetActive(false);
        }
    }
}
