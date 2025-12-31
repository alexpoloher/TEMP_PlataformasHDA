using Unity.Cinemachine;
using UnityEngine;

public class CambioLimitesCamara : MonoBehaviour
{

    [SerializeField] PolygonCollider2D polygonCollider2D;
    [SerializeField] CinemachineConfiner2D confiner2D;


    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player")){ 
        confiner2D.BoundingShape2D = polygonCollider2D;
        confiner2D.InvalidateBoundingShapeCache();
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
