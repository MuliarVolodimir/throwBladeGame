using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Image _loadImage;
    [SerializeField] float _loadDuration;

    public event Action OnLoad;
    private void Start()
    {
        StartCoroutine(Load());
    }

    private IEnumerator Load()
    {
        float elapsedTime = 0f;

        while (elapsedTime <= _loadDuration)
        {
            elapsedTime += Time.deltaTime;
            _loadImage.fillAmount = Mathf.Clamp01(elapsedTime / _loadDuration);
            yield return null;
        }

        OnLoad?.Invoke();
        this.gameObject.SetActive(false);
    }
}

