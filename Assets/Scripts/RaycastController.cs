using UnityEngine;

public class RaycastController : MonoBehaviour
{
    // Raycast and visibility stuff
    public Transform playerCamera; 
    public float detectionRange = 100f; 
    public LayerMask visibilityMask; 
    public float fieldOfView = 60f; 
    public int rayCount = 30; 
    public GameObject mannequin;
    public MannequinController mannequinController = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mannequinController = mannequin.GetComponent<MannequinController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 origin = playerCamera.position;
        Vector3 forward = playerCamera.forward;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -fieldOfView / 2 + (fieldOfView / (rayCount - 1)) * i;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, detectionRange, visibilityMask))
            {
                Debug.DrawRay(origin, direction * hit.distance, Color.green); // Stop at hit
                if (hit.collider.CompareTag("Mannequin"))
                {
                    mannequinController.isSeen = true;
                    return;
                }
            }
            else
            {
                Debug.DrawRay(origin, direction * detectionRange, Color.red); // No hit, full length
            }

            mannequinController.isSeen = false; // if it makes it here no ray hit mannequin
        }
    }
}
