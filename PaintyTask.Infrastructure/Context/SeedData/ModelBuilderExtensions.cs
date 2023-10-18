using Microsoft.EntityFrameworkCore;
using PaintyTask.Domain.Models;

namespace PaintyTask.Infrastructure.Context.SeedData;

public static class ModelBuilderExtensions
{
    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserData>().HasData(
            new UserData
            {
                Id = 1,
                Name = "User1",
                Login = "User1",
                Email = "user1@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("user1")
            },
            new UserData
            {
                Id = 2,
                Name = "User2",
                Login = "User2",
                Email = "user2@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("user2")
            },
            new UserData
            {
                Id = 3,
                Name = "User3",
                Login = "User3",
                Email = "user3@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("user3")
            },
            new UserData
            {
                Id = 4,
                Name = "User4",
                Login = "User4",
                Email = "user4@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("user4")
            }
        );
    }

    public static void SeedPhotos(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PhotoData>().HasData(
            new PhotoData
            {
                Id = 1,
                UserId = 1,
                Url = "/Photos/69850.jpg"
            },
            new PhotoData
            {
                Id = 2,
                UserId = 1,
                Url = "/Photos/9acebdc6-126a-4e5b-857e-fcf777e50dd4apple-logo.jpg"
            },
            new PhotoData
            {
                Id = 3,
                UserId = 1,
                Url = "/Photos/bd455fb4-0ca9-4c9f-8bcd-7d44647d49f8admin.jpg"
            },
            new PhotoData
            {
                Id = 4,
                UserId = 2,
                Url = "/Photos/bdfe5019-5f62-4662-8355-400f5921f2d2bob.jpg"
            },
            new PhotoData
            {
                Id = 5,
                UserId = 2,
                Url = "/Photos/brandFon.jpg"
            },
            new PhotoData
            {
                Id = 6,
                UserId = 3,
                Url = "/Photos/cmoeeddpcevumpqs.jpg"
            },
            new PhotoData
            {
                Id = 7,
                UserId = 3,
                Url = "/Photos/DjNqxwoKLNY.jpg"
            },
            new PhotoData
            {
                Id = 8,
                UserId = 3,
                Url = "/Photos/homeFon.jpg"
            },
            new PhotoData
            {
                Id = 9,
                UserId = 3,
                Url = "/Photos/images_anubis.jpg"
            }
        );
    }

    public static void SeedFriendships(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FriendshipData>().HasData(
            new FriendshipData
            {
                UserSenderId = 1,
                UserReceiverId = 2,
                IsAccepted = true
            },
            new FriendshipData
            {
                UserSenderId = 1,
                UserReceiverId = 3,
                IsAccepted = false
            },
            new FriendshipData
            {
                UserSenderId = 2,
                UserReceiverId = 4,
                IsAccepted = true
            },
            new FriendshipData
            {
                UserSenderId = 2,
                UserReceiverId = 1,
                IsAccepted = false
            },
            new FriendshipData
            {
                UserSenderId = 3,
                UserReceiverId = 4,
                IsAccepted = false
            },
            new FriendshipData
            {
                UserSenderId = 4,
                UserReceiverId = 3,
                IsAccepted = true
            }
        );
    }
}