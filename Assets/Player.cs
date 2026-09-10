using UnityEngine;

public class Player : MonoBehaviour
{
    private InputSystem_Actions _input;
    //矢印のデータ（動く方向）
    private Vector2 _playerPos;
    [SerializeField]private float _speed = 5;
    [SerializeField] private Transform _shotPos;
    [SerializeField] private GameObject _shotObj;


    private void OnEnable()
    {
        _input = new InputSystem_Actions();
        _input.Enable();

        _input.Player.Attack.started += context => 
        Instantiate(_shotObj,_shotPos.position,Quaternion.identity);
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        _playerPos = _input.Player.Move.ReadValue<Vector2>();
        transform.position += new Vector3(_playerPos.x,_playerPos.y,0) * _speed * Time.deltaTime;
    }
}
