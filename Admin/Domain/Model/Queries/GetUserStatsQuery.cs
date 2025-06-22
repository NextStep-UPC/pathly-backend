using Admin.Domain.Model.Entities;
namespace Admin.Domain.Model.Queries
{
    public class GetUserStatsQuery
    {
        public UserStats Execute()
        {
            int totalUsers = 120;
            int activeUsers = 30;
            double percentage = (double)activeUsers / totalUsers * 100;

            return new UserStats
            {
                Count = totalUsers,
                Percentage = (int)Math.Round(percentage) 
            };
        }

    }
}
