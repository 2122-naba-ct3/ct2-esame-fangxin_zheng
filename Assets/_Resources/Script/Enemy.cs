using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    public GameObject helathBar;
    public Transform barPoint;
    private Camera camMain;
    private Transform barUI;    //血条UI
    private Image hpUI;         //血量UI
    public float hpMax;         //最大血量
    private float hp;           //挡前血量
    [SerializeField] private bool following = false;    //寻路状态

    // Start is called before the first frame update
    private void OnEnable()
    {
        camMain = Camera.main;
        foreach (Canvas c in FindObjectsOfType<Canvas>())
        {
            if (c.renderMode == RenderMode.WorldSpace)
            {
                barUI = Instantiate(helathBar, c.transform).transform;
                hpUI = barUI.GetChild(0).GetComponent<Image>();
            }
        }
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<ThirdPersonShooterController>().transform;
        hp = hpMax;
    }

    private void Update()
    {
            if (following)
            {
                agent.destination = player.position;
            }
    }

    private void LateUpdate()
    {
        //更新血条位置
        if (barUI != null)
        {
            barUI.position = barPoint.position;
            barUI.forward = camMain.transform.forward;
        }
    }

    public void Hurt(int dmg)
    {
        //更新血量
        hp -= dmg;
        float tmp = (float)hp / hpMax;
        hpUI.fillAmount = tmp;
        if (hp <= 0)
        {
            Destroy(barUI.gameObject);
            Destroy(gameObject);
        }
    }

    public bool IsFollow()
    {
        return following;
    }
    public void SetFollow(bool b)
    {
        following = b;
    }
}
