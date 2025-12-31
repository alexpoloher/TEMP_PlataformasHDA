using System.Collections.Generic;
using UnityEngine;

public class PlataformaAscendente : MonoBehaviour
{

    [SerializeField] Transform posicionFinal;
    [SerializeField] private float speed;
    [SerializeField] private float speedDescenso;
    private Vector3 objetivoActual;
    protected Vector3 ubicacionEnStart;
    private bool estaActivada = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ubicacionEnStart = transform.position;
        objetivoActual = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if (objetivoActual != transform.position)
        {
            if (estaActivada)
            {
                transform.position = Vector2.MoveTowards(transform.position, objetivoActual, speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, objetivoActual, speedDescenso * Time.deltaTime);
            }


        }
    }

    private void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            estaActivada = true;
            elOtro.transform.SetParent(this.transform);
            objetivoActual = posicionFinal.position;
            animator.SetBool("EstaActivada", true);
        }
    }

    private void OnCollisionExit2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            estaActivada = false;
            elOtro.transform.SetParent(null);
            objetivoActual = ubicacionEnStart;
            animator.SetBool("EstaActivada", false);
        }

    }

}
