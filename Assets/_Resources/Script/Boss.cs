using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    private Enemy enemy;
    private NavMeshAgent agent;
    private Animator ani;
    [SerializeField] private bool following = false;

    [SerializeField] private Transform overUI;

    private float dis = 0f;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<Enemy>();
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        dis = (transform.position - enemy.player.position).magnitude;


        if (following)
        {
            ani.SetBool("isRun", true);
            agent.destination = enemy.player.position;
        }
        else
        {
            ani.SetBool("isRun", false);
            agent.ResetPath();
        }

        if (dis < 5f)
        {
            following = false;
            ani.SetTrigger("Atk");
        }

        if (dis > 8.5f)
        {
            following = true;
        }
#if UNITY_EDITOR
        Debug.Log((transform.position - enemy.player.position).magnitude);
#endif
    }

    private void OnDestroy()
    {

        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        overUI.gameObject.SetActive(true);
    }

    public void Atk()
    {
        if (dis <= 5f)
        {
            enemy.player.GetComponent<ThirdPersonShooterController>().Hurt();
        }
    }
}
