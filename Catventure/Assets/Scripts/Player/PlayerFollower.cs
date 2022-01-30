using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{

    void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = GameObject.Find("Player(Clone)").transform;
    }
}
