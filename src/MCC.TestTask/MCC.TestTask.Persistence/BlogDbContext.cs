using MCC.TestTask.Domain;
using Microsoft.EntityFrameworkCore;

namespace MCC.TestTask.Persistance;

public class BlogDbContext : DbContext
{
    public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Community> Communities { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Community>().HasMany(c => c.Administrators).WithMany();
        builder.Entity<Community>().HasMany(c => c.Subscribers).WithMany(u => u.SubscribedTo);
        builder.Entity<Community>().HasMany(c => c.Posts).WithOne(p => p.Community);
        builder.Entity<Community>().HasOne(c => c.Creator);

        builder.Entity<Post>().HasOne(p => p.Author).WithMany();
        builder.Entity<Post>().HasMany(p => p.LikedBy).WithMany();
        builder.Entity<Post>().HasMany(p => p.Tags).WithMany();
                    
        builder.Entity<Comment>().HasMany(c => c.Replies).WithOne(r => r.Parent);
    }
}