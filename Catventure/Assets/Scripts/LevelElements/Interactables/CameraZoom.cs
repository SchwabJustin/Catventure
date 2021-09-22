using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    bool zoomedIn;
    public float zoomedOrthographicSize = 10;
    float normalOrthographicSize;
    void Awake()
    {
        normalOrthographicSize = vCam.m_Lens.OrthographicSize;

    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (!zoomedIn)
            {
                zoomedIn = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (zoomedIn)
            {
                zoomedIn = false;
            }
        }
    }

    void Update()
    {
        if (zoomedIn && vCam.m_Lens.OrthographicSize != zoomedOrthographicSize)
        {
            vCam.m_Lens.OrthographicSize = vCam.m_Lens.OrthographicSize < zoomedOrthographicSize ? vCam.m_Lens.OrthographicSize + Mathf.Lerp(0, zoomedOrthographicSize, Time.deltaTime * 2) : zoomedOrthographicSize;
        }
        else if (!zoomedIn && vCam.m_Lens.OrthographicSize != normalOrthographicSize)
        {
            vCam.m_Lens.OrthographicSize = vCam.m_Lens.OrthographicSize < normalOrthographicSize ? vCam.m_Lens.OrthographicSize + Mathf.Lerp(0, normalOrthographicSize, Time.deltaTime * 2) : normalOrthographicSize;

        }
    }

}
