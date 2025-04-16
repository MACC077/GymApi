using GymControlAPI.Models;

namespace GymControlAPI.Repositories.Interfaces
{
    public interface IPlan
    {
        public Task<Plan>GetPlanById(int id, bool incluirinactivos = false);
        public Task<IEnumerable<Plan>> GetAllPlanes(bool incluirinactivos = false);
        public Task<Plan>AddPlan(Plan plan);
        public Task<Plan> UpdatePlan(Plan plan);
        public Task<bool> ChangeStatePlan(int id,bool activo);
        public Task<bool> DeletePlan(int id);
        public Task<bool> SaveChangesAsync();
    }
}
