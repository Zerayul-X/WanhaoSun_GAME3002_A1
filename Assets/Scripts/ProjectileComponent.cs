using UnityEngine.Assertions;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using TMPro;

//[RequireComponent(typeof(Rigidbody))]
public class ProjectileComponent : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI t_Score = null;

    [SerializeField]
    private TextMeshProUGUI t_Shots = null;

    [SerializeField]
    private Vector3 m_vInitialVelocity = Vector3.zero;

    private Rigidbody m_rb = null;

    private GameObject m_landingDisplay = null;

    private bool m_bIsGrounded = true;

    public int shots = 0;

    public int score = 0;

    private bool m_bGoal = false;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        Assert.IsNotNull(m_rb, "Houston, we've got a problem! Rigidbody is not attached!");

        CreateLandingDisplay();
    }

    private void Update()
    {
        UpdateScore();
        UpdateLandingPosition();
    }

    private void CreateLandingDisplay()
    {
        m_landingDisplay = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        m_landingDisplay.transform.position = Vector3.zero;
        m_landingDisplay.transform.localScale = new Vector3(1f, 0.1f, 1f);

        m_landingDisplay.GetComponent<Renderer>().material.color = Color.yellow;
        m_landingDisplay.GetComponent<Collider>().enabled = false;
    }

    private void UpdateLandingPosition()
    {
        if (m_landingDisplay != null && m_bIsGrounded)
        {
            m_landingDisplay.transform.position = GetLandingPosition();
        }
    }

    private Vector3 GetLandingPosition()
    {
        float fTime = 2f * (0f - m_vInitialVelocity.y / Physics.gravity.y);

        Vector3 vFlatVel = m_vInitialVelocity;
        vFlatVel.y = 0f;
        vFlatVel *= fTime;

        return transform.position + vFlatVel;
    }

    #region CALLBACKS
    public void OnLaunchProjectile()
    {
        if (!m_bIsGrounded)
        {
            return;
        }
        shots += 1;

        ShotTextUpdate();

        m_landingDisplay.transform.position = GetLandingPosition();
        m_bIsGrounded = false;

        transform.LookAt(m_landingDisplay.transform.position, Vector3.up);

        m_rb.velocity = m_vInitialVelocity;
    }

    public void OnMoveForward(float fDelta)
    {
        if (m_vInitialVelocity.z <= 20.0f)
        {

            m_vInitialVelocity.z += fDelta;
        }
    }

    public void OnMoveBackward(float fDelta)
    {
        if (m_vInitialVelocity.z >= 10.0f)
        {
            m_vInitialVelocity.z -= fDelta;
        }
    }

    public void OnMoveRight(float fDelta)
    {
        if (m_vInitialVelocity.x <= 10.0f)
        {
            m_vInitialVelocity.x += fDelta;
        }
    }

    public void OnMoveLeft(float fDelta)
    {
        if (m_vInitialVelocity.x >= -10.0f)
        {
            m_vInitialVelocity.x -= fDelta;
        }
    }

    public void ResetPosition()
    {
        m_bGoal = false;
        m_bIsGrounded = true;
        Vector3 dftPosition = Vector3.zero;
        dftPosition.z -= 10.0f;
        m_rb.velocity = Vector3.zero;

        m_rb.angularVelocity = Vector3.zero;
        m_rb.position = dftPosition;

        m_landingDisplay.transform.position = dftPosition;
        m_landingDisplay.transform.localScale = new Vector3(1f, 0.1f, 1f);
    }
    public void UpdateScore()
    {
        if (!m_bGoal)
        {
            if (m_rb.position.x >= -3.6f && m_rb.position.x <= 3.6f)
            {
                if (m_rb.position.z >= 6.4f && m_rb.position.z <= 6.6f)
                {
                    score++;
                    ScoreTextUpdate();
                    m_bGoal = true;
                }
            }
        }
    }

    public void ScoreTextUpdate()
    {
        t_Score.text = "Score: " + score;
    }
    public void ShotTextUpdate()
    {
        t_Shots.text = "Shots: " + shots;
    }
    #endregion
}
