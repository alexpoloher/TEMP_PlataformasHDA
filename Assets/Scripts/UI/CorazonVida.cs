using UnityEngine;
using UnityEngine.UI;

public class CorazonVida : MonoBehaviour
{
    [SerializeField] Animator animator;
    public bool activo = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void AddCorazon()
    {
        animator.SetTrigger("Cura");
        activo = true;
    }

    public void QuitarCorazon()
    {

        animator.SetTrigger("Hit");
        activo = false;
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
