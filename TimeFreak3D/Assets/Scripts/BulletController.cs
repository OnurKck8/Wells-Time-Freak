using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] float speed = 30f;


    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

}
