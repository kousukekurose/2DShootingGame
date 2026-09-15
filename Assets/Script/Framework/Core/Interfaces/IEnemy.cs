
namespace Framework.Core.Interfaces
{
    public interface IEnemy : ICharacter,IMovable,IAttacker,IDamageable
    {
         ///<summary>
         /// 敵の種類
         ///</summary>
        EnemyType EnemyType {get; }
         ///<summary>
         ///現在のターゲット
         ///</summary>
        ITargetable CurrentTarget{get; }
         ///<summary>
         /// ターゲットを設定する
         ///</summary>
        void SetTarget(ITargetable target);
         ///<summary>
         /// AIを有効にする
         ///</summary>
        void EnableAI();
         ///<summary>
         /// AIを無効にする
         ///</summary>
        void DisableAI();
    }

　　　///<summary>
　　　/// 敵の種類
　　　///</summary>
    public enum EnemyType
    {
        Basic,
        Fast,
        Tank,
        Ranged,
        Boss
    }
}

