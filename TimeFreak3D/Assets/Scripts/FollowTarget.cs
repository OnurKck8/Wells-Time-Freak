﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarwanZaky;
public class FollowTarget : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 5f;
    private Rigidbody rb;
    public Animator myAnim;

    public string characterName;
    public GameObject magic;
    float timer = 0;
    float distance;

    void Start(){
        rb = GetComponent<Rigidbody>();
        if (player == null)
        {
            player = GameObject.Find("Third Person Player");
        }
    }

    // Update is called once per frame
    
    private void FixedUpdate() {
        Vector3 pos = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(pos);
        transform.LookAt(player.transform);
    }

    public void Update()
    {
        if (player.GetComponent<HealthManager>().isGameOver == false)
        {
             distance = Vector3.Distance(gameObject.transform.position, player.transform.position);

            if (characterName == "Barbar")
            {
                if (distance <= 2f)
                {
                    myAnim.SetBool("Attack", true);
                    moveSpeed = 0f;
                }
                else
                {
                    myAnim.SetBool("Attack", false);
                    moveSpeed = 5f;
                }
            }


            if (characterName == "Wizard")
            {
                if (distance <= 4f)
                {
                    myAnim.SetBool("Attack", true);

                    timer += Time.deltaTime;
                    if (timer >= 1.5f)
                    {
                        GameObject newMagic = Instantiate(magic, transform.position, transform.rotation);
                        Destroy(newMagic, 1.5f);
                        timer = 0;
                    }

                    moveSpeed = 0f;
                }
                else
                {
                    myAnim.SetBool("Attack", false);
                    moveSpeed = 5f;
                }
            }
        }
        else
        {
            moveSpeed = 2.5f;
            return;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject,0.15f);
            
            if(other.gameObject.name== "Sword")
            {
                GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[1], transform.position, Quaternion.identity);
                SoundManager.Instance.SoundPlay(2);
                Destroy(newParticle, 0.40f);
            }
            else
            {
                GameObject newParticle = Instantiate(ParticleManager.Instance.bulletParticle[0], transform.position, Quaternion.identity);
                SoundManager.Instance.SoundPlay(7);
                Destroy(newParticle, 0.40f);
            }
        }

        else if(other.CompareTag("Granade"))
        {
            Destroy(gameObject, 0.15f);
            gameObject.GetComponent<Rigidbody>().AddExplosionForce(5f, Vector3.up, 0.5f);
        }

    }

}


