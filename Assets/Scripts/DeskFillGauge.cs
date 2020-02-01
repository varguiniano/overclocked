using UnityEngine;
using UnityEngine.UI;

public class DeskFillGauge : MonoBehaviour
{
    public Image Background;
    public Image GaugeImage;

    public void Disable()
    {
        Background.enabled = false;
        GaugeImage.enabled = false;
    }

    public void SetValue(int health)
    {
        Background.enabled = true;
        GaugeImage.enabled = true;
        GaugeImage.fillAmount = (float) health / 100f;
    }
}