using UnityEngine;

public class MoverFondoScript : MonoBehaviour
{
    [SerializeField] Transform transformJugador;
    [SerializeField] float velocidadOffset = 0.3f;
    private float ultimaPosX;
    private Material material;
    Vector2 offsetMaterial;


    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transformJugador = GameObject.FindGameObjectWithTag("Player").transform;
        ultimaPosX = transformJugador.position.x;
        offsetMaterial = material.mainTextureOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = transformJugador.position.x - ultimaPosX;
        offsetMaterial += new Vector2(deltaX * velocidadOffset, 0);
        material.mainTextureOffset = offsetMaterial;
        //transform.position += Vector3.right * deltaX * velocidadOffset;
        ultimaPosX = transformJugador.position.x;
    }
}
