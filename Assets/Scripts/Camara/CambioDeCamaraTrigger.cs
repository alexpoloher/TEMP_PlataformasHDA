using Unity.Cinemachine;
using UnityEngine;

public class CambioDeCamaraTrigger : MonoBehaviour
{

    [SerializeField] CinemachineCamera camaraAPoner;
    [SerializeField] CinemachineCamera camaraAQuitar;


    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player"))
        {
            camaraAQuitar.Priority = 0;
            camaraAPoner.Priority = 10;
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
