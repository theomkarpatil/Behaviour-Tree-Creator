// Developed by Sora
//
// Copyright(c) Sora Arts 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sora.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;

        private Vector3 moveDir;

        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
                moveDir.z = 1.0f;
            else if (Input.GetKey(KeyCode.S))
                moveDir.z = -1.0f;
            else 
                moveDir.z = 0.0f;

            if (Input.GetKey(KeyCode.A))
                moveDir.x = -1.0f;
            else if (Input.GetKey(KeyCode.D))
                moveDir.x = 1.0f;
            else
                moveDir.x = 0.0f;

            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
}