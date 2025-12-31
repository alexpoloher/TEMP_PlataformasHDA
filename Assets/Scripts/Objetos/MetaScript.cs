using UnityEngine;

public class MetaScript : MonoBehaviour
{

    [SerializeField] float amplitud = 0.1f;
    [SerializeField] float speed = 2f;
    private Transform transformInicial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transformInicial = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float movimientoEnY = Mathf.Sin(Time.time * speed) * amplitud;
        transform.position = transformInicial.position + new Vector3(0.0f, movimientoEnY, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D elOtro)
    {
        if (elOtro.gameObject.CompareTag("Player")) {
            Destroy(gameObject);
        }
    }
}
