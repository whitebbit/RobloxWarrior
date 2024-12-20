using System;
using _3._Scripts.Inputs.Interfaces;
using _3._Scripts.Singleton;
using UnityEngine;
using YG;

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
                switch (YG2.envir.device)
                {
                    case YG2.Device.Desktop:
                        mobileInput.gameObject.SetActive(false);
                        return _desktopInput ??= new DesktopInput();
                    case YG2.Device.Mobile:
                        mobileInput.gameObject.SetActive(true);
                        UnityEngine.Input.multiTouchEnabled = true;

                        return mobileInput;
                    case YG2.Device.Tablet:
                        break;
                    case YG2.Device.TV:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return default;
            }
        }
    }
}