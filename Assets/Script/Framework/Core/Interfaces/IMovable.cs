using UnityEngine;

namespace Framework.Core.Interfaces
{
    /// <summary>
    /// 移動可能なオブジェクトのインターフェース
    /// プレイヤー、敵、移動する障害物などが実装
    /// </summary>
    public interface IMovable
    {
        /// <summary>
        /// 指定された方向に移動する
        /// </summary>
        /// <param name="direction">移動方向(正規化されていること)</param>
        void Move(Vector3 direction,float deltaTime);

        ///<summary>
        /// 現在の位置を取得する
        ///</summary>
        /// <returns>現在のワールド座標</returns>
        Vector3 GetCurrentPosition();

        /// <summary>
        /// 移動速度を設定する
        /// </summary>
        /// <param name="speed">新しい移動速度</param>
        void SetMoveSpeed(float speed);
    }
}
