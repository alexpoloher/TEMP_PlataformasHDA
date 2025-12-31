using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GestorScene : MonoBehaviour
{

    private Animator animator;
    [SerializeField] float tiempoEspera = 1.0f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PasarDeNivel() {
        StartCoroutine(EsperarCambioEscena());
    }

    IEnumerator EsperarCambioEscena()
    {
        animator.SetTrigger("Activar");
        yield return new WaitForSeconds(tiempoEspera);
        int siguienteScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(siguienteScene);
    }

    public void VolverAMenuPrincipal() {
        SceneManager.LoadScene("MenuPrincipal");
    }

}
