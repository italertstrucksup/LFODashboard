using System.Data;
using System.Threading.Tasks;

namespace DashboardService.DAL.Interface
{
    public interface IDashboardDAL
    {
        Task<DataTable> GetDashboardDataAsync();
    }
}
