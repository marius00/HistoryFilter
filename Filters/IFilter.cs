using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HistoryFilter.Filters {
    public interface IFilter {
        void Purge();
    }
}
