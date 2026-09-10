using UnityEngine;

namespace Framework.Core.Interfaces
{
    /// <summary>
    /// 全キャラクターの基本インターフェース
    /// </summary>
    public interface ICharacter
    {
        ///<summary>
        /// キャラクターの一意識別子
        /// システム内でキャラクターを識別するためのID
        /// </summary>
        string CharacterId{get;}

        ///<summary>
        /// キャラクターステータス情報(Hp,移動速度、攻撃力など)
        /// </summary>
        Game.Shared.Character.CharacterStats Stats{get;}

        ///<summary>
        /// キャラクターがアクティブかどうか
        /// 非アクティブの場合が更新や描画をスキップ
        /// </summary>
        bool IsActive{get;}

        ///<summary>
        /// キャラクターをアクティブ化する
        /// ゲーム開始時やリスポーン時に呼び出す
        /// </summary>
        void Activate();

        ///<summary>
        /// キャラクターを非アクティブにする
        /// 死亡時や一時的な非表示時に呼び出す
        /// </summary>
        void Deactivate();
    }
}

