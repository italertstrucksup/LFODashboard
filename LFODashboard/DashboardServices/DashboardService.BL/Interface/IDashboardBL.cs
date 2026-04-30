using Common.Core;
using System.Threading.Tasks;

namespace DashboardService.BL.Interface
{
    public interface IDashboardBL
    {
        Task<ApiResponse<object>> GetDashboardDataAsync();
    }
}
