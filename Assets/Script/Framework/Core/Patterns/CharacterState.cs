using Framework.Core.Interfaces;
using Game.Shared.Character;
using UnityEngine;

namespace Framework.Core.Patterns
{
    /// <summary>
    /// キャラクターステートの基底クラス
    /// プレイヤー、敵などの全キャラクターステートが継承
    /// State Patternの実装
    /// </summary>
    public abstract class CharacterState
    {
        protected readonly ICharacter _character;
        protected readonly CharacterStateMachine _stateMachine;

        ///<summary>
        /// コンストラクタ
        /// </summary>
        protected CharacterState(ICharacter character, CharacterStateMachine stateMachine)
        {
            _character = character;
            _stateMachine = stateMachine;
        }

        ///<summary>
        /// ステートに入った時に呼ばれる
        /// 初期化処理、アニメーション開始など
        /// </summary>
        public virtual void Enter(){}

        ///<summary>
        /// 毎フレームっ呼ばれる更新処理
        /// ステートごとの振る舞いを実施
        /// </summary>
        /// <param name="deltaTime">前フレームからの経過時間</param>
        public virtual void Update(float deltaTime) {}

        ///<summary>
        /// ステートから出る時に呼ばれる
        /// クリーンアップ処理、アニメーション停止など
        /// </summary>
        public virtual void Exit(){}

        ///<summary>
        /// ダメージを受けた時に呼ばれる
        /// </summary>
        /// <param name="damage">受けたダメージ<param>
        public virtual void OnDamageReceived(float damage){}

        ///<summary>
        /// 死亡した時に呼ばれる
        /// </summary>
        public virtual void OnDeath(){}

        ///<summary>
        /// 指定した遷移するヘルパーメソッド
        /// </summary>
        protected void ChangeState<T>() where T : CharacterState
        {
            _stateMachine.CHangeState<T>();
        }

    }
}