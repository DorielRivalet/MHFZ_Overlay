// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
using System;

namespace MHFZ_Overlay.UI.Class;

//TODO: ORM
public class MezFes
{
    public long MezFesID { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; } = string.Empty;
    public long? MezFesMinigameID { get; set; }
    public long Score { get; set; } = 0;
    
}
