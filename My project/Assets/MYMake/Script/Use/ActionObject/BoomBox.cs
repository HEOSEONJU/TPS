using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBox : MonoBehaviour
{


    public bool ck;
    public Vector3 position;//���ڿ����÷��̾���ġ
    public float Xrot;//���ڿ����÷��̾�� ����������ƾ������
    public Animator ani;//���ڿ����¾ִϸ��̼�
    AudioSource open;//���ڿ����¼Ҹ�
    public CanvasGroup canvas;//����UI
    bool canvascheck;//�������̴����Ⱥ��̴���

    void Start()
    {
        Xrot = 0;
        position = transform.GetChild(0).position;
        open = GetComponent<AudioSource>();
        ani=GetComponent<Animator>();
        canvas.alpha = 0.0f;
        canvascheck = false;
        ck = true;
        
        
    }
    public void SelfDestroy(float time)
    {
        transform.GetChild(2).GetChild(4).GetComponent<Outline>().enabled = false;
        
    }
    public void PlayerOpenSound()
    {
        open.Play();
        GameManager.instance.Score += 20;
        Destroy(gameObject, 3.0f);


    }
    
    private void Update()
    {
        
        if (Vector3.Distance(transform.position,GameManager.instance.Cam_Player_Position.position)<8.0f)
        {
            canvas.alpha = Mathf.Lerp(canvas.alpha, 1.0f, Time.deltaTime * 3);
        }
        else
        {
            canvas.alpha = Mathf.Lerp(canvas.alpha, 0.0f, Time.deltaTime * 3);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        LayerMask Player = 9;

        
        if (other.gameObject.layer == Player)
        {
            
            canvascheck = true;

        }

    }
    public void OnTriggerExit(Collider other)
    {
        LayerMask Player = 9;
        
        if (other.gameObject.layer == Player)
        {
            
            canvascheck = false;
        }

    }
}
