using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySetting : MonoBehaviour
{
    public enum UserAction
    {
        MoveForward,
        MoveBackward,
        MoveLeft,
        MoveRight,

        Attack,
        Jump,

        // UI
        UI_Inventory,
        UI_Pause,
    }

    //바인딩 클래스
    [Serializable]
    public class InputBinding
    {
        public Dictionary<UserAction, KeyCode> Bindings => _bindingDict;
        private Dictionary<UserAction, KeyCode> _bindingDict;

        // 생성자
        public InputBinding(bool initalize = true)
        {
            _bindingDict = new Dictionary<UserAction, KeyCode>();

            if (initalize)
            {
                ResetAll();
            }
        }

        // 새로운 바인딩 적용
        public void ApplyNewBindings(InputBinding newBinding)
        {
            _bindingDict = new Dictionary<UserAction, KeyCode>(newBinding._bindingDict);
        }

        // 바인딩 지정 메소드 : allowOverlap 매개변수를 통해 중복 바인딩 허용여부를 결정한다.
        public void Bind(in UserAction action, in KeyCode code, bool allowOverlap = false)
        {
            if (!allowOverlap && _bindingDict.ContainsValue(code))
            {
                var copy = new Dictionary<UserAction, KeyCode>(_bindingDict);

                foreach (var pair in copy)
                {
                    if (pair.Value.Equals(code))
                    {
                        _bindingDict[pair.Key] = KeyCode.None;
                    }
                }
            }
            _bindingDict[action] = code;
        }

        // 초기 바인딩셋 지정 메소드
        public void ResetAll()
        {
            Bind(UserAction.Attack, KeyCode.Mouse0);

            Bind(UserAction.MoveForward, KeyCode.W);
            Bind(UserAction.MoveBackward, KeyCode.S);
            Bind(UserAction.MoveLeft, KeyCode.A);
            Bind(UserAction.MoveRight, KeyCode.D);

            Bind(UserAction.Jump, KeyCode.Space);

            Bind(UserAction.UI_Inventory, KeyCode.F);
            Bind(UserAction.UI_Pause, KeyCode.Escape);
        }
    }

    //직렬화 가능한 클래스
    [Serializable]
    public class SerializableInputBinding
    {
        public BindPair[] bindPairs;
        //생성자
        public SerializableInputBinding(InputBinding binding)
        {
            int len = binding.Bindings.Count;  //binding의 Bindings딕셔너리에 접근, 딕셔너리가 포함된 ICollection 기본 메서드인 Count를 통해 갯수를 셈
            int index = 0;

            bindPairs = new BindPair[len];

            foreach (var pair in binding.Bindings)
            {
                bindPairs[index++] =
                    new BindPair(pair.Key, pair.Value);
            }
        }
    }

    [Serializable]
    public class BindPair
    {
        public UserAction key;
        public KeyCode value;

        public BindPair(UserAction key, KeyCode value)
        {
            this.key = key;
            this.value = value;
        }
    }
}
