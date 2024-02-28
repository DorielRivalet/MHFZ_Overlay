// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models.Collections;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using MHFZ_Overlay.Models.Structures;

/// <summary>
/// The patches list.
/// </summary>
public static class QuestsGamePatches
{
    public static ReadOnlyDictionary<string, Dictionary<GamePatch, GamePatchLanguage>> datHashInfo { get; } = new (new Dictionary<string, Dictionary<GamePatch, GamePatchLanguage>>
    {
        { "593C418FA1408B76F655F0BB8D7DF8AA307CD89613A7734EB057A175825A4DA5", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Vanilla, GamePatchLanguage.JA }
            }
        },
        { "3E6FD419AD668AA973454B9AE9F1C49C739A0CA41FCE15CBF1DB1D2068E8C29D", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Seph, GamePatchLanguage.EN }
            }
        },
        { "F65080754B99BED5BE91BF1B4B0EC8EE61D7DED06831E3E7824F62B63F60E624", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Ezemania, GamePatchLanguage.FR }
            }
        },
        { "E1B0C47FE8BFA222BE1284CC232DAB63459574F40F8DA90FAE7FF20638480F96", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Otyav1_1, GamePatchLanguage.JA }
            }
        },
        { "735EFD86140E835E8DA2670CEA15A19154AE37ED046CBBB5A8918C39FC88B9E8", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Otyav1_1, GamePatchLanguage.EN }
            }
        },
        { "2B328D91626DB1DC8DD3FB9520D1AE52CF0C6F412B595D5795298DDC550FD06D", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Otyav1_1, GamePatchLanguage.FR }
            }
        },
        { "EE39C4776A1CF1271D0728D450F723B035144503298DB4C771C0376BF43AD4B8", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Mezelounge, GamePatchLanguage.EN }
            }
        },
    });

    public static ReadOnlyDictionary<string, Dictionary<GamePatch, GamePatchLanguage>> emdHashInfo { get; } = new(new Dictionary<string, Dictionary<GamePatch, GamePatchLanguage>>
    {
        { "E68E03F1274D9D055CC8E42B0427BB99B0CEBFE9C905F951900B991F4CC8AFD1", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Vanilla, GamePatchLanguage.JA }
            }
        },
        { "5B70CE51EC6F95337B615C76C8BE8A0C42B16B14ED698739ADC097C4C507503E", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Otyav1_1, GamePatchLanguage.JA }
            }
        },
        { "1755B8C5DA0D488901BF4FC7179E0B036AC6F9C936C7CA5911C693119173F8E5", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Mezelounge, GamePatchLanguage.EN }
            }
        },
        { "A90B90E6A66F1983C1807DB1298EF2BF94BFD05681021307C8F4213BBE31B890", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.FiveMusous, GamePatchLanguage.ZH }
            }
        },
    });

    public static ReadOnlyDictionary<string, Dictionary<GamePatch, GamePatchLanguage>> mhfodllHashInfo { get; } = new(new Dictionary<string, Dictionary<GamePatch, GamePatchLanguage>>
    {
        { "69B65205B1F23A6989395CD811C414D410FE43D7ACD3C33A565E2C564AD184F4", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Standard, GamePatchLanguage.JA }
            }
        },
        { "A21322CA5864D6A4C8599FD4522FDBCC8997225C05D82D958D9AEBA2678ADFC7", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Standard, GamePatchLanguage.EN }
            }
        },
        { "23177AECAEB8222392461F5B6F714D810953329E4622E948F387B9B318D97589", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Mezelounge, GamePatchLanguage.EN }
            }
        },
        { "514DC802AABE3C7709EC1AABEB25B586839EE86E15CC9C569C87CC459E9206A0", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Tenrou, GamePatchLanguage.EN }
            }
        },
    });

    public static ReadOnlyDictionary<string, Dictionary<GamePatch, GamePatchLanguage>> mhfohddllHashInfo { get; } = new(new Dictionary<string, Dictionary<GamePatch, GamePatchLanguage>>
    {
        { "95C580195F4080D2E9582C8C9DF36ABEB280476E088B6366583C5F138DA8F301", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Standard, GamePatchLanguage.JA }
            }
        },
        { "01F980A4B892CA0DBEBAD8EB54B224D080B32253BB8E793E23CD060089003D25", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Standard, GamePatchLanguage.EN }
            }
        },
        { "6FD7A31663D7AF23D9A8D903E4FFB84F45390D5AEE7BC6C70347D2F17988FA02", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Mezelounge, GamePatchLanguage.EN }
            }
        },
        { "5A65673537E97522E4767A32EAA8622312AC118AF3ED64F02D56751A27FE0B90", new Dictionary<GamePatch, GamePatchLanguage>()
            {
                { GamePatch.Tenrou, GamePatchLanguage.EN }
            }
        },
    });
}
