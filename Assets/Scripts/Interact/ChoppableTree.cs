using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
 
public class ChoppableTree : MonoBehaviour
{
    public bool playerInRange;
    public bool canBeChopped;
    public float treeMaxHealth = 10f;
    public float treeHealth;
    public Animator animator;

    void Start()
    {
        treeHealth = treeMaxHealth;
        animator = transform.parent.GetComponent<Animator>();
    }

    public void GetHit()
    {
        Debug.Log("Tree hit");
        animator.SetTrigger("Shake");
    }

    public void ApplyDamage()
    {
        treeHealth -= 1f;
        if (treeHealth <= 0f)
        {
            animator.SetTrigger("Die");
        }
    }

    public void TreeIsDead()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.Instance.selectedTree = null;
        SelectionManager.Instance.chopHolder.gameObject.SetActive(false);

        GameObject log1 = Instantiate(Resources.Load<GameObject>("Wood1"), treePosition + new Vector3(2f, 0, 0), Quaternion.Euler(0, 0, 0));
        GameObject log2 = Instantiate(Resources.Load<GameObject>("Wood1"), treePosition + new Vector3(-2f, 0, 0), Quaternion.Euler(0, 0, 0));
        GameObject log3 = Instantiate(Resources.Load<GameObject>("Wood1"), treePosition + new Vector3(0f, 0, 2f), Quaternion.Euler(0, 0, 0));

    }

    void Update()
    {
        if (canBeChopped)
        {
            GlobalState.Instance.resourceHealth = treeHealth;
            GlobalState.Instance.resourceMaxHealth = treeMaxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange=true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange=false;
        }
    }
}