using Photon.Pun;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Camera camera;
    public Transform cameraTransform;
    public Vector3 offset;
    public float smoothSpeed = 0.1f;
    
    
    public Animator mainAnim;
    public Animator leftArmAnim;
    public Animator rightArmAnim;

    public GameObject leftArm;
    public GameObject rightArm;
    // Start is called before the first frame update

    private float playerXSpd = 5.0f;
    private float playerYSpd = 2.5f;
    private Vector2 playerVel = Vector2.zero;

    private PhotonView _photonView;

    
    void Start()
    {
        _photonView = GetComponent<PhotonView>();

        if(!_photonView.IsMine)
            return;

        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTransform = camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_photonView.IsMine)
            return;
        
        if (Input.GetMouseButtonDown(1))
        {
            leftArmAnim.SetTrigger("LeftAttack");
        }
        if (Input.GetMouseButtonDown(0))
        {
            rightArmAnim.SetTrigger("RightAttack");
        }
        
        float xVel = 0.0f;
        float yVel = 0.0f;
        
        if (Input.GetKey("d"))
        {
            xVel += playerXSpd;
        }
        if (Input.GetKey("a"))
        {
            xVel -= playerXSpd;
        }
        if (Input.GetKey("w"))
        {
            yVel += playerYSpd;
        }
        if (Input.GetKey("s"))
        {
            yVel -= playerYSpd;
        }

        if ((transform.localScale.x > 0 && xVel < 0) || (transform.localScale.x < 0 && xVel > 0))
        {
            xVel *= 0.7f;
        }

        playerVel = new Vector2(xVel, yVel); 
        transform.Translate(playerVel * Time.deltaTime);
        mainAnim.SetFloat("moveMagnitude", playerVel.magnitude);
        mainAnim.SetFloat("moveXVel", xVel);

        Vector3 mousePos = Input.mousePosition;

        if (leftArmAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle_LeftArm")
            && rightArmAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle_RightArm"))
        {
            if (mousePos.x > camera.WorldToScreenPoint(transform.position).x)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
        }
    }
    
    private void LateUpdate()
    {
        if (!_photonView.IsMine)
            return;

        SmoothFollow();
    }
    public void SmoothFollow()
    {
        Vector3 targetPos = transform.position + offset;
        Vector3 smoothFollow = Vector3.Lerp(cameraTransform.position, targetPos, smoothSpeed);
        cameraTransform.position = smoothFollow;
    }
}
