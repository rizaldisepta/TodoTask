using System;
using System.Threading.Tasks;
using ToDoListApp.Data;
using ToDoListApp.Models;
using ToDoListApp.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;


namespace ToDoListApp.UnitTests
{
    public class ToDoListTaskServiceTests
    {
        //Test: Verify the AddTaskAsynch() method successfully adds a new task to databse.
        [Fact]
        public async Task AddTaskSuccessTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_AddTask").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {

                var service = new ToDoListTaskService(context);

                var testUser = new ApplicationUser
                {
                    Id = "TestUser0",
                    UserName = "TestUser0@example.com"
                };

                await service.AddTaskAsync(new ToDoListTask
                {
                    Title = "TestTitle",
                    Details = "Test Details"
                }, testUser);

                var tasksTotal = await context.Tasks.CountAsync();
                Assert.Equal(1, tasksTotal);

                var task = await context.Tasks.FirstAsync();
                Assert.Equal("TestTitle", task.Title);
                Assert.Equal("Test Details", task.Details);
                Assert.Equal(false, task.IsDone);

            }
         
        }



        //Test: Verify the MarkDoneAsynch() method successfully sets the tasks IsDone value to TRUE.
        [Fact]
        public async Task MarkDoneSuccessTest()
        {
            // ...
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_MarkTaskDone1").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ToDoListTaskService(context);

                var testUser = new ApplicationUser
                {
                    Id = "TestUser0",
                    UserName = "TestUser0@example.com"
                };


                await service.AddTaskAsync(new ToDoListTask
                {
                    Title = "TestTitle",
                }, testUser); 

        

                var task = await context.Tasks.FirstAsync();

                await service.MarkDoneAsync(task.Id, testUser);

                Assert.Equal(true, task.IsDone);
            }

        }


        //Test: Verify the MarkDoneAsynch() method does not set the tasks IsDone value to TRUE when the incorrect Guid is used.
        [Fact]
        public async Task MarkDoneInvalidGuidTest()
        {
            // ...
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_MarkTaskDone2").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ToDoListTaskService(context);

                var testUser = new ApplicationUser
                {
                    Id = "TestUser0",
                    UserName = "TestUser0@example.com"
                };

                await service.AddTaskAsync(new ToDoListTask
                {
                    Title = "TestTitle",
                }, testUser);

                var task = await context.Tasks.FirstAsync();

                Guid randomGuid = Guid.NewGuid();

                await service.MarkDoneAsync(randomGuid, testUser);

                Assert.Equal(false, task.IsDone);
            }

        }

        //Test: Verify the MarkDoneAsynch() method does not set the tasks IsDone value to TRUE when the incorrect UserId is used.
        [Fact]
        public async Task MarkDoneInvalidUserIdTest()
        {
            // ...
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_MarkTaskDone3").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ToDoListTaskService(context);

                var testUser1 = new ApplicationUser
                {
                    Id = "TestUser1",
                    UserName = "TestUser1@example.com"
                };


                await service.AddTaskAsync(new ToDoListTask
                {
                    Title = "Testing2",
                }, testUser1);



                var task = await context.Tasks.FirstAsync();


                var testUser2 = new ApplicationUser
                {
                    Id = "TestUser2",
                    UserName = "TestUser2@example.com"
                };

                await service.MarkDoneAsync(task.Id, testUser2);


                Assert.Equal(false, task.IsDone);
            }

        }

        //Test: Verify the GetUnfinishedTasksAsynch()  method returns only the specifies User's tasks.
        [Fact]
        public async Task GetUnfinishedTasksCorrectUserTest()
        {
            // ...
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "Test_CorrectUser").Options;

            // Set up a context (connection to the "DB") for writing
            using (var context = new ApplicationDbContext(options))
            {
                var service = new ToDoListTaskService(context);

                var testUser1 = new ApplicationUser
                {
                    Id = "TestUser1",
                    UserName = "TestUser1@example.com"
                };
                var testUser2 = new ApplicationUser
                {
                    Id = "TestUser2",
                    UserName = "TestUser2@example.com"
                };

                //Adding 3 tasks for user1, and one task for user2

                await service.AddTaskAsync(new ToDoListTask
                {
                    Title = "TestingUser1Task1",
                }, testUser1);

                await service.AddTaskAsync(new ToDoListTask
                {
                    Title = "TestingUser1Task2",
                }, testUser1);

                await service.AddTaskAsync(new ToDoListTask
                {
                    Title = "TestingUser1Task3",
                }, testUser1);


                await service.AddTaskAsync(new ToDoListTask
                {
                    Title = "TestingUser2",
                }, testUser2);
                

                ToDoListTask[] taskArrayUser1  = await service.GetUnfinishedTasksAsync(testUser1);
                ToDoListTask[] taskArrayUser2 = await service.GetUnfinishedTasksAsync(testUser2);


                foreach (ToDoListTask task in taskArrayUser1)
                {
                    Assert.Equal("TestUser1", task.UserId);
                }

                Assert.Equal("TestUser2", taskArrayUser2[0].UserId);
            }

        }
    }
}