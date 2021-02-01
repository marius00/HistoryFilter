using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryFilter.Settings {
    public class AppSettings {

        public event EventHandler OnMutate;
        private List<string> _prefixes;

        public void AddPrefix(string prefix) {
            if (_prefixes == null) {
                _prefixes = new List<string>();
            }

            if (!_prefixes.Contains(prefix)) {
                _prefixes.Add(prefix);
            }

            OnMutate?.Invoke(null, null);
        }

        public void RemovePrefix(string prefix) {
            _prefixes?.Remove(prefix);

            OnMutate?.Invoke(null, null);
        }


        public List<string> Prefixes {
            get => _prefixes;
            set {
                _prefixes = value;
                OnMutate?.Invoke(null, null);
            }
        }
    }
}
