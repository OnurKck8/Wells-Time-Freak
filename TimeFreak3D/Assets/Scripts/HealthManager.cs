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
        public float boxBulletCount = 10;
        public Image healthFill;
        public Image[] finihsPanels;
        [SerializeField] float fadeDuration;
        public TextMeshProUGUI ammoText, reloadText;
        public GameObject portalagitText,enteraBas,portalaGit,startPortal,gearYeri;
        public float currentHealth;
        public float currentAmmo;
        public float reloadBullet;
        public bool isGameOver = false;
        public PlayerMovement playerMovement;
        protected int ItemIndex;
        public int grenadeCount;
        public int gearCount;

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
                SoundManager.Instance.SoundPlay(6);
                grenadeCount++;
            }

            if (other.CompareTag("Gear"))
            {
                ItemIndex = 0;
                GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[8], transform.position, Quaternion.identity);
                Destroy(newParticle, 0.40f);
                SoundManager.Instance.SoundPlay(11);
                other.gameObject.GetComponent<BoxCollider>().enabled = false;
                gearCount++;
            }

            if (other.CompareTag("StartPortal"))
            {
                enteraBas.SetActive(false);
                isGameOver = true;
                StartCoroutine(WaitMayhem());
                
            }


        }

        public void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("PutTheGear"))
            {
                enteraBas.SetActive(true);
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PutTheGear"))
            {
                enteraBas.SetActive(false);
            }
        }

        IEnumerator WaitMayhem()
        {
            yield return new WaitForSeconds(0.75f);
            GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[10], transform.position, Quaternion.identity);
            Destroy(newParticle, 2f);
            yield return new WaitForSeconds(1f);
            SoundManager.Instance.SoundPlay(13);
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < finihsPanels.Length; i++)
            {
                finihsPanels[i].DOFade(1f, fadeDuration).From(0f);
            }
        }

    }

    

}
