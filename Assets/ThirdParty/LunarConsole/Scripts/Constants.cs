//
//  Constants.cs
//
//  Lunar Unity Mobile Console
//  https://github.com/SpaceMadness/lunar-unity-console
//
//  Copyright 2016 Alex Lementuev, SpaceMadness.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using UnityEngine;
using System.Collections;

namespace LunarConsoleInternal
{
    public static class Constants
    {
        public static readonly string Version = "0.2.0b";
        public static readonly string UpdateJsonURL = "https://raw.githubusercontent.com/SpaceMadness/lunar-unity-console/master/Builder/updater.json";

        public static readonly string PluginName = "LunarConsole";
        public static readonly string PluginDisplayName = "Lunar Mobile Console";

        public static readonly string PluginScriptPath = "Assets/" + PluginName + "/Scripts/" + PluginName + ".cs";
    }
}