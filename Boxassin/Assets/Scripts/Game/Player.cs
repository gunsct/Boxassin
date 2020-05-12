using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody m_Rig;
    public float m_MaxSpd, m_MaxAngleSpd;
    public float m_AngleSpd;
    // Start is called before the first frame update
    void Start()
    {
        m_Rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput() {
        if (Input.GetKey(KeyCode.W)) {
            if (m_Rig.velocity != transform.forward * m_MaxSpd)
                m_Rig.velocity += transform.forward * 4f * Time.deltaTime;
        }
        else {
            if (Input.GetKey(KeyCode.S)) {
                if (m_Rig.velocity != -transform.forward * m_MaxSpd)
                    m_Rig.velocity += -transform.forward * 4f * Time.deltaTime;
            }
            else {
                if (m_Rig.velocity != Vector3.zero)
                    m_Rig.velocity = Vector3.Slerp(m_Rig.velocity, Vector3.zero, 0.05f);
            }
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.eulerAngles += new Vector3(0f, m_AngleSpd * Time.deltaTime, 0f);
        }
        else if (Input.GetKey(KeyCode.A)) {
            transform.eulerAngles += new Vector3(0f, -m_AngleSpd * Time.deltaTime, 0f);
        }
    }
}
