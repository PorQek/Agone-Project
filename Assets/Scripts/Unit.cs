using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool selected; //sprawdza czy ta jednostka jest wybrana
    GameMaster gm; //daje dostęp do skryptu GameMastera

    public int tileSpeed; //ile tilesów ta jednostka może się poruszyć
    public bool hasMoved; //sprawdza czy ta jednostka poruszyła się już w tej turze

    public float moveSpeed; //prędkość poruszania się jednostki;

    public int playerNumber; //mówi do którego gracza należy ta jednostka

    public int attackRange; //zasięg ataku jednostki
    List<Unit> enemiesInRange = new List<Unit>(); //przechowuje inne jednostki, które znajdą się w zasięgu ataku
    public bool hasAttacked; //sprawdza czy ta jednostka już w tej turze atakowała

    public GameObject attackIcon; //miejsce na ikone możliwości ataku danej jednostki

    public Animator animator;

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        animator.speed = Random.Range(0.9f, 1.1f);
    }

    private void OnMouseDown() //po kliknięciu na jednostkę
    {
        ResetAttackIcons();

        if (selected == true) //jeżeli jednostka jest już wybrana - odwybiera ją
        {
            selected = false;
            gm.selectedUnit = null;
            gm.ResetTiles();
        }
        else //jeżeli ta jednostka nie jest wybrana
        {
            if (playerNumber == gm.playerTurn) //sprawdza czy odpowiedni gracz zaznacza tą jednostkę
            {
                if (gm.selectedUnit != null) //inna jednostka jest już wybrana - odwybiera ją
                {
                    gm.selectedUnit.selected = false;
                }

                selected = true; //wybiera tą jednostkę
                gm.selectedUnit = this;

                gm.ResetTiles();
                GetEnemies();
                GetWalkableTiles();
            }           
        }
    }
    void GetWalkableTiles() //pokazuje tile na, które ta jednostka może się poruszyć
    {
        if (hasMoved == true) //jeżeli ta jednostka już wcześniej się poruszyła - wyjście  funkcji
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>()) //oblicza, gdzie jednostka może dojść i podświetla te tile
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

    void GetEnemies() //funkcja zwracająca jednostki, które można zaatakować
    {
        enemiesInRange.Clear(); //w pierwszej kolejności czyści poprzednio zapisanych przeciwników w zasięgu

        foreach (Unit unit in FindObjectsOfType<Unit>()) //dla wszystkich jednostek
        {
            if (Mathf.Abs(transform.position.x - unit.transform.position.x) + Mathf.Abs(transform.position.y - unit.transform.position.y) <= attackRange) //sprawdza które jednostki są w zasięgu ataku
            {
                if (unit.playerNumber != gm.playerTurn && hasAttacked == false) //sprawdza czy znaleziona jednostka nie należy przypadkiem do gracza && nasza jednostka już atakowała
                {
                    enemiesInRange.Add(unit); //dodaje do listy jednostki, które można zaatakować
                    unit.attackIcon.SetActive(true); //włącza ikone ataku jednostce, którą właśnie dodaje do listy
                }
            }
        }
    }

    public void ResetAttackIcons() //funkcja, która ma wyłączać wszystkie aktyywne ikony ataku
    {
        foreach (Unit unit in FindObjectsOfType<Unit>()) //dla wszystkich jednostek
        {
            unit.attackIcon.SetActive(false);
        }
    }

    public void Move(Vector2 tilePos) //funkcja przesuwająca jednostke na wyznaczony tile
    {
        gm.ResetTiles();
        StartCoroutine(StartMovement(tilePos));        
    }
    IEnumerator StartMovement(Vector2 tilePos) 
    {
        while(transform.position.x != tilePos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y), moveSpeed * Time.deltaTime); //poruszanie się poziomo
            yield return null;
        }

        while (transform.position.y != tilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tilePos.y), moveSpeed * Time.deltaTime); //poruszanie sie pionowo
            yield return null;
        }

        hasMoved = true;
        ResetAttackIcons();
        GetEnemies();
    }
}
