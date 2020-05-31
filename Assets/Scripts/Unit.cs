using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public int health; //punkty życia jednostki
    public int attackDamage; //punkty obrażeń jednostki podczas ataku
    public int defenseDamage; //punkty obrażeń jednostek kiedy się broni
    public int armor; //ogranicznik przyjmowanych obrażeń

    public DamageIcon damageIcon;
    public GameObject deathEffect;

    public Animator animator; //animator jednostki

    private Animator camAnim; //animator kamery

    public Text kingHealth;
    public bool isKing;

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        animator.speed = Random.Range(0.9f, 1.1f); //losowa prędkość animacji jednostki w zakresie
        camAnim = Camera.main.GetComponent<Animator>();
        UpdateKingHealth();
    }

    public void UpdateKingHealth()
    {
        if (isKing == true)
        {
            kingHealth.text = health.ToString();
        }
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

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f); //w miejscu klikniecia tworzy okrągły collider
        Unit unit = col.GetComponent<Unit>();
        if (gm.selectedUnit != null) //jeżeli mamy wybraną jednostke
        {
            if (gm.selectedUnit.enemiesInRange.Contains(unit) && gm.selectedUnit.hasAttacked == false) //jeżeli wybrana jednostka ma w zasięgu przeciwnika i nie atakowała jeszcze w tej turze
            {
                gm.selectedUnit.Attack(unit);
            }
        }
    }

    void Attack(Unit enemy)
    {
        camAnim.SetTrigger("shake");

        hasAttacked = true;

        int enemyDamage = attackDamage - enemy.armor; //ile obrażeń ta jednostka zada przeciwnikowi
        int myDamage = enemy.defenseDamage - armor; //ile obrażeń przy ataku przeciwnika otrzyma ta jednostka

        if (enemyDamage >= 1)
        {
            DamageIcon instance = Instantiate(damageIcon, enemy.transform.position, Quaternion.identity);
            instance.Setup(enemyDamage); 
            enemy.health -= enemyDamage;
            enemy.UpdateKingHealth();
        }

        if (myDamage >= 1)
        {
            DamageIcon instance = Instantiate(damageIcon, transform.position, Quaternion.identity);
            instance.Setup(myDamage);
            health -= myDamage;
            UpdateKingHealth();;
        }

        if (enemy.health <= 0)
        {
            Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }

        if (health <= 0)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gm.ResetTiles();
            Destroy(this.gameObject);
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
