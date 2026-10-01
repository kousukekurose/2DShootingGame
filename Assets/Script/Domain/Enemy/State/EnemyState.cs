namespace Project.Domain.Enemy.State
{
    public class EnemyState : Project.Domain.Character.CharacterState
    {
        protected readonly Enemy _enemy;

        protected EnemyState(
            Enemy enemy,
            Project.Domain.Character.CharacterStateMachine stateMachine
        ):base(enemy,stateMachine)
        {
            _enemy = enemy;
        }
    }
}
