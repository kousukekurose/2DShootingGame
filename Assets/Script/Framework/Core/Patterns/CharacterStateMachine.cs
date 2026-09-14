using System.Collections.Generic;
using System;
using Framework.Core.Interfaces;

namespace Framework.Core.Patterns
{
    /// <summary>
    /// キャラクターステートマシン
    /// ステートの登録、遷移、更新を管理
    /// プレイヤー、敵など全キャラクターで使用
    /// </summary>
    public class CharacterStateMachine
    {
        private CharacterState _currentState;
        private readonly Dictionary<Type,CharacterState> _states;
        private readonly ICharacter _character;

        ///<summary>
        /// 現在のステート
        /// </summary>
        public CharacterState CurrentState => _currentState;

        ///<summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="character">このステートマシンを持つキャラクター</param>
        public CharacterStateMachine(ICharacter character)
        {
            _character = character;
            _states = new Dictionary<Type, CharacterState>();
        }

        ///<summary>
        /// ステートを登録する
        /// </summary>
        public void RegisterState<T>(T state) where T : CharacterState
        {
            var stateType = typeof(T);
            if(_states.ContainsKey(stateType))
            {
                CustomLogger.LogError($"State{stateType.Name} is not registered");
                return;
            }
            _states[stateType] = state;
        }

        ///<summary>
        /// 指定したステートに遷移する
        /// </summary>
        public void ChangeState<T>() where T : CharacterState
        {
            var newStateType = typeof(T);

            if(!_states.ContainsKey(newStateType))
            {
                CustomLogger.LogError($"State{newStateType.Name} is not registered");
                return;
            }

            var newState = _states[newStateType];

            if(_currentState == newState)
            {
                return;
            }

            //現在のステートのExitを呼ぶ
            _currentState?.Exit();
            //新しいステートに遷移
            _currentState = newState;

            //新しいステートのEnterを呼ぶ
            _currentState?.Enter();

            CustomLogger.Log($"State change to {newStateType.Name}");
        }

        ///<summary>
        /// 現在のステートを更新する
        /// </summary>
        ///<param name="deltaTime">前フレームからの経過時間</param>
        public void Update(float deltaTime)
        {
            _currentState?.Update(deltaTime);
        }

        ///<summary>
        /// 現在のステートが指定した型かどうか確認
        /// </summary>
        public bool IsCurrentState<T>() where T :CharacterState
        {
            return _currentState is T;
        }

        ///<summary>
        /// 現在のステートを指定した型として取得
        /// </summary>
        public T GetCurrentState<T>() where T :CharacterState
        {
            return _currentState as T;
        }

        ///<summary>
        /// 登録されているステート数を取得
        /// </summary>
        public int RegisteredStateCount => _states.Count;
    }
}