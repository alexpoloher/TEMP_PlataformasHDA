using UnityEngine;

public class ObjetoBase : MonoBehaviour
{

    public enum enumTipoObjeto
    {
        Coin,
        Vida,
        LongPunch,
        DoubleJump,
        DownAttack,
        Intento,
        Combo
    }

    [SerializeField] public enumTipoObjeto tipoObjeto;
    [SerializeField] public float cantidadEfecto = 0.0f;
    protected Collider2D colliderObjeto;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected bool recogido = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        colliderObjeto = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") && !recogido) {
            recogido = true;
            Destroy(gameObject);
        }  
    }


    public void ObtenerObjeto()
    {
        Destroy(gameObject);
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
