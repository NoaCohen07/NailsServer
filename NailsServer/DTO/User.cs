﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace NailsServer.DTO
{
    public class User
    {
        public int UserId { get; set; }

       
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }

   
        public string Email { get; set; } = null!;

    
        public string PhoneNumber { get; set; } = null!;

    
        public string UserAddress { get; set; } = null!;

        public string Gender { get; set; } = null!;


        public string Pass { get; set; } = null!;

        public bool IsManicurist { get; set; }

        public bool IsBlocked { get; set; }

 
        public string? ProfilePic { get; set; }

        public bool IsManager { get; set; }

        public User() { }
        public User(Models.User modelUser)
        {
            this.UserId = modelUser.UserId;
            this.FirstName = modelUser.FirstName;
            this.LastName = modelUser.LastName;
            this.DateOfBirth = modelUser.DateOfBirth;
            this.Email = modelUser.Email;
            this.PhoneNumber = modelUser.PhoneNumber;
            this.UserAddress = modelUser.UserAddress;
            this.Gender= modelUser.Gender;
            this.Pass = modelUser.Pass;
            this.IsManicurist = modelUser.IsManicurist;
            this.IsBlocked = modelUser.IsBlocked;
            this.ProfilePic = modelUser.ProfilePic;
            this.IsManager = modelUser.IsManager;
        }

        public Models.User GetModel()
        {
            Models.User model = new Models.User();
            model.FirstName = this.FirstName;
            model.LastName = this.LastName;
            model.DateOfBirth = this.DateOfBirth;
            model.Email = this.Email;
            model.PhoneNumber = this.PhoneNumber;
            model.UserAddress = this.UserAddress;
            model.Gender = this.Gender;
            model.Pass = this.Pass;
            model.IsManicurist = this.IsManicurist;
            model.IsBlocked = this.IsBlocked;
            model.ProfilePic = this.ProfilePic;
            model.IsManager = this.IsManager;
            return model;
        }

    }
}
