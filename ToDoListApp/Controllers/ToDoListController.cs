using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoListApp.Services;
using ToDoListApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ToDoListApp.Controllers
{
    [Authorize]
    public class ToDoListController : Controller
    {
        private readonly IToDoListTaskService _toDoListTaskService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoListController(IToDoListTaskService toDoListTaskService, UserManager<ApplicationUser> userManager)
        {
            _toDoListTaskService = toDoListTaskService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var tasks = await _toDoListTaskService.GetUnfinishedTasksAsync(currentUser);

            var model = new ToDoListViewModel()
            {
                Tasks = tasks
            };

            return View(model);
            
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTask(ToDoListTask newTask)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();


            var successful = await _toDoListTaskService.AddTaskAsync(newTask, currentUser);
            if (!successful)
            {
                return BadRequest("Failed to add task.");
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty)
            {
                return RedirectToAction("Index");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return RedirectToAction("Index");

            var successful = await _toDoListTaskService.MarkDoneAsync(id, currentUser);
            if (!successful)
            {
                return BadRequest("Could not mark task as done.");
            }

            return RedirectToAction("Index");
        }

    }
}