using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerScript : MonoBehaviour
{
    private Animator _mainAnim;
    private Animator _leftArmAnim;
    private Animator _rightArmAnim;

    private GameObject _leftArm;
    private GameObject _rightArm;
    // Start is called before the first frame update

    private float playerXSpd = 5.0f;
    private float playerYSpd = 2.5f;
    private Vector2 playerVel = Vector2.zero;
    void Start()
    {
        _leftArm = GameObject.Find("Bone_LeftArm");
        _rightArm = GameObject.Find("Bone_RightArm");
        _mainAnim = GameObject.Find("Bone_Body").GetComponent<Animator>();
        _leftArmAnim = _leftArm.GetComponent<Animator>();
        _rightArmAnim = _rightArm.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
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
            xVel -= playerXSpd * 0.7f;
        }
        if (Input.GetKey("w"))
        {
            yVel += playerYSpd;
        }
        if (Input.GetKey("s"))
        {
            yVel -= playerYSpd;
        }

        playerVel = new Vector2(xVel, yVel); 
        transform.Translate(playerVel * Time.deltaTime);
        _mainAnim.SetFloat("moveMagnitude", playerVel.magnitude);
        _mainAnim.SetFloat("moveXVel", xVel);

        /*if (_mainAnim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            if (xVel < 0)
            {
                _mainAnim.Play("BackRun");
            }
        }else if (_mainAnim.GetCurrentAnimatorStateInfo(0).IsName("BackRun"))
        {
            if (xVel > 0)
            {
                _mainAnim.Play("Run");
            }
        }*/
    }
}
