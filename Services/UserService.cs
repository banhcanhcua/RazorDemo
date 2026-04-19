using RazorDemo.Data;
using RazorDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace RazorDemo.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            // Kiểm tra username hoặc email đã tồn tại
            if (await GetUserByUsernameAsync(user.Username) != null ||
                await GetUserByEmailAsync(user.Email) != null)
            {
                return false;
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await GetUserByIdAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            // Kiểm tra username hoặc email đã tồn tại (trừ user hiện tại)
            var duplicateUser = await _context.Users
                .FirstOrDefaultAsync(u => (u.Username == user.Username || u.Email == user.Email) && u.Id != user.Id);
            if (duplicateUser != null)
            {
                return false;
            }

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password; // Trong thực tế nên hash password
            existingUser.Role = user.Role;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> SearchUsersAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllUsersAsync();
            }

            return await _context.Users
                .Where(u => u.Username.Contains(keyword) || u.Email.Contains(keyword))
                .ToListAsync();
        }
    }
}