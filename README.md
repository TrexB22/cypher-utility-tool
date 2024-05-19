# Data Persistence in Unity

## Requirements
* Requires Unity 2020.3 LTS or higher.

## Supported Platforms
The Cipher Utility Toolkit has been thoroughly tested and is fully operational on the following platforms:
* Windows
* Android

## How AES Encryption Works
* AES encryption transforms data into encrypted text using a key and an initialization vector (IV).
* The IV ensures each encryption is unique, making it harder to decode.
* To decrypt, both the key and IV are required, ensuring safe storage of information.

## Data Handling
**Encryption**
* Data is passed.
* It's then transformed into a suitable format for storage using JsonUtility.
* Next, it undergoes encryption to ensure security.
* Finally, the encrypted data is saved.

  Data -> JSON -> Encryption -> Save

**Decryption**
* Encrypted data is retrieved.
* Decryption is performed to restore it to its original form.
* JsonUtility is used to convert the decrypted data into its original format.
* The restored data is then accessible.

  Load -> Decryption -> JSON -> Original Format

## Installation
Download and Import the Latest .unitypackage file from <a href="https://github.com/TrexB22/cypher-utility-tool/releases">Releases Page</a>.

## Usage
**Update Key & IV**

Follow the steps depicted in the image below.
* Open the Editor Tool

  ![Editor Tool](https://github.com/TrexB22/cypher-utility-tool/blob/main/Images/1.png)
* Press the Generate Button to create a random Key and IV.

  ![Editor Tool](https://github.com/TrexB22/cypher-utility-tool/blob/main/Images/2.png)

* Select the Copy button to copy the Key and IV to the clipboard.

  ![Editor Tool](https://github.com/TrexB22/cypher-utility-tool/blob/main/Images/3.png)

* Open the SecureDataManager.cs script.

  ![Editor Tool](https://github.com/TrexB22/cypher-utility-tool/blob/main/Images/4.png)

* Substitute the Key and IV values in the script accordingly.

  ![Editor Tool](https://github.com/TrexB22/cypher-utility-tool/blob/main/Images/5.png)

* Done!

**Example**
```csharp
using TrexB.CipherUtility;

[System.Serializable]
public class SaveDataFormat
{
    public string PlayerName = "TrexB";
    public int Health = 100;
    public int Level = 1;
}

public class DataPersistenceManager
{
    private SaveDataFormat _playerData = new SaveDataFormat();

    private const string _FileNameWithExtension = "GameData.json";

    public void SaveData()
    {
        bool result = SecureDataManager.SaveData<SaveDataFormat>(_FileNameWithExtension, _playerData);

        if (result)
        {
            Debug.Log("Game data has been successfully saved.");
        }
        else
        {
            Debug.Log("Failed to save game data.");
        }
    } // SaveData

    public void LoadData()
    {
        try
        {
            SaveDataFormat _playerData = SecureDataManager.LoadData<SaveDataFormat>(_FileNameWithExtension);
            Debug.Log("Game data loaded successfully.");
        }
        catch (Exception e)
        {
            Debug.Log("There was an issue while loading the data: " + e.Message);
        }
    } // LoadData
} // class
```

## Note
* When using this tool in the editor, the data will be saved in Application.dataPath, which essentially corresponds to the Assets folder of the project. This approach enables developers to conveniently test things within the Unity project, ensuring smooth functionality during development.
* Inside the build, the data will be stored in the Application.persistentDataPath.
