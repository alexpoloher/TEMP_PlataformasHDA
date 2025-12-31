using UnityEngine;

public class DetenerJugadorTrigger : MonoBehaviour
{

    private bool yaActivado = false;


    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") && !yaActivado)
        {
            yaActivado = true;
            GestorPlayer.Instance.ImpedirMovimiento();
        }
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
