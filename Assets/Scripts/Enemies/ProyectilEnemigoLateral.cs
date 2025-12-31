using UnityEngine;

public class ProyectilEnemigoLateral : MonoBehaviour
{
    [SerializeField] float speed = 2.0f;
    [SerializeField] float tiempoParaDesaparecer = 3.0f;
    private float tiempoAcumulado = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        tiempoAcumulado += Time.deltaTime;
        if (tiempoAcumulado >= tiempoParaDesaparecer) { 
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") || elOtro.gameObject.CompareTag("Entorno"))
        {
            Destroy(gameObject);
        }
    }

}
