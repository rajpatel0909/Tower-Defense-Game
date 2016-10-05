using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    // Use this for initialization
    public float moveSensitivityX = 1f;
    public float moveSensitivityZ = 1f;
    public float minimumX;
    public float maximumX;
    public float minimumZ;
    public float maximumZ;
    public bool invertMoveX = false;
    public bool invertMoveZ = false;

    private Camera _camera;

    //GameObject king;
	void Start () {
        _camera = Camera.main;
    }
	
	// Update is called once per frame
	void Update () {
        //old code : keyboard input for camera movement
        float xMove = Input.GetAxis("Horizontal") * Time.deltaTime * 10;
        xMove = xMove + transform.position.x;
        xMove = Mathf.Clamp(xMove, minimumX, maximumX);
        float zMove = Input.GetAxis("Vertical") * Time.deltaTime * 10;
        zMove = zMove + transform.position.z;
        zMove = Mathf.Clamp(zMove, minimumZ, maximumZ);
        this.transform.position = new Vector3(xMove, transform.position.y, zMove);
        //old code

        /*//new code : swipe gestures for camera movement
        Touch[] touches = Input.touches;
        if(touches.Length > 0)
        {
            if (touches.Length == 1)
            {
                if(touches[0].phase == TouchPhase.Began)
                {
                }
                if(touches[0].phase == TouchPhase.Ended)
                {
                }
                if (touches[0].phase == TouchPhase.Moved)
                { 
                    Vector2 delta = touches[0].deltaPosition;
                    float positionX = delta.x * moveSensitivityX * Time.deltaTime;
                    positionX = invertMoveX ? positionX : -positionX;
                    positionX += _camera.transform.position.x;
                    positionX = Mathf.Clamp(positionX, minimumX, maximumX);

                    float positionZ = delta.y * moveSensitivityZ * Time.deltaTime;
                    positionZ = invertMoveZ ? positionZ : -positionZ;
                    positionZ += _camera.transform.position.z;
                    positionZ = Mathf.Clamp(positionZ, minimumZ, maximumZ);

                    
                    _camera.transform.position = new Vector3(positionX, _camera.transform.position.y, positionZ);
                }
            }
        }
        //new code*/

	}

}
