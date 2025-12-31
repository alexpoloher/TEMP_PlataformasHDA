using UnityEngine;

public class CheckPointScript : MonoBehaviour
{
    private bool utilizado = false;
    private Animator animator;
    [Header("Sonidos")]
    [SerializeField] AudioClip sonidoActivar;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") && !utilizado){
            utilizado = true;
            animator.SetTrigger("Activar");
            GestorSonido.Instance.EjecutarSonido(sonidoActivar);
            GestorPlayer.Instance.GuardarCheckPoint();
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
