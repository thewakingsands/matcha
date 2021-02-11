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
        private static Models.ConfigFormatter Config => Matcha.Config.Instance.Formatter;
        private static string GetFateText(int code)
        {
            if (!Data.Instance.Fates.TryGetValue(code, out var fate))
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            if (Config.Fate.Level)
            {
                sb.AppendFormat("{0}级 ", fate.Level);
            }

            if (Config.Fate.Name)
            {
                sb.Append(fate.Name);
            }

            return sb.ToString();
        }

        private static string GetDynamicEventText(DynamicEventDTO dto)
        {
            if (Config.CriticalEngagement.Name)
            {
                if (Data.Instance.DynamicEvents.TryGetValue(dto.Event, out var dynamicEvent))
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
            if (Config.Instance.Type && Data.Instance.InstanceTypes.TryGetValue(instance.Type, out var instanceType))
            {
                sb.Append(instanceType);
                sb.Append(' ');
            }

            if (Config.Instance.Level)
            {
                sb.AppendFormat("{0}级 ", instance.Level);
            }

            if (Config.Instance.Item && instance.ItemLevelSync != 0)
            {
                sb.AppendFormat("装等{0} ", instance.ItemLevelSync);
            }

            if (Config.Instance.Name)
            {
                sb.Append(instance.Name);
            }

            return sb.ToString();
        }

        private static string GetZoneText(InitZoneDTO dto)
        {
            StringBuilder sb = new StringBuilder();
            if (Config.Zone.Name)
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

            if (Config.Zone.Instance && dto.Instance != 0)
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
            if (!Config.Fish.Bite && !Config.Fish.BiteType)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            if (Config.Fish.Bite)
            {
                sb.Append("咬钩 ");
            }

            if (Config.Fish.BiteType)
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
