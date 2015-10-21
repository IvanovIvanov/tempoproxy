using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempoProxy
{
    public interface IFileWorker
    {
        String ReadFile();
        void UpdateFile(String newData);
    }
}
