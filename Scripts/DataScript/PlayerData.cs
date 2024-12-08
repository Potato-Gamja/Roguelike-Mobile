using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptble Object/PlayerData")]
//에셋메뉴를 만들어 무기 데이터를 관리

public class PlayerData : ScriptableObject
{
    public float baseHp;                //플레이어의 체력
    public float maxHp;                 
    public float hp;                    

    public float baseDefense;           //플레이어의 방어력
    public float defense;               

    public float baseOffense;           //플레이어의 공격력
    public float offense;               

    public float baseSpeed;             //플레이어의 이동속도
    public float speed;

    public float baseAttackDelay;       //플레이어의 공격속도
    public float attackDelay;

    public bool baseIsTrueDamage;       //플레이어의 고정피해 여부
    public bool isTrueDamage;

    public float baseCritical;          //플레이어의 치명타 확률
    public float critical;

    public float exp;                   //플레이어의 경험치
    public float[] exps;                //플레이어의 레벨 별 필요 경험치
    public Vector3 scale;               //플레이어의 크기
}
