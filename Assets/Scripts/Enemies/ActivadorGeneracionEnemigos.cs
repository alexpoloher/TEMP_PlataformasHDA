using Unity.VisualScripting;
using UnityEngine;

public class ActivadorGeneracionEnemigos : MonoBehaviour
{

    [SerializeField] bool esActivador = false;
    [SerializeField] GameObject enemyInstantiator;


    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player")) {

            if (esActivador && enemyInstantiator != null)
            {
                enemyInstantiator.SetActive(true);
            }
            else{
                if (enemyInstantiator != null) {
                    enemyInstantiator.SetActive(false);
                }
            }
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
