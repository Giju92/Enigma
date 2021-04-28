using UnityEngine;
using System.Collections;

public enum ItemID { Key, Banana, Head, Ball, Glove, Tessel, Triangle, Gear, Lever, Coin1, Coin2, Rock1, Rock2, Rock3,
    Chalk, Handle1, Handle2, Handle3, Handle4 };

[System.Serializable]
public class Item {
    public Sprite icon;
    public ItemID id;
}
