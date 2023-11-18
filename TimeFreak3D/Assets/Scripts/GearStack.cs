using UnityEngine;
using MarwanZaky;
public class GearStack : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        StackManager characterStack;
        if (other.TryGetComponent(out characterStack))
        {
            characterStack.AddNewItem(this.transform);
        }
    }
}
