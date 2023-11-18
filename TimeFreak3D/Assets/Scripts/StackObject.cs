using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarwanZaky;
public class StackObject : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        StackManager characterStack;
        if (other.TryGetComponent(out characterStack))
        {
            if(gameObject.name == "HealthBox" && other.gameObject.GetComponent<HealthManager>().currentHealth<1)
            {
                characterStack.AddNewItem(this.transform);
            }

            if(gameObject.name != "HealthBox")
            {
                characterStack.AddNewItem(this.transform);
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
