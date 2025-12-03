using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minZ;
    [SerializeField]
    private float maxZ;
    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(Mathf.Clamp((transform.position.x + cameraSpeed * Time.deltaTime),minX,maxX), transform.position.y, Mathf.Clamp(transform.position.z,minZ,maxZ));
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(Mathf.Clamp((transform.position.x - cameraSpeed * Time.deltaTime),minX,maxX), transform.position.y, Mathf.Clamp(transform.position.z,minZ,maxZ));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x,minX,maxX), transform.position.y, Mathf.Clamp((transform.position.z - cameraSpeed * Time.deltaTime),minZ,maxZ));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), transform.position.y, Mathf.Clamp((transform.position.z + cameraSpeed * Time.deltaTime), minZ, maxZ));
        }

    }
}
