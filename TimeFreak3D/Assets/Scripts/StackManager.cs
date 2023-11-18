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
        void Update()
        {
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

                if (healthFill.fillAmount <= 0)
                {
                    currentHealth = 0;
                    isGameOver = true;
                }

                ammoText.text = currentAmmo.ToString();
                reloadText.text = reloadBullet.ToString();
            }
            if (isGameOver)
            {
                Time.timeScale = 0.25f;
                for(int i = 0; i < enemy.Count; i++)
                {
                    enemy[i].gameObject.GetComponent<FollowTarget>().moveSpeed = 0;
                    enemy[i].gameObject.GetComponent<Animator>().enabled = false;

                }

            }

        }

        public void AddNewItem(Transform _itemToAdd)
        {
            _itemToAdd.DOJump(ItemHolderTransform[ItemIndex].position + new Vector3(0, 0.025f * numOfItemsHolding, 0), 1.5f, 1, 0.25f).OnComplete(
                () =>
                {
                    _itemToAdd.SetParent(ItemHolderTransform[ItemIndex], true);
                    _itemToAdd.localPosition = new Vector3(0, 5f * numOfItemsHolding, 0);
                    _itemToAdd.localRotation = Quaternion.identity;
                    numOfItemsHolding++;

                }
            );

        }
    }

}
