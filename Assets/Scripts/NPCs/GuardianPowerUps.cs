using UnityEngine;
using UnityEngine.InputSystem;

public class GuardianPowerUps : MonoBehaviour
{

    [SerializeField] GameObject pedirBotonAPulsar;
    [SerializeField] InputActionReference leer;
    [SerializeField] LayerMask personaje;

    private bool pedirBotonMostrandose;
    private bool mostrandoDialogo = false;

    public Textos textos;

    private void OnEnable()
    {
        leer.action.Enable();
        leer.action.started += OnLeer;
    }

    private void OnDisable()
    {
        leer.action.started -= OnLeer;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnLeer(InputAction.CallbackContext obj)
    {

        GestorCanvasPausa gestorPausa = FindFirstObjectByType<GestorCanvasPausa>();

        if (pedirBotonMostrandose && !mostrandoDialogo && gestorPausa != null && gestorPausa.estaEnPausa == false)
        {
            Object.FindFirstObjectByType<ControlDialogos>().ActivarCartel(textos);
            mostrandoDialogo = true;
            GestorPlayer.Instance.ImpedirMovimiento();
            GestorPlayer.Instance.ImpedirPausa();
            Object.FindFirstObjectByType<ControlDialogos>().cerrarCuadro += CerrarDialogo;

        }
        else if (mostrandoDialogo == true)    //Si se pulsa enter mientras se está en el diálogo, se pasa al siguiente texto
        {
            Object.FindFirstObjectByType<ControlDialogos>().SiguienteFrase();
        }

    }

    private void CerrarDialogo()
    {
        Object.FindFirstObjectByType<ControlDialogos>().cerrarCuadro -= CerrarDialogo;
        GestorPlayer.Instance.PermitirMovimiento();
        GestorPlayer.Instance.PermitirPausa();
        mostrandoDialogo = false;
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
    }
}
