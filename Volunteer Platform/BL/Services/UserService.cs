﻿using System.Security.Claims;
using System.Security.Cryptography;
using AutoMapper;
using BL.IServices;
using BL.Models;
using BL.Security;
using DAL.IRepositories;
using DAL.Models;

using Microsoft.Extensions.Configuration;


namespace BL.Services;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _userMapper;

    public UserService(IRepositoryManager repositoryManager, IConfiguration configuration, IMapper userMapper)
    {
        _repositoryManager = repositoryManager;
        _configuration = configuration;
        _userMapper = userMapper;
    }

    public ICollection<UserDto> GetAll()
    {
        var allUsers = _repositoryManager.User.GetAll(u => u.IsDeleted == false);
        return _userMapper.Map<ICollection<UserDto>>(allUsers).ToList();


    }

    public UserDto GetById(int id)
    {
        var user = _repositoryManager.User.GetFirstOrDefault(u => u.Id == id && u.IsDeleted == false);
        if (user == null) return null;
        return user == null ? null : _userMapper.Map<UserDto>(user);
    }




    public UserDto Create(UserRegistrationDto user)
    {
        var existingUser = _repositoryManager.User.GetFirstOrDefault(u => u.Username == user.UserName );
        if (existingUser != null)
            throw new InvalidOperationException("User already exists");
        existingUser = _repositoryManager.User.GetFirstOrDefault(u => u.Email == user.Email);
        if (existingUser != null)
            throw new InvalidOperationException("Email already exists");

        bool isFirstUser = _repositoryManager.User.GetFirstOrDefault(u => u.Role == 2) == null;
        var salt = PasswordHashProvider.GetSalt();
        var hash = PasswordHashProvider.GetHash(user.Password, salt);

        var securityKey = RandomNumberGenerator.GetBytes(256 / 8);
        var b64SecKey = Convert.ToBase64String(securityKey);

        var newUser = new User
        {
            Username = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PwdHash = hash,
            PwdSalt = salt,
            Phone = user.Phone,
           
            SecurityToken = b64SecKey,
            Role = isFirstUser ? 2 : 0
        };

  
        _repositoryManager.User.Add(newUser);
        _repositoryManager.Save();  
        return _userMapper.Map<UserDto>(newUser);

    }

    public UserDto Update(int id, UserDto user) 
    {
        var userToUpdate = _repositoryManager.User.GetFirstOrDefault(u => u.Id == id && u.IsDeleted == false);
        if (userToUpdate == null) throw new InvalidOperationException("User does not exist"); ;
    

        userToUpdate.FirstName = user.FirstName;
        userToUpdate.LastName = user.LastName;
        userToUpdate.Username = user.Username;
        userToUpdate.Email = user.Email;
        userToUpdate.Phone = user.Phone;
        if (user.Role.HasValue)
        {
	        userToUpdate.Role = user.Role.Value;
        }


		_repositoryManager.Save();
        return _userMapper.Map<UserDto>(userToUpdate);

    }

	public UserDto Delete(int id)
    {
        var toDelete = _repositoryManager.User.GetFirstOrDefault(u => u.Id == id && u.IsDeleted == false);
        if (toDelete == null) return null;
       

        toDelete.IsDeleted = true;
        toDelete.DeletedAt = DateTime.Now;
        _repositoryManager.Save();

        return _userMapper.Map<UserDto>(toDelete);
    }



    public string GenerateToken(UserLoginDto request)
    {

        var user = Authenticate(request.Username, request.Password);

        if (user != null)
        {
            var jwtKey = _configuration["Jwt:Key"];
            var role = user.Role.ToString(); 

         
            var additionalClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role)
            };

            var token = JwtTokenProvider.CreateToken(jwtKey, 10, user.Username, additionalClaims);
            return token;
        }

        throw new Exception("Authentication failed");
    }



    public User Authenticate(string username, string password)
    {
	    var user = _repositoryManager.User.GetFirstOrDefault(u => u.Username == username);
        if (user == null) throw new InvalidOperationException("User does not exist");

        var hash = PasswordHashProvider.GetHash(password, user.PwdSalt);

        return hash == user.PwdHash ? user : null;
    }

    public UserDto GetByUserName(string username)
	{
		var user = _repositoryManager.User.GetFirstOrDefault(u => u.Username == username);
        if (user == null) return null;
        return  _userMapper.Map<UserDto>(user);
	}

    public UserDto GetByEmail(string email)
    {
        var user = _repositoryManager.User.GetFirstOrDefault(u => u.Email == email);
        if (user == null) return null;
        return _userMapper.Map<UserDto>(user);
    }


    public UserDto GetByName(string name)
    {
		var user = _repositoryManager.User.GetFirstOrDefault(u => u.Username == name);
        if (user == null) return null;
        return _userMapper.Map<UserDto>(user);


	}



    public void ChangePassword(UserPasswordChangeDto request)
    {
        var user = Authenticate(request.Username, request.OldPassword);
        if (user != null)
        {
            string b64Salt = PasswordHashProvider.GetSalt();
            string b64Hash = PasswordHashProvider.GetHash(request.NewPassword, b64Salt);
            user.PwdSalt = b64Salt;
            user.PwdHash = b64Hash;

            _repositoryManager.Save();
        }
        else
        {
            throw new InvalidOperationException("Invalid username or password");
        }
    }
}