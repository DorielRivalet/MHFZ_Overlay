// © 2023 The mhfz-overlay developers.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.

namespace MHFZ_Overlay.Models;

public sealed class OverlaySetting
{
    public string? Value { get; set; } = "null";

    public string? DefaultValue { get; set; } = "null";

    public string PropertyType { get; set; } = "null";

    public string IsReadOnly { get; set; } = "false";

    public string? Provider { get; set; } = "null";

    public string ProviderName { get; set; } = "null";

    public string ProviderApplicationName { get; set; } = "null";

    public string ProviderDescription { get; set; } = "null";
}
