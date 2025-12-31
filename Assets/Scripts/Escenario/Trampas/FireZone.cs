using Unity.VisualScripting;
using UnityEngine;

public class FireZone : TrampaBase
{

    [SerializeField] float tiempoEntreGolpes = 1.0f;
    private float tiempoAcumulado = 0.0f;
    private bool jugadorEnZona = false;
    private PlayerController jugador;

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            jugadorEnZona = true;
            jugador = elOtro.gameObject.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.Quemar(damageToDeal);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            tiempoAcumulado = 0.0f;
            jugadorEnZona = false;
            jugador = null;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (jugadorEnZona && jugador != null) { 
            tiempoAcumulado += Time.deltaTime;

            if (tiempoAcumulado >= tiempoEntreGolpes)
            {
                tiempoAcumulado = 0.0f;
                jugador.Quemar(damageToDeal);
            }

        }
    }
}
