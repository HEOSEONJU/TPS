using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Gun : MonoBehaviour
{


    #region//����������Ʈ
    public Gun_Manager Main;
    public Camera cam;
    public GameObject GunModel;
    [SerializeField]
    protected GameObject FirePosition;
    [SerializeField]
    protected GameObject Pooling;
    protected int EffectCount = 0;
    #endregion
    #region//���� �������
    public float Delay;//�������������
    protected float Reload; //�ִ�����
    [SerializeField]
    protected float ReloadTime;//�����ð�
    protected float Pack;//����
    public float MAXPack;//�ִ�����
    #endregion
    #region//���� ����
    public int GunID = 1;//����ִ� ���� ��ȣ
    [SerializeField]
    protected float GunDistance;
    protected float BoomGunDelay;
    protected float BoomGunMAXDelay;
    protected int GrenadeCount;
    protected int GrenadeMAXCount;
    [SerializeField]
    protected LayerMask layer;
    public float CurrentDelay;//���ǵ�����
    public float CurrentAmmo;
    public float CurrentReload;
    public float CurrentPack;
    [SerializeField]
    protected int GunDamage = 200;
    #endregion

    [SerializeField]
    protected Animator _animator;
    

    public abstract void Init(Gun_Manager main,Animator _ani);
    public  abstract void Shoot();
    public abstract void Reload_Function();
    public abstract void Reload_After_Function();
    




}
