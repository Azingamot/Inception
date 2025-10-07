using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerItemInHand : MonoBehaviour
{
    [SerializeField] private SpriteRenderer itemSprite;
    [SerializeField] private Animator itemAnimator;
    [SerializeField] private float returnSpeed;
    private Coroutine baseRotation;
    private readonly Dictionary<string, float> directionValues = new Dictionary<string, float>()
    {
        {"Up", 180f },
        {"Down", 0f },
        {"Right", 90f },
        {"Left", 270f },
        {"DownRight", 45f },
        {"UpRight", 135f },
        {"UpLeft",  225f},
        {"DownLeft", 315f}
    };

    private void Awake()
    {
        itemSprite.sprite = null;
    }

    public void ChangeItemSprite(Sprite newSprite)
    {
        itemSprite.sprite = newSprite;
    }

    public void SetItemAnimatorController(AnimatorController controller)
    {
        itemAnimator.runtimeAnimatorController = controller;
    }

    public void TriggerAnimation(string trigger)
    {
        itemAnimator.SetTrigger(trigger);
    }
    public void SetRotation(Vector3 mousePos)
    {
        if (baseRotation != null) 
            StopCoroutine(baseRotation);
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        string directionName = GetDirectionName(mouseWorldPos);
        ApplyDirection(directionName);
    }

    private string GetDirectionName(Vector3 mouseWorldPos)
    {
        Vector2 direction = (mouseWorldPos - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;

        if (angle < 0) angle += 360f;

        string closestDirection = "Down";
        float smallestDifference = 360f;

        foreach (KeyValuePair<string, float> pair in directionValues)
        {
            float diff = Mathf.Abs(Mathf.DeltaAngle(angle, pair.Value));
            if (diff < smallestDifference)
            {
                smallestDifference = diff;
                closestDirection = pair.Key;
            }
        }

        return closestDirection;
    }

    private void ApplyDirection(string directionName)
    {
        float targetAngle = directionValues[directionName];
        transform.rotation = Quaternion.Euler(0f, 0f, targetAngle);
    }

    public void ResetRotation()
    {
        baseRotation = StartCoroutine(RotateToBase());
    }

    private IEnumerator RotateToBase()
    {
        while (true)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, returnSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, Quaternion.identity) < 0.1f)
            {
                transform.rotation = Quaternion.identity;
                break;
            }
            yield return null;
        }
    }
}