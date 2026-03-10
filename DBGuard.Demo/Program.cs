using DBGuard.Demo;

var context = new AppDbContext();

var user = new User
{
    FirstName = "ThisNameIsTooLongMoreThanColumnLengthAllowedInDatabase",
    Email = "test@test.com"
};

context.Users.Add(user);

await context.SaveChangesAsync();
