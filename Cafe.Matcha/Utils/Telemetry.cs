// Copyright (c) FFCafe. All rights reserved.
// Licensed under the AGPL-3.0 license. See LICENSE file in the project root for full license information.

namespace Cafe.Matcha.Utils
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Windows;
    using Cafe.Matcha.Constant;
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

        private bool enabled = false;

        private void UpdateEnabled()
        {
            enabled = Config.Instance.UUID != null && Config.Instance.UUID != "no" && !string.IsNullOrEmpty(Secret.TelemetryRoot);
        }

        public Telemetry()
        {
            if (Config.Instance.UUID == null)
            {
                var dialog = new TelemetrySetting();
                dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                dialog.Topmost = true;
                dialog.ShowDialog();
            }

            UpdateEnabled();
            Config.Instance.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "UUID")
                {
                    UpdateEnabled();
                }
            };
        }

        public void Send(string bucketId, IEnumerable<Models.TelemetryData> data)
        {
            if (!enabled)
            {
                return;
            }

            _ = Request.SendAsJson($"{Secret.TelemetryRoot}/{bucketId}/batch", "", data);
        }
    }
}
