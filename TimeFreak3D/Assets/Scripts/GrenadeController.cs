using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeController : MonoBehaviour
{
    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.GetComponent<Collider>()!=null)
        {
            if(collision.gameObject.tag != "Player")
            {
                Destroy(gameObject);
                GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[3], transform.position, Quaternion.identity);
                Destroy(newParticle, 0.40f);
                SoundManager.Instance.SoundPlay(7);
            }
            
        }
    }
}
