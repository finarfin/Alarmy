﻿using NDesk.Options;
using System;
using System.IO;
using System.Linq;

namespace Alarmy.Infrastructure
{
    class Settings
    {
        public string AlarmSoundFile { get; private set; }
        public double CheckInterval { get; private set; }
        public bool EnableSound { get; private set; }
        public bool PopupOnAlarm { get; private set; }
        public bool SmartAlarm { get; private set; }
        public bool StartHidden { get; private set; }
        public int AlarmListGroupInterval { get; set; }
        public string AlarmDatabasePath { get; private set; }

        public Settings()
        {
            this.AlarmSoundFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "alarm.wav");
            this.CheckInterval = 1000;
            this.EnableSound = true;
            this.PopupOnAlarm = false;
            this.SmartAlarm = true;
            this.StartHidden = false;
            this.AlarmListGroupInterval = 15;
            this.AlarmDatabasePath = Environment.ExpandEnvironmentVariables("%TEMP%\\alarms.db");

            new OptionSet()
            {
                { "a|alarmsound=", x => this.AlarmSoundFile = x },
                { "c|checkinterval=", x => this.CheckInterval = double.Parse(x)},
                { "m|mute", x => this.EnableSound = false },
                { "p|popupOnalarm", x => this.PopupOnAlarm = true },
                { "s|smartalarm", x => this.SmartAlarm = false },
                { "h|hidden", x => this.StartHidden = true },
                { "g|alarmListGroupInterval=", x => this.AlarmListGroupInterval = int.Parse(x)},
                { "db|database=", x => this.AlarmDatabasePath = x }
            }.Parse(Environment.GetCommandLineArgs().Skip(1));
        }
    }
}
