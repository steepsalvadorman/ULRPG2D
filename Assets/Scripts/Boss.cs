using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarGO;
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private int bossDamage;

    [SerializeField] private float speed;
    [SerializeField] private GameObject player; //Referencia al jugador para calcular la distancia
    [SerializeField] private float minFollowDistance; //Minima distancia para seguir
    [SerializeField] private float minAttackDistance; //Minima distancia para atacar
    [SerializeField] private float fireArea;
    [SerializeField] private float CDfire = 2;
    [SerializeField] private GameObject fire;

    private float distancePlayer;

    //FMS
    public enum BossState
    {
        Idle,
        FollowPlayer,
        AttackPlayer
    }

    public BossState currentState;
    void Start()
    {
        currentState = BossState.Idle;
        //Igualamos actual al maximo de vida al iniciar.
        hp = maxHp;
    }

    void Update()
    {
        //bajar cd de ataque
        CDfire -= Time.deltaTime;
        //Para actualizar vida
        healthBar.fillAmount = hp / maxHp;

        //Actualizar la logica del estado actual


        //Calcular distancia con el jugador
        distancePlayer = Vector2.Distance(player.transform.position, transform.position);

        switch (currentState)
        {
            case BossState.Idle:

                //Si la distancia es menor a la distancia minima, cambio de estado
                currentState = (distancePlayer < minFollowDistance) ? BossState.FollowPlayer : currentState;
                break;
            case BossState.FollowPlayer:

                //Si el personaje sale de rango volvemos a idle. Mientras no, lo seguimos.
                currentState = (distancePlayer > minFollowDistance) ? BossState.Idle : currentState; 
                if (distancePlayer <= minFollowDistance)
                {
                        FollowPlayer();
                        healthBarGO.SetActive(true);
                }
                else
                {
                    healthBarGO.SetActive(false);
                }
                
                //Si la distancia es minima, pasamos a atacar
                currentState = (distancePlayer < minAttackDistance) ? BossState.AttackPlayer : currentState;

                break;
            case BossState.AttackPlayer:
                currentState = (distancePlayer > minAttackDistance) ? BossState.FollowPlayer : currentState;

                //Logica para atacar al jugador

                StartCoroutine(Fire());


                break;
        }
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(1);

        fire.SetActive(currentState == BossState.AttackPlayer);

        if (CDfire < 0)
        {
            Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fireArea);
            foreach (Collider2D col in objects)
            {
                if (col.CompareTag("Jugador"))
                {
                    col.gameObject.GetComponent<PlayerMovement>().TakeDamagePlayer(bossDamage);
                    CDfire = 2;
                }
            }
        }
    }
    private void FollowPlayer()
    {
        // Calcula la direcci�n hacia el jugador
        Vector3 direction = player.transform.position - transform.position;

        direction.Normalize();

        float movementAmount = speed * Time.deltaTime;
        Vector3 targetPosition = transform.position + direction * movementAmount;
        transform.position = targetPosition;
    }
    public void TakeDamageBoss(int damage)
    {
        hp -= damage;
        if(hp <= 0){

            Destroy(gameObject,1f);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minFollowDistance);
        Gizmos.DrawWireSphere(transform.position, minAttackDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireArea);

    }
}
