using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private WeaponManager weapon_Manager;

    public float fireRate = 15f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnim;
    private bool zoomed;
    private Camera mainCam;
    private GameObject crosshair;
    private bool is_Aiming;
    [SerializeField]
    private GameObject arrow_Prefab, spear_Prefab;
    [SerializeField]
    private Transform arrow_Bow_StartPosition;
    // Start is called before the first frame update
    void Awake()
    {
        weapon_Manager = GetComponent < WeaponManager>();

        zoomCameraAnim = transform.Find(Tags.LOOK_ROOT)
           .transform.Find(Tags.ZOOM_CAMERA).GetComponent<Animator>();

        crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);

        mainCam = Camera.main;
        
    }

    // Update is called once per frame
    void Update()
    {
        WeaponShoot();
        ZoomInAndOut();
       
    }


    public void WeaponShoot()
    {
        if(weapon_Manager.GetCurrentSelectedWeapon().fireType == WeaponFireType.MULTIPLE)
        {
            if(Input.GetMouseButton(0) && Time.time > nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;

                weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
            }

                 BulletFired();

        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(weapon_Manager.GetCurrentSelectedWeapon().tag == Tags.AXE_TAG)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }
            
                if(weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.BULLET)
                {
                    weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                }

                BulletFired();
            
            }  
                else
                {
                    if(is_Aiming)
                    {
                         weapon_Manager.GetCurrentSelectedWeapon().ShootAnimation();
                         
                         if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.ARROW)
                         { 
                             
                         }
                        else if (weapon_Manager.GetCurrentSelectedWeapon().bulletType == WeaponBulletType.SPEAR)
                        {
                        
                        }
                        
                    }

                }
        }
    }       
            
              
    void ZoomInAndOut()
    {
        if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
        {
            if (Input.GetMouseButtonDown(1))
            {
                zoomCameraAnim.Play(AnimationTags.ZOOM_IN_ANIM);

                crosshair.SetActive(false);


            }




            if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.AIM)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    zoomCameraAnim.Play(AnimationTags.ZOOM_OUT_ANIM);

                    crosshair.SetActive(true);
                }


            }

            if (weapon_Manager.GetCurrentSelectedWeapon().weapon_Aim == WeaponAim.SELF_AIM)
            {
                if(Input.GetMouseButtonDown(1))
                {
                    weapon_Manager.GetCurrentSelectedWeapon().Aim(true);
                    is_Aiming = true;
                    ThrowArrowOrSpear(true);
                    
                }

                if (Input.GetMouseButtonUp(1))
                {
                    weapon_Manager.GetCurrentSelectedWeapon().Aim(false);
                    is_Aiming = false;
                    ThrowArrowOrSpear(false);
                }

            }



        }    
    
       
     }
    
    
    void ThrowArrowOrSpear(bool ThrowArrow)
    {
        if(ThrowArrow)
        {
            GameObject arrow = Instantiate(arrow_Prefab);
            arrow.transform.position = arrow_Bow_StartPosition.position;
            arrow.GetComponent<BowAndArrow>().Launch(mainCam);
        }

        else
        {
            GameObject spear = Instantiate(spear_Prefab);
            spear.transform.position = arrow_Bow_StartPosition.position;
            spear.GetComponent<BowAndArrow>().Launch(mainCam);
        }
    
    
    }
   void BulletFired()
    {
        RaycastHit hit;
        if(Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            if(hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
        }
    }
}    







