using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


 public class CartelInfo : MonoBehaviour
 {
    [SerializeField] GameObject pedirBotonAPulsar;
    [SerializeField] GameObject mostrarInformacion;
    [SerializeField] InputActionReference leer;

    [SerializeField] LayerMask personaje;
    private bool pedirBotonMostrandose;
    private bool mostrarInformacionMostrandose;
    private bool estaEnRangoParaLeer = false;
    private bool mostrandoInfo = false;

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
        pedirBotonAPulsar.gameObject.SetActive(false);
        mostrarInformacion.gameObject.SetActive(false);
        mostrandoInfo = false;
    }

    // Update is called once per frame
    void Update()
    {
        pedirBotonMostrandose = Physics2D.OverlapCircle(transform.position, 1f, personaje);
        if (pedirBotonMostrandose && !mostrandoInfo) {
            pedirBotonAPulsar.gameObject.SetActive(true);
        }
        else
        {
            pedirBotonAPulsar.gameObject.SetActive(false);
        }

        mostrarInformacionMostrandose = Physics2D.OverlapCircle(transform.position, 1f, personaje);
        if (mostrarInformacionMostrandose)
        {
            estaEnRangoParaLeer = true;
        }
        else
        {
            estaEnRangoParaLeer = false;
            mostrandoInfo = false;
            mostrarInformacion.gameObject.SetActive(false);
        }
    }

    private void OnLeer(InputAction.CallbackContext obj)
    {
        if (estaEnRangoParaLeer)
        {
            mostrarInformacion.gameObject.SetActive(true);
            pedirBotonAPulsar.gameObject.SetActive(false);
            mostrandoInfo = true;
        }

    }

}

