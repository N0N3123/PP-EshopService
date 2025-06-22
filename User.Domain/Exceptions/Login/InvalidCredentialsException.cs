namespace User.Domain.Exceptions.Login
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("Invalid credentials") { }
    }
}
