using System.Collections.Generic;

namespace Lykke.Service.Dwh.Client
{
    public interface IDwhClient
    {
        ResponceDataSet Get();

        ResponceDataSet Call(Dictionary<string, string> parameters);
    }
}
