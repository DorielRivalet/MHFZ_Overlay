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
using System.Windows.Documents;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using EZlion.Mapper;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Messengers;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Views.Windows;
using Wpf.Ui.Common;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;

public partial class BingoWindowViewModel : ObservableRecipient
{
    public BingoWindowViewModel(SnackbarPresenter snackbarPresenter)
    {
        WeakReferenceMessenger.Default.Register<QuestIDMessage>(this, OnReceivedQuestID);
        WeakReferenceMessenger.Default.Register<RunIDMessage>(this, OnReceivedRunID);
        BingoWindowSnackbarPresenter = snackbarPresenter;
    }

    private static readonly BingoService BingoServiceInstance = BingoService.GetInstance();

    // TODO
    private void UpdateBingoBoard(int questID)
    {
        MessageBox.Show($"Updated bingo board, questID {questID}");
    }

    private void UpdateRunIDs(int runID)
    {
        RunIDs.Add(runID);
        MessageBox.Show($"Updated RunIDs, runID {runID}");
    }

    partial void OnReceivedQuestIDChanged(int value) => UpdateBingoBoard(value);

    partial void OnReceivedRunIDChanged(int value) => UpdateRunIDs(value);

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
    [NotifyPropertyChangedFor(nameof(BingoBoard))]
    private int receivedQuestID;

    /// <summary>
    /// The received Run ID.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RunIDs))]
    private int receivedRunID;

    /// <summary>
    /// The MonsterList field in Bingo table.
    /// </summary>
    [ObservableProperty]
    private List<int> runIDs = new();

    /// <summary>
    /// The number of carts in the bingo board.
    /// </summary>
    [ObservableProperty]
    private List<int> carts = new();

    /// <summary>
    /// The weapon type bonuses in the bingo board.
    /// </summary>
    [ObservableProperty]
    private List<int> weaponTypeBonuses = new();

    /// <summary>
    /// The bingo board holding a list of bingo monsters.
    /// </summary>
    [ObservableProperty]
    private List<BingoMonster> bingoBoard = new ();

    /// <summary>
    /// Whether bingo was started.
    /// </summary>
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(WeaponRerollCommand))]
    private bool isBingoRunning;

    /// <summary>
    /// The cost for rerolling the weapon bonuses in each bingo grid.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WeaponRerollButtonContent))]
    private long weaponRerollCost = 2;

    /// <summary>
    /// The player bingo points. For view purposes only.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PlayerBingoPointsText))]
    private long playerBingoPoints = BingoServiceInstance.GetPlayerBingoPoints();

    public string? PlayerBingoPointsText => $"Bingo Points: {PlayerBingoPoints}";

    public string? WeaponRerollButtonContent => $"Reroll weapon bonuses ({WeaponRerollCost} Bingo Points)";

    public bool IsBingoNotRunning => IsBingoRunning == false;

    public SnackbarPresenter BingoWindowSnackbarPresenter { get; }

    [RelayCommand]
    private void StartBingo()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        if (!s.EnableQuestLogging)
        {
            MessageBox.Show("Enable quest logging.");
            return;
        }

        if (!IsBingoRunning)
        {
            IsBingoRunning = true;
            // TODO Do you want to restart notice
            // Implement your logic to start bingo here
            BingoButtonText = "Cancel";
        }
        else
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
        Carts.Clear();
        WeaponTypeBonuses.Clear();
        ReceivedQuestID = 0;
        ReceivedRunID = 0;
        WeaponRerollCost = 2;
    }

    /// <summary>
    /// Rerolls the weapon bonuses in each cell of the bingo board.
    /// </summary>
    private void RerollWeaponBonuses()
    {
        PlayerBingoPoints -= WeaponRerollCost;
        WeaponRerollCost *= 2;
        // TODO 
    }

    [RelayCommand(CanExecute = nameof(IsBingoRunning))]
    private void WeaponReroll()
    {
        if (BingoServiceInstance.BuyWeaponReroll(WeaponRerollCost))
        {
            RerollWeaponBonuses();
        }
        else
        {
            var snackbar = new Snackbar(BingoWindowSnackbarPresenter)
            {
                Style = (Style)Application.Current.FindResource("CatppuccinMochaSnackBar"),
                Title = "Not enough bingo points!",
                Content = "You need more bingo points in order to buy more weapon rerolls.",
                Icon = new SymbolIcon(SymbolRegular.ErrorCircle24),
                Appearance = ControlAppearance.Danger,
                Timeout = TimeSpan.FromSeconds(5),
            };
            snackbar.Show();
        }
    }
}
