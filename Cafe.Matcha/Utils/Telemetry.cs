// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using Cafe.Matcha.Constant;
    using Cafe.Matcha.Models;
    using Cafe.Matcha.Views;

    internal class TelemetryWorker<T> where T : Models.TelemetryData
    {
        protected string bucketId;
        /**
         * Aggregation time in milliseconds
         */
        protected int aggregationTime = 10000;
        protected List<T> list = new List<T>();

        private object listLock = new object();

        private Task task = null;

        public TelemetryWorker(string bucketId)
        {
            this.bucketId = bucketId;
        }

        public void Send(T item)
        {
            lock (listLock)
            {
                foreach (var oldItem in list)
                {
                    if (oldItem.Equals((object)item))
                    {
#if DEBUG
                        Log.Warn($"[Telemetry] {GetType().Name} Dropping {item}, as it equals {oldItem}");
#endif
                        return;
                    }
                }

#if DEBUG
                Log.Warn($"[Telemetry] {GetType().Name} Adding {item}");
#endif
                list.Add(item);
            }

            if (task == null)
            {
                task = new Task(async () =>
                {
                    await Task.Delay(aggregationTime);

                    List<T> list;
                    lock (listLock)
                    {
                        list = new List<T>(this.list);
                        this.list.Clear();
                        task = null;
                    }

                    Telemetry.Instance.Send(bucketId, list);
                });

                task.Start();
            }
        }
    }

    internal class Telemetry
    {
        public static Telemetry Instance { get; private set; } = null;
        public static void Init()
        {
            Instance = new Telemetry();
        }

        public bool Enabled
        {
            get
            {
                var hasUUID = Config.UUID != null && Config.UUID != "no";
                return Config.Enable && hasUUID && !string.IsNullOrEmpty(Secret.TelemetryRoot);
            }
            set
            {
                if (value)
                {
                    Config.Enable = true;
                    Config.Agreement = CurrentAgreement;
                    if (string.IsNullOrEmpty(Config.UUID))
                    {
                        Config.UUID = Guid.NewGuid().ToString();
                    }
                }
                else
                {
                    Config.Enable = false;
                    Config.Agreement = "no";
                    Config.UUID = null;
                }
            }
        }

        private const string CurrentAgreement = "20220406";

        private ConfigTelemetry Config => Matcha.Config.Instance.Telemetry;

        public Telemetry()
        {
            if (Config.Agreement == null || (Config.Enable && Config.Agreement != CurrentAgreement))
            {
                var dialog = new TelemetrySetting(true);
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dialog.Topmost = true;
                dialog.Show();
            }
        }

        public void Send(string bucketId, IEnumerable<Models.TelemetryData> data)
        {
            if (!Enabled)
            {
                return;
            }

#if DEBUG
            Log.Warn($"[Telemetry] Posting to {Secret.TelemetryRoot}/{bucketId}");
#endif
            _ = Request.SendAsJson($"{Secret.TelemetryRoot}/{bucketId}/batch", "", data);
        }
    }
}
