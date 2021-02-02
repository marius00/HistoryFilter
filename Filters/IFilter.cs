using System.Collections.Generic;

namespace HistoryFilter.Filters {
    public interface IFilter {
        void Purge();

        void SetMasks(List<string> filters);
    }
}
