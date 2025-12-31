using UnityEngine;
using UnityEngine.InputSystem;

public class GuardianMeta : MonoBehaviour
{

    [SerializeField] GameObject pedirBotonAPulsar;
    [SerializeField] InputActionReference leer;
    [SerializeField] LayerMask personaje;
    [SerializeField] PuertaMeta puertaMeta; //Puera a abrir si se cumplen los requisitos

    [Header("Sonidos")]
    [SerializeField] AudioClip sonidoDialogo;

    private bool pedirBotonMostrandose;
    private bool mostrandoDialogo = false;
    private bool haAbiertoPuerta = false;


    public Textos textosPaso;
    public Textos textosNoPaso;
    private void OnEnable()
    {
        leer.action.Enable();
        leer.action.started += OnLeer;
    }

    private void OnDisable()
    {
        leer.action.started -= OnLeer;
    }

    private void OnLeer(InputAction.CallbackContext obj)
    {
        GestorCanvasPausa gestorPausa = FindFirstObjectByType<GestorCanvasPausa>();

        if (pedirBotonMostrandose && !mostrandoDialogo && gestorPausa != null && gestorPausa.estaEnPausa == false)
        {
            if (GestorPlayer.Instance.JugadorTieneTodasMonedas())
            {
                Object.FindFirstObjectByType<ControlDialogos>().ActivarCartel(textosPaso);
            }
            else
            {
                Object.FindFirstObjectByType<ControlDialogos>().ActivarCartel(textosNoPaso);
            }
            mostrandoDialogo = true;
            GestorSonido.Instance.EjecutarSonido(sonidoDialogo);
            GestorPlayer.Instance.ImpedirMovimiento();
            GestorPlayer.Instance.ImpedirPausa();
            Object.FindFirstObjectByType<ControlDialogos>().cerrarCuadro += CerrarDialogo;

        }
        else if (mostrandoDialogo == true)    //Si se pulsa enter mientras se está en el diálogo, se pasa al siguiente texto
        {
            Object.FindFirstObjectByType<ControlDialogos>().SiguienteFrase();
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void CerrarDialogo()
    {
        Object.FindFirstObjectByType<ControlDialogos>().cerrarCuadro -= CerrarDialogo;
        GestorPlayer.Instance.PermitirMovimiento();
        GestorPlayer.Instance.PermitirPausa();
        mostrandoDialogo = false;
        if (puertaMeta != null && GestorPlayer.Instance.JugadorTieneTodasMonedas() && !haAbiertoPuerta)
        {
            haAbiertoPuerta = true;
            puertaMeta.AbrirPuerta();
        }



    }


    // Update is called once per frame
    void Update()
    {
        pedirBotonMostrandose = Physics2D.OverlapCircle(transform.position, 1f, personaje);
        if (pedirBotonMostrandose && !mostrandoDialogo)
        {
            pedirBotonAPulsar.gameObject.SetActive(true);
        }
        else
        {
            pedirBotonAPulsar.gameObject.SetActive(false);
        }

        /*dialogoMostrandose = Physics2D.OverlapCircle(transform.position, 1f, personaje);
        if (dialogoMostrandose && mostrandoDialogo)
        {
            estaEnRangoParaLeer = true;
        }
        else
        {
            if (mostrandoDialogo)
            {
                estaEnRangoParaLeer = false;
                Object.FindFirstObjectByType<ControlDialogos>().CerrarCuadroDialogo();
                mostrandoDialogo = false;

            }

        }*/


    }
}
