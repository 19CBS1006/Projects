using System;
using System.ComponentModel.DataAnnotations;
namespace Vispl.Training.Crickinfo.VO
{
    public class Login_VO : IDisposable, ILogin_VO
    {
        public string FlatFile = @"C:\Users\Asus\source\repos\Cricket_ScoreBoard\loginFile.xml";

        /// <summary>
        /// Username
        /// </summary>
        [Required(ErrorMessage = "Enter the Username")]
        [Display(Name = "Username")]
        public string Username { get; set; }


        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessage = "Enter the Password")]
        [Display(Name = "Password")]
        public string Password { get; set; }


        /// <summary>
        /// Disposing VO
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Username = null;
                Password = null;
            }
        }
        ~Login_VO()
        {
            Dispose(false);
        }
    }
}