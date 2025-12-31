using UnityEngine;

public class CoinScript : ObjetoBase
{


    protected override void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") && !recogido)
        {
            recogido = true;
            animator.SetTrigger("Obtenida");
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
