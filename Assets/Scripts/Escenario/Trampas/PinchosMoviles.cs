using UnityEngine;

public class PinchosMoviles : TrampaBase
{

    [SerializeField] float tiempoParaActivar = 3.0f;
    [SerializeField] bool empezarActivada = false;

    Animator animator;
    Collider2D colliderPinchos;
    private float tiempoAcumulado = 0.0f;
    private bool estaActivo = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        colliderPinchos = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (empezarActivada)
        {
            tiempoAcumulado = tiempoParaActivar;
            estaActivo = true;
        }
        else
        {
            tiempoAcumulado = 0.0f;
            estaActivo = false;
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        tiempoAcumulado += Time.deltaTime;
        if(tiempoAcumulado >= tiempoParaActivar)
        {
            tiempoAcumulado = 0.0f;
            animator.SetTrigger("PinchosActivados");
            estaActivo = !estaActivo;
        }

    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        /*if (elOtro.gameObject.CompareTag("Weapon") && elOtro.bounds.min.y > colliderPinchos.bounds.max.y)
        {
            PlayerController playerController = elOtro.GetComponent<PlayerController>();
            if (playerController != null) {
                playerController.ImpulsarArribaPorRebote();
            }
        }*/
    }
}
