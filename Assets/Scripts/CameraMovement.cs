using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraSpeed;
    

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
		// forward
        if(Keyboard.current.wKey.isPressed)
        {
            transform.position += new Vector3(0, 0, cameraSpeed * Time.deltaTime);
        }

		// left
		if (Keyboard.current.aKey.isPressed)
		{
			transform.position += new Vector3(-cameraSpeed * Time.deltaTime, 0, 0);
		}

		// down
		if (Keyboard.current.sKey.isPressed)
		{
			transform.position += new Vector3(0, 0, -cameraSpeed * Time.deltaTime);
		}

		// right
		if (Keyboard.current.dKey.isPressed)
		{
			transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
		}
	}
}
