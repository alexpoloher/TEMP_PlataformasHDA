using UnityEngine;

public class PlataformaGiratoriaBase : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float rotationSpeed = 60.0f;
    private Vector3 dirreccionRotacion = new Vector3(0, 0, 1);
    private int numPlataformas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numPlataformas = transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(dirreccionRotacion * rotationSpeed * Time.deltaTime, Space.World);
        for(int i = 0; i < numPlataformas; i++)
        {
            transform.GetChild(i).transform.Rotate(dirreccionRotacion * rotationSpeed * -1 * Time.deltaTime, Space.World);
        }

    }
}
