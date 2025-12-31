using UnityEngine;

public class BossDisparo : MonoBehaviour
{

    [SerializeField] float speed = 3.0f;
    [SerializeField] float tiempoParaDesaparecer = 100.0f;
    private float tiempoAcumulado = 0.0f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        tiempoAcumulado += Time.deltaTime;
        if (tiempoAcumulado >= tiempoParaDesaparecer)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") || elOtro.gameObject.CompareTag("Entorno"))
        {
            animator.SetTrigger("Golpeo");
        }
    }

    public void EliminarProyectil()
    {
        Destroy(gameObject);
    }
}
