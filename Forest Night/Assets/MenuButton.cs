using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuButtonType
{
    Forward,
    View,
    Special
}

public class MenuButton : MonoBehaviour
{
    public MenuButtonType Type;
}
