using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera aimVirtualCamera;

    ThirdPersonController _thirdPersonController;

    StarterAssetsInputs _starterAssetsInputs;

    [SerializeField] GameObject aimPanel;

    [SerializeField] float normalSensitivity = 1.0f;

    [SerializeField] float aimSensitivity = 0.5f;

    private void Awake()
    {
        _thirdPersonController = GetComponent<ThirdPersonController>();

        _starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        if (_starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);

            _thirdPersonController.SetSensitivity(aimSensitivity);

            _thirdPersonController.SetRotateOnMove(false);

            aimPanel.SetActive(true);

            Vector3 mouseWorldPosition = Vector3.zero;

            Vector2 screenCenterPoint = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);

            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000.0f))
            {
                mouseWorldPosition = raycastHit.point;
            }

            Vector3 worldAimTarget = mouseWorldPosition;

            worldAimTarget.y = transform.position.y;

            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 15.0f); //aimDirection;
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);

            _thirdPersonController.SetSensitivity(normalSensitivity);

            _thirdPersonController.SetRotateOnMove(true);

            aimPanel.SetActive(false);
        }
    }
}