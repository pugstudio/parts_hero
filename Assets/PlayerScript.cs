using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Camera camera;
    public Transform cameraTransform;
    public Vector3 offset;
    public float smoothSpeed = 0.1f;
    
    
    private Animator _mainAnim;
    private Animator _leftArmAnim;
    private Animator _rightArmAnim;

    private GameObject _leftArm;
    private GameObject _rightArm;
    // Start is called before the first frame update

    private float playerXSpd = 5.0f;
    private float playerYSpd = 2.5f;
    private Vector2 playerVel = Vector2.zero;

    
    PhotonView m_PhotonView;

    void Start()
    {
        _leftArm = GameObject.Find("Bone_LeftArm");
        _rightArm = GameObject.Find("Bone_RightArm");
        _mainAnim = GameObject.Find("Bone_Body").GetComponent<Animator>();
        _leftArmAnim = _leftArm.GetComponent<Animator>();
        _rightArmAnim = _rightArm.GetComponent<Animator>();
        
        m_PhotonView = GetComponent<PhotonView>();
        
        if (!m_PhotonView.isMine)
            return;
        
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        cameraTransform = camera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_PhotonView.isMine)
            return;
        
        if (Input.GetMouseButtonDown(1))
        {
            _leftArmAnim.SetTrigger("LeftAttack");
        }
        if (Input.GetMouseButtonDown(0))
        {
            _rightArmAnim.SetTrigger("RightAttack");
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
        _mainAnim.SetFloat("moveMagnitude", playerVel.magnitude);
        _mainAnim.SetFloat("moveXVel", xVel);

        Vector3 mousePos = Input.mousePosition;

        if (_leftArmAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle_LeftArm")
            && _rightArmAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle_RightArm"))
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
        if (!m_PhotonView.isMine)
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
