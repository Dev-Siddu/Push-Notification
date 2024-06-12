using System.ComponentModel.DataAnnotations;
using Microsoft.Data.SqlClient;
using System.Data;
using Models;
namespace DataAccess
{
    public class CommonDataAccess
    {
        private readonly string ConStr = "";
        public CommonDataAccess()
        {
            ConStr = "Server=DESKTOP-TC1CGT4;Initial Catalog=SIGNALR;Integrated Security=true;TrustServerCertificate=True";
        }
        public int AuthenticateUser(SignInModel user)
        {
            int RS = 0;
            string stp = "AUTHENTICATE_USER";
            using (SqlConnection con = new SqlConnection(ConStr))
            {
                using (SqlCommand cmd = new SqlCommand(stp, con) { CommandType = CommandType.StoredProcedure})
                {
                    SqlParameter p1 = new SqlParameter("@NAME",user.Name);
                    SqlParameter p2 = new SqlParameter("@PASSWORD",user.Password);
                    SqlParameter p3 = new SqlParameter("@RETURN_STATUS", SqlDbType.Int) {Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(p1);
                    cmd.Parameters.Add(p2);
                    cmd.Parameters.Add(p3);

                    con.Open();
                    cmd.ExecuteNonQuery();

                    RS = (int)p3.Value;
                }
            }
            return RS;
        }
    }
}
