using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
namespace MarwanZaky
{
    public class HealthManager : MonoBehaviour
    {
        float maxHealth = 1f;
        int boxBulletCount = 10;
        public Image healthFill;
        public TextMeshProUGUI ammoText, reloadText;
        public float currentHealth;
        public int currentAmmo;
        public int reloadBullet;
        public bool isGameOver = false;
        public PlayerMovement playerMovement;
        protected int ItemIndex;
        public int grenadeCount;
        void Start()
        {
            ItemIndex = 0;
            currentHealth = maxHealth;
            grenadeCount = 0;
            currentAmmo = boxBulletCount;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                currentHealth -= 0.2f;
                healthFill.DOFillAmount(currentHealth, 0.15f);

                GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[2], transform.position, Quaternion.identity);
                Destroy(newParticle, 0.40f);

                SoundManager.Instance.SoundPlay(8);
                if (other.gameObject.name == "Magic(Clone)")
                {
                    Destroy(other.gameObject);
                    newParticle = Instantiate(ParticleManager.Instance.bulletParticle[4], transform.position, Quaternion.identity);
                    Destroy(newParticle, 0.40f);
                    other.GetComponent<Rigidbody>().AddForce(Vector3.back*20f);
                }
                   
            }

            if (other.CompareTag("HealthBox"))
            {
                if (currentHealth < 1)
                {
                    Destroy(other.gameObject, 0.45f);
                    currentHealth = maxHealth;
                    healthFill.DOFillAmount(currentHealth, 0.15f);
                    ItemIndex = 0;
                    SoundManager.Instance.SoundPlay(6);
                    GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[5], transform.position, Quaternion.Euler(-90,0,0));
                    newParticle.transform.parent=gameObject.transform;
                    Destroy(newParticle, 0.40f);
                }
            }

            if (other.CompareTag("ReloadBox"))
            {
                Destroy(other.gameObject, 0.45f);
                reloadBullet += boxBulletCount;
                ItemIndex = 0;
                SoundManager.Instance.SoundPlay(6);
                GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[6], transform.position, Quaternion.identity);
                Destroy(newParticle, 0.40f);
            }

            if (other.CompareTag("Granade"))
            {
                ItemIndex = 1;
                GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[7], transform.position, Quaternion.identity);
                Destroy(newParticle, 0.40f);
                grenadeCount++;
            }

        }

    }

}
