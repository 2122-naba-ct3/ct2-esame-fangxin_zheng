using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ThirdPersonShooterController : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform objBullet;
    [SerializeField] private Transform bulletSpwanPos;
    [SerializeField] private Transform fxBoom;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider bulletBar;
    [SerializeField] private Transform dieUI;
    [SerializeField] private AudioClip footAudio;


    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private AudioSource shootAudio;


    [SerializeField] private float shootIntervalTime;
    private float shootTimer;

    [SerializeField] private int hpMax;
    [SerializeField] private int bulletNumMax;
    private int hp;
    private int bulletNum;

    private bool reloading = false;

    private void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        shootAudio = GetComponent<AudioSource>();
        shootTimer = shootIntervalTime;
    }
    // Start is called before the first frame update
    void Start()
    {
        hp = hpMax;
        bulletNum = bulletNumMax;
        healthBar.maxValue = hpMax;
        healthBar.value = hp;
        bulletBar.maxValue = bulletNumMax;
        bulletBar.value = bulletNum;
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        Vector3 mouseWorldPosition = Vector3.zero;
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        Vector3 hitPoint = Vector3.one;
        //检测屏幕中心的碰撞
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            //debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
            hitPoint = raycastHit.point;
        }

        Move();

        //如果按下鼠标右键
        if (starterAssetsInputs.aim)
        {
            //瞄准时放大视角
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1f, Time.deltaTime * 10f));

            //瞄准时人物朝向前方
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 10f));
        }

        if (starterAssetsInputs.aim && starterAssetsInputs.shoot && !reloading)
        {
            //if (bulletNum <= 0)
            //{
            //    animator.SetBool("isShoot", false);
            //}

            //animator.SetBool("isShoot", false);
            //不发射实体子弹，直接检测
            if (shootTimer >= shootIntervalTime && bulletNum > 0)
            {
                animator.SetBool("isShoot", true);
                shootTimer = 0f;
                bulletNum -= 1;
                bulletBar.value = bulletNum;

                shootAudio.Play();
                if (hitTransform != null)
                {
                    //子弹碰撞粒子
                    Instantiate(fxBoom, hitPoint, Quaternion.identity);
                    if (hitTransform.tag == "Enemy" || hitTransform.tag == "Boss")
                    {
                        hitTransform.GetComponentInParent<Enemy>().Hurt(1);
                    }
                }
            }
            else
            {
                animator.SetBool("isShoot", false);
            }
            //发射实体子弹
            //Vector3 aimDir = (mouseWorldPosition - bulletSpwanPos.position).normalized;
            //Instantiate(objBullet, bulletSpwanPos.position, Quaternion.LookRotation(aimDir, Vector3.up));

            //starterAssetsInputs.shoot = false;
        }
        else if (!starterAssetsInputs.shoot)
        {
            animator.SetBool("isShoot", false);
        }
        //换单动画
        if (starterAssetsInputs.reload)
        {
            reloading = true;
            starterAssetsInputs.shoot = false;
            starterAssetsInputs.reload = false;
            //animator.SetLayerWeight(2, 1f);
            StartCoroutine(SetLayerWeightTime(2, 1f));

            animator.SetBool("isReload", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);

        if (other.tag == "Enemy")
        {
            Hurt();
        }
    }

    private void Move()
    {
        if (starterAssetsInputs.move == Vector2.zero)
        {
            animator.SetBool("isRun", false);
            //Debug.Log("no");
        }
        if (starterAssetsInputs.move != Vector2.zero)
        {
            animator.SetBool("isRun", true);
            //Debug.Log("move");
        }

        if (starterAssetsInputs.jump && !thirdPersonController.Grounded)
        {
            animator.SetBool("isJump", true);
        }
        if (thirdPersonController.Grounded)
        {
            animator.SetBool("isJump", false);
        }
    }

    public void Hurt()
    {
        if (hp > 0)
        {
            hp -= 1;
            healthBar.value = hp;
        }
        else
        {

            Time.timeScale = 0;
            starterAssetsInputs.cursorLocked = false;
            starterAssetsInputs.cursorInputForLook = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            dieUI.gameObject.SetActive(true);
        }
    }

    public void ReloadEnd()
    {
        reloading = false;
        bulletNum = bulletNumMax;
        bulletBar.value = bulletNum;
        animator.SetBool("isReload", false);
        //animator.SetLayerWeight(2, 0f);
        StartCoroutine(SetLayerWeightTime(2, 0f));
    }

    IEnumerator SetLayerWeightTime(int layerindex, float weight)
    {
        //Debug.Log("Start");
        if (weight == 0f)
        {

            //Debug.Log("0");
            float tmp = 1f;
            while (animator.GetLayerWeight(layerindex) > weight)
            {
                tmp -= Time.deltaTime * 5f;
                animator.SetLayerWeight(layerindex, tmp);
                yield return null;
            }
            animator.SetLayerWeight(layerindex, weight);
        }
        else if (weight == 1f)
        {
            //Debug.Log("1");
            float tmp = 0f;
            while (animator.GetLayerWeight(layerindex) < weight)
            {
                tmp += Time.deltaTime * 5f;
                animator.SetLayerWeight(layerindex, tmp);
                yield return null;
            }
            animator.SetLayerWeight(layerindex, weight);
        }
        yield return null;
    }

    public void PlayFootAudio()
    {
        AudioSource.PlayClipAtPoint(footAudio, transform.position);
    }

}
