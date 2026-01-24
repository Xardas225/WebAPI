using Microsoft.EntityFrameworkCore;
using WebAPI.Models.User;

namespace WebAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Для корректной работы наследования и базовой конфигурации
            base.OnModelCreating(modelBuilder);

            // Конфигурация таблицы Users
            modelBuilder.Entity<User>(entity =>
            {
                // Название таблицы
                entity.ToTable("users");

                // Первичный ключ
                entity.HasKey(e => e.Id);

                // Автоинкремент
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                // Уникальный индекс для Email
                entity.HasIndex(e => e.Email)
                    .IsUnique()
                    .HasDatabaseName("IX_users_email");

                // Индекс для Phone (не уникальный, так как может быть null)
                entity.HasIndex(e => e.Phone)
                    .HasDatabaseName("IX_users_phone");

                // Индекс для RefreshToken (для поиска)
                entity.HasIndex(e => e.RefreshToken)
                    .HasDatabaseName("IX_users_refresh_token");

                // Конфигурация свойств
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password_hash");

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .HasColumnName("phone");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("last_name");

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.RefreshToken)
                    .HasMaxLength(500)
                    .HasColumnName("refresh_token");

                entity.Property(e => e.RefreshTokenExpiryTime)
                    .HasColumnType("datetime")
                    .HasColumnName("refresh_token_expiry_time");
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is User &&
                           (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var user = (User)entry.Entity;
                var now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    user.CreatedAt = now;
                    user.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    // Не перезаписываем CreatedAt при обновлении
                    entry.Property("CreatedAt").IsModified = false;
                    user.UpdatedAt = now;
                }
            }
        }


    }
}
