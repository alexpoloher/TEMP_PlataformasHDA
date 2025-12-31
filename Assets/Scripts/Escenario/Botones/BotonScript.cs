using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class BotonScript : MonoBehaviour
{

    private Animator animator;
    private Collider2D colliderBoton;
    private bool estaPulsado = false;

    [SerializeField] MonoBehaviour[] elementosQueActiva;
    [SerializeField] CinemachineCamera camaraPlayer;
    [SerializeField] CinemachineCamera camaraBoton;
    [SerializeField] float tiempoCambioCamara;

    [Header("Sonidos")]
    [SerializeField] AudioClip sonidoBoton;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        colliderBoton = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") && elOtro.collider.attachedRigidbody.linearVelocityY < 0)
        {
            animator.SetTrigger("Pulsado");
            if (!estaPulsado)
            {
                PulsarBoton();
            }
        }
    }

    private void PulsarBoton() {
        estaPulsado = true;
        GestorSonido.Instance.EjecutarSonido(sonidoBoton);
        if (camaraBoton != null && camaraPlayer != null)
        {
            camaraPlayer.Priority = 0;
            camaraBoton.Priority = 10;
            StartCoroutine(VolverACamaraJugador());
        }


        foreach (MonoBehaviour elemento in elementosQueActiva)
        {
            //Se comprueba que es un elemento de la intefaz activar, antes de ejecutar esa función
            if(elemento is IElementoActivable elementoActivable)
            {
                elementoActivable.Activar();
            }

        }
    }


    IEnumerator VolverACamaraJugador()
    {
        GestorPlayer.Instance.ImpedirMovimiento();
        yield return new WaitForSeconds(tiempoCambioCamara);
        camaraBoton.Priority = 0;
        camaraPlayer.Priority = 10;
        yield return new WaitForSeconds(1.5f);
        GestorPlayer.Instance.PermitirMovimiento();
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
