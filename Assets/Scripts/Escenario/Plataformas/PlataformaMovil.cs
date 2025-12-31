using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [SerializeField] private List<Transform> posiciones;
    [SerializeField] private float speed;
    private Vector3 objetivoActual;
    private int indiceActual;
    protected Vector2 ubicacionEnStart;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ubicacionEnStart = transform.position;
        objetivoActual = posiciones[indiceActual].position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Desplazarse desde la posición en la que está la paltaforma, hasta el objetivo
        /*Vector2 nuevaPos = Vector2.MoveTowards(transform.position, objetivoActual, speed * Time.deltaTime);
        rb.MovePosition(nuevaPos);*/
        transform.position = Vector2.MoveTowards(transform.position, objetivoActual, speed * Time.deltaTime);

        //Al alcanzar el objetivo, se cambia de objetivo
        if (transform.position == objetivoActual) {
            indiceActual++;
            if (indiceActual >= posiciones.Count) { 
                indiceActual = 0;
            }
            objetivoActual = posiciones[indiceActual].position;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player")){
            elOtro.transform.SetParent(this.transform);
        }

    }

    protected virtual void OnCollisionExit2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            if(elOtro != null)
            {
                elOtro.transform.SetParent(null);
            }

        }

    }
}
