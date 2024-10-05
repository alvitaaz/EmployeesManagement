namespace EmployeesManagement
{
    public class Helpers
    {
        public static string GetUserIpAddress(HttpContext httpContext)
        {
            var remoteIpAddress = httpContext.Connection.RemoteIpAddress;
            return remoteIpAddress != null ? remoteIpAddress.ToString() : "Unknown";
        }
    }

}
