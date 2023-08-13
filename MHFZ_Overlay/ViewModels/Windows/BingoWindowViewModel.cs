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
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Messengers;
using MHFZ_Overlay.Models.Structures;
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

    public BingoCell[,]? Cells
    {
        get => _cells;
        set => SetProperty(ref _cells, value);
    }

    public string? PlayerBingoPointsText => $"Bingo Points: {PlayerBingoPoints}";

    public string? WeaponRerollButtonContent => $"Reroll weapon bonuses ({WeaponRerollCost} Bingo Points)";

    public bool IsBingoNotRunning => IsBingoRunning == false;

    public SnackbarPresenter BingoWindowSnackbarPresenter { get; }

    public IEnumerable<Difficulty> Difficulties
    {
        get
        {
            return Enum.GetValues(typeof(Difficulty))
                       .Cast<Difficulty>()
                       .Where(difficulty => difficulty != Difficulty.Unknown);
        }
    }

    private static readonly BingoService BingoServiceInstance = BingoService.GetInstance();

    private BingoCell[,]? _cells = new BingoCell[5, 5];

    // TODO
    private void UpdateBingoBoard(int questID)
    {
        if (Cells == null)
        {
            MessageBox.Show($"Null cells");
            return;
        }

        MessageBox.Show($"Updated bingo board, questID {questID}");

        foreach (var cell in Cells)
        {
            if (cell == null || cell.Monster == null || cell.Monster.QuestIDs == null)
            {
                continue;
            }

            if (cell.Monster.QuestIDs.Contains(questID))
            {
                cell.IsComplete = true;
            }
        }

        if (CheckForBingoCompletion())
        {
            // The game is over, perform any necessary actions.
            MessageBox.Show($"Game over");
            StopBingo();
        }
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
    [NotifyPropertyChangedFor(nameof(Cells))]
    private int receivedQuestID;

    /// <summary>
    /// The received Run ID.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(RunIDs))]
    private int receivedRunID;

    /// <summary>
    /// The MonsterList field in Bingo table. Works in conjunction with Carts and WeaponTypeBonuses in order to calculate the final score.
    /// </summary>
    [ObservableProperty]
    private List<int> runIDs = new();

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

    [ObservableProperty]
    private Difficulty selectedDifficulty = Difficulty.Easy;

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
            GenerateBoard(selectedDifficulty);
        }
        else
        {
            StopBingo();
        }
    }

    private void GenerateBoard(Difficulty difficulty)
    {
        if (Cells == null)
        {
            MessageBox.Show($"Null cells");
            return;
        }

        int boardSize = (difficulty == Difficulty.Extreme) ? 10 : 5;
        Cells = new BingoCell[boardSize, boardSize];
        var bingoMonsterListDifficulty = difficulty == Difficulty.Extreme ? Difficulty.Hard : difficulty;

        var monsters = BingoMonsters.DifficultyBingoMonster[bingoMonsterListDifficulty].ToList();

        PopulateBingoBoardCells(boardSize, difficulty, monsters);
    }

    private void PopulateBingoBoardCells(int boardSize, Difficulty difficulty, List<BingoMonster> monsters)
    {
        if (Cells == null)
        {
            MessageBox.Show($"Null cells");
            return;
        }

        // Shuffle the list of monsters.
        var rng = new Random();
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = 0; j < boardSize; j++)
            {
                int index = rng.Next(monsters.Count); // Get a random index
                BingoMonster selectedMonster = monsters[index]; // Select a monster

                // TODO test if magspike shows
                Cells[i, j] = new BingoCell
                {
                    Monster = selectedMonster,
                    WeaponTypeBonus = (FrontierWeaponTypes)rng.Next(Enum.GetValues(typeof(FrontierWeaponTypes)).Length),
                };
            }
        }

        if (difficulty == Difficulty.Extreme)
        {
            Cells[boardSize / 2, boardSize / 2] = new BingoCell
            {
                Monster = BingoMonsters.DifficultyBingoMonster[Difficulty.Hard].FirstOrDefault(monster => monster.Name == "Burning Freezing Elzelion")
            };
        }
    }

    private void StopBingo()
    {
        IsBingoRunning = false;
        BingoButtonText = "Start";
        RunIDs.Clear();
        Cells = new BingoCell[0, 0];
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

    private bool CheckForBingoCompletion()
    {
        if (Cells == null)
        {
            MessageBox.Show($"Null cells");
            return false;
        }

        // Check each row.
        for (int i = 0; i < Cells.GetLength(0); i++)
        {
            if (Enumerable.Range(0, Cells.GetLength(1)).All(j => Cells[i, j].IsComplete))
            {
                return true;
            }
        }

        // Check each column.
        for (int j = 0; j < Cells.GetLength(1); j++)
        {
            if (Enumerable.Range(0, Cells.GetLength(0)).All(i => Cells[i, j].IsComplete))
            {
                return true;
            }
        }

        // Check the diagonal from top-left to bottom-right.
        if (Enumerable.Range(0, Cells.GetLength(0)).All(i => Cells[i, i].IsComplete))
        {
            return true;
        }

        // Check the diagonal from top-right to bottom-left.
        if (Enumerable.Range(0, Cells.GetLength(0)).All(i => Cells[i, Cells.GetLength(0) - 1 - i].IsComplete))
        {
            return true;
        }

        return false;
    }
}
