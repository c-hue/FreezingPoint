using UnityEngine;

[CreateAssetMenu(fileName = "New Blueprint", menuName = "Inventory/Blueprint")]
public class Blueprint : ScriptableObject
{
    public string itemName;

    public string Req1;
    public string Req2;
    public string Req3;

    public int Req1amount;
    public int Req2amount;
    public int Req3amount;
    public int numOfRequirements;

    public int numOfItems;
}
