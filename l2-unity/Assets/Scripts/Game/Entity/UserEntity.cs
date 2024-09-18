using UnityEngine;

[RequireComponent(typeof(NetworkAnimationController)),
    RequireComponent(typeof(NetworkTransformReceive)),
    RequireComponent(typeof(NetworkCharacterControllerReceive))]

public class UserEntity : NetworkEntity
{
    private CharacterAnimationAudioHandler _characterAnimationAudioHandler;

    public override void Initialize()
    {
        base.Initialize();
        _characterAnimationAudioHandler = transform.GetChild(0).GetComponentInChildren<CharacterAnimationAudioHandler>();

        EquipAllArmors();

        EntityLoaded = true;
    }

    public void EquipAllArmors()
    {
        PlayerAppearance appearance = (PlayerAppearance)_appearance;
        if (appearance.Chest != 0)
        {
            ((UserGear)_gear).EquipArmor(appearance.Chest, ItemSlot.chest);
        }
        else
        {
            ((UserGear)_gear).EquipArmor(ItemTable.NAKED_CHEST, ItemSlot.chest);
        }

        if (appearance.Legs != 0)
        {
            ((UserGear)_gear).EquipArmor(appearance.Legs, ItemSlot.legs);
        }
        else
        {
            ((UserGear)_gear).EquipArmor(ItemTable.NAKED_LEGS, ItemSlot.legs);
        }

        if (appearance.Gloves != 0)
        {
            ((UserGear)_gear).EquipArmor(appearance.Gloves, ItemSlot.gloves);
        }
        else
        {
            ((UserGear)_gear).EquipArmor(ItemTable.NAKED_GLOVES, ItemSlot.gloves);
        }

        if (appearance.Feet != 0)
        {
            ((UserGear)_gear).EquipArmor(appearance.Feet, ItemSlot.feet);
        }
        else
        {
            ((UserGear)_gear).EquipArmor(ItemTable.NAKED_BOOTS, ItemSlot.feet);
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        _networkAnimationReceive.SetAnimationProperty((int)PlayerAnimationEvent.death, 1f, true);
    }

    public override bool StartAutoAttacking()
    {
        if (base.StartAutoAttacking())
        {
            _networkAnimationReceive.SetBool("atk01_" + _gear.WeaponAnim, true);
        }

        return true;
    }

    public override bool StopAutoAttacking()
    {
        if (base.StopAutoAttacking())
        {
            _networkAnimationReceive.SetBool("atk01_" + _gear.WeaponAnim, false);
            if (!_networkCharacterControllerReceive.IsMoving() && !IsDead())
            {
                _networkAnimationReceive.SetBool("atkwait_" + _gear.WeaponAnim, true);
            }
        }

        return true;
    }

    protected override void OnHit(bool criticalHit)
    {
        base.OnHit(criticalHit);
        _characterAnimationAudioHandler.PlaySound(CharacterSoundEvent.Dmg);
    }

    public override void UpdateWaitType(ChangeWaitTypePacket.WaitType moveType)
    {
        base.UpdateWaitType(moveType);

        if (moveType == ChangeWaitTypePacket.WaitType.WT_SITTING)
        {
            _networkAnimationReceive.SetBool("sit", true);
            //  _characterAnimationAudioHandler.PlaySound(CharacterSoundEvent.Sitdown);
        }
        else if (moveType == ChangeWaitTypePacket.WaitType.WT_STANDING)
        {
            _networkAnimationReceive.SetBool("stand", true);
            //  _characterAnimationAudioHandler.PlaySound(CharacterSoundEvent.Standup);
        }
        else if (moveType == ChangeWaitTypePacket.WaitType.WT_START_FAKEDEATH)
        {
            _networkAnimationReceive.SetBool("death", true);
            //   _characterAnimationAudioHandler.PlaySound(CharacterSoundEvent.Death);
        }
        else if (moveType == ChangeWaitTypePacket.WaitType.WT_STOP_FAKEDEATH)
        {
            //  _characterAnimationAudioHandler.PlaySound(CharacterSoundEvent.Atk_1H_S);
        }
    }
}