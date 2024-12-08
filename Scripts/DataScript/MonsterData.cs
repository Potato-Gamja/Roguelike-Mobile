using UnityEngine;

[CreateAssetMenu(fileName = "MonData", menuName = "Scriptble Object/MonData")]
//에셋메뉴를 만들어 무기 데이터를 관리

public class MonsterData : ScriptableObject
{
    public float hp;                    //몬스터의 체력
    
    public float waveHp;                //몬스터의 웨이브 추가 체력
    
    public float defense;               //몬스터의 방어력
    
    public int damage;                  //몬스터의 데미지
    
    public float offense;               //몬스터의 공격력
    
    public float speed;                 //몬스터의 이동속도
    
    public float knockbackDefense;      //몬스터의 넉백 저항
}
