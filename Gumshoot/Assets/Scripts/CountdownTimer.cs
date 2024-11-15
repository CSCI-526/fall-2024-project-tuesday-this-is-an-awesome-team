using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] private Image uiFill;
    [SerializeField] private TextMeshProUGUI uiText;

    private float duration;
    private float remainingDuration;

    private Transform target;
    private Vector3 offset;

    public void Init(Transform targetTransform, float durationSeconds)
    {
        target = targetTransform;
        offset = new Vector3(-1f, 0.5f, 0);
        duration = durationSeconds;
        remainingDuration = durationSeconds;

        uiText.gameObject.SetActive(false);
        StartCoroutine(UpdateTimer());
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Begin(int Second)
    {
        remainingDuration = Second;
        StartCoroutine(UpdateTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingDuration > 0)
        {
            if (remainingDuration <= 6f)
            {
                //if (!uiText.gameObject.activeSelf)
                //{
                //    uiText.gameObject.SetActive(true);
                //}

                if (remainingDuration > 3f)
                {
                    uiText.color = new Color(1.0f, 0.5f, 0.0f, 1.0f); // orange
                    uiFill.color = new Color(1.0f, 0.5f, 0.0f, 1.0f);
                }
                else
                {
                    uiText.color = new Color(1.0f, 0.3f, 0.3f, 1.0f); // soft red
                    uiFill.color = new Color(1.0f, 0.3f, 0.3f, 1.0f);
                }
            }

            uiText.text = Mathf.CeilToInt(remainingDuration).ToString();
            uiFill.fillAmount = remainingDuration / duration;

            yield return new WaitForSeconds(1f);

            remainingDuration -= 1f;
        }

        uiText.text = "0";
        uiFill.fillAmount = 0f;

        Destroy(gameObject);
    }

}
