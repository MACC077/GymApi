using GymControlAPI.Data;
using GymControlAPI.Models;
using GymControlAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymControlAPI.Repositories
{
    public class PlanRepo : IPlan
    {
        private readonly GymDbContext _context;
        public PlanRepo(GymDbContext context) 
        { 
            _context = context;
        }

        public async Task<Plan> GetPlanById(int id, bool incluirinactivos = false) 
        {
            return await _context.Planes.FirstOrDefaultAsync(p => p.Id ==id && (p.Activo || incluirinactivos));    
        }

        public async Task<IEnumerable<Plan>> GetAllPlanes(bool incluirinactivos = false) 
        {
            return await _context.Planes.Where(p => p.Activo || incluirinactivos).ToListAsync();
        }

        public async Task<Plan> AddPlan(Plan plan) 
        {
            await _context.AddAsync(plan);
            await SaveChangesAsync();
            return plan;
        }

        public async Task<Plan> UpdatePlan(Plan plan) 
        {
            _context.Update(plan);
            await SaveChangesAsync();
            return plan;
        }

        public async Task<bool> ChangeStatePlan(int id, bool activo) 
        {
            var plan = await GetPlanById(id, true);

            if (plan == null) 
            { 
                return false;
            }

            plan.Activo = activo;
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeletePlan(int id) 
        {

            var plan = await GetPlanById(id);

            if (plan == null) 
            { 
                return false;
            }

            _context.Remove(plan);
            await SaveChangesAsync();
            return true;
        }

        public async Task<bool> SaveChangesAsync() 
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
