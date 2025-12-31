using System.Collections;
using UnityEngine;

public class LadrilloRompible : MonoBehaviour
{
    [Header("Parametros")]
    [SerializeField] private Rigidbody2D[] partesLadrillo;
    [SerializeField] private GameObject objetoPartesLadrillo;
    [SerializeField] float fuerzaImpulso;
    [SerializeField] float tiempoParaRomperse = 2.0f;

    [Header("Sonidos")]
    [SerializeField] protected AudioClip sonidoRomperse;

    private bool destruido = false;         //Está destruido cuando las piezas han salido disparadas
    private float numPiezasEnPosicion = 0.0f;
    private float numPiezasRotas = 4.0f;
    private bool estaRompiendose = false;   //Cuando se pisa, empieza a romperse

    private Collider2D colliderLadrillo;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        colliderLadrillo = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player") && !estaRompiendose)
        {
            estaRompiendose = true;
            StartCoroutine(EsperarRomperLadrillo());
        }
    }



    IEnumerator EsperarRomperLadrillo()
    {
        yield return new WaitForSeconds(tiempoParaRomperse);
        RomperLadrillo();
    }

    private void RomperLadrillo()
    {
        destruido = true;
        GestorSonido.Instance.EjecutarSonido(sonidoRomperse);
        objetoPartesLadrillo.SetActive(true);
        numPiezasEnPosicion = 0;
        foreach (Rigidbody2D parte in partesLadrillo)
        {
            //parte.transform.SetParent(null);
            parte.gameObject.SetActive(true);

            Vector2 direccionLanzamiento = parte.transform.position - transform.position;   //Dirección a la que saldrña disparada la caja. Restando la pos de la parte con la cjaa, se tiene dónde está situada
            float fuerzaFinal = Random.Range(fuerzaImpulso - 1f, fuerzaImpulso);
            parte.AddForce(direccionLanzamiento * fuerzaFinal, ForceMode2D.Impulse);

        }
        colliderLadrillo.enabled = false;
        spriteRenderer.enabled = false;
    }


    public void SumarPiezaColocada()
    {
        if (destruido)
        {
            numPiezasEnPosicion++;
            if (numPiezasEnPosicion == numPiezasRotas)
            {
                ReconstruirLadrillo();
            }
        }


    }

    private void ReconstruirLadrillo()
    {
        destruido = false;
        estaRompiendose = false;
        colliderLadrillo.enabled = true;
        spriteRenderer.enabled = true;
        objetoPartesLadrillo.SetActive(false);
        foreach (Rigidbody2D parte in partesLadrillo)
        {
            parte.gameObject.SetActive(false);
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
