using UnityEngine;

namespace Framework.Core.Interfaces
{
    /// <summary>
    /// 攻撃を行う機能のインターフェース
    /// プレイヤー、敵など
    /// </summary>
    public  interface IAttacker : ICharacter
    {
        ///<summary>
        /// 攻撃力
        /// 基本ダメージ量
        /// </summary>
        float AttackPower{get;}

        ///<summary>
        /// 攻撃範囲
        /// 近接攻撃の場合は半径、遠距離攻撃の場合は射程
        /// </summary>
        float AttackRange{get;}

        ///<summary>
        /// 攻撃クールダウン
        /// 連続攻撃間隔
        /// </summary>
        float AttackCooldown{get;}

        ///<summary>
        /// 攻撃可能かどうか
        /// クールダウン中やステートによって攻撃できない場合がある
        /// </summary>
        bool CanAttack{get;}

        ///<summary>
        /// 指定した位置に攻撃を行う
        /// </summary>
        ///<param name="targetPosition">攻撃対象の位置</param>
        void Attack(Vector3 targetPosition, Domain.Bullet.BulletType bulletType);

        ///<summary>
        /// 攻撃ターゲットを設定する
        /// AIや自動攻撃で仕様
        /// </summary>
        ///<param name="target">ターゲットオブジェクト</param>
        void SetAttackTarget(ITargetable target);
    }

}
