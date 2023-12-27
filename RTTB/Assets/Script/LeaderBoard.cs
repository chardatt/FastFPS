using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Dan.Main;
using Steamworks;
using Dan.Models;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Linq;

public class LeaderBoard : MonoBehaviour
{
    //public List<Text> names;
    //public List<Text> scores;
    public Text[] _entryTextObjects;
    private string publicLeaderBoardKey = "744120c653bd0e09c903423a1884bedc95bb96e692eb1b50ad9f49d3c4a7730b";
    //private List<Entry> _entries = new List<Entry>();
    private void Start()
    {
        //UploadEntry("Poope", 100000, "0");
    }
    private void LoadEntries()
    {
        Leaderboards.LeaderBoardRTTB.GetEntries(entries =>
        {
            foreach (var t in _entryTextObjects)
                t.text = "";
            var length = Mathf.Min(_entryTextObjects.Length, entries.Length);
            for (int i = 0; i < length; i++)
                _entryTextObjects[i].text = $"{entries[i].Rank}. {entries[i].Username} - {entries[i].Score}";
        });
    }
    public void UploadEntry(string username,int score,string extra)
    {
        Leaderboards.LeaderBoardRTTB.GetEntries(entries =>
        {
            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i].Username == username)
                {
                    Leaderboards.LeaderBoardRTTB.UpdateEntryUsername(username);
                        LeaderboardCreator.ResetPlayer();
                        LoadEntries();

                }
                else
                {
                    Leaderboards.LeaderBoardRTTB.UploadNewEntry(username, score);
                        LeaderboardCreator.ResetPlayer();
                        LoadEntries();
                }
            }
        });
        
    }
    /*public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, ((msg) =>
        {
            int loopLength = (msg.Length < names.Count) ? msg.Length : names.Count;
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = msg[i].Username;
                scores[i].text = msg[i].Extra;
            }
        }));
    }
    public void SetLeaderBoardEntry(string username, int score, string scoreNice)
    {
        //LeaderboardCreator.GetLeaderboard(publicLeaderBoardKey, entrie => entrie.ToList().ForEach(A );
        LeaderboardCreator.UploadNewEntry(publicLeaderBoardKey, username, score, scoreNice, ((msg) =>
        {
            GetLeaderBoard();
        }));
    }*/
}
