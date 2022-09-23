using UnityEngine;
using UnityEngine.UI;

public class SplashSetter : MonoBehaviour
{
    [SerializeField] Image splash1;
    [SerializeField] Image splash2;
    [SerializeField] Image splash3;
    [Tooltip("strawberry, mint")]
    [SerializeField] Sprite [] splash1s;
    [SerializeField] Sprite [] splash2s;
    [SerializeField] Sprite [] splash3s;

    public static SplashSetter splasher;

    //---------------------------------------------

    private void Awake()
    {
        splasher = this;
    }

    //---------------------------------------------

    public void SetSplash(Team team)
    {
        int teamnumber = (int)team;
        splash1.sprite = splash1s[teamnumber];
        splash2.sprite = splash2s[teamnumber];
        splash3.sprite = splash3s[teamnumber];
    }

    //---------------------------------------------
}
