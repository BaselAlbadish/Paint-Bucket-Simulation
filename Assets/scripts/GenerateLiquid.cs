using UnityEngine;
public class GenerateLiquid : MonoBehaviour
{
    public int amount;
    public GameObject liquid;

    private void FixedUpdate()
    {
        var position = transform.position + Vector3.up * 0.01f;
        Instantiate(liquid, position , Quaternion.identity);
        Instantiate(liquid, position + Vector3.forward * 0.1f , Quaternion.identity); 
        Instantiate(liquid, position + Vector3.back * 0.1f, Quaternion.identity); 
        Instantiate(liquid, position + Vector3.left * 0.1f, Quaternion.identity); 
        Instantiate(liquid, position + Vector3.right * 0.1f, Quaternion.identity);
        
        var position2 = position + Vector3.up * 0.01f;
        Instantiate(liquid, position2 , Quaternion.identity);
        Instantiate(liquid, position2 + Vector3.forward * 0.1f , Quaternion.identity); 
        Instantiate(liquid, position2 + Vector3.back * 0.1f, Quaternion.identity); 
        Instantiate(liquid, position2 + Vector3.left * 0.1f, Quaternion.identity); 
        Instantiate(liquid, position2 + Vector3.right * 0.1f, Quaternion.identity);

    }
}
