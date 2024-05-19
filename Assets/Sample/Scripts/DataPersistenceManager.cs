using System;
using UnityEngine;
using TMPro;
using TrexB.CipherUtility;

public class DataPersistenceManager : MonoBehaviour
{
    #region Variables
    private SaveDataFormat _saveObject;

    private const string _FileNameWithExtension = "GameData.json";

    #region UI
    [SerializeField] private TextMeshProUGUI _saveLoadStatus;
    [SerializeField] protected TextMeshProUGUI _decryptedData;

    private const string _GameDataSavedMessage = "Game data has been successfully saved.";
    private const string _FailedToSaveGameDataMessage = "Failed to save game data.";
    private const string _GameDataLoadedMessage = "Game data loaded successfully.";
    #endregion
    #endregion

    private void Start()
    {
        _saveObject = new SaveDataFormat
        {
            PlayerName = "TrexB",
            Health = 100,
            Level = 1,
            HasUnlockedPistol = true,
            HasUnlockedShotGun = true,
            HasUnlockedBomb = false,
        };
    }

    #region Button Events
    public void HandleSaveDataButtonClick()
    {
        bool result = SecureDataManager.SaveData<SaveDataFormat>(_FileNameWithExtension, _saveObject);

        if (result)
        {
            DisplaySaveLoadStatus(true, _GameDataSavedMessage);
        }
        else
        {
            DisplaySaveLoadStatus(false, _FailedToSaveGameDataMessage);
        }
    }

    public void HandleLoadDataButtonClick()
    {
        try
        {
            SaveDataFormat _saveData = SecureDataManager.LoadData<SaveDataFormat>(_FileNameWithExtension);

            _decryptedData.text = JsonUtility.ToJson(_saveData, true);

            DisplaySaveLoadStatus(true, _GameDataLoadedMessage);
        }
        catch (Exception e)
        {
            DisplaySaveLoadStatus(false, e.Message);
        }
    }
    #endregion

    #region UI - Display Status
    private void DisplaySaveLoadStatus(bool result, string status)
    {
        _saveLoadStatus.text = status;
        _saveLoadStatus.color = result ? Color.green : Color.red;

        float delayTime = 2;

        // Handling: Example - when user press the save and load button quickly, then you see both messages but load message will fade quickly because of the Invoke
        // function that's been invoked during saving the data.
        if (IsInvoking(nameof(EmptyStatusText)))
        {
            CancelInvoke(nameof(EmptyStatusText));
        }

        Invoke(nameof(EmptyStatusText), delayTime);
    }

    private void EmptyStatusText()
    {
        _saveLoadStatus.text = string.Empty;
    }
    #endregion
} // class