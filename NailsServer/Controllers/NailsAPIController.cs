﻿using Microsoft.AspNetCore.Mvc;
using NailsServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Reflection;
using NailsServer.DTO;



namespace NailsServer.Controllers
{

    [Route("api")]
    [ApiController]
    public class NailsAPIController : ControllerBase
    {
        //a variable to hold a reference to the db context!
        private NailsDbContext context;
        //a variable that hold a reference to web hosting interface (that provide information like the folder on which the server runs etc...)
        private IWebHostEnvironment webHostEnvironment;
        //Use dependency injection to get the db context and web host into the constructor
        public NailsAPIController(NailsDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.webHostEnvironment = env;
        }

        [HttpGet]
        [Route("TestServer")]
        public ActionResult<string> TestServer()
        {
            return Ok("Server Responded Successfully");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] DTO.LoginInfo loginDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Get model user class from DB with matching email. 
                Models.User? modelsUser = context.GetUser(loginDto.Email);

                //Check if user exist for this email and if password match, if not return Access Denied (Error 403) 
                if (modelsUser == null || modelsUser.Pass != loginDto.Password)
                {
                    return Unauthorized();
                }

                //Login suceed! now mark login in session memory!
                HttpContext.Session.SetString("loggedInUser", modelsUser.Email);

                DTO.User dtoUser = new DTO.User(modelsUser);
                //profile img
                //dtoUser.ProfilePic = GetProfileImageVirtualPath(dtoUser.UserId);
                //dtoUser.ProfilePic = GetProfileImageVirtualPath(dtoUser);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] DTO.User userDto)
        {
            try
            {
                HttpContext.Session.Clear(); //Logout any previous login attempt

                //Create model user class
                Models.User modelsUser = userDto.GetModel();
                modelsUser.ProfilePic = "//profileImages/default.jpg";
                context.Users.Add(modelsUser);
                context.SaveChanges();

                //User was added!
                DTO.User dtoUser = new DTO.User(modelsUser);
                //profile pic
                //dtoUser.ProfilePic = GetProfileImageVirtualPath(dtoUser);
                return Ok(dtoUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("UploadProfileImage")]
        public async Task<IActionResult> UploadProfileImageAsync(IFormFile file)
        {
            //Check if who is logged in

            string? userEmail = HttpContext.Session.GetString("loggedInUser");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching email. 
            Models.User? user = context.GetUser(userEmail);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            if (user == null)
            {
                return Unauthorized("User is not found in the database");
            }


            //Read all files sent
            long imagesSize = 0;

            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";
                
                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                    user.ProfilePic = $"\\profileImages\\{user.UserId}{extention}";
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}\\profileImages\\{user.UserId}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                    }

                }

            }
            //Update image extention in DB
            
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();

            DTO.User dtoUser = new DTO.User(user);
            return Ok(dtoUser);
        }


        //Helper functions

        //this function gets a file stream and check if it is an image
        private static bool IsImage(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);

            List<string> jpg = new List<string> { "FF", "D8" };
            List<string> bmp = new List<string> { "42", "4D" };
            List<string> gif = new List<string> { "47", "49", "46" };
            List<string> png = new List<string> { "89", "50", "4E", "47", "0D", "0A", "1A", "0A" };
            List<List<string>> imgTypes = new List<List<string>> { jpg, bmp, gif, png };

            List<string> bytesIterated = new List<string>();

            for (int i = 0; i < 8; i++)
            {
                string bit = stream.ReadByte().ToString("X2");
                bytesIterated.Add(bit);

                bool isImage = imgTypes.Any(img => !img.Except(bytesIterated).Any());
                if (isImage)
                {
                    return true;
                }
            }

            return false;
        }

        ////this function check which profile image exist and return the virtual path of it.
        ////if it does not exist it returns the default profile image virtual path
        //private string GetProfileImageVirtualPath(DTO.User dtoUser)
        //{
  
        //    string virtualPath = $"/profileImages/{dtoUser.UserId}";
        //    if (dtoUser.ProfilePic == null)
        //    {
        //        virtualPath = $"/profileImages/default.jpg";
        //    }
        //    else
        //        virtualPath += dtoUser.ProfilePic;


        //    return virtualPath;
        //}

        [HttpPost("updateuser")]
        public IActionResult UpdateUser([FromBody] DTO.User userDto)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Get model user class from DB with matching email. 
                Models.User? theUser = context.GetUser(userEmail);
                //Clear the tracking of all objects to avoid double tracking
                context.ChangeTracker.Clear();

                //Check if the user that is logged in is the same user of the task
                //this situation is ok only if the user is a manager
                if (theUser == null || (userDto.UserId != theUser.UserId))
                {
                    return Unauthorized("Failed to update user");
                }

                Models.User appUser = userDto.GetModel();

                context.Entry(appUser).State = EntityState.Modified;

                context.SaveChanges();

                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("UploadPostImage")]
        public async Task<IActionResult> UploadPostImageAsync(IFormFile file, [FromQuery] int postId)
        {

            //Check if who is logged in

            string? userEmail = HttpContext.Session.GetString("loggedInUser");
            if (string.IsNullOrEmpty(userEmail))
            {
                return Unauthorized("User is not logged in");
            }

            //Get model user class from DB with matching email. 
            Models.Post? post = context.GetPost(postId);
            //Clear the tracking of all objects to avoid double tracking
            context.ChangeTracker.Clear();

            if (post == null)
            {
                return Unauthorized("Post is not found in the database");
            }


            //Read all files sent
            long imagesSize = 0;

            if (file.Length > 0)
            {
                //Check the file extention!
                string[] allowedExtentions = { ".png", ".jpg" };
                string extention = "";

                if (file.FileName.LastIndexOf(".") > 0)
                {
                    extention = file.FileName.Substring(file.FileName.LastIndexOf(".")).ToLower();
                    post.Pic = $"/postsImages/{postId}{extention}";
                }
                if (!allowedExtentions.Where(e => e == extention).Any())
                {
                    //Extention is not supported
                    return BadRequest("File sent with non supported extention");
                }

                //Build path in the web root (better to a specific folder under the web root
                string filePath = $"{this.webHostEnvironment.WebRootPath}/postsImages/{postId}{extention}";

                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);

                    if (IsImage(stream))
                    {
                        imagesSize += stream.Length;
                    }
                    else
                    {
                        //Delete the file if it is not supported!
                        System.IO.File.Delete(filePath);
                    }

                }
            }
            //Update image extention in DB
            context.Entry(post).State = EntityState.Modified;
            context.SaveChanges();
            DTO.Post p = new DTO.Post(post);
            // post.PostPicturePath = GetImageVirtualPath(dtoUser);

            return Ok(p);
        }


        [HttpGet("GetPosts")]
        public IActionResult GetPosts([FromQuery] string email)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read posts of the user

                List<Models.Post> list = context.GetPosts(email);

                List<DTO.Post> posts = new List<DTO.Post>();
                
                foreach(Models.Post p in list)
                {
                    DTO.Post post = new DTO.Post(p);
                    
                    //post.Pic = $"/postsImages/{post.PostId}{p.Pic}";
                    posts.Add(post);
                }
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetPostComments")]
        public IActionResult GetPostComments([FromQuery] int postId)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read posts of the user

                List<Models.Comment> list = context.GetComments(postId);

                List<DTO.Comment> comments = new List<DTO.Comment>();

                foreach (Models.Comment c in list)
                {
                    DTO.Comment comment = new DTO.Comment(c);
                    //post.PostPicturePath = $"/postsImages/{post.PostId}{p.Pic}";
                    comments.Add(comment);
                }
                return Ok(comments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetLikes")]
        public IActionResult GetLikes([FromQuery] int postId)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read posts of the user

                int likes = context.GetNumLikes(postId);
                return Ok(likes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetUser")]
        public IActionResult GetUser([FromQuery] int userId)
        {
            try
            {
                //Check who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read posts of the user

                Models.User u = context.GetUser1(userId);
                DTO.User user= new DTO.User(u);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("AddComment")]
        public IActionResult AddComment([FromBody] DTO.Comment c)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Create model user class
                Models.Comment comment = c.GetModel();

                context.Comments.Add(comment);
                context.SaveChanges();

                //Comment was added!
              
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("AddPost")]
        public IActionResult AddPost([FromBody] DTO.Post p)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Create model user class
                Models.Post post = p.GetModel();
                //post.ProfilePic = $"\\profileImages\\{user.UserId}{extention}";
                //p.PostPicturePath=$"/postsImages/{p.PostId}{p.Pic}";
                context.Posts.Add(post);
                context.SaveChanges();
                DTO.Post newPost = new DTO.Post(post);
                //Post was added!
                 
                return Ok(newPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetManicurists")]
        public IActionResult GetManicurists()
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read all manicurists

                List<Models.User> list = context.GetManicurists();

                List<DTO.User> manicurists = new List<DTO.User>();

                foreach (Models.User u in list)
                {
                    DTO.User user = new DTO.User(u);
                   
                   
                    manicurists.Add(user);
                }
                return Ok(manicurists);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read all users

                List<Models.User> list = context.GetUsers();

                List<DTO.User> users = new List<DTO.User>();

                foreach (Models.User u in list)
                {
                    DTO.User user = new DTO.User(u);

                    users.Add(user);
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetAllPosts")]
        public IActionResult GetAllPosts()
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read all posts

                List<Models.Post> list = context.GetAllPosts();

                List<DTO.Post> allPosts = new List<DTO.Post>();

                foreach (Models.Post p in list)
                {
                    DTO.Post post = new DTO.Post(p);

                    allPosts.Add(post);
                }
                return Ok(allPosts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetFavorites")]
        public IActionResult GetFavorites()
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read all posts

                List<Models.Post> list = context.GetFavorites(userEmail);
              

                List<DTO.Post> allPosts = new List<DTO.Post>();

                foreach (Models.Post p in list)
                {
                    DTO.Post post = new DTO.Post(p);

                    allPosts.Add(post);
                }
                return Ok(allPosts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("AddFavorite")]
        public IActionResult AddFavorite([FromBody] DTO.Favorite f)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Create model user class
                Models.Favorite fav = f.GetModel();

                context.Favorites.Add(fav);
                context.SaveChanges();
                DTO.Favorite newFavorite = new DTO.Favorite(fav);
                //Post was added!

                return Ok(newFavorite);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Block")]
        public IActionResult Block([FromBody] DTO.User u)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Create model user class
                Models.User user = context.GetUser1(u.UserId);
                user.IsBlocked = u.IsBlocked;
                context.Entry(user).State = EntityState.Modified;

                context.SaveChanges();
                //DTO.U
                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Like")]
        public IActionResult Like([FromBody] DTO.Like u)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Create model user class
                //Models.User user = context.GetUser1(u.UserId);


                Models.Like l = u.GetModel();
                context.Likes.Add(l);
                context.SaveChanges();
                //DTO.Like newFavorite = new DTO.Like(l);
               

                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("RemoveLike")]
        public IActionResult RemoveLike([FromBody] DTO.Like u)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Create model user class

                Models.Like l = u.GetModel();
                context.Likes.Remove(l);
                context.SaveChanges();
               // DTO.Like newFavorite = new DTO.Like(l);

                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("RemoveFavorite")]
        public IActionResult RemoveFavorite([FromBody] DTO.Favorite fav)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Create model user class

                Models.Favorite f = fav.GetModel();
                context.Favorites.Remove(f);
                context.SaveChanges();
                // DTO.Like newFavorite = new DTO.Like(l);

                //Task was updated!
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetLike")]
        public IActionResult GetLike([FromQuery] int userId, [FromQuery] int postId)
        {
            try
            {
                //Check who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read posts of the user

                bool u = context.GetLiked(userId,postId);
                //DTO.User user = new DTO.User(u);
                return Ok(u);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetFavorite")]
        public IActionResult GetFavorite([FromQuery] int userId, [FromQuery] int postId)
        {
            try
            {
                //Check who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read posts of the user

                bool u = context.GetFavorite(userId, postId);
                //DTO.User user = new DTO.User(u);
                return Ok(u);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("AddTreatment")]
        public IActionResult AddTreatment([FromBody] DTO.Treatment u)
        {
            try
            {
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Create model user class
                //Models.User user = context.GetUser1(u.UserId);


                Models.Treatment t = u.GetModel();
                context.Treatments.Add(t);
                context.SaveChanges();
                DTO.Treatment newTreatment = new DTO.Treatment(t);


                //Task was updated!
                return Ok(newTreatment);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetTreatments")]
        public IActionResult GetTreatments([FromQuery] string email)
        {
            try
            {
                //Check if who is logged in
                string? userEmail = HttpContext.Session.GetString("loggedInUser");
                if (string.IsNullOrEmpty(userEmail))
                {
                    return Unauthorized("User is not logged in");
                }

                //Read posts of the user

                List<Models.Treatment> list = context.GetTreatments(email);

                List<DTO.Treatment> treats = new List<DTO.Treatment>();

                foreach (Models.Treatment p in list)
                {
                    DTO.Treatment treat = new DTO.Treatment(p);
                    treats.Add(treat);
                }
                return Ok(treats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}

