using UnityEngine;

public class PuertaMeta : MonoBehaviour
{

    Animator animator;
    [Header("Sonidos")]
    [SerializeField] AudioClip sonidoPuerta;

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
        
    }

    public void AbrirPuerta()
    {
        GestorSonido.Instance.EjecutarSonido(sonidoPuerta);
        animator.SetTrigger("Abrir");
    }

    public void CerrarPuerta()
    {
        animator.SetTrigger("Cerrar");
    }

}
