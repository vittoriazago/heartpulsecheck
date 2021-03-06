﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace HealthCheck.Helper.Models
{
    public class UIHealthReport
    {
        public UIHealthStatus Status { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public string Version { get; set; }
        public Dictionary<string, UIHealthReportEntry> Entries { get; }

        public UIHealthReport(Dictionary<string, UIHealthReportEntry> entries, TimeSpan totalDuration)
        {
            Entries = entries;
            TotalDuration = TimeSpan.FromMilliseconds(0);
            var assembly = System.Reflection.Assembly.GetCallingAssembly().GetName().Version;
            Version = assembly.Major + "." + assembly.Minor + "." + assembly.Build + "." + assembly.Revision;
        }
        public static UIHealthReport CreateFrom(HealthReport report)
        {
            var uiReport = new UIHealthReport(new Dictionary<string, UIHealthReportEntry>(), report.TotalDuration)
            {
                Status = (UIHealthStatus)report.Status,
            };

            foreach (var item in report.Entries)
            {
                var entry = new UIHealthReportEntry()
                {
                    Data = item.Value.Data,
                    Description = item.Value.Description,
                    Duration = TimeSpan.FromMilliseconds(0),
                    Exception = "",
                    Status = (UIHealthStatus)item.Value.Status
                };

                if (item.Value.Exception != null)
                {
                    var message = item.Value.Exception?
                        .Message
                        .ToString();

                    entry.Exception = message;
                    entry.Description = item.Value.Description ?? message;
                }

                uiReport.Entries.Add(item.Key, entry);
            }

            return uiReport;
        }
        public static UIHealthReport CreateFrom(Exception exception)
        {
            var uiReport = new UIHealthReport(new Dictionary<string, UIHealthReportEntry>(), TimeSpan.FromSeconds(0))
            {
                Status = UIHealthStatus.Unhealthy,
            };

            const string SERVICE_NAME = "Endpoint";

            uiReport.Entries.Add(SERVICE_NAME, new UIHealthReportEntry()
            {
                Exception = exception.Message,
                Description = exception.Message,
                Duration = TimeSpan.FromSeconds(0),
                Status = UIHealthStatus.Unhealthy
            });

            return uiReport;
        }
    }
    public enum UIHealthStatus
    {
        Unhealthy = 0,
        Degraded = 1,
        Healthy = 2
    }
    public class UIHealthReportEntry
    {
        public IReadOnlyDictionary<string, object> Data { get; set; }
        public string Description { get; set; }
        public TimeSpan Duration { get; set; }
        public string Exception { get; set; }
        public UIHealthStatus Status { get; set; }
    }
}
