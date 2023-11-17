using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class HealthManager : MonoBehaviour
{
    float maxHealth = 1f;
    int boxBulletCount = 10;
    public Image healthFill;
    public TextMeshProUGUI ammoText,reloadText;
    public float currentHealth;
    public int currentAmmo;
    public int reloadBullet;
    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = boxBulletCount;
    }

    public void Update()
    {
        if (currentAmmo == 0)
            currentAmmo = 0;

        if (currentHealth == 0)
            currentHealth = 0;

        healthFill.DOFillAmount(currentHealth, 0.15f);

        ammoText.text = currentAmmo.ToString();
        reloadText.text = reloadBullet.ToString();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            currentHealth -= 0.2f;
           
            if(other.gameObject.name == "Magic(Clone)")
                Destroy(other.gameObject);
        }

        if (other.CompareTag("HealthBox"))
        {
            Destroy(other.gameObject);
            currentHealth = maxHealth;
        }
        else if (other.CompareTag("ReloadBox"))
        {
            Destroy(other.gameObject);
            reloadBullet += boxBulletCount;
        }

    }

}
