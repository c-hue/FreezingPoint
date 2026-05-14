using UnityEngine;

public class TreeAnimationRelay : MonoBehaviour
{
    public void ApplyDamage()
    {
        ChoppableTree tree = GetComponentInChildren<ChoppableTree>();
        if (tree != null)
        {
            tree.ApplyDamage();
        }
    }

    public void TreeIsDead()
    {
        ChoppableTree tree = GetComponentInChildren<ChoppableTree>();
        if (tree != null)
        {
            tree.TreeIsDead();
        }
    }
}