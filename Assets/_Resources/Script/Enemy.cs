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
    private Transform barUI;    //Ѫ��UI
    private Image hpUI;         //Ѫ��UI
    public float hpMax;         //���Ѫ��
    private float hp;           //��ǰѪ��
    [SerializeField] private bool following = false;    //Ѱ·״̬

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
        //����Ѫ��λ��
        if (barUI != null)
        {
            barUI.position = barPoint.position;
            barUI.forward = camMain.transform.forward;
        }
    }

    public void Hurt(int dmg)
    {
        //����Ѫ��
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
