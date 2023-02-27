using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using System;
public class Second_Gun : Base_Gun
{
    
    public VisualEffect L2E;
    public ParticleSystem SE1;
    public AudioSource SecondGunSound;

    private void Start()
        {
            L2E.gameObject.SetActive(false);
            Main = GetComponentInParent<Gun_Manager>();
        }

    // Start is called before the first frame update
    public override void Shoot()
    {
        
        if (Main.Shooting == false)
        {
            if (((Input.GetMouseButton(0) && CurrentAmmo == 0) && CurrentPack >= 1))
            {
                ReloadFunction();
            }
            else if (Input.GetMouseButton(0) && Delay <= 0 & CurrentAmmo >= 1)
            {   
                Main.PIN = false;
                Main.Shooting = true;
                CurrentDelay = Delay;
                CurrentAmmo -= 1;
                HitGUN2();
            }
        }

    }
    public void HitGUN2()
    {
        StartCoroutine(GunDelay(2.5f));
        SE1.Play();
        Vector3 RecoilUp = new Vector3(0, 45, 0);
        Vector3 RecoilDown = new Vector3(0, -45, 0);
        Vector3 RecoilLeft = new Vector3(-45, 00, 0);
        Vector3 RecoilRight = new Vector3(45, 0, 0);
        Main.Up.rectTransform.localPosition = RecoilUp;
        Main.Down.rectTransform.localPosition = RecoilDown;
        Main.Left.rectTransform.localPosition = RecoilLeft;
        Main.Right.rectTransform.localPosition = RecoilRight;

        RaycastHit[] hitInfos;
        Vector3 posi = cam.transform.GetChild(1).position; //시작위치
        Vector3 endposi = cam.transform.GetChild(0).position; //마지막위치
        RaycastHit hit;

        GameObject VE = Pooling.transform.gameObject;



        if (Physics.SphereCast(cam.transform.position, 0.75f, cam.transform.forward, out hit, GunDistance))
        {
            hitInfos = Physics.SphereCastAll(cam.transform.position, 0.75f, cam.transform.forward, GunDistance);

            Array.Sort(hitInfos, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance)); // cast한거거리순으로정렬
                                                                                                    

            List<int> attackEnemy = new List<int>();
            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].collider.gameObject.layer == 8)

                {
                    Vector3 a = hitInfos[i].point - cam.transform.position;
                    EffectCount = GunEffectCount(hitInfos[i].point, hitInfos[i].normal, VE, EffectCount);

                    break;

                }
                else if (hitInfos[i].collider.gameObject.layer == 10 & hitInfos[i].collider.tag != "Head")
                {

                    EffectCount = GunEffectCount(hitInfos[i].point, hitInfos[i].normal, VE, EffectCount);
                    StartCoroutine(Main.HitCross(1.0f));

                    int k = 0;
                    bool ck = true;
                    Base_HP temp = hitInfos[i].transform.GetComponent<Base_HP>();
                    if (temp != null)
                    {

                        k = temp.ID;

                        for (int j = 0; j < attackEnemy.Count; j++)
                        {
                            if (attackEnemy[j] == k)
                            {
                                ck = false;
                            }
                            if (ck == true)
                            {
                                attackEnemy.Add(k);
                                if (temp.Live)
                                {
                                    temp.Damged(GunDamage, true);
                                }



                            }
                        }
                    }


                }
                else if (hitInfos[i].transform.CompareTag("Bullet"))
                {
                    StartCoroutine(Main.HitCross(0.3f));

                    hitInfos[i].transform.GetComponent<Base_Bullet>().Damaged(20);
                }
            }
            L2E.gameObject.SetActive(true);
        }
    }
    IEnumerator GunDelay(float Del)
    {
        Main.SRshot = true;
        Main.Shooting = true;
        yield return new WaitForSeconds(Del);
        Main.SRshot = false;
        Main.Shooting = false;
        L2E.gameObject.SetActive(false);
    }
    int GunEffectCount(Vector3 posi, Vector3 rot, GameObject VE, int EffectCount)
    {
        if (EffectCount >= VE.transform.childCount)//40을 풀링최대갯수찾아서넣기
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
    public override void Reload_Function()
    {

        StartCoroutine(ReloadFunction());
    }


    IEnumerator ReloadFunction()
    {

        Main.PIN = false;
        Main.Reloading = true;
        Main.SRRealod = true;


        yield return new WaitForSeconds(ReloadTime + 0.3f);
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

        Main.SRRealod = false;



        Main.Reloading = false;
    }



}
