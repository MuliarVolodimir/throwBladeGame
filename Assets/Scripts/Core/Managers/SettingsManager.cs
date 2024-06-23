using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Slider _volumeSlider;
    [SerializeField] Button _muteButton;
    [SerializeField] AudioClip _applayClip;

    [SerializeField] Sprite _OnMuteSprite;
    [SerializeField] Sprite _NonMuteSprite;

    private ApplicationData _appData;

    private void Start()
    {
        _appData = ApplicationData.Instance;

        _muteButton.onClick.AddListener(ToggleMute);
        _volumeSlider.onValueChanged.AddListener(SetVolume);
        _volumeSlider.value = _appData.Volume;
        SwitchMuteSprite();
    }

    private void SetVolume(float volume)
    {
        if (!_appData.IsMute)
        {
            AudioManager.Instance.SetVolume(volume);
            _appData.Volume = volume;
        }
    }

    private void ToggleMute()
    {
        AudioManager.Instance.PlayOneShotSound(_applayClip);

        _appData.IsMute = !_appData.IsMute;

        AudioManager.Instance.Mute(_appData.IsMute);
        SwitchMuteSprite();
    }

    private void SwitchMuteSprite()
    {
        _muteButton.GetComponent<Image>().sprite = _appData.IsMute == true ? _OnMuteSprite : _NonMuteSprite;
    }
}

