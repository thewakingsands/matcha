// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System;
    using System.Text;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.DTO;

    internal class Formatter
    {
        private static Models.ConfigFormatter FormatterConfig => Config.Instance.Formatter;
        private static string GetFateText(int code)
        {
            if (!Data.Instance.Fates.TryGetValue(code, out var fate))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            if (FormatterConfig.Fate.Level)
            {
                sb.AppendFormat("{0}级 ", fate.Level);
            }

            if (FormatterConfig.Fate.Name)
            {
                sb.Append(fate.Name);
            }

            return sb.ToString();
        }

        private static string GetDynamicEventText(DynamicEventDTO dto)
        {
            if (FormatterConfig.CriticalEngagement.Name)
            {
                var id = dto.Zone * 1000 + dto.Event;
                if (Data.Instance.DynamicEvents.TryGetValue(id, out var dynamicEvent))
                {
                    return dynamicEvent.Name.ToString();
                }
            }

            return null;
        }

        private static string GetInstanceText(MatchAlertDTO dto)
        {
            if (dto.Roulette != 0)
            {
                if (Data.Instance.Roulettes.TryGetValue(dto.Roulette, out var rouletteName))
                {
                    return rouletteName.ToString();
                }
                else
                {
                    return "未知随机任务";
                }
            }

            if (!Data.Instance.Instances.TryGetValue(dto.Instance, out var instance))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            if (FormatterConfig.Instance.Type && Data.Instance.InstanceTypes.TryGetValue(instance.Type, out var instanceType))
            {
                sb.Append(instanceType);
                sb.Append(' ');
            }

            if (FormatterConfig.Instance.Level)
            {
                sb.AppendFormat("{0}级 ", instance.Level);
            }

            if (FormatterConfig.Instance.Name)
            {
                sb.Append(instance.Name);
            }

            return sb.ToString();
        }

        private static string GetZoneText(InitZoneDTO dto)
        {
            StringBuilder sb = new StringBuilder();
            if (FormatterConfig.Zone.Name)
            {
                if (Data.Instance.Territories.TryGetValue(dto.Zone, out var territoryData))
                {
                    sb.Append(territoryData.ToString());
                    sb.Append(' ');
                }
                else
                {
                    sb.Append("未知地区 ");
                }
            }

            if (FormatterConfig.Zone.Instance && dto.Instance != 0)
            {
                if (Data.Instance.Instances.TryGetValue(dto.Instance, out var instanceData))
                {
                    sb.Append("任务 ");
                    sb.Append(instanceData.Name);
                }
                else
                {
                    sb.Append("未知副本");
                }
            }

            return sb.ToString();
        }

        private static string GetFishText(FishBiteDTO dto)
        {
            if (!FormatterConfig.Fish.Bite && !FormatterConfig.Fish.BiteType)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            if (FormatterConfig.Fish.Bite)
            {
                sb.Append("咬钩 ");
            }

            if (FormatterConfig.Fish.BiteType)
            {
                switch (dto.Type)
                {
                    case 1:
                        sb.Append("轻杆");
                        break;
                    case 2:
                        sb.Append("中杆");
                        break;
                    case 3:
                        sb.Append("重杆");
                        break;
                }
            }

            return sb.ToString();
        }

        private static string GetTreasureResultText(TreasureResultDTO dto)
        {
            if (!FormatterConfig.Treasure.Result)
            {
                return null;
            }

            switch (dto.Value)
            {
                case "wheel-low":
                    return "下级召唤";
                case "wheel-medium":
                    return "中级召唤";
                case "wheel-high":
                    return "上级召唤";
                case "wheel-shift":
                    return "召唤式变动";
                case "wheel-special":
                    return "下级召唤";
                case "wheel-end":
                    return "召唤失败";
                case "gate-open":
                    return "开门";
                case "gate-fail":
                    return "失败";
            }

            return null;
        }

        public static string GetEventText(BaseDTO dto)
        {
            switch (dto.EventType)
            {
                case EventType.MatchAlert:
                    return GetInstanceText((MatchAlertDTO)dto);
                case EventType.Fate:
                    var fateDto = (FateDTO)dto;
                    if (fateDto.Type == "start")
                    {
                        return GetFateText(fateDto.Fate);
                    }
                    else if (fateDto.Type == "progress")
                    {
                        var fateText = GetFateText(fateDto.Fate);
                        if (string.IsNullOrEmpty(fateText))
                        {
                            return null;
                        }

                        if (fateDto.Progress != 0)
                        {
                            return string.Format("{0} 进度 {1}%", fateText, fateDto.Progress);
                        }
                        else
                        {
                            return fateText;
                        }
                    }

                    return null;
                case EventType.FishBite:
                    return GetFishText((FishBiteDTO)dto);
                case EventType.InitZone:
                    var zoneText = GetZoneText((InitZoneDTO)dto);
                    if (string.IsNullOrEmpty(zoneText))
                    {
                        return null;
                    }

                    return zoneText;
                case EventType.DynamicEvent:
                    return GetDynamicEventText((DynamicEventDTO)dto);
                case EventType.TreasureResult:
                    return GetTreasureResultText((TreasureResultDTO)dto);
                default:
                    return string.Format("{0} {1}", Enum.GetName(typeof(EventType), dto.EventType), dto.ToJSON());
            }
        }

        private static StringBuilder InitLogLine(string category)
        {
            var sb = new StringBuilder();
            sb.Append("00|");
            sb.Append(DateTime.Now.ToString("O"));
            sb.Append("|0|Matcha#");
            sb.Append(Data.Version);
            sb.Append('#');
            sb.Append(Config.GetLanguageString());
            sb.Append('-');
            sb.Append(category);
            sb.Append('|');

            return sb;
        }

        public static string GetLog(BaseDTO dto)
        {
            var sb = InitLogLine(Enum.GetName(typeof(EventType), dto.EventType));
            sb.Append(dto.ToJSON());

            return sb.ToString();
        }
    }
}
