namespace Vispl.Training.Crickinfo.VO
{
    public interface ILogin_VO
    {
        string Password { get; set; }
        string Username { get; set; }

        void Dispose();
    }
}