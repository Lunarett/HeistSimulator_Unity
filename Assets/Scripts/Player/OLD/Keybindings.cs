using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Keybinding
{
    None, Interact, HolsterWeapon, ReloadWeapon, PlayerSprint, PlayerCrouch, PlayerJump
}

public class Keybindings
{
    public static KeyCode Interact { get; set; }
    public static KeyCode Hide_ShowWeapon { get; set; }
    public static KeyCode ReloadWeapon { get; set; }
    public static KeyCode PlayerCrouch { get; set; }
    public static KeyCode PlayerSprint { get; set; }
    public static KeyCode PlayerJump { get; set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Setup()
    {
        Interact = KeyCode.E;
        Hide_ShowWeapon = KeyCode.Q;
        ReloadWeapon = KeyCode.R;
        PlayerCrouch = KeyCode.LeftControl;
        PlayerSprint = KeyCode.LeftShift;
        PlayerJump = KeyCode.Space;
    }

    public static KeyCode Get(Keybinding keybinding)
    {
        switch (keybinding)
        {
            case Keybinding.Interact:
                return Interact;
            case Keybinding.HolsterWeapon:
                return Hide_ShowWeapon;
            case Keybinding.ReloadWeapon:
                return ReloadWeapon;
            case Keybinding.PlayerCrouch:
                return PlayerCrouch;
            case Keybinding.PlayerSprint:
                return PlayerSprint;
            case Keybinding.PlayerJump:
                return PlayerJump;
            default:
                return KeyCode.None;
        }
    }

    public static void Set(Keybinding keybinding, KeyCode newKey)
    {
        switch (keybinding)
        {
            case Keybinding.Interact:
                Interact = newKey;
                break;
            case Keybinding.HolsterWeapon:
                Hide_ShowWeapon = newKey;
                break;
            case Keybinding.ReloadWeapon:
                ReloadWeapon = newKey;
                break;
            case Keybinding.PlayerCrouch:
                PlayerCrouch = newKey;
                break;
            case Keybinding.PlayerSprint:
                PlayerSprint = newKey;
                break;
            case Keybinding.PlayerJump:
                PlayerJump = newKey;
                break;
            default:
                break;
        }
    }
}
