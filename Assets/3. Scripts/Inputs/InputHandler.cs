using System;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Singleton;
using GBGamesPlugin;
using UnityEngine;

namespace _3._Scripts.Inputs
{
    public class InputHandler : Singleton<InputHandler>
    {
        [SerializeField] private MobileInput mobileInput;
        private DesktopInput _desktopInput;

        public IInput Input
        {
            get
            {
                switch (GBGames.deviceType)
                {
                    case "mobile":
                        if(!mobileInput.gameObject.activeSelf)
                            mobileInput.gameObject.SetActive(true);
                        UnityEngine.Input.multiTouchEnabled = true;
                        return mobileInput;
                    case "desktop":
                        if(mobileInput.gameObject.activeSelf)
                            mobileInput.gameObject.SetActive(false);
                        return _desktopInput ??= new DesktopInput();
                    default: 
                        return default;
                }
            }
        }

        public void SetState(bool state)
        {
            switch (GBGames.deviceType)
            {
                case "mobile":
                    mobileInput.SetState(state);
                    break;
                case "desktop":
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}