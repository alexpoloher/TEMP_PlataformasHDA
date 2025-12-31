using System;
using UnityEngine;

public class JarronRompible : MonoBehaviour
{
    [SerializeField] private float golpesResisteJarron = 1.0f;
    [SerializeField] GameObject objetoInterior;
    [SerializeField] Transform posicionSpawnItem;

    [Header("Sonidos")]
    [SerializeField] AudioClip sonidoRoto;

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
        
    }

    public void JarronGolpeado()
    {
        golpesResisteJarron -= 1;

        if (golpesResisteJarron <= 0)
        {
            RomperJarron();
        }


    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Weapon"))
        {
            JarronGolpeado();
        }
    }

    private void RomperJarron()
    {
        GestorSonido.Instance.EjecutarSonido(sonidoRoto);
        animator.SetTrigger("Roto");
    }

    public void GenerarObjetoInterior()
    {
        if(objetoInterior != null)
        {
            Instantiate(objetoInterior, posicionSpawnItem.position, Quaternion.identity);
        }

    }


}
