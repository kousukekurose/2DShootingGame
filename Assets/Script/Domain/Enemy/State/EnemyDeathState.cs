namespace Project.Domain.Enemy.State
{
    public class EnemyDeathState : EnemyState
    {
        public EnemyDeathState(
            Enemy enemy,
            Project.Domain.Character.CharacterStateMachine stateMachine
        ):base(enemy ,stateMachine){}

        public override void Enter()
        {
            _enemy.Deactivate();
            _enemy.DisableAI();
        }
    }
}



