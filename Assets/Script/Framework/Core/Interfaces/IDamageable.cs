using UnityEngine;

namespace Framework.Core.Interfaces
{
    /// <summary>
    /// ダメージを受ける機能のインターフェース
    /// プレイヤー、敵、破壊可能オブジェクトなど
    /// </summary>
    public interface IDamageable : ICharacter
    {
        ///<summary>
        /// 現在のHP
        /// </summary>
        float CurrentHP{get;}

        ///<summary>
        /// 最大HP
        /// </summary>
        float MaxHP {get;}

        ///<summary>
        /// 死亡しているかどうか
        /// HPが０以下の場合はtrue
        /// </summary>
        bool IsDead{get;}

        ///<summary>
        /// 無敵状態かどうか
        /// 無敵中はダメージを受けない
        /// </summary>
        bool IsInvinvible{get;}

        ///<summary>
        /// ダメージを受ける
        /// </summary>
        /// <param name="damage">ダメージ量</param>
        /// <param name="source">ダメージ源</param>
        void TakeDamage(float damage,DamageSource source);

        ///<summary>
        /// 回復する
        /// </summary>
        /// <param name="amout">回復量</param>
        void Heal(float amout);

        ///<summary>
        /// 無敵状態を設定
        /// </summary>
        /// <param name="duration">無敵時間</param>
        void SetInvincible(float duration);
    }

    /// <summary>
    /// ダメージ源の種類
    /// </summary>
    public enum DamageSource
    {
        /// <summary>プレイヤーからのダメージ</summary>
        Player,
        /// <summary>敵からのダメージ</summary>
        Enemy,
        /// <summary>環境(落下、トラップなど)からのダメージ</summary>
        Enviromental,
        /// <summary>トラップからのダメージ</summary>
        Trap
    }
}
