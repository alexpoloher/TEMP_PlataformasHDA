using System.Collections;
using UnityEngine;

public class PlataformaRompible : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Collider2D colliderPlataforma;

    [SerializeField] private float tiempoHastaRomperse;
    [SerializeField] private float tiempoRespawn;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        colliderPlataforma = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Romperse());
        }
    }

    IEnumerator Romperse()
    {
        animator.SetBool("Rompiendose", true);  //Animación de ir rompiendose

        //La plataforma va a vibrar levemente al tocarla
        float frecuencia = 50f;
        float amplitud = 0.035f;
        Vector3 posInicial = transform.position;
        float t = 0.0f;
        while(t < tiempoHastaRomperse)
        {
            t += Time.deltaTime;
            float sine = Mathf.Sin(t * frecuencia) * amplitud;
            transform.position = posInicial + Vector3.right * sine;
        }

        yield return new WaitForSeconds(tiempoHastaRomperse);
        animator.SetBool("Rompiendose", false); //Cunado pasa ese tiempo, se rompe
        spriteRenderer.enabled = false;
        colliderPlataforma.enabled = false;
        yield return new WaitForSeconds(tiempoRespawn); //Se espera para volver a aparecer
        spriteRenderer.enabled = true;
        colliderPlataforma.enabled = true;
        transform.position = posInicial;
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
