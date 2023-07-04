// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MHFZ_Overlay.UI.Class;

public class QuestAttempts
{
    public long QuestAttemptsID { get; set; }
    public long QuestID { get; set; } = 0;
    public long WeaponTypeID { get; set; } = 0;
    public string ActualOverlayMode { get; set; } = string.Empty;
    public long Attempts { get; set; } = 0;
}
