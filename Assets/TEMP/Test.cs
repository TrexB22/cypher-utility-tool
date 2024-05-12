using UnityEngine;
using TrexB.CipherUtility;

public class Test : MonoBehaviour
{
    private struct SaveDataFormat 
    {
        public float health;
        public float bullets;
        public bool isPlayerAlive;
        public bool isEnemyAlive;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadData();
        }
    }

    private void SaveData()
    {
        SaveDataFormat dataFormat = new SaveDataFormat
        {
            health = 100,
            bullets = 100,
            isPlayerAlive = true,
            isEnemyAlive = false
        };

        Debug.Log(SecureDataManager.SaveData("savedata.json", dataFormat));
    }

    private void LoadData()
    {
        Debug.Log(SecureDataManager.LoadData<SaveDataFormat>("savedata.json"));
    }

} // class