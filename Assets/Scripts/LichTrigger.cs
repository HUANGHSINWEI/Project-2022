using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichTrigger : MonoBehaviour
{
    private int count;
    public Animator lich;
    void Update()
    {
        //Debug.Log("���a            "+GameObject.Find("Character(Clone)").transform.position);
        //Debug.Log("Ĳ�o            "+ gameObject.transform.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        count++;
        if(other.name== "Character(Clone)"&& count==1)
        {
            Debug.Log("+++++++++++++++++++++++�i�J");
            Camera.main.depth = -5;
            StartCoroutine(PlayAnimation());
        }
    }

    IEnumerator PlayAnimation()
    {
        //yield return new WaitUntil(lich.GetCurrentAnimatorStateInfo(0).normalizedTime > 1);
        yield return new WaitForSeconds(2.0f);
        Camera.main.depth = -1;
    }
    bool CheckAnimationEnd(int a)
    {
        if(a==1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
