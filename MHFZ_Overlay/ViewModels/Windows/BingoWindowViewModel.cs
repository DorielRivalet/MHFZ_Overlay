// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace MHFZ_Overlay.ViewModels.Windows;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using MHFZ_Overlay.Models;
using MHFZ_Overlay.Models.Collections;
using MHFZ_Overlay.Models.Constant;
using MHFZ_Overlay.Models.Messengers;
using MHFZ_Overlay.Models.Structures;
using MHFZ_Overlay.Services;
using MHFZ_Overlay.Views.Windows;
using SkiaSharp;
using Wpf.Ui;
using Wpf.Ui.Controls;
using MessageBox = System.Windows.MessageBox;
using LiveChartsCore.SkiaSharpView.VisualElements;
using System.Numerics;
using LiveChartsCore.Defaults;
using LiveChartsCore.Kernel.Sketches;
using System.Windows.Markup;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Extensions;
using LiveChartsCore.VisualElements;
using Xunit.Abstractions;
using System.Globalization;

public partial class BingoWindowViewModel : ObservableRecipient, IRecipient<QuestIDMessage>, IRecipient<RunIDMessage>
{
    private readonly Random _random = new();

    public IEnumerable<ISeries> GaugeSeries { get; set; }

    public IEnumerable<VisualElement<SkiaSharpDrawingContext>> VisualElements { get; set; }

    public NeedleVisual Needle { get; set; }

    private static void SetStyle(
        double sectionsOuter, double sectionsWidth, PieSeries<ObservableValue> series, int order)
    {
        series.OuterRadiusOffset = sectionsOuter;
        series.MaxRadialColumnWidth = sectionsWidth;
        switch (order)
        {
            default:
                series.Fill = new SolidColorPaint(new SKColor(AddressModel.StaticHexColorToDecimal("#11111b")));
                break;
            case 1:
                series.Fill = new SolidColorPaint(new SKColor(AddressModel.StaticHexColorToDecimal("#f9e2af")));
                break;
            case 2:
                series.Fill = new SolidColorPaint(new SKColor(AddressModel.StaticHexColorToDecimal("#94e2d5")));
                break;
            case 3:
                series.Fill = new SolidColorPaint(new SKColor(AddressModel.StaticHexColorToDecimal("#89b4fa")));
                break;
        }
    }

    [RelayCommand]
    public void DoRandomChange()
    {
        // modifying the Value property updates and animates the chart automatically
        Needle.Value = _random.Next(0, 100);
    }

    private void SetGauge()
    {
        var sectionsOuter = 130;
        var sectionsWidth = 20;

        Needle = new NeedleVisual
        {
            Value = 45,
            Fill = new SolidColorPaint(SKColor.FromHsl(226, 64, 88, 127)),
        };

        GaugeSeries = GaugeGenerator.BuildAngularGaugeSections(
            new GaugeItem(60, s => SetStyle(sectionsOuter, sectionsWidth, s, 1)),
            new GaugeItem(30, s => SetStyle(sectionsOuter, sectionsWidth, s, 2)),
            new GaugeItem(10, s => SetStyle(sectionsOuter, sectionsWidth, s, 3)));

        VisualElements = new VisualElement<SkiaSharpDrawingContext>[]
        {
            new AngularTicksVisual
            {
                LabelsSize = 16,
                LabelsOuterOffset = 15,
                OuterOffset = 65,
                TicksLength = 20,
                LabelsPaint = new SolidColorPaint(new SKColor(AddressModel.StaticHexColorToDecimal("#cdd6f4"))),
                Stroke = new SolidColorPaint(new SKColor(AddressModel.StaticHexColorToDecimal("#cdd6f4"))),
            },
            Needle
        };
    }  

    private ObservableCollection<ObservableValue>? _observableValues { get; set; }

    private static readonly NLog.Logger LoggerInstance = NLog.LogManager.GetCurrentClassLogger();

    public BingoWindowViewModel(SnackbarPresenter snackbarPresenter)
    {
        BingoWindowSnackbarPresenter = snackbarPresenter;
        SetGraphs();
        SetGauge();
    }

    public void Receive(QuestIDMessage message) => OnReceivedQuestID(message);

    public void Receive(RunIDMessage message) => OnReceivedRunID(message);

    private void SetGraphs()
    {
        return;
        // Use ObservableCollections to let the chart listen for changes (or any INotifyCollectionChanged). 
        _observableValues = new ObservableCollection<ObservableValue>
        {
            // Use the ObservableValue or ObservablePoint types to let the chart listen for property changes 
            // or use any INotifyPropertyChanged implementation 
            new ObservableValue(2),
            new(5), // the ObservableValue type is redundant and inferred by the compiler (C# 9 and above)
            new(4),
            new(5),
            new(2),
            new(6),
            new(6),
            new(6),
            new(4),
            new(2),
            new(3),
            new(4),
            new(3)
        };

        Series = new ObservableCollection<ISeries>
        {
            new LineSeries<ObservableValue>
            {
                Values = _observableValues,
                Fill = null
            }
        };

        // in the following sample notice that the type int does not implement INotifyPropertyChanged
        // and our Series.Values property is of type List<T>
        // List<T> does not implement INotifyCollectionChanged
        // this means the following series is not listening for changes.
        // Series.Add(new ColumnSeries<int> { Values = new List<int> { 2, 4, 6, 1, 7, -2 } }); 
    }

    [ObservableProperty]
    private IEnumerable<BingoCell>? flatCells;

    public string? PlayerBingoPointsText => $"Bingo Points: {PlayerBingoPoints}";

    public string? WeaponRerollButtonContent => $"Reroll weapon bonuses ({WeaponRerollCost} Bingo Points)";

    public string? CartsBuyButtonContent => $"Buy carts ({CartsCost} Bingo Points)";

    public string? BingoStartButtonContent => IsBingoRunning ? "Cancel" : $"Start ({BingoStartCost} Bingo Points)";

    public string? BingoStartButtonIcon => IsBingoRunning ? "ArrowCounterClockwise20" : "Play20";

    public string? BingoStartButtonBackground => IsBingoRunning ? CatppuccinMochaColors.NameHex["Red"] : CatppuccinMochaColors.NameHex["Green"];

    public string? ZenithBoostText => $"Zenith Boost ({ZenithGauntletItems} left)";

    public string? SolsticeBoostText => $"Solstice Boost ({SolsticeGauntletItems} left)";

    public string? MusouBoostText => $"Musou Boost ({MusouGauntletItems} left)";

    public bool IsGauntletBoostMax => GauntletBoost.HasFlag(GauntletBoost.Zenith) &&
                    GauntletBoost.HasFlag(GauntletBoost.Solstice) &&
                    GauntletBoost.HasFlag(GauntletBoost.Musou);

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

    public IEnumerable<BingoLineColorOption> BingoLineOptions => (IEnumerable<BingoLineColorOption>)Enum.GetValues(typeof(BingoLineColorOption));

    private static readonly BingoService BingoServiceInstance = BingoService.GetInstance();

    [ObservableProperty]
    private BingoCell[,]? cells = new BingoCell[5, 5];

    public ObservableCollection<ISeries>? Series { get; set; }

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

    partial void OnSelectedDifficultyChanged(Difficulty value) => UpdateBingoStatsFromSelectedDifficulty(value);

    partial void OnIsMusouElzelionBoostActiveChanged(bool value) => BingoStartCost = BingoServiceInstance.CalculateBingoStartCost(GauntletBoost, SelectedDifficulty, value);

    partial void OnZenithBoostCheckedChanged(bool value)
    {
        if (value)
        {
            GauntletBoost |= GauntletBoost.Zenith;
        }
        else
        {
            GauntletBoost &= ~GauntletBoost.Zenith;
        }

        BingoStartCost = BingoServiceInstance.CalculateBingoStartCost(GauntletBoost, SelectedDifficulty, IsMusouElzelionBoostActive);
    }

    partial void OnSolsticeBoostCheckedChanged(bool value)
    {
        if (value)
        {
            GauntletBoost |= GauntletBoost.Solstice;
        }
        else
        {
            GauntletBoost &= ~GauntletBoost.Solstice;
        }

        BingoStartCost = BingoServiceInstance.CalculateBingoStartCost(GauntletBoost, SelectedDifficulty, IsMusouElzelionBoostActive);
    }

    partial void OnMusouBoostCheckedChanged(bool value)
    {
        if (value)
        {
            GauntletBoost |= GauntletBoost.Musou;
        }
        else
        {
            GauntletBoost &= ~GauntletBoost.Musou;
        }

        BingoStartCost = BingoServiceInstance.CalculateBingoStartCost(GauntletBoost, SelectedDifficulty, IsMusouElzelionBoostActive);
    }

    private void UpdateBingoStatsFromSelectedDifficulty(Difficulty difficulty)
    {
        // TODO disable controls if bingo is running
        if (IsBingoRunning)
        {
            return;
        }

        BingoStartCost = BingoServiceInstance.CalculateBingoStartCost(GauntletBoost, difficulty, IsMusouElzelionBoostActive);
        Carts = BingoServiceInstance.CalculateCartsAtBingoStartFromSelectedDifficulty(difficulty);
    }


    private void OnReceivedQuestID(QuestIDMessage message)
    {
        if (!IsBingoRunning)
        {
            LoggerInstance.Info("Received Quest {0} but bingo is not running.", message);
            return;
        }

        ReceivedQuestID = message.Value;
    }

    private void OnReceivedRunID(RunIDMessage message)
    {
        if (!IsBingoRunning)
        {
            LoggerInstance.Info("Received Run {0} but bingo is not running.", message);
            return;
        }

        ReceivedRunID = message.Value;
    }

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
    [NotifyCanExecuteChangedFor(nameof(CartsBuyCommand))]
    [NotifyCanExecuteChangedFor(nameof(SetPointsCommand))]
    [NotifyPropertyChangedFor(nameof(BingoNotRunning))]
    [NotifyCanExecuteChangedFor(nameof(TranscendCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShuffleBingoCellsCommand))]
    [NotifyCanExecuteChangedFor(nameof(SelectCellsOrderCommand))]
    [NotifyCanExecuteChangedFor(nameof(SelectWeaponOrderCommand))]
    [NotifyPropertyChangedFor(nameof(BingoStartButtonContent))]
    [NotifyPropertyChangedFor(nameof(BingoStartButtonIcon))]
    [NotifyPropertyChangedFor(nameof(BingoStartButtonBackground))]
    private bool isBingoRunning;

    /// <summary>
    /// The cost for rerolling the weapon bonuses in each bingo grid.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(WeaponRerollButtonContent))]
    private long weaponRerollCost = 2;

    /// <summary>
    /// The cost for buying more carts during a bingo run.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CartsBuyButtonContent))]
    private long cartsCost = 2;

    /// <summary>
    /// The amount of carts.
    /// </summary>
    [ObservableProperty]
    private long carts;

    /// <summary>
    /// The cost for starting bingo.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(BingoStartButtonContent))]
    private long bingoStartCost = 0;

    /// <summary>
    /// The player bingo points. For view purposes only.
    /// </summary>
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(PlayerBingoPointsText))]
    private long playerBingoPoints = BingoServiceInstance.GetPlayerBingoPoints();

    [ObservableProperty]
    private Difficulty selectedDifficulty = Difficulty.Easy;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsGauntletBoostMax))]
    private GauntletBoost gauntletBoost = GauntletBoost.None;

    [ObservableProperty]
    private BingoLineColorOption selectedBingoLineOption = BingoLineColorOption.Hardest;

    [ObservableProperty]
    private bool bingoExplanationShown = true;

    [ObservableProperty]
    private bool zenithBoostChecked = false;

    [ObservableProperty]
    private bool solsticeBoostChecked = false;

    [ObservableProperty]
    private bool musouBoostChecked = false;

    [ObservableProperty]
    private bool isMusouElzelionBoostActive = false;

    [ObservableProperty]
    private int boardSize;

    [ObservableProperty]
    private float x0;

    [ObservableProperty]
    private float y0;

    [ObservableProperty]
    private float x1;

    [ObservableProperty]
    private float y1;

    [ObservableProperty]
    private float x2;

    [ObservableProperty]
    private float y2;

    [ObservableProperty]
    private float x3;

    [ObservableProperty]
    private float y3;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ZenithBoostText))]
    private int zenithGauntletItems;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SolsticeBoostText))]
    private int solsticeGauntletItems;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(MusouBoostText))]
    private int musouGauntletItems;

    [RelayCommand]
    private void StartBingo()
    {
        var s = (Settings)Application.Current.TryFindResource("Settings");
        if (!s.EnableQuestLogging)
        {
            MessageBox.Show("Enable quest logging.");
            return;
        }

        if (!BingoServiceInstance.SpendBingoPoints(BingoStartCost) && !IsBingoRunning)
        {
            var snackbar = new Snackbar(BingoWindowSnackbarPresenter)
            {
                Style = (Style)Application.Current.FindResource("CatppuccinMochaSnackBar"),
                Title = "Not enough bingo points!",
                Content = "You need more bingo points in order to start a bingo run. Try starting at Easy difficulty without any boosts.",
                Icon = new SymbolIcon(SymbolRegular.ErrorCircle24),
                Appearance = ControlAppearance.Danger,
                Timeout = TimeSpan.FromSeconds(5),
            };

            snackbar.Show();
            return;
        }

        if (!IsBingoRunning)
        {
            IsBingoRunning = true;
            BingoExplanationShown = false;
            // TODO Do you want to restart notice
            // Implement your logic to start bingo here
            PlayerBingoPoints -= BingoStartCost;
            GenerateBoard(selectedDifficulty);
        }
        else
        {
            StopBingo();
        }
    }

    public ObservableCollection<Vector2> CurvePoints = new ObservableCollection<Vector2>();

    public ISeries[] Series2 { get; set; } =
    {
        new LineSeries<double>
        {
            Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
        }
    };


    [RelayCommand]
    private void RefreshPoints()
    {
        BezierCurve curve = new BezierCurve(
                    new Vector2(X0, Y0),
                    new Vector2(X1, Y1),
                    new Vector2(X2, Y2),
                    new Vector2(X3, Y3)
                );

        BezierCurve curve2 = new BezierCurve(
            new Vector2(2 * 60 * 60, 0),
            new Vector2(2 * 60 * 60, 1000),
            new Vector2(10 * 60, 0),
            new Vector2(10 * 60, 1000)
        );

        for (float t = 0; t <= 1; t += 0.01f)
        {
            CurvePoints.Add(curve2.Evaluate(t));
        }

        ObservableCollection<ISeries> series = new();
        ObservableCollection<ObservablePoint> collection = new();

        foreach (var entry in CurvePoints)
        {
            collection.Add(new ObservablePoint(entry.X, entry.Y));
        }

        series.Add(new LineSeries<ObservablePoint>
        {
            Values = collection,
            LineSmoothness = .5,
            GeometrySize = 0,
            Stroke = new SolidColorPaint(new SKColor(0xff,0xff,0xff)) { StrokeThickness = 2 },
            Fill = new SolidColorPaint(new SKColor(0x74, 0xc7, 0xec)) { StrokeThickness = 2 },
        });

        Series = series;

        //Series =
        //{
        //    new LineSeries
        //    {
        //        Values = new ChartValues<ObservablePoint>(CurvePoints.Select(p => new ObservablePoint(p.X, p.Y))),
        //        Fill = null
        //    }
        //};

        //// Calculate the elapsed time (in seconds)
        //float elapsedTime = 3000;

        //// Calculate the total time (in seconds)
        //float totalTime = 10000;

        //// Calculate the t parameter
        //float t = Math.Min(elapsedTime / totalTime, 1);

        //// Calculate the score
        //Vector2 scorePoint = curve.Evaluate(t);
        //var score = scorePoint.Y;

        //return score;
    }

    private void GenerateBoard(Difficulty difficulty)
    {
        if (Cells == null)
        {
            MessageBox.Show($"Null cells");
            return;
        }

        BoardSize = (difficulty == Difficulty.Extreme) ? 10 : 5;
        Cells = new BingoCell[BoardSize, BoardSize];
        var bingoMonsterListDifficulty = difficulty == Difficulty.Extreme ? Difficulty.Hard : difficulty;

        var monsters = BingoMonsters.DifficultyBingoMonster[bingoMonsterListDifficulty].ToList();

        PopulateBingoBoardCells(difficulty, monsters);
        FlatCells = Cells.Cast<BingoCell>();
    }

    private void PopulateBingoBoardCells(Difficulty difficulty, List<BingoMonster> monsters)
    {
        if (Cells == null)
        {
            MessageBox.Show($"Null cells");
            return;
        }

        // Shuffle the list of monsters.
        var rng = new Random();
        for (int i = 0; i < BoardSize; i++)
        {
            for (int j = 0; j < BoardSize; j++)
            {
                int index = rng.Next(monsters.Count); // Get a random index
                BingoMonster selectedMonster = monsters[index]; // Select a monster

                Cells[i, j] = new BingoCell
                {
                    Monster = selectedMonster,
                    WeaponTypeBonus = (FrontierWeaponType)rng.Next(Enum.GetValues(typeof(FrontierWeaponType)).Length),
                };
            }
        }

        if (difficulty == Difficulty.Extreme)
        {
            Cells[BoardSize / 2, BoardSize / 2] = new BingoCell
            {
                Monster = BingoMonsters.DifficultyBingoMonster[Difficulty.Hard].FirstOrDefault(monster => monster.Name == "Burning Freezing Elzelion"),
                WeaponTypeBonus = (FrontierWeaponType)rng.Next(Enum.GetValues(typeof(FrontierWeaponType)).Length),
            };
        }
    }

    private void StopBingo()
    {
        IsBingoRunning = false;
        RunIDs.Clear();
        Cells = new BingoCell[0, 0];
        BoardSize = 0;
        Carts = 0;
        ReceivedQuestID = 0;
        ReceivedRunID = 0;
        WeaponRerollCost = 2;
        CartsCost = 100;
    }

    /// <summary>
    /// Rerolls the weapon bonuses in each cell of the bingo board.
    /// </summary>
    private void RerollWeaponBonuses()
    {
        PlayerBingoPoints -= WeaponRerollCost;
        WeaponRerollCost *= 2;
        // TODO upgrades affecting cost.
    }

    /// <summary>
    /// Buys the cart.
    /// </summary>
    private void BuyCart()
    {
        PlayerBingoPoints -= CartsCost;
        CartsCost *= 2;
        Carts += 1;
        // TODO upgrades affecting cost.
    }

    [RelayCommand(CanExecute = nameof(IsBingoNotRunning))]
    private void Transcend()
    {
        // var AllGems = new List<ChallengeAncientDragonPart>();

        // Show the player the gems that they have not yet obtained
        //var availableGems = AllGems.Where(g => !g.IsObtained).ToList();

        // Allow the player to choose a gem
        //var chosenGem = ChooseGem(availableGems);

        // Add the chosen gem to the player's gauntlet
        // Player.Gauntlet.Gems.Add(chosenGem);

        // Reset the player's progress
    }

    [RelayCommand(CanExecute = nameof(IsBingoRunning))]
    private void ShuffleBingoCells()
    {

    }

    [RelayCommand(CanExecute = nameof(IsBingoRunning))]
    private void SelectWeaponOrder()
    {

    }

    [RelayCommand(CanExecute = nameof(IsBingoRunning))]
    private void SelectCellsOrder()
    {

    }

    [RelayCommand(CanExecute = nameof(IsBingoRunning))]
    private void WeaponReroll()
    {
        if (BingoServiceInstance.SpendBingoPoints(WeaponRerollCost))
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

    [RelayCommand(CanExecute = nameof(IsBingoRunning))]
    private void CartsBuy()
    {
        if (BingoServiceInstance.SpendBingoPoints(WeaponRerollCost))
        {
            BuyCart();
        }
        else
        {
            var snackbar = new Snackbar(BingoWindowSnackbarPresenter)
            {
                Style = (Style)Application.Current.FindResource("CatppuccinMochaSnackBar"),
                Title = "Not enough bingo points!",
                Content = "You need more bingo points in order to buy more carts.",
                Icon = new SymbolIcon(SymbolRegular.ErrorCircle24),
                Appearance = ControlAppearance.Danger,
                Timeout = TimeSpan.FromSeconds(5),
            };
            snackbar.Show();
        }
    }

    [RelayCommand(CanExecute = nameof(IsBingoNotRunning))]
    private void SetPoints()
    {
        // PlayerBingoPoints = BingoServiceInstance.SetPlayerBingoPoints(99_999);
    }

    public bool IsBingoNotRunning()
    {
        return !IsBingoRunning;
    }

    public bool BingoNotRunning => IsBingoNotRunning();

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

    // TODO
    private ChallengeAncientDragonPart ChooseGem(List<ChallengeAncientDragonPart> availableGems)
    {
        return new ChallengeAncientDragonPart();
    }
}
