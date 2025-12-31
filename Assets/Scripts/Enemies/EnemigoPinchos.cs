using UnityEngine;

public class EnemigoPinchos : MonoBehaviour
{
    [SerializeField] float speedBajada;
    [SerializeField] float speedSubida;
    private Vector3 posInicial;
    [SerializeField] Transform transformFinal;
    private Vector3 posFinal;
    Animator animator;
    private bool estaBajando = false;
    private bool estaSubiendo = false;

    [Header("Sonidos")]
    [SerializeField] protected AudioClip sonidoDeteccion;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posInicial = transform.position;
        posFinal = transformFinal.position;
        estaBajando = false;
        estaSubiendo = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (estaBajando)
        {
            transform.position = Vector2.MoveTowards(transform.position, posFinal, speedBajada * Time.deltaTime);
            if (transform.position == posFinal)
            {
                estaBajando = false;
                estaSubiendo = true;
                animator.SetBool("EstaSubiendo", true);
            }
              


        }
        else if(estaSubiendo)
        {
            transform.position = Vector2.MoveTowards(transform.position, posInicial, speedSubida * Time.deltaTime);
            if (transform.position == posInicial)
            {
                estaBajando = false;
                estaSubiendo = false;
                animator.SetBool("EstaSubiendo", false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        //Solo le afecta si no está ni subiendo ni bajando
        if (elOtro.gameObject.CompareTag("Player") && !estaBajando && !estaSubiendo) {
            animator.SetTrigger("EmpezarABajar");
            GestorSonido.Instance.EjecutarSonido(sonidoDeteccion);
            estaBajando = true;
            estaSubiendo = false;
        }
    }



}
