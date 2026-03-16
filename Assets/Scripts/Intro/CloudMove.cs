using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float speed;
    public float resetX;
    public float endX;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.x > endX)
        {
            transform.position = new Vector3(resetX, transform.position.y, transform.position.z);
        }
    }
}