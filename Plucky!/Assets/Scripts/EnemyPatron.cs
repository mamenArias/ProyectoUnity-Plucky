using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatron : MonoBehaviour
{

    public float speed = 1f;
    public float minX;
    public float maxX;
    public float waitingTime = 2f;

    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        UpdateTarget();
        StartCoroutine("PatrolToTarget");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateTarget()
    {
        //Si es la primera vez, crea el target en la izquierda
        if (target == null)
        {
            target = new GameObject("Target");
            target.transform.position = new Vector2(minX, transform.position.y);
            transform.localScale = new Vector3(1, 1, 1);
            return; //esto se pone para que no ejecute el resto porque ya se ha creado
        }

        //Si está en la izquierda, cambia el target a la derecha
        if (target.transform.position.x == minX)
        {
            target.transform.position = new Vector2(maxX, transform.position.y);
            transform.localScale = new Vector3(-1,1,1);
        }

        //Si está en la derecha, cambia el target a la izquierda
        if (target.transform.position.x == maxX)
        {
            target.transform.position = new Vector2(minX, transform.position.y);
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private IEnumerator PatrolToTarget()
    {
        //Corrutina para mover al enemigo:
        while (Vector2.Distance(transform.position, target.transform.position) > 0.05f)
        {
            Vector2 direction = target.transform.position - transform.position; //la dirección se saca restando la posición del target - la nuestra
            float xDirection = direction.x;

            transform.Translate(direction.normalized * speed * Time.deltaTime);

            /*Le estamos diciendo que se ejecuta sólo cuando estamos lejos, pero que una vez que lleguemos al target, vuelva a
             llamar a la función, pero se salta el while (que es falso) y sigue leyendo el código restante*/
            yield return null;
        }

        //Hemos llegado al target y vamos a ver nuestra posición con respecto a él
        transform.position = new Vector2(target.transform.position.x, transform.position.y);

        //Vamos a esperar un momento. Va a volver a llamar al método pero después de los 3 segundos de espera.
        yield return new WaitForSeconds(waitingTime);

        UpdateTarget();
        StartCoroutine("PatrolToTarget");
    }
}
