using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using Newtonsoft.Json;

namespace HistoryFilter.Settings {
    class SettingsReader {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SettingsReader));

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Culture = System.Globalization.CultureInfo.InvariantCulture,
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        private readonly AppSettings _data;
        private readonly string _persistentStorage;

        public SettingsReader(AppSettings data, string persistentStorage) {
            _persistentStorage = persistentStorage;
            _data = data;

            _data.OnMutate += (_, __) => Persist();
        }

        private void Persist() {
            string json = JsonConvert.SerializeObject(_data, Settings);
            try {
                File.WriteAllText(_persistentStorage, json);
            }
            catch (Exception ex) {
                Logger.Warn("Error storing settings, once of twice of these is unfortunate but can live with it..", ex);
            }
        }

        public AppSettings GetSettings => _data;

        public static SettingsReader Load(string filename) {
            if (File.Exists(filename)) {
                try {
                    string json = File.ReadAllText(filename);
                    var template = JsonConvert.DeserializeObject<AppSettings>(json, Settings);
                    if (template != null) {
                        return new SettingsReader(template, filename);
                    }
                }
                catch (IOException ex) {
                    Logger.Error($"Error reading settings from {filename}, discarding settings.", ex);
                }
                catch (JsonReaderException ex) {
                    Logger.Error($"Error parsing settings from {filename}, discarding settings.", ex);
                }
            }

            Logger.Info("Could not find settings JSON, defaulting to no settings.");
            return new SettingsReader(new AppSettings {Prefixes = new List<string>()}, filename);
        }
    }
}