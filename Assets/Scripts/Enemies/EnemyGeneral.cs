using System.Collections;
using UnityEngine;

public class EnemyGeneral : MonoBehaviour
{
    [Header("Parametros")]
    [SerializeField] protected float vida;
    [SerializeField] protected float speed;
    [SerializeField] float tiempoInmunidad = 0.5f;
    [SerializeField] float knocBackForceX = 0.5f;
    [SerializeField] float knocBackForceY = 0.5f;
    [SerializeField] public float damageToDeal = 1.0f;

    [Header("Detecciones")]
    [SerializeField] protected Transform groundCheck, precipicioCheck, paredCheck, puntoA, puntoB;
    [SerializeField] float radioDeteccion;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] protected bool isRight = false;
    [SerializeField] float tiempoEspera;

    [Header("Sonidos")]
    [SerializeField] protected AudioClip sonidoMuerte;
    [SerializeField] protected AudioClip sonidoHit;

    protected bool isDamaged = false;
    protected bool isDead = false;
    protected Vector2 desiredMove = Vector2.zero;
    protected bool mustJump = false;
    protected bool mustPunch = false;
    private bool haciaA, haciaB;
    private bool mustWait;
    protected bool isWaiting = false;
    protected bool estaAtacando = false;
    protected bool isGrounded, hayPrecipicio, hayMuro;

    //El static no se moverá. El caminante cambiará de dirección cunado tope con muro o caida. Y el patrulla busca entre 2 puntos y luego se detiene.
    protected enum enumTipoEnemigo
    {
        Static,
        Caminante,
        Patrulla,
        Boss
    };

    [SerializeField] protected enumTipoEnemigo tipoEnemigo;
    Blink blink;    //Encargado de al golpearle, hacerle parpadear

    //Componentes
    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;
    protected Animator animator;

    private void Awake()
    {
        blink = GetComponent<Blink>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        //Si empieza yendo hacia la izquierda, el primer punto al que va es a A
        if (tipoEnemigo.Equals(enumTipoEnemigo.Patrulla))
        {
            animator.SetBool("IsWalking", true);
            haciaA = !isRight;
        } else if (tipoEnemigo.Equals(enumTipoEnemigo.Static)){
            animator.SetBool("IsWalking", false);
        }
        else if (tipoEnemigo.Equals(enumTipoEnemigo.Caminante))
        {
            animator.SetBool("IsWalking", true);
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isDead)
        {
            //Detectar si esta en el aire
            if (groundCheck != null)
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, radioDeteccion, whatIsGround);
            }

            //Movimiento
            if (tipoEnemigo.Equals(enumTipoEnemigo.Caminante) && !isDamaged)
            {
                //animator.SetBool("IsWalking", true);
                rb.constraints = RigidbodyConstraints2D.FreezeRotation; //Para que no se mueva nada, no se le arrastre
                if (!isRight)
                {
                    desiredMove = Vector2.left;
                }
                else
                {
                    desiredMove = Vector2.right;
                }
                rb.linearVelocityX = desiredMove.x * speed;

                hayPrecipicio = !Physics2D.OverlapCircle(precipicioCheck.position, radioDeteccion, whatIsGround);
                hayMuro = Physics2D.OverlapCircle(paredCheck.position, radioDeteccion, whatIsGround);

                if ((hayMuro || hayPrecipicio) && isGrounded)
                {
                    Voltear();
                }

            }
            else if (tipoEnemigo.Equals(enumTipoEnemigo.Static))
            {
                //animator.SetBool("IsWalking", false);
                rb.constraints = RigidbodyConstraints2D.FreezeAll; //Para que no se mueva nada, no se le arrastre
            }
            else if (tipoEnemigo.Equals(enumTipoEnemigo.Patrulla))
            {
                //Detectar si esta en el aire
                if (groundCheck != null)
                {
                    isGrounded = Physics2D.OverlapCircle(groundCheck.position, radioDeteccion, whatIsGround);
                    if (!isGrounded)
                    {
                        tipoEnemigo = enumTipoEnemigo.Caminante;
                    }
                }



                if (!isRight)
                {
                    desiredMove = Vector2.left;
                }
                else
                {
                    desiredMove = Vector2.right;
                }

                if (!isWaiting){
                    rb.linearVelocityX = desiredMove.x * speed;

                    //Ver si ya ha llegado al punto final
                    if (!isRight)
                    {
                        if (Vector2.Distance(transform.position, puntoA.position) <= 0.2f) {
                                StartCoroutine(EsperarPorPatrulla());
                                

                        }
                    }
                    else  //Si va hacia B
                    {
                        if (Vector2.Distance(transform.position, puntoB.position) <= 0.2f)
                        {

                                StartCoroutine(EsperarPorPatrulla());
                                
                        }
                    }
                }

            }



        }

 

    }


    IEnumerator EsperarPorPatrulla()
    {
        animator.SetBool("IsWalking", false);
        isWaiting = true;
        rb.linearVelocityX = 0;
        yield return new WaitForSeconds(tiempoEspera);
        isWaiting = false;
        animator.SetBool("IsWalking", true);
        Voltear();
    }

    private void Voltear()
    {
        isRight = !isRight;
        //spriteRenderer.flipX = !spriteRenderer.flipX;
        Vector3 scaleEnemigo = transform.localScale;
        scaleEnemigo.x *= -1;
        transform.localScale = scaleEnemigo;

    }
    protected virtual void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Weapon") && !isDamaged && !isDead)
        {
            RecibirDaño(elOtro.GetComponentInParent<PlayerController>().damageToDeal, elOtro.transform.position);
        }
    }

    public void RecibirDaño(float dañoAAplicar, Vector2 elOtroPosicion)
    {
        if (elOtroPosicion.x < transform.position.x) //Si se le golpea desde la izquierda, se impulsa a la derecha
        {
            rb.linearVelocity = new Vector2(knocBackForceX, knocBackForceY);
            //rb.AddForce(new Vector2(knocBackForceX, knocBackForceY), ForceMode2D.Force);  //Al golpear al enemigo, irá hacia atrás 
        }
        else  //Si se le golpea desde la derecha, se impulsa a la izquierda
        {
            rb.linearVelocity = new Vector2(-knocBackForceX, knocBackForceY);
            //rb.AddForce(new Vector2(-knocBackForceX, knocBackForceY), ForceMode2D.Force);  //Al golpear al enemigo, irá hacia atrás 
        }

        vida -= dañoAAplicar;
        if (vida <= 0.0f)
        {
            vida = 0.0f;
            Morir();
        }
        else {
            GestorSonido.Instance.EjecutarSonido(sonidoHit);
            StartCoroutine(ReestablecerIsDamaged());
        }
    }

    protected virtual void Morir()
    {
        gameObject.layer = LayerMask.NameToLayer("EnemyInmune");
        transform.parent.gameObject.layer = LayerMask.NameToLayer("EnemyInmune");
        animator.SetTrigger("Morir");
        rb.linearVelocityX = 0.0f;
        isDead = true;
        GestorSonido.Instance.EjecutarSonido(sonidoMuerte);
    }

    //Al golpear el enemigo, se le da un timepo de inmunidad
    IEnumerator ReestablecerIsDamaged()
    {
        isDamaged = true;
        spriteRenderer.material = blink.blink;
        yield return new WaitForSeconds(tiempoInmunidad);
        isDamaged = false;
        spriteRenderer.material = blink.original;

    }

    public void DestruirEnemigo()
    {
        Destroy(gameObject);
    }

}
