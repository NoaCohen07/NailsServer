﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NailsServer.Models;

namespace NailsServer.DTO
{
    public class Post
    {
   
        public int? UserId { get; set; }

        public DateTime PostTime { get; set; }

        public string? PostText { get; set; }

      
        public string Pic { get; set; } = null!;

    
        public int PostId { get; set; }

 
        
        public string FullPostImagePath { get; set; } = null!;

        public int NumLikes { get; set; }


        public Post() { }
        public Post(Models.Post post)
        {
            this.UserId = post.UserId;
            this.PostTime = post.PostTime;
            this.PostText = post.PostText;
            this.Pic = post.Pic;
            this.PostId = post.PostId;
            this.NumLikes = 0;
            if (post.Users != null)
            {
                foreach (Models.User u in post.Users)
                {
                   NumLikes++;
                }
            }
        }

        public Models.Post GetModel()
        {
            Models.Post p = new Models.Post();
            p.UserId = this.UserId;
            p.PostTime = this.PostTime;
            p.PostText = this.PostText;
            p.Pic = this.Pic;
            p.PostId = this.PostId;
            return p;
        }
    }
}
