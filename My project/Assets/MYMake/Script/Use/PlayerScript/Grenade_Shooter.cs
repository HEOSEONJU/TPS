using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_Shooter : MonoBehaviour
{
    [SerializeField]
    Gun_Manager Main;
    
    public List<GrenadeBoom> GrenadePooling;
    public int GrenadeIndex = 0;
    public Transform LeftHand;
    public float ThrowPower = 30.0f;
    public float ThrowDelay = 2.0f;
    public float ThrowMAXDelay = 2.0f;
    public float MAXThrowAngle = 45.5f;
    public float MINThrowAngle = 43.5f;


    public int GrenadeCount;
    public int GrenadeMAXCount;


    
    public Transform GrenadePosi;
    public float step = 0.01f;
    public float angle;
    public void Regen_Granade()
    {

        if (ThrowDelay <= ThrowMAXDelay)
        {
            ThrowDelay += Time.deltaTime;
        }

        if (Main.Shooting == true && Main.Move.Action == false)//�׼��߿� ���α׸��� ���
        {
            Main.PIN = false;
        }

    }
    public void Throw_Grande()
    {
        if (GrenadeCount >= 1 & ThrowDelay >= ThrowMAXDelay)
        {
            if (Main.PIN == false)
            {
                Main.PIN = true;
                return;
            }
            ThrowDelay = 0.0f;
            GrenadeCount -= 1;
            switch(Main.Equip_Gun.GunID)
            {
                case 3:
                    Main.SpineAction.SetTrigger("HG_Grenade");
                    break;
                default:
                    Main.SpineAction.SetTrigger("AR_Grenade");
                    break;
            }
        }
    }
    public void GrendaeThrow_INDEX_Function()
    {
        GrenadePooling[GrenadeIndex].transform.position = LeftHand.position;
        GrenadePooling[GrenadeIndex].transform.rotation = LeftHand.rotation;
        GrenadePooling[GrenadeIndex].Throw(GrenadePosi.forward, ThrowPower, angle);
        GrenadeIndex++;
        if (GrenadeIndex >= GrenadePooling.Count)
        {
            GrenadeIndex = 0;
        }
    }
    public void ThrowLine()
    {
        if (Main.PIN == true & Main.Move.Action == false & Main.Swaping == false & Main.Reloading == false)
        {
            if (Main.Line.enabled == false)
            {
                Main.Line.enabled = true;
            }
            Vector3 direction = GrenadePosi.forward;
            Vector3 Grounddirection = new Vector3(direction.x, 0, direction.z);//����ź�� ��������
            float t = (0.5f - Main.cam.transform.GetComponent<GameCamera>().currXRot / 60);//����ź�� ���� ����

            angle = Mathf.Lerp(MAXThrowAngle, MINThrowAngle, t);//���� ī�޶��ִ�ġ����

            step = Mathf.Max(0.01f, step);//������ ���� �ֱ�
            DrewLine(Grounddirection.normalized, ThrowPower, angle, step);
        }
        else
        {
            Main.Line.enabled = false;
        }
    }
    public void DrewLine(Vector3 direction, float Power, float angle, float step)//����,��,����,�ֱ�
    {
        float time = 6.0f;//�ִ�γ��ư����ִ� �ð�
        Main.Line.positionCount = (int)(time / step) + 2;//���λ����� �� ����
        int count = 0;//���� ����
        for (float i = 0; i < time; i += step)
        {
            float x = Power * i * Mathf.Cos(angle);
            float y = Power * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            //������ ���̿� ���̸� ���           
            Main.Line.SetPosition(count, GrenadePosi.position + direction * x + Vector3.up * y);
            //�Ű������� ���� �������� ������ ���̿� ���̿� ���� �Է�
            count++;
        }
        float Finalx = Power * time * Mathf.Cos(angle);
        float Finaly = Power * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        Main.Line.SetPosition(count, GrenadePosi.position + direction * Finalx + Vector3.up * Finaly);
        //��������ġ�� ���� �Է�
    }
}
