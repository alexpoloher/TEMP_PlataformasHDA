using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    [Header("Parametros")]
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float dashSpeed = 10f;

    //Parea comprobar si estoy en el suelo
    [Header("Detecciones")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] CinemachineConfiner2D confiner2D;
   
    //Variables de detecciones
    protected bool isGrounded;
    protected bool estaAtacando = false;
    protected bool estaAracandoHaciaAbajo = false;
    protected bool estaEnDash = false;
    protected bool isDamaged = false;   //Tiempo que va a estar sin poder moverse
    protected bool isInmune = false;    //Tiempo que va a ser inmune a daño
    protected bool isDead = false;
    protected bool isRight = true;
    protected Blink blink;
    private Vector2 tamanioOriginalCollider;
    protected bool puedeDobleSalto = true;  //Se inicia a true ya que aunque pueda, si no lo tiene, no podrá usarlo
    protected bool tieneDobleSalto = false;




    [Header("Combo")]
    [SerializeField] public float damageToDeal = 5.0f;
    [SerializeField] protected float tiempoPermiteCombo;

    //Variables de combo
    protected int indiceCombo = 0;                  //Indice del combo por el que estça mientras ataca
    private int numIndiceDeAtaquesDelCombo = 2;     //Como un combo son 3 ataques, los indices van de 0 a 2
    private float damageToDealOriginal;
    private float tiempoDelUltimoAtaque;            //Tiempo desde que se pegó la última vez
    private bool puedeGolpearDeNuevo = true;
    private float tiempoRecargaDespuesDeCombo = 1.0f;
    protected bool tienePowerUpCombo = false;

    [Header("Sonidos")]
    [SerializeField] protected AudioClip sonidoSalto;
    [SerializeField] protected AudioClip sonidoAtaque1;
    [SerializeField] protected AudioClip sonidoAtaque2;
    [SerializeField] protected AudioClip sonidoPowerUp;
    [SerializeField] protected AudioClip sonidoDeath;
    [SerializeField] protected AudioClip sonidoMoneda;
    [SerializeField] protected AudioClip sonidoHit;
    [SerializeField] protected AudioClip sonidoDash;
    [SerializeField] protected AudioClip sonidoPinchos;
    [SerializeField] protected AudioClip sonidoVida;
    [SerializeField] protected AudioClip sonidoLongPunch;

    //Componentes
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    protected CapsuleCollider2D elCollider;

    //Acciones a realizar
    protected Vector2 desiredMove = Vector2.zero;
    protected bool mustJump = false;        //Se pone protected para que las clases derivadas/hijas, puedan acceder a ellas
    protected bool mustPunch = false;
    protected bool mustDash = false;
    protected bool mustDownAttack = false;
    protected bool haSaltado = false;
    protected bool lanzadoEstaCayendo = false;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        blink = GetComponent<Blink>();
        elCollider = GetComponent<CapsuleCollider2D>();
        tamanioOriginalCollider = elCollider.size;
        damageToDealOriginal = damageToDeal;
    }

    // Update is called once per frame
    protected virtual void Update() //Con virtual, se permite hacer override en las clases que hereden de esta
    {
        if (!isDead)
        {
            //Detectar si esta en el aire
            isGrounded = ComprobarIsGrounded();
            if (isGrounded && rb.linearVelocityY <= 0.01)
            {
                animator.SetBool("IsGrounded", true);
                haSaltado = false;
                lanzadoEstaCayendo = false;
                puedeDobleSalto = true;
                animator.ResetTrigger("PerformDownAttack");
            }
            else
            {
                animator.SetBool("IsGrounded", false);
            }

            //Movimiento
            if (!estaAtacando && !estaEnDash)
            {
                if (!isDamaged)
                {
                    rb.linearVelocityX = desiredMove.x * walkSpeed;
                }
                if (desiredMove.x != 0)
                {
                    animator.SetBool("IsWalking", true);
                }
                else
                {
                    animator.SetBool("IsWalking", false);
                }

                if ((desiredMove.x < 0 && isRight) || (desiredMove.x > 0 && !isRight))
                {
                    Voltear();
                }
                /*if (desiredMove.x < 0 && isRight)
                {
                    spriteRenderer.flipX = true;
                }
                else if (desiredMove.x > 0)
                {
                    spriteRenderer.flipX = false;
                }*/

            }
            else
            {
                if (estaAtacando)
                {
                    animator.SetBool("IsWalking", false);
                    rb.linearVelocityX = 0;
                }


            }


            if ((mustJump && isGrounded && !estaAtacando && !estaEnDash) || (mustJump && rb.linearVelocityY < 0 && puedeDobleSalto && tieneDobleSalto && !estaAtacando && !estaEnDash))
            {
                //mustJump = false;
                if (haSaltado == true || (haSaltado == false && rb.linearVelocityY < -0.1)) {
                    puedeDobleSalto = false;
                }


                haSaltado = true;
                animator.SetTrigger("PerformJump");
                rb.linearVelocityY = jumpSpeed;
                lanzadoEstaCayendo = false;
                GestorSonido.Instance.EjecutarSonido(sonidoSalto);
            }
            //mustJump = false;
            if (mustPunch && isGrounded && !estaAtacando && !estaEnDash && puedeGolpearDeNuevo)
            {

                if (tienePowerUpCombo && (indiceCombo > 0 && Time.time > tiempoDelUltimoAtaque + tiempoPermiteCombo || indiceCombo > numIndiceDeAtaquesDelCombo))
                {
                    ResetearCombo();
                }


                //mustPunch = false;
                if (indiceCombo < numIndiceDeAtaquesDelCombo)
                {
                    GestorSonido.Instance.EjecutarSonido(sonidoAtaque1);
                    animator.SetTrigger("PerformPunch");
                }
                else {
                    GestorSonido.Instance.EjecutarSonido(sonidoAtaque2);
                    damageToDeal = damageToDeal * 2;
                    animator.SetTrigger("Punch2");
                }
                estaAtacando = true;
                tiempoDelUltimoAtaque = Time.time;

                //Si acaba de hacer el último golpe del combo, se tiene un pequeño delay antes de volver a atacar
                if(indiceCombo == numIndiceDeAtaquesDelCombo)
                {
                    StartCoroutine(RecargarDespuesDeCombo());
                }

                //Se incrementa el indice del combo para hacer el siguietne ataque que corresponda
                if (tienePowerUpCombo) {
                    indiceCombo++;
                }


            }
            if ((mustDash && isGrounded && !estaAtacando && !estaEnDash && !haSaltado) || mustDash && animator.GetBool("SeQuedaAgachado"))
            {
                //mustDash = false;
                estaEnDash = true;
                if (transform.localScale.x < 0 )
                {
                    rb.linearVelocityX = Vector2.left.x * dashSpeed;
                }
                else
                {
                    rb.linearVelocityX = Vector2.right.x * dashSpeed;
                }

                GestorSonido.Instance.EjecutarSonido(sonidoDash);
                animator.SetTrigger("PerformCrouchDash");
                //rb.AddForceX(20, ForceMode2D.Impulse);
                //rb.linearVelocityX = desiredMove.x * walkSpeed * 3;
            }


            //Si cae y no ha saltado, o si ha saltado pero la velocidad vertical es menor que 0, entonces animación de caida
            bool estaCayendo = !isGrounded && (!haSaltado || rb.linearVelocityY < 0) && !isDamaged;
            //animator.SetBool("IsFalling", estaCayendo);
            if (!lanzadoEstaCayendo && estaCayendo)
            {
                animator.SetTrigger("Falling");
                lanzadoEstaCayendo = true;
            }

            if (estaCayendo && mustDownAttack) {
                mustDownAttack = false;
                estaAracandoHaciaAbajo = true;
                animator.SetTrigger("PerformDownAttack");
            }
        }
    }


    private void ResetearCombo() {
        indiceCombo = 0;
        damageToDeal = damageToDealOriginal;
    }

    //Después de un combo, durante un pequeño momento no puedes atacar, para balancear el haber hecho el doble de daño
    IEnumerator RecargarDespuesDeCombo()
    {
        puedeGolpearDeNuevo = false;
        yield return new WaitForSeconds(tiempoRecargaDespuesDeCombo);
        puedeGolpearDeNuevo = true;
    }



    protected virtual void Morir()
    {
        isDead = true;
        GestorSonido.Instance.EjecutarSonido(sonidoDeath);
        animator.SetBool("IsWalking", false);
        desiredMove = Vector2.zero;
        rb.linearVelocityX = 0.0f;
        confiner2D.BoundingShape2D = null;
        confiner2D.InvalidateBoundingShapeCache();
    }

    private void Voltear()
    {
        Vector3 scalePlayer = transform.localScale;
        scalePlayer.x *= -1;
        transform.localScale = scalePlayer;
        isRight = !isRight;
    }


    public void DestruirGameObject()
    {

        spriteRenderer.enabled = false;
        GestorPlayer.Instance.QuitarIntento();
        //Destroy(gameObject);
    }

    //Función que se llama desde un evento en la animación
    public void TerminarAtaque()
    {
        estaAtacando = false;
    }

    public void TerminarAtaqueHaciaAbajo()
    {
        estaAracandoHaciaAbajo = false;
    }

    public void ResetearDobleSalto()
    {
        puedeDobleSalto = true;

    }
    //Función que se llama desde un evento en la animación
    public void TerminarDash()
    {
        rb.linearVelocityX = 0.0f;
        bool sePuedeLevantar = SePuedeLevantar();
        if (sePuedeLevantar)
        {
            animator.SetBool("SeQuedaAgachado", false);
        }
        else
        {
            animator.SetBool("SeQuedaAgachado", true);
            rb.linearVelocityX = 0.0f;
        }
            estaEnDash = !sePuedeLevantar;
    }

    //Se comprueba si al hacer el dash, hay pared encima, por lo que se queda agachado
    private bool SePuedeLevantar()
    {
        Vector2 center = new Vector2(transform.position.x, transform.position.y) + Vector2.up * (tamanioOriginalCollider.y / 2);
        float alturaExtraComprobacion = 0.1f;

        RaycastHit2D hayContacto = Physics2D.BoxCast(center, new Vector2(tamanioOriginalCollider.x, tamanioOriginalCollider.y), 0f, Vector2.up, alturaExtraComprobacion, whatIsGround);
        if(hayContacto){
            rb.linearVelocityX = 0.0f;
            return false;   //Si hay algo encima, no se levanta
        }
        else
        {
            return true;    //Si no hay nada encima, se puede levantar
        }
    }

    private bool ComprobarIsGrounded()
    {
        /*float alturaExtraComprobacion = 0.1f;
        RaycastHit2D hayContactoSuelo = Physics2D.BoxCast(collider.bounds.min, collider.bounds.size, 0f, Vector2.down, alturaExtraComprobacion, whatIsGround);
        return hayContactoSuelo;*/

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    /*public void SetDesiredMove(Vector2 desiredMove) {
        this.desiredMove = desiredMove;
    }

    public void MustJump() {
         mustJump = true;
    }

    public void MustPunch()
    {
        mustPunch = true;
    }*/

}
