using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with Zoom")]
public class MouseOrbit : MonoBehaviour
{

    public Transform target;
    public float defaultDistance = 5.0f;
    public float zoomSpeed = 0.1f;
    private float wantedDistance = 5.0f;
    private float currentDistance = 5.0f;
    public float panSpeed = 0.1f;
    private Vector3 wantedPosition = new Vector3();
    private Vector3 currentPosition = new Vector3();
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;
    public float zSpeed = 20.0f;
    public float fovSpeed = 1.0f;
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;
    public float fovMin;
    public float fovMax;
    public Vector2 firstMousePosition;
    public int unlockThreshold = 5;
    public bool useClickAndDrag = false;
    public float dampening = 1.0f;
    public bool spinUnlocked = true;

    private Rigidbody rigidbody;
    private Quaternion currentRotation;
    private Camera camera;

    private

    float x = 0.0f;
    float y = 0.0f;
    float f = 0.0f;
    bool isZoomed = false;

    // Use this for initialization
    private void Awake()
    {
        //Events.EUpdateFloatingOrigin += EUpdateFloatingOrigin;
    }

    void Start()
    {
        camera = GetComponent<Camera>();
        f = camera.fieldOfView;
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }

        currentDistance = defaultDistance;
        wantedDistance = defaultDistance;

        /*
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        */
    }

    void LateUpdate()
    {
        DoZoom();
        if (target)
        {
            if (useClickAndDrag)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    firstMousePosition = Input.mousePosition;
                }

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (!spinUnlocked)
                    {
                        if (Vector2.Distance(Input.mousePosition, firstMousePosition) >= unlockThreshold)
                        {
                            spinUnlocked = true;
                        }
                    }
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    spinUnlocked = false;
                }
            }
            else
            {
                spinUnlocked = true;
            }

            if (spinUnlocked)
            {
                x += Input.GetAxis("MOUSE_X") * xSpeed * 0.02f;
                y -= Input.GetAxis("MOUSE_Y") * ySpeed * 0.02f;
            }

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion wantedRotation = Quaternion.Euler(y, x, 0);
            currentRotation = Quaternion.Lerp(currentRotation, wantedRotation, dampening);

            wantedDistance = Mathf.Clamp(wantedDistance - Input.GetAxis("MOUSE_Z") * zSpeed, distanceMin, distanceMax);
            currentDistance = Mathf.Lerp(currentDistance, wantedDistance, zoomSpeed);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -currentDistance);

            wantedPosition = target.position;
            currentPosition = Vector3.Lerp(currentPosition, wantedPosition, panSpeed);

            Vector3 position = currentRotation * negDistance + currentPosition;

            transform.rotation = currentRotation;
            transform.position = position;
        }
    }

    private void DoZoom()
    {
        if (isZoomed)
        {
            f = Mathf.MoveTowards(f, fovMin, fovSpeed);
        }
        else
        {
            f = Mathf.MoveTowards(f, fovMax, fovSpeed);
        }
        camera.fieldOfView = f;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public void Zoom(bool b)
    {
        isZoomed = b;
    }

    public void EUpdateFloatingOrigin(Vector3 offset)
    {
        if (target)
        {
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            currentDistance = wantedDistance;
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -currentDistance);

            currentPosition = target.position;

            Vector3 position = rotation * negDistance + currentPosition;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
}