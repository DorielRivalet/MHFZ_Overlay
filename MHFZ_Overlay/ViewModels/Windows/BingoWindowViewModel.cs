// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.ViewModels.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Messengers;
using MHFZ_Overlay.Services;

public partial class BingoWindowViewModel : ObservableRecipient
{
    public BingoWindowViewModel()
    {
        WeakReferenceMessenger.Default.Register<QuestIDMessage>(this, OnReceivedQuestID);
        WeakReferenceMessenger.Default.Register<RunIDMessage>(this, OnReceivedRunID);
    }

    private static readonly BingoService BingoServiceInstance = BingoService.GetInstance();

    // TODO
    private void UpdateBingoBoard(List<BingoMonster> bingoBoard, int questID)
    {

    }

    private void OnReceivedQuestID(object recipient, QuestIDMessage message)
    {
        // Handle the message here, with r being the recipient and m being the
        // input message. Using the recipient passed as input makes it so that
        // the lambda expression doesn't capture "this", improving performance.
        MessageBox.Show(nameof(recipient));
        if (!IsBingoRunning)
        {
            return;
        }

        ReceivedQuestID = message.Value;
        UpdateBingoBoard(BingoBoard, ReceivedQuestID);
    }

    private void OnReceivedRunID(object recipient, RunIDMessage message)
    {
        // Handle the message here, with r being the recipient and m being the
        // input message. Using the recipient passed as input makes it so that
        // the lambda expression doesn't capture "this", improving performance.
        MessageBox.Show(nameof(recipient));
        if (!IsBingoRunning)
        {
            return;
        }

        ReceivedRunID = message.Value;
        RunIDs.Add(message.Value);
    }

    /// <summary>
    /// 
    /// </summary>
    [ObservableProperty]
    private string? bingoButtonText = "Start";

    /// <summary>
    /// The received Quest ID.
    /// </summary>
    [ObservableProperty]
    private int receivedQuestID;

    /// <summary>
    /// The received Run ID.
    /// </summary>
    [ObservableProperty]
    private int receivedRunID;

    /// <summary>
    /// The MonsterList field in Bingo table.
    /// </summary>
    [ObservableProperty]
    private List<int> runIDs;

    /// <summary>
    /// The bingo board holding a list of bingo monsters.
    /// </summary>
    [ObservableProperty]
    private List<BingoMonster> bingoBoard;

    /// <summary>
    /// Whether bingo was started.
    /// </summary>
    [ObservableProperty]
    private bool isBingoRunning;

    [RelayCommand]
    private void StartBingo()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        if (!s.EnableQuestLogging)
        {
            MessageBox.Show("Enable quest logging.");
            return;
        }

        if (BingoButtonText == "Start")
        {
            // TODO Do you want to restart notice
            // Implement your logic to start bingo here
            BingoButtonText = "Cancel";
            IsBingoRunning = true;
            MessageBox.Show("Running");
        }
        else if (BingoButtonText == "Cancel")
        {
            StopBingo();
        }
    }

    private void StopBingo()
    {
        IsBingoRunning = false;
        BingoButtonText = "Start";
        BingoBoard.Clear();
        RunIDs.Clear();
        ReceivedQuestID = 0;
        ReceivedRunID = 0;
        MessageBox.Show("Stopped");
    }

    private bool CanWeaponReroll() => BingoButtonText == "Cancel";

    [RelayCommand(CanExecute = nameof(CanWeaponReroll))]
    private void WeaponReroll()
    {
        MessageBox.Show("Reroll");
    }
}
