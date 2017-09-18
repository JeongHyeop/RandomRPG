using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour {

    public eCharacterType characterType {set; get;}

    public void SkillStart(eCharacterType _type, eSkillState _state,eWeaponType _weaponType, GameObject _characterObj)
    {
        GameObject weapon = CGame.Instance.GameObject_get_child(_characterObj, "Dummy Prop Right");
        GameObject fx = CGame.Instance.PlayFx((int)_weaponType, weapon.transform.position);
        fx.transform.position = weapon.transform.position;

        switch (_state)
        {
            case eSkillState.eSkillState_Hit:
                break;
            case eSkillState.eSkillState_Start:
                fx.AddComponent<Skill>().characterType = _type; 
                break;         
        }
    }

}
public enum eSkillState
{
    eSkillState_Hit = 0,
    eSkillState_Start = 1,    
}