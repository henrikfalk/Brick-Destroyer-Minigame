using UnityEngine;
using System.Collections;
using System; //This allows the IComparable Interface

[System.Serializable]
public class PlayerInfo : IComparable<PlayerInfo>
{
    public string playerName;
    public string playerRecord;

    public PlayerInfo(string name, string record)
    {
        playerName = name;
        playerRecord = record;
    }

    //This method is required by the IComparable
    //interface. 
    public int CompareTo(PlayerInfo other)
    {
        if(other == null)
        {
            return 1;
        }

        int thisRecord = System.Convert.ToInt32(playerRecord);
        int otherRecord = System.Convert.ToInt32(other.playerRecord);

        return thisRecord.CompareTo(otherRecord);
    }
}