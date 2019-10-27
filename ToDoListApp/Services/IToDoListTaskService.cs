using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoListApp.Models;

namespace ToDoListApp.Services
{
    public interface IToDoListTaskService
    {
        Task<ToDoListTask[]> GetUnfinishedTasksAsync(ApplicationUser user);

        Task<bool> AddTaskAsync(ToDoListTask newTask, ApplicationUser user);

        Task<bool> MarkDoneAsync(Guid id, ApplicationUser user);
    }
}