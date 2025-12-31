using System.Collections;
using UnityEngine;

public class PiezasLadrilloRompible : MonoBehaviour
{


    private Vector3 posInicial;
    private Quaternion rotacionInicial;
    [SerializeField] float speed = 5.0f;
    [SerializeField] float tiempoParaVolverAPosInicial = 5.0f;
    private Rigidbody2D rb;
    private bool seEstaRecolocando = false;

    private void OnEnable()
    {
        StartCoroutine(EsperarRecolocacion());

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        posInicial = transform.position;
        rotacionInicial = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (seEstaRecolocando)
        {
            MoverPieza();
        }

    }

    private void MoverPieza() {
        //Se mueve hasta que llegue a su pos inicial
        if (transform.position != posInicial)
        {
            transform.position = Vector2.MoveTowards(transform.position, posInicial, speed * Time.deltaTime);
        }
        else
        {
            //Al llegar, se avisa que ya está
            transform.GetComponentInParent<LadrilloRompible>().SumarPiezaColocada();
            seEstaRecolocando = false;
        }


    }

    IEnumerator EsperarRecolocacion()
    {
        seEstaRecolocando = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        yield return new WaitForSeconds(tiempoParaVolverAPosInicial);
        rb.bodyType = RigidbodyType2D.Kinematic;
        seEstaRecolocando = true;
    }
}
