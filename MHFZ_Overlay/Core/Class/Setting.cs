// Copyright 2023 The mhfz-overlay Authors.
// Use of this source code is governed by a MIT license that can be
// found in the LICENSE file.
namespace MHFZ_Overlay.Core.Class
{
    public class Setting
    {
        public string Value { get; set; } = "null";
        public string DefaultValue { get; set; } = "null";
        public string PropertyType { get; set; } = "null";
        public string IsReadOnly { get; set; } = "false";
        public string Provider { get; set; } = "null";
        public string ProviderName { get; set; } = "null";
        public string ProviderApplicationName { get; set; } = "null";
        public string ProviderDescription { get; set; } = "null";
    }
}
