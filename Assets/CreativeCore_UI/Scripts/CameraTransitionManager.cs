using UnityEngine;
using System.Collections;
using UnityTemplateProjects; 

public class CameraTransitionManager : MonoBehaviour
{
    [Header("Camera & Targets")]
    public Transform mainCamera;
    public Transform settingsViewTarget;

    [Header("Settings")]
    public float transitionDuration = 1.0f;
    public AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private SimpleCameraController cameraController;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main.transform;

        cameraController = mainCamera.GetComponent<SimpleCameraController>();

        defaultPosition = mainCamera.position;
        defaultRotation = mainCamera.rotation;
    }

    public void GoToSettings()
    {
        StopAllCoroutines();

        if (cameraController != null)
            cameraController.enabled = false;

        StartCoroutine(MoveCamera(settingsViewTarget.position, settingsViewTarget.rotation));
    }

    public void GoToMain()
    {
        StopAllCoroutines();
        StartCoroutine(MoveCameraBack());
    }

    private IEnumerator MoveCamera(Vector3 targetPosition, Quaternion targetRotation)
    {
        Vector3 startPos = mainCamera.position;
        Quaternion startRot = mainCamera.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            float curveValue = transitionCurve.Evaluate(t);

            mainCamera.position = Vector3.Lerp(startPos, targetPosition, curveValue);
            mainCamera.rotation = Quaternion.Lerp(startRot, targetRotation, curveValue);

            yield return null;
        }

        mainCamera.position = targetPosition;
        mainCamera.rotation = targetRotation;
    }

    private IEnumerator MoveCameraBack()
    {
        yield return StartCoroutine(MoveCamera(defaultPosition, defaultRotation));

        if (cameraController != null)
            cameraController.enabled = true;
    }
}