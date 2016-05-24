// Smooth Follow from Standard Assets
// Converted to C# because I fucking hate UnityScript and it's inexistant C# interoperability
// If you have C# code and you want to edit SmoothFollow's vars ingame, use this instead.
using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{

    private GameManager GM;

    // The target we are following
    public Transform target;
    // The distance in the x-z plane to the target
    public float distance = 10.0f;
    // the height we want the camera to be above the target
    public float height = 5.0f;
    // How much we 
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;
    public float cameraVertMod;
    private Quaternion originalRot;

    public bool moveCam;

    // Place the script in the Camera-Control group in the component menu
    [AddComponentMenu("Camera-Control/Smooth Follow")]

    void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Start()
    {
        originalRot = target.transform.localRotation;
    }

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target) return;
        if (GM.gameTimer <= 0) return;

        MouseControls();

        if (!Input.GetMouseButton(1))
        {
            ControllerControls();
        }

        //Clamp hor and vert
        //cameraHorMod = Mathf.Clamp(cameraHorMod, -90, 90);
        cameraVertMod = Mathf.Clamp(cameraVertMod, -1, 3);

        // Calculate the current rotation angles and reset modifiers
        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + height + cameraVertMod;
        cameraVertMod = 0;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the height
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the target
        transform.LookAt(target);
    }

    void MouseControls()
    {
        //Move cam around horizontally
        if (Input.GetAxis("Mouse X") != 0)
        {
            target.transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * 100, 0);
            //cameraHorMod += (Input.GetAxis("HorizontalRight") * Time.deltaTime) * 1000;
        }
        else if (!Input.GetMouseButton(1))
        {
            target.transform.localRotation = Quaternion.Lerp(target.transform.localRotation, originalRot, Time.deltaTime * 3);
            //cameraHorMod = Mathf.Lerp(cameraHorMod, 0, Time.deltaTime / 4);
        }

        //Move cam around vertically
        if (Input.GetAxis("Mouse Y") != 0)
        {
            cameraVertMod += (-Input.GetAxis("Mouse Y") * Time.deltaTime) * 25;
        }
        else if (!Input.GetMouseButton(1))
        {
            cameraVertMod = Mathf.Lerp(cameraVertMod, 0, Time.deltaTime / 2);
        }
    }

    void ControllerControls()
    {

        //Move cam around horizontally
        if (Input.GetAxis("HorizontalRight") != 0)
        {
            target.transform.Rotate(0, Input.GetAxis("HorizontalRight") * Time.deltaTime * 100, 0);
            //cameraHorMod += (Input.GetAxis("HorizontalRight") * Time.deltaTime) * 1000;
        }
        else if (Input.GetAxis("Vertical") != 0)
        {
            target.transform.localRotation = Quaternion.Lerp(target.transform.localRotation, originalRot, Time.deltaTime * 5);
            //cameraHorMod = Mathf.Lerp(cameraHorMod, 0, Time.deltaTime / 4);
        }

        //Move cam around vertically
        if (Input.GetAxis("VerticalRight") != 0)
        {
            cameraVertMod += (Input.GetAxis("VerticalRight") * Time.deltaTime) * 25;
        }
        else if (Input.GetAxis("Vertical") != 0)
        {
            cameraVertMod = Mathf.Lerp(cameraVertMod, 0, Time.deltaTime / 2);
        }

    }
}