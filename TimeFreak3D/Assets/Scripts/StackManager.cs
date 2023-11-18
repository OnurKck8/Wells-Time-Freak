using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace MarwanZaky
{
    public class StackManager : HealthManager
    {
        public Transform[] ItemHolderTransform;
        public int numOfItemsHolding;
        public List<GameObject> enemy;
        public GameObject[] portal;
        
        void Update()
        {

            if (reloadBullet <= 0)
            {
                reloadBullet = 0;
            }

            if (isGameOver == false)
            {
                if (grenadeCount == 0 && ItemHolderTransform[1].childCount != 0)
                {
                    Destroy(ItemHolderTransform[1].GetChild(0).gameObject);
                    ItemHolderTransform[1].transform.parent = gameObject.transform.GetChild(0);
                }
                else
                    ItemHolderTransform[1].transform.parent = gameObject.transform.GetChild(0);
            }

            if (isGameOver == false)
            {
                if (currentAmmo == 0)
                    currentAmmo = 0;

                if (healthFill.fillAmount <= 0 || currentHealth <= 0)
                {
                    currentHealth = 0;
                    SoundManager.Instance.SoundPlay(9);
                    isGameOver = true;
                }

                ammoText.text = currentAmmo.ToString();
                reloadText.text = reloadBullet.ToString();
            }

            if (isGameOver)
            {
                Time.timeScale = 0.25f;  
            }

            if (isGameOver == false && gearCount >= 6)
            {
                //portal için yeri aç
                Debug.Log("Portal");
                portal[0].SetActive(true);
                portal[1].SetActive(true);
                portalagitText.SetActive(true);
            }
        }

      

        public void AddNewItem(Transform _itemToAdd)
        {
            _itemToAdd.DOJump(ItemHolderTransform[ItemIndex].position + new Vector3(0, 0.025f * numOfItemsHolding, 0), 1.5f, 1, 0.25f).OnComplete(
                () =>
                {
                    _itemToAdd.SetParent(ItemHolderTransform[ItemIndex], true);
                    _itemToAdd.localPosition = new Vector3(0, 2f * numOfItemsHolding, 0);
                    _itemToAdd.localRotation = Quaternion.identity;
                    numOfItemsHolding++;

                }
            );

        }
        public void AddNewItemGrenade(Transform _itemToAdd)
        {
            _itemToAdd.DOJump(ItemHolderTransform[ItemIndex].position + new Vector3(0, 0.025f, 0), 1.5f, 1, 0.25f).OnComplete(
                () =>
                {
                    _itemToAdd.SetParent(ItemHolderTransform[ItemIndex], true);
                    _itemToAdd.localPosition = new Vector3(0, 0, 0);
                    _itemToAdd.localRotation = Quaternion.identity;
                }
            );

        }

    }

}
