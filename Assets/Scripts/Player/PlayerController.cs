using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MovementController
{

    [Header("Valores Jugador")]
    [SerializeField] private float delayDash = 0.2f;
    [SerializeField] float vidaActual = 3.0f;
    [SerializeField] float vidaMaxima = 3.0f;
    [SerializeField] float knocBackForceX = 100f;
    [SerializeField] float knocBackForceY = 100f;
    [SerializeField] float tiempoHit = 100f;
    [SerializeField] float tiempoInmunidad = 100f;
    [SerializeField] float velocidadRebote = 5.0f;

    [Header("Controles")]
    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;
    [SerializeField] InputActionReference punch;
    [SerializeField] InputActionReference longPunch;
    [SerializeField] InputActionReference crouchDash;
    [SerializeField] InputActionReference downAttack;


    //Parámetros para controlar los Power Ups que se tienen
    private bool tieneLongPunch = false;
    private bool tieneDownAttack = false;

    //Variables de acciones
    private float tiempoInactivo = 0.0f;
    private bool mustLongPunch = false;
    private bool puedeUsarDash = true;

    //Acciones a las que se suscriben otros
    public static Action<float> playerGolpeado;   //Acción a la que se va a suscibir el GestorVibracionCamara para aplicar esta vibración
    public static Action<float> playerCuraVida;
    


    private void Start()
    {
        //vidaActual = GestorPlayer.Instance.vidaMaxima;
    }

    protected override void Update()        //Con override, ejecutará este update en lugar del de la clase padre
    {
        //UpdateRawMove();
        base.Update();  //Con base. ejecuta el update de la clase padre de la que hereda esta

        //Comportaminetos de solo el player
        if (mustLongPunch && isGrounded && !estaAtacando && !estaEnDash)
        {
            animator.SetTrigger("PerformLongPunch");
            GestorSonido.Instance.EjecutarSonido(sonidoLongPunch);
            estaAtacando = true;
        }

        if(isDead && rb.linearVelocityX != 0f)
        {
            rb.linearVelocityX = 0f;
        }

        ReiniciarAcciones();
        ContarTiempoParado();
    }

    private void ReiniciarAcciones()
    {
        mustDash = false;
        mustJump = false;
        mustPunch = false;
        mustLongPunch = false;
        mustDownAttack = false;
    }

    private void ContarTiempoParado()
    {
        tiempoInactivo += Time.deltaTime;
        if (desiredMove != Vector2.zero || rb.linearVelocity != Vector2.zero)
        {
            tiempoInactivo = 0.0f;
        }
        else
        {
            if(tiempoInactivo >= 10.0f)
            {
                animator.SetTrigger("EstaInactivo");
                tiempoInactivo = 0.0f;
            }

        }
    }

    public void ImpulsarArribaPorRebote()
    {
        rb.linearVelocityY = velocidadRebote;

    }

    private void OnEnable()
    {
        //Se habilita la lectura de los inputs
        move.action.Enable();
        jump.action.Enable();
        punch.action.Enable();
        longPunch.action.Enable();
        crouchDash.action.Enable();
        downAttack.action.Enable();

        move.action.started += OnMove;
        move.action.performed += OnMove;
        move.action.canceled += OnMove;

        jump.action.started += OnJump;
        punch.action.started += OnPunch;
        longPunch.action.started += OnLongPunch;
        crouchDash.action.started += OnCrouchDash;
        downAttack.action.started += OnDownAttack;
    }

    public void ImpedirMovimientos()
    {
        move.action.Disable();
        jump.action.Disable();
        punch.action.Disable();
        longPunch.action.Disable();
        crouchDash.action.Disable();
        downAttack.action.Disable();
    }

    public void Permitirmovimientos()
    {
        move.action.Enable();
        jump.action.Enable();
        punch.action.Enable();
        longPunch.action.Enable();
        crouchDash.action.Enable();
        downAttack.action.Enable();
    }

    private void OnDisable()
    {
        move.action.started -= OnMove;
        move.action.performed -= OnMove;
        move.action.canceled -= OnMove;

        jump.action.started -= OnJump;
        punch.action.started -= OnPunch;
        longPunch.action.started -= OnLongPunch;
        crouchDash.action.started -= OnCrouchDash;
        downAttack.action.started -= OnDownAttack;
    }

    private void OnMove(InputAction.CallbackContext obj) {

        if (!isDead)
        {
            desiredMove = obj.ReadValue<Vector2>();
            ReiniciarTiempoInactivo();
        }
    }
    private void OnJump(InputAction.CallbackContext obj)
    {
        if (!isDead)
        {
            mustJump = true;
            ReiniciarTiempoInactivo();
        }
    }
    private void OnPunch(InputAction.CallbackContext obj)
    {
        if (!isDead)
        {
            mustPunch = true;
            ReiniciarTiempoInactivo();
        }
    }
    private void OnLongPunch(InputAction.CallbackContext obj)
    {
        if (!isDead && tieneLongPunch)
        {
            mustLongPunch = true;
            ReiniciarTiempoInactivo();
        }
    }
    private void OnCrouchDash(InputAction.CallbackContext obj)
    {
        if (!isDead && puedeUsarDash)
        {
            StartCoroutine(RecargarDash());
            mustDash = true;
            ReiniciarTiempoInactivo();
        }
    }

    private void OnDownAttack(InputAction.CallbackContext obj)
    {
        if (!isDead && tieneDownAttack)
        {
            mustDownAttack = true;
            ReiniciarTiempoInactivo();
        }
    }


    IEnumerator RecargarDash()
    {
        puedeUsarDash = false;
        yield return new WaitForSeconds(delayDash);
        puedeUsarDash = true;
    }

    private void UpdateRawMove() {

        /*if (!isDead)
        {
            Vector2 rawMove = Vector2.zero;
            if (Keyboard.current.aKey.isPressed)
            {
                rawMove += Vector2.left;

            }
            else if (Keyboard.current.dKey.isPressed)
            {
                rawMove += Vector2.right;
            }

            //movementController.SetDesiredMove(rawMove);
            desiredMove = rawMove;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                mustJump = true;
                ReiniciarTiempoInactivo();
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                mustPunch = true;
                ReiniciarTiempoInactivo();
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                mustLongPunch = true;
                ReiniciarTiempoInactivo();
            }
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                mustDash = true;
                ReiniciarTiempoInactivo();
            }

        }*/



    }

    private void ReiniciarTiempoInactivo()
    {
        tiempoInactivo = 0.0f;

    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Enemy") && !isInmune && !isDead)
        {
            if (elOtro.GetComponent<EnemyGeneral>() != null)
            {
                RecibirDaño(elOtro.GetComponent<EnemyGeneral>().damageToDeal, elOtro.transform.position);
            }
            else
            {
                RecibirDaño(elOtro.GetComponentInParent<EnemyGeneral>().damageToDeal, elOtro.transform.position);
            }
        }
        else if(elOtro.gameObject.CompareTag("Pinchos") && !isDead)
        {
            //Si se tocan los pinchos, se morirá directamente
       
            if (estaAracandoHaciaAbajo /*&& collider.bounds.min.y >= elOtro.bounds.max.y*/) {
                ImpulsarArribaPorRebote();
            }
            else
            {
                RecibirDaño(elOtro.GetComponent<TrampaBase>().damageToDeal, elOtro.transform.position);
            }
            GestorSonido.Instance.EjecutarSonido(sonidoPinchos);

        }
        else if (elOtro.gameObject.CompareTag("Objeto") && !isDead)
        {
            //Si el objeto es una moneda
            if(elOtro.gameObject.GetComponent<ObjetoBase>() != null && elOtro.gameObject.GetComponent<ObjetoBase>().tipoObjeto == ObjetoBase.enumTipoObjeto.Coin)
            {
                GestorSonido.Instance.EjecutarSonido(sonidoMoneda);
                GestorPlayer.Instance.ObtenerMoneda();
            }
            else if (elOtro.gameObject.GetComponent<ObjetoBase>() != null && elOtro.gameObject.GetComponent<ObjetoBase>().tipoObjeto == ObjetoBase.enumTipoObjeto.Vida)
            {
                vidaActual += elOtro.gameObject.GetComponent<ObjetoBase>().cantidadEfecto;
                if (vidaActual >= vidaMaxima)
                {
                    vidaActual = vidaMaxima;
                }
                playerCuraVida?.Invoke(vidaActual);
                GestorSonido.Instance.EjecutarSonido(sonidoVida);

            }
            else if (elOtro.gameObject.GetComponent<ObjetoBase>() != null && elOtro.gameObject.GetComponent<ObjetoBase>().tipoObjeto == ObjetoBase.enumTipoObjeto.LongPunch)
            {
                GestorPlayer.Instance.ObtenerLongPunch();
                tieneLongPunch = true;
                GestorSonido.Instance.EjecutarSonido(sonidoPowerUp);
            }
            else if (elOtro.gameObject.GetComponent<ObjetoBase>() != null && elOtro.gameObject.GetComponent<ObjetoBase>().tipoObjeto == ObjetoBase.enumTipoObjeto.DoubleJump)
            {
                GestorPlayer.Instance.ObtenerDobleSalto();
                tieneDobleSalto = true;
                GestorSonido.Instance.EjecutarSonido(sonidoPowerUp);
            }
            else if (elOtro.gameObject.GetComponent<ObjetoBase>() != null && elOtro.gameObject.GetComponent<ObjetoBase>().tipoObjeto == ObjetoBase.enumTipoObjeto.DownAttack)
            {
                GestorPlayer.Instance.ObtenerDownAttack();
                tieneDownAttack = true;
                GestorSonido.Instance.EjecutarSonido(sonidoPowerUp);
            }
            else if (elOtro.gameObject.GetComponent<ObjetoBase>() != null && elOtro.gameObject.GetComponent<ObjetoBase>().tipoObjeto == ObjetoBase.enumTipoObjeto.Intento)
            {
                GestorPlayer.Instance.ObtenerIntento();
                GestorSonido.Instance.EjecutarSonido(sonidoPowerUp);
            }
            else if (elOtro.gameObject.GetComponent<ObjetoBase>() != null && elOtro.gameObject.GetComponent<ObjetoBase>().tipoObjeto == ObjetoBase.enumTipoObjeto.Combo)
            {
                GestorPlayer.Instance.ObtenerCombo();
                tienePowerUpCombo = true;
                GestorSonido.Instance.EjecutarSonido(sonidoPowerUp);
            }

        }
        else if (elOtro.gameObject.CompareTag("Sierra") && !isDead && !isInmune)
        {
            //Si se tocan los pinchos, se morirá directamente
            RecibirDaño(elOtro.GetComponent<TrampaBase>().damageToDeal, elOtro.transform.position);

        }
        else if (elOtro.gameObject.CompareTag("Meta") && !isDead)
        {
            //Al coger el bocadillo, se supera el nivel
            ImpedirMovimientos();
            rb.linearVelocity = new Vector2(0.0f, 0.0f);
            animator.SetTrigger("Victoria");
            animator.SetBool("EstaGanando", true);

            PasarDeNivel();
        }
    }


    private void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Pinchos") && !isDead)
        {
            //Si se tocan los pinchos, se morirá directamente

            if (estaAracandoHaciaAbajo /*&& collider.bounds.min.y >= elOtro.bounds.max.y*/)
            {
                ImpulsarArribaPorRebote();
            }
            else
            {
                RecibirDaño(elOtro.collider.GetComponent<TrampaBase>().damageToDeal, elOtro.transform.position);
            }
            GestorSonido.Instance.EjecutarSonido(sonidoPinchos);

        }
    }

    public void JuegoCompletado()
    {
        //Al coger el bocadillo, se supera el nivel
        ImpedirMovimientos();
        rb.linearVelocity = new Vector2(0.0f, 0.0f);
        animator.SetTrigger("Victoria");
        animator.SetBool("EstaGanando", true);
    }



    private void PasarDeNivel()
    {
        GestorPlayer.Instance.PasarDeNivel();
    }

    private void OnTriggerStay2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Enemy") && !isInmune && !isDead)
        {
            if (elOtro.GetComponent<EnemyGeneral>() != null)
            {
                RecibirDaño(elOtro.GetComponent<EnemyGeneral>().damageToDeal, elOtro.transform.position);
            }
            else
            {
                RecibirDaño(elOtro.GetComponentInParent<EnemyGeneral>().damageToDeal, elOtro.transform.position);
            }
                
        }
    }

    public void RecibirDaño(float dañoAAplicar, Vector2 posElOtro)
    {
        lanzadoEstaCayendo = false; //Se puede volver a caer
        vidaActual -= dañoAAplicar;
        GestorSonido.Instance.EjecutarSonido(sonidoHit);
        if (vidaActual <= 0.0f)
        {
            vidaActual = 0.0f;
            Morir();

        }

        if (posElOtro.x < transform.position.x && !isDead) //Si se te golpea desde la izquierda, se impulsa a la derecha
        {
            if (!isDead)
            {
                rb.linearVelocity = new Vector2(knocBackForceX, knocBackForceY);
            }

            //rb.AddForce(new Vector2(knocBackForceX, knocBackForceY), ForceMode2D.Force);  //Al golpearte, irás hacia atrás
        }
        else  //Si se te golpea desde la derecha, se impulsa a la izquierda
        {
            if (!isDead)
            {
                rb.linearVelocity = new Vector2(-knocBackForceX, knocBackForceY);
            }

            //rb.AddForce(new Vector2(-knocBackForceX, knocBackForceY), ForceMode2D.Force);  
        }

        StartCoroutine(ReestablecerInmunidad());
        if (!isDead)
        {
            StartCoroutine(ParpadeoInmunidad());
        }

        //VibrarCamara();
        playerGolpeado?.Invoke(vidaActual);   //Se suscribe el me´todo de vibración cámara a este

    }





    public void Quemar(float dañoAAplicar)
    {
        if (!isInmune && !isDead)
        {
            vidaActual -= dañoAAplicar;
            GestorSonido.Instance.EjecutarSonido(sonidoHit);
            if (vidaActual <= 0.0f)
            {
                vidaActual = 0.0f;
                Morir();
            }
            rb.linearVelocityX = 0.0f;  //Se quita el movimineto al recibir el golpe de quemadura
            StartCoroutine(ReestablecerInmunidad());
            if (!isDead)
            {
                StartCoroutine(ParpadeoInmunidad());
            }
            playerGolpeado?.Invoke(vidaActual);
        }

    }

    //Al golpearte el enemigo, se le da un timepo de inmunidad
    IEnumerator ReestablecerInmunidad()
    {
        isDamaged = true;
        isInmune = true;
         if(vidaActual <= 0.0f)
         {
             animator.SetTrigger("Morir");
         }
         else
         {
             animator.SetTrigger("IsHit");
         }
        lanzadoEstaCayendo = false;     //Se recibe el golpe y se pasa a aniamción de Hit, pero se permite que tras ella, se pueda volver a caer
        gameObject.layer = LayerMask.NameToLayer("PlayerInmune");   //Para evitar todo tipo de contacto mientras se es inmune, se pasa al player a otra layer de colisión que no tiene contacto con la layer enemigo
        yield return new WaitForSeconds(tiempoHit);
        isDamaged = false;
        yield return new WaitForSeconds(tiempoInmunidad - tiempoHit);
        isInmune = false;
        //Si no se ha muerto, se vuelve a la layer de player normal, donde si colisiona con enemigos
        if (!isDead)
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }

    }

    IEnumerator ParpadeoInmunidad()
    {
        while (isInmune)
        {
            spriteRenderer.material = blink.blink;  //Se accede al script que tiene el material de parpadeo
            yield return new WaitForSeconds(tiempoInmunidad / 6);
            spriteRenderer.material = blink.original;
            yield return new WaitForSeconds(tiempoInmunidad / 6);
        }
    }

    public void RellenarParametrosIniciales(bool hasLongPunch, bool hasDoubleJump, bool hasDownAttack, float vidaMaxima, bool tieneCombo)
    {
        tieneLongPunch = hasLongPunch;
        tieneDobleSalto = hasDoubleJump;
        tieneDownAttack = hasDownAttack;
        vidaActual = vidaMaxima;
        tienePowerUpCombo = tieneCombo;
    }

    public void Reaparecer()
    {
        Permitirmovimientos();
        isDead = false;
        estaAtacando = false;
        estaEnDash = false;
        haSaltado = false;
        spriteRenderer.enabled = true;
        if (tieneDobleSalto)
        {
            puedeDobleSalto = true;
        }
        gameObject.layer = LayerMask.NameToLayer("Player");
        vidaActual = GestorPlayer.Instance.vidaMaxima;
        playerCuraVida?.Invoke(vidaActual);
        animator.SetTrigger("Reaparecer");
    }


}
