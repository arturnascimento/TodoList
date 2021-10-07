﻿using BluenitosToDo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BluenitosToDo.Data
{
    public class TodoContext: IdentityDbContext
    {
        public TodoContext(DbContextOptions options) : base(options) { }

        public DbSet<TodoModel> TodoModel { get; set; }
        public DbSet<Users> User { get; set; }
    }
}
