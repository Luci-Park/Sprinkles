using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoopJoystickUISet : MonoBehaviour
{
    [SerializeField] Sprite mintJoystick;
    [SerializeField] Sprite strawberryJoystick;
    [SerializeField] Image joystickImage;

    public void SetUI()
    {
        switch (Player.localPlayerInstance.GetComponent<Player>().myTeam)
        {
            case Team.strawberry:
                joystickImage.sprite = strawberryJoystick;
                break;
            case Team.mint:
                joystickImage.sprite = mintJoystick;
                break;
        }
    }
}
