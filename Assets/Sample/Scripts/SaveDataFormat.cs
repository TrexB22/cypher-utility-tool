#region [System.Serializable] Attribute Explanation
// [System.Serializable] is an attribute in C# that indicates a class or a struct can be serialized, which means converting its instances into a format that
// can be easily stored or transmitted (e.g., to a file or over a network) and later reconstructed.
// The JsonUtility class in Unity does not require [System.Serializable] to work.
// It's still a good practice to use it for clarity and compatibility with other serialization mechanisms
#endregion
[System.Serializable]
public class SaveDataFormat
{
    public string PlayerName;
    public int Health;
    public int Level;
    public bool HasUnlockedPistol;
    public bool HasUnlockedShotGun;
    public bool HasUnlockedBomb;
} // class