using System;
using System.Collections;
using UnityEngine;

public class BossScript : EnemyGeneral
{
    [Header("Sonidos del Boss")]
    [SerializeField] AudioClip sonidoPresentacion;
    [SerializeField] AudioClip sonidoAtaque;

    [Header("Comportamiento")]
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] float tiempoEsperaEnPunto = 3f;
    [SerializeField] Transform puntoDisparo;
    [SerializeField] GameObject prefabDisparo;
    [SerializeField] GestorVidaBoss gestorBarraVida;

    private bool esperando = false;
    private bool enSuelo = true;
    private Transform objetivoActual;
    private float distanciaLlegada = 0.5f;
    private bool enRebotes = false;
    private bool estaMoviendose = false;
    public static Action BossGolpeado;   //Acción a la que se va a suscibir el GestorVibracionCamara para aplicar esta vibración
    private float vidaMaximaBoss;


    private enum tiposAtaque
    {
        Saltos,
        Proyectil
    }
    private tiposAtaque siguienteAtaque;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        objetivoActual = puntoA;
        vidaMaximaBoss = vida;
    }

    private void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Entorno") && estaMoviendose && !isDead)
        {
            enSuelo = true;
            animator.SetBool("EstaRebotando", false);
            animator.SetTrigger("Land");
            if (esperando) 
            {
                return;
                
            }
            else
            {
                float distanciaActual = Mathf.Abs(transform.position.x - objetivoActual.position.x);
                if (distanciaActual <= distanciaLlegada)
                {
                    StartCoroutine(EsperarVoltear(true));
                }
                else
                {
                    SaltarHaciaObjetivo();
                }
                
            }
        }
    }

    private void SaltarHaciaObjetivo()
    {
        if (enSuelo && estaMoviendose && !isDead) {
            enSuelo = false;
            float direccion = Mathf.Sign(objetivoActual.position.x - transform.position.x);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(new Vector2(direccion * speed, jumpSpeed), ForceMode2D.Impulse);
           //Solo se hace la animación de perform jump si es la primera vez que salta, los sucesivos rebotes no la hacen
            if (!enRebotes)
            {
                animator.SetTrigger("PerformJump");
            }
            enRebotes = true;
            animator.SetBool("EstaRebotando", true);
        }
    }

    IEnumerator EsperarVoltear(bool voltear)
    {
        esperando = true;
        enRebotes = false;
        animator.ResetTrigger("Land");
        if (voltear) {
            VoltearBoss();
        }
        yield return new WaitForSeconds(tiempoEsperaEnPunto);

        if (!isDead)
        {
            if (voltear)
            {
                if (objetivoActual == puntoA)
                {
                    objetivoActual = puntoB;
                }
                else
                {
                    objetivoActual = puntoA;
                }
            }
            esperando = false;
            siguienteAtaque = DecidirAtaque();
            if (siguienteAtaque == tiposAtaque.Saltos && !isDead)
            {
                SaltarHaciaObjetivo();
            }
            else
            {
                if (!isDead)
                {
                    LanzarProyectil();
                }
            }
        }
    }

    private void LanzarProyectil()
    {
        GestorSonido.Instance.EjecutarSonido(sonidoAtaque);
        animator.SetTrigger("Disparar");
        StartCoroutine(EsperarVoltear(false));
    }

    public void InstanciarProyectil()
    {
        Instantiate(prefabDisparo, puntoDisparo.position, puntoDisparo.rotation);
    }




    //Decide si salta o lanza proyectil
    private tiposAtaque DecidirAtaque()
    {
        if (!isDead)
        {
            int numRandom = UnityEngine.Random.Range(0, 4);
            if (numRandom < 2)
            {    //Si es menor que 2 el número, hace saltos
                return tiposAtaque.Saltos;
            }
            else
            {
                return tiposAtaque.Proyectil;
            }
        }
        return tiposAtaque.Saltos;
    }


    private void VoltearBoss()
    {
        Vector3 scaleEnemigo = transform.localScale;
        scaleEnemigo.x *= -1;
        transform.localScale = scaleEnemigo;
    }


    // Update is called once per frame
    protected override void Update()
    {
        //Si meintras salta el boss se pasa el objetivo, se cae haciaa abajod irectamente para no pasarse
        if (!esperando && estaMoviendose)
        {
            if (!enSuelo)
            {
                if(Mathf.Abs(objetivoActual.position.x - transform.position.x) <= distanciaLlegada)
                {
                    rb.linearVelocityX = 0;
                }
            }
        }
        //Al quitarle cierta cantidad de vida, actúa más rápido
        if(vida <= 10 && tiempoEsperaEnPunto >= 2)
        {
            tiempoEsperaEnPunto = tiempoEsperaEnPunto / 2;
        }

        if (transform.localScale.x < 0)
        {
            puntoDisparo.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else
        {
            puntoDisparo.rotation = Quaternion.Euler(0f, 0f, 0f);
        }


    }

    public void Activar()
    {
        GestorPlayer.Instance.ImpedirMovimiento();  //Se impide que el jugador se mueva por un momento
        StartCoroutine(ActivarBoss());
       
    }

    IEnumerator ActivarBoss()
    {
        GestorSonido.Instance.EjecutarSonido(sonidoPresentacion);
        gestorBarraVida.MostrarBarraVidaBoss();
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Activar");

    }
    public void EmpezarAMoverse()
    {
        StartCoroutine(EmpezarMovimientos());
    }

    IEnumerator EmpezarMovimientos()
    {
        
        yield return new WaitForSeconds(2f);
        GestorPlayer.Instance.PermitirMovimiento();
        estaMoviendose = true;  //Desde aquí, empieza la pelea contra el Boss
        SaltarHaciaObjetivo();
    }

    protected override void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Weapon") && !isDamaged)
        {
            BossGolpeado?.Invoke();
        }
        base.OnTriggerEnter2D(elOtro);
        gestorBarraVida.ActualizarVida(vida, vidaMaximaBoss);
    }

    protected override void Morir()
    {
        rb.linearVelocityY = 0.0f;
        GestorPlayer.Instance.FinalizarJuego();
        StartCoroutine(OcultarBarraVida());
        base.Morir();
    }

    IEnumerator OcultarBarraVida() {

        yield return new WaitForSeconds(3.0f);
        gestorBarraVida.OcultarBarraVida();
    }

}
