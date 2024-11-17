using System;
using System.Collections;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    private const float MaxTimer = 3f;
    private float timer = MaxTimer;

    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            }
            return canvasGroup;
        }
    }

    private RectTransform rect = null;
    protected RectTransform Rect
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>() ?? gameObject.AddComponent<RectTransform>();
            }
            return rect;
        }
    }

    public Transform Target { get; protected set; } = null;
    private Transform player = null;
    private Action unRegister = null;

    private IEnumerator IE_Countdown = null;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;

    public void Register(Transform target, Transform player, Action unRegister)
    {
        this.Target = target;
        this.player = player;
        this.unRegister = unRegister;

        StartCoroutine(RotateToTheTarget());
        StartTimer();
    }

    public void Restart()
    {
        timer = MaxTimer;
        StartTimer();
    }

    private void StartTimer()
    {
        if (IE_Countdown != null) { StopCoroutine(IE_Countdown); }
        IE_Countdown = CountDown();
        StartCoroutine(IE_Countdown);
    }

    private IEnumerator RotateToTheTarget()
    {
        while (enabled)
        {
            if (Target)
            {
                tPos = Target.position;
                tRot = Target.rotation;
            }

            Vector3 direction = player.position - tPos;

            tRot = Quaternion.LookRotation(direction);
            tRot.z = tRot.y;
            tRot.x = 0;
            tRot.y = 0;

            Vector3 northDirection = new Vector3(0, 0, player.eulerAngles.y);
            Rect.localRotation = tRot * Quaternion.Euler(northDirection);

            yield return null;
        }
    }

    private IEnumerator CountDown()
    {
        while (CanvasGroup.alpha < 1f)
        {
            CanvasGroup.alpha += 4f * Time.deltaTime;
            yield return null;
        }
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        while (CanvasGroup.alpha > 0f)
        {
            CanvasGroup.alpha -= 2f * Time.deltaTime;
            yield return null;
        }
        unRegister?.Invoke();
        Destroy(gameObject);
    }
}
