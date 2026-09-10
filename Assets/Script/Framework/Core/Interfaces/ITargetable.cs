using UnityEngine;

namespace Framework.Core.Interfaces
{
    ///<summary>
    /// ターゲットとして認識されるオブジェクトのインターフェース
    /// 敵、攻撃可能な障害物など
    /// AIのターゲット選択や攻撃判定
    /// </summary>
    public interface ITargetable
    {
        ///<summary>
        /// ダーゲットの位置情報
        /// </summary>
        Vector3 Position{get;}

        ///<summary>
        /// 有効可能なターゲットか
        /// 死亡している、非アクティブ、無敵状態などfalse
        /// </summary>
        bool IsValidTarget{get;}
    }
}