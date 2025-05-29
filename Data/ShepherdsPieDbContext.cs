using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ShepherdsPie.Models;

namespace ShepherdsPie.Data;

public class ShepherdsPieDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;

    public DbSet<Order> Orders { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Deliverer> Deliverers { get; set; }
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Size> Sizes { get; set; }
    public DbSet<Cheese> Cheeses { get; set; }
    public DbSet<Sauce> Sauces { get; set; }
    public DbSet<Topping> Toppings { get; set; }
    public DbSet<PizzaTopping> PizzaToppings { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    public ShepherdsPieDbContext(DbContextOptions<ShepherdsPieDbContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
    {
        Id = "role-admin-guid-0001",
        Name = "Admin",
        NormalizedName = "ADMIN"
    });

    modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole
    {
        Id = "role-employee-guid-0002",
        Name = "Employee",
        NormalizedName = "EMPLOYEE"
    });

    var passwordHasher = new PasswordHasher<IdentityUser>();

    // Seed Identity Users first
    modelBuilder.Entity<IdentityUser>().HasData(
        new IdentityUser
        {
            Id = "e1-id-user",
            UserName = "jamie.smith",
            NormalizedUserName = "JAMIE.SMITH",
            Email = "jamie.smith@example.com",
            NormalizedEmail = "JAMIE.SMITH@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null, "StrongPassword1!")
        },
        new IdentityUser
        {
            Id = "e2-id-user",
            UserName = "morgan.taylor",
            NormalizedUserName = "MORGAN.TAYLOR",
            Email = "morgan.taylor@example.com",
            NormalizedEmail = "MORGAN.TAYLOR@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null, "StrongPassword2!")
        },
        new IdentityUser
        {
            Id = "e3-id-user",
            UserName = "alex.johnson",
            NormalizedUserName = "ALEX.JOHNSON",
            Email = "alex.johnson@example.com",
            NormalizedEmail = "ALEX.JOHNSON@EXAMPLE.COM",
            EmailConfirmed = true,
            PasswordHash = passwordHasher.HashPassword(null, "StrongPassword3!")
        }
    );

    // Then seed UserProfiles referencing the above users
    modelBuilder.Entity<UserProfile>().HasData(
        new UserProfile
        {
            Id = 1,
            IdentityUserId = "e1-id-user",
            FirstName = "Jamie",
            LastName = "Smith",
            Address = "123 Pizza Lane"
        },
        new UserProfile
        {
            Id = 2,
            IdentityUserId = "e2-id-user",
            FirstName = "Morgan",
            LastName = "Taylor",
            Address = "456 Dough Street"
        },
        new UserProfile
        {
            Id = 3,
            IdentityUserId = "e3-id-user",
            FirstName = "Alex",
            LastName = "Johnson",
            Address = "789 Crust Avenue"
        }
    );

    // Link Employee users to Role Employee
    modelBuilder.Entity<IdentityUserRole<string>>().HasData(
        new IdentityUserRole<string>
        {
            RoleId = "role-employee-guid-0002",
            UserId = "e1-id-user"
        },
        new IdentityUserRole<string>
        {
            RoleId = "role-employee-guid-0002",
            UserId = "e2-id-user"
        },
        new IdentityUserRole<string>
        {
            RoleId = "role-employee-guid-0002",
            UserId = "e3-id-user"
        }
    );


    modelBuilder.Entity<Size>().HasData(
        new Size { Id = 1, Name = "Small (10\")" },
        new Size { Id = 2, Name = "Medium (14\")" },
        new Size { Id = 3, Name = "Large (18\")" }
    );

    modelBuilder.Entity<Cheese>().HasData(
        new Cheese { Id = 1, Name = "Buffalo Mozzarella" },
        new Cheese { Id = 2, Name = "Four Cheese" },
        new Cheese { Id = 3, Name = "Vegan" },
        new Cheese { Id = 4, Name = "None (cheeseless)" }
    );

    modelBuilder.Entity<Sauce>().HasData(
        new Sauce { Id = 1, Name = "Marinara" },
        new Sauce { Id = 2, Name = "Arrabbiata" },
        new Sauce { Id = 3, Name = "Garlic White" },
        new Sauce { Id = 4, Name = "None (sauceless pizza)" }
    );

    modelBuilder.Entity<Topping>().HasData(
        new Topping { Id = 1, Name = "Sausage", Price = 0.5m },
        new Topping { Id = 2, Name = "Pepperoni", Price = 0.5m },
        new Topping { Id = 3, Name = "Mushroom", Price = 0.5m },
        new Topping { Id = 4, Name = "Onion", Price = 0.5m },
        new Topping { Id = 5, Name = "Green Pepper", Price = 0.5m },
        new Topping { Id = 6, Name = "Black Olive", Price = 0.5m },
        new Topping { Id = 7, Name = "Basil", Price = 0.5m },
        new Topping { Id = 8, Name = "Extra Cheese", Price = 0.5m }
    );

    modelBuilder.Entity<Employee>().HasData(
        new Employee { Id = 1, Name = "Jamie Smith" },
        new Employee { Id = 2, Name = "Morgan Taylor" }
    );

    modelBuilder.Entity<Deliverer>().HasData(
        new Deliverer { Id = 1, Name = "Chris Carter" },
        new Deliverer { Id = 2, Name = "Drew Lee" }
    );

    modelBuilder.Entity<Pizza>().HasData(
        new Pizza { Id = 1, SizeId = 1, CheeseId = 1, SauceId = 1, Price = 10.00m },
        new Pizza { Id = 2, SizeId = 2, CheeseId = 2, SauceId = 2, Price = 12.00m },
        new Pizza { Id = 3, SizeId = 3, CheeseId = 4, SauceId = 4, Price = 15.00m }
    );

    modelBuilder.Entity<PizzaTopping>().HasData(
        new PizzaTopping { Id = 1, PizzaId = 1, ToppingId = 2 },
        new PizzaTopping { Id = 2, PizzaId = 1, ToppingId = 4 },
        new PizzaTopping { Id = 3, PizzaId = 2, ToppingId = 1 },
        new PizzaTopping { Id = 4, PizzaId = 2, ToppingId = 5 },
        new PizzaTopping { Id = 5, PizzaId = 3, ToppingId = 6 }
    );

    modelBuilder.Entity<Order>().HasData(
        new Order
        {
            Id = 1,
            EmployeeId = 1,
            DelivererId = 1,
            TotalCost = 11,
            Tip = 1,
            TableNumber = 5,
            OrderTime = "2024-05-28T18:00:00",
        },
        new Order
        {
            Id = 2,
            EmployeeId = 2,
            DelivererId = 2,
            TotalCost = 13,
            Tip = 1,
            TableNumber = 3,
            OrderTime = "2024-05-28T19:00:00",
        }
    );
}
}
