using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frist_Gun : Base_Gun
{

    public override void Init(Gun_Manager main, Animator _ani)
    {
        Main=main;
        _animator=_ani;
    }
    
    #region //�Ҹ�������Ʈ/����Ʈ
    int Fcount;
    public AudioSource[] FLSound;
    public AudioSource[] FHSound;
    public AudioSource[] FirstGunSound;
    int SoundCount;
    public ParticleSystem L1E;
    #endregion

    public override void Shoot()
    {
        #region //�������Ѿ��̾��� �������Ѿ���������� �����۵�
        if (((Input.GetMouseButton(0) && CurrentAmmo == 0) && CurrentPack >= 1))
        {
            Reload_Function();
        }
        #endregion
        #region //�߻�

        else if (Input.GetMouseButton(0) && Delay <= 0 & CurrentAmmo >= 1)//�������Ѿ��� �ְ� �Ѿ˵����̰� 0�����ϰ�� �߻�
        {
            Main.PIN = false;
            Delay = CurrentDelay; //�������� ���
            CurrentAmmo -= 1;//�Ѿ˼Ҹ�
            Main.Shooting = true;//��ݾִϸ��̼��۵������Ѻ���
            HitGUN1();//���
            L1E.Play();//�����÷����۵�
            FirstGunSound[Fcount].Play();//������ƮǮ���صмҸ�������Ʈ �۵� (������ �����Ҹ��� �ƴ� ����Ѵٸ��Ҹ��� ���� �������� ����)
            Fcount++;//�÷��� ���Ļ���ϴ� ������ �迭�� �ʰ��Ҽ��ֱ⶧��
            if (Fcount == FirstGunSound.Length)
            {
                Fcount = 0;
            }
            Main.ARshot = true;
        }
        #endregion
        #region//�۵������ʾ���
        else
        {
            Main.ARshot = false;
            Main.Shooting = false;
        }
        #endregion
        #region//�ݵ��۵�
        if (Main.ARshot == true)//�i�ٸ� �ݵ�����
        {
            Main.Recoil = true;
        }
        else
        {
            Main.Recoil = false;
        }
        #endregion

    }
    public override void Reload_Function()
    {
        if(CurrentAmmo==CurrentReload)
            return;
        Main.PIN = false;

        _animator.CrossFade("reload_shoot_ar", 0.2f);
    }
    public override void Reload_After_Function()
    {
        if (Reload > CurrentPack)
        {
            CurrentAmmo = CurrentPack;
            CurrentPack = 0;
        }
        else
        {
            float temp = CurrentAmmo;
            CurrentAmmo = CurrentReload;
            CurrentPack -= CurrentReload;
            CurrentPack += temp;
        }
    }
    








    void HitGUN1()//��ݱ��
    {
        #region //��ǥ�� ���߽� �ε����� Ȯ��
        if (SoundCount >= FLSound.Length)
        {
            SoundCount = 0;
        }
        #endregion
        RaycastHit hitInfo;//����ĳ��Ʈ ��Ʈ ����
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, GunDistance, layer))
        //ī�޶� ���� �������� �߻�Ǿ� ���ǻ�Ÿ���ŭ ����ĳ��Ʈ�� ����
        {
            Vector3 dir = hitInfo.point - FirePosition.transform.position;
            if (Physics.Raycast(FirePosition.transform.position, dir, out hitInfo, GunDistance, layer))
            //�ѱ�߻籸���� ��Ʈ�ȴ��������� ���ǻ�Ÿ���ŭ ����ĳ��Ʈ�� ����
            //����ĳ��Ʋ �ι��ϴ� ������ ������ �÷��̾��ѱ��� �������ϴµ� ī�޶� ���� �� �� �ִ� ��Ȳ���� ���� �������ϸ� �������� ���̱� �����Դϴ�.
            {
                #region//��ź�������� ������Ʈ�̵��� ��ź�Ҹ��۵�
                if (hitInfo.collider.gameObject.layer == 8)
                {
                    EffectCount = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.gameObject, EffectCount);
                    FLSound[SoundCount].transform.position = hitInfo.transform.position;
                    FLSound[SoundCount].Play();
                    SoundCount++;
                }
                #endregion
                HitHeadShot1(hitInfo);//������߽��۵�
            }
        }
        Main.Move.GunRecoilActive();////�ѱ�ݵ��۵�
        L1E.Stop();//�����÷��õ��۸���
    }


    int GunEffectCount(Vector3 posi, Vector3 rot, GameObject VE, int EffectCount)
    {
        if (EffectCount >= Pooling.transform.childCount)//40�� Ǯ���ִ밹��ã�Ƽ��ֱ�
        {
            EffectCount = 0;
        }

        ParticleSystem effect;

        effect = VE.transform.GetChild(EffectCount).GetComponent<ParticleSystem>();
        effect.transform.position = posi;
        effect.transform.LookAt(cam.transform.position);
        effect.Play();


        EffectCount++;


        return EffectCount;
    }





    void HitHeadShot1(RaycastHit hitInfo)
    {
        
        if (hitInfo.transform.CompareTag("Head") & hitInfo.collider.gameObject.layer == 10)//�����κ������±�,������ ���̾� ��
        {
            #region//�������߱��

            StartCoroutine(Main.HitCross(0.3f));
            Base_HP temp = hitInfo.transform.GetComponent<Base_HP>();
            if (temp != null)
            {
                if (temp.Armor)
                {

                    FHSound[SoundCount].transform.position = hitInfo.transform.position;
                    FHSound[SoundCount].Play();
                    SoundCount++;
                }
                else
                {
                    FLSound[SoundCount].transform.position = hitInfo.transform.position;
                    FLSound[SoundCount].Play();
                    SoundCount++;
                }

                temp.Damged(GunDamage * 5);



                EffectCount = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.gameObject, EffectCount);
            }
            #endregion
        }
        else
        {

            HitShot1(hitInfo);//�����̾ƴ϶�� �Ϲ� ����
        }
    }//���� ���� �������� ��� �۵� �����������ƴ϶�� HitShot�۵�
    void HitShot1(RaycastHit hitInfo)
    {

        if (hitInfo.transform.CompareTag("hitbox") & hitInfo.collider.gameObject.layer == 10)//��Ʈ�ڽ��±�, ������ ���̾� ��
        {
            StartCoroutine(Main.HitCross(0.3f));//���߽� 0.3�ʰ� ������ ǥ���ϱ����� ũ�ν���� ���򺯰�    
            Base_HP temp = hitInfo.transform.GetComponent<Base_HP>();//�߻�Ŭ������ ü���� ������
            temp.Damged(GunDamage);//�Ѹ�ŭ ������
            #region//���� ���Ÿ�Կ� ���� �Ҹ����
            if (temp.Armor)
            {
                FHSound[SoundCount].transform.position = hitInfo.transform.position;
                FHSound[SoundCount].Play();
                SoundCount++;
            }
            else
            {
                FLSound[SoundCount].transform.position = hitInfo.transform.position;
                FLSound[SoundCount].Play();
                SoundCount++;
            }
            EffectCount = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.gameObject, EffectCount);
            #endregion
        }
        else if (hitInfo.transform.CompareTag("Bullet"))
        {
            StartCoroutine(Main.HitCross(0.3f));//���߽� 0.3�ʰ� ������ ǥ���ϱ����� ũ�ν���� ���򺯰�
            Base_Bullet temp = hitInfo.transform.GetComponent<Base_Bullet>();//�߻�Ŭ������ �������� ����ü�� ü���� ������
            //���� ���� ����ü�� ���缭 ���߽�ų �� �ִ� ���
            temp.Damaged(GunDamage);//�Ѹ�ŭ ������
            #region//����ü�� ���Ÿ�Կ� ���� �Ҹ����
            if (temp.Armor)
            {
                FHSound[SoundCount].transform.position = hitInfo.transform.position;
                FHSound[SoundCount].Play();
                SoundCount++;
            }
            else
            {
                FLSound[SoundCount].transform.position = hitInfo.transform.position;
                FLSound[SoundCount].Play();
                SoundCount++;
            }
            #endregion
        }
    }//��ݽ� ����������� �۵�
}