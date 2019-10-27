using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListApp.Data;
using ToDoListApp.Models;
using Microsoft.EntityFrameworkCore;


namespace ToDoListApp.Services
{
    public class ToDoListTaskService : IToDoListTaskService
    {
        private readonly ApplicationDbContext _context;

        public ToDoListTaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ToDoListTask[]> GetUnfinishedTasksAsync(ApplicationUser user)
        {
            return await _context.Tasks
                .Where(x => x.IsDone == false && x.UserId == user.Id)
                .ToArrayAsync();
        }

        public async Task<bool> AddTaskAsync(ToDoListTask newTask, ApplicationUser user)
        {
            newTask.Id = Guid.NewGuid();
            newTask.IsDone = false;
            newTask.DateModified = DateTime.Now;
            newTask.UserId = user.Id;

            _context.Tasks.Add(newTask);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<bool> MarkDoneAsync(Guid id, ApplicationUser user)
        {
            var task = await _context.Tasks
                .Where(x => x.Id == id && x.UserId == user.Id)
                .SingleOrDefaultAsync();

            if (task == null) return false;

            task.IsDone = true;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; 
        }
    }
}