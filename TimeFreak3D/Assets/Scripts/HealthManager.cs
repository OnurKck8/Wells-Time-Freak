using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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

        ammoText.text = currentAmmo.ToString();
        reloadText.text = reloadBullet.ToString();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            currentHealth -= 0.2f;
            healthFill.fillAmount = currentHealth;
            if(other.gameObject.name== "Magic")
                Destroy(other.gameObject);
        }

        if (other.CompareTag("HealthBox"))
        {
            Destroy(other.gameObject);
            healthFill.fillAmount = maxHealth;
            currentHealth = maxHealth;
        }
        else if (other.CompareTag("ReloadBox"))
        {
            Destroy(other.gameObject);
            reloadBullet += boxBulletCount;
        }

    }

}
