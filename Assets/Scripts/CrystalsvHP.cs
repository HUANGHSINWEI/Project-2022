using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalsvHP: MonoBehaviour
{
    //�������
    public GameObject crystalsvHp;
    public Image hpImage;
    public Image hpImage0;

    // Update is called once per frame
    void Update()
    {
        //����������V��v��
        crystalsvHp.transform.forward = GameObject.Find("Main Camera").transform.forward * -1; //�Ǫ�Hp�����V��v��
    }
}
