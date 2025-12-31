using UnityEngine;

public class ActivadorDeBoss : MonoBehaviour
{
    private bool yaActivado = false;
    [SerializeField] BossScript boss;
    [SerializeField] PuertaMeta puerta;

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") && !yaActivado) {
            yaActivado = true;
            boss.Activar();
            puerta.CerrarPuerta();
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
