using System.Collections;
using UnityEngine;

public class EnemyInstantiator : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenEnemyInstantiations;
    private float posYInstanciar;
    [SerializeField] Transform posInstanciar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnEnable()
    {
        posYInstanciar = posInstanciar.position.y;
        StartCoroutine(InstantiateEnemies(timeBetweenEnemyInstantiations));
    }

    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator InstantiateEnemies(float timeBetweenInstantiations) {

        while (true) {

            Instantiate(enemyPrefab, new Vector2(transform.position.x, posYInstanciar), Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenInstantiations);
        }
    }

}
