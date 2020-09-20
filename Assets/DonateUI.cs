using UnityEngine;

public class DonateUI : MonoBehaviour
{
    public void HideDonatePanel()
    {
        AudioManager.instance.Play("ClickUI");
        GetComponent<MovementUI>().MoveToStart();
    }
}
