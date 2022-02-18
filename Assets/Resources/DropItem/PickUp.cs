using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Character(Clone)")
        {
            Debug.Log("�ߨ�");
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
