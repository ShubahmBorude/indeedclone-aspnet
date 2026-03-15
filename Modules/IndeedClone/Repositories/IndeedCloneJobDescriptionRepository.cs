using IndeedClone.Modules.IndeedClone.DTO;
using IndeedClone.Modules.IndeedClone.Helpers;
using IndeedClone.Modules.IndeedClone.RepoContracts;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using Microsoft.Data.SqlClient;

namespace IndeedClone.Modules.IndeedClone.Repositories
{
    public class IndeedCloneJobDescriptionRepository : IIndeedCloneJobDescriptionRepository
    {
        private readonly string _connectionString;

        public IndeedCloneJobDescriptionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IndeedCloneJobDescriptionDTO?> GetJobDescriptionAsync(string jobUid)
        {
            using var con = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                SELECT 
                    jo.job_uid,
                    jo.company_name,
                    jo.full_name,
                    jo.mobile_number,

                    jb.job_title,
                    jb.job_location,
                    jb.city,
                    jb.area,
                    jb.street_address,
                    jb.work_arrangement,

                    jd.emptype,

                    jp.pay_type,
                    jp.minimum_pay,
                    jp.maximum_pay,
                    jp.pay_rate_type,
                    jp.supplemented_pay,
                    jp.benefits,

                    jds.description,

                    jq.employment_time,
                    jq.education,
                    jq.experience,
                    jq.skills,
                    jq.language,
                    jq.certifications

                FROM job_organization jo
                LEFT JOIN job_basics jb ON jb.job_uid = jo.job_uid
                LEFT JOIN job_pay_benefits jp ON jp.job_uid = jo.job_uid
                LEFT JOIN job_details jd ON jd.job_uid = jo.job_uid
                LEFT JOIN job_description jds ON jds.job_uid = jo.job_uid
                LEFT JOIN job_qualifications jq ON jq.job_uid = jo.job_uid

                WHERE jo.job_uid = @JobUid
                  AND jo.status = @Status
            ", con);

            cmd.Parameters.AddWithValue("@JobUid", jobUid);
            cmd.Parameters.AddWithValue("@Status", (int)JobPostStatus.ACTIVE);

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return new IndeedCloneJobDescriptionDTO
            {
                JobUid = reader["job_uid"].ToString()!,
                CompanyName = reader["company_name"].ToString() ?? "",
                JobTitle = reader["job_title"].ToString() ?? "",
                FullName = reader["full_name"]?.ToString() ?? "",
                MobileNumber = reader["mobile_number"]?.ToString() ?? "",
                JobLocation = reader["job_location"]?.ToString() ?? "",
                City = reader["city"]?.ToString() ?? "",
                Area = reader["area"]?.ToString() ?? "",
                StreetAddress = reader["street_address"]?.ToString() ?? "",
                EmployeeType = Parser.JsonArray(reader["emptype"]?.ToString() ?? "[]"),
                PayType = (PayType)Convert.ToInt32(reader["pay_type"]),
                MinimumPay = Convert.ToDecimal(reader["minimum_pay"]),
                MaximumPay = Convert.ToDecimal(reader["maximum_pay"]),
                PayRateType = (PayRateType)Convert.ToInt32(reader["pay_rate_type"]),
                SupplementedPay = Parser.JsonArray(reader["supplemented_pay"]?.ToString() ?? "[]"),
                Benefits = Parser.JsonArray(reader["benefits"]?.ToString() ?? "[]"),
                JobDescription = reader["description"]?.ToString() ?? "",
                WorkArrangement = reader["work_arrangement"]?.ToString() ?? "",
                EmploymentTime = reader["employment_time"] == DBNull.Value ? null : (EmploymentTime)Convert.ToInt32(reader["employment_time"]),
                Experience = reader["experience"] == DBNull.Value ? null : (ExperienceLevel)Convert.ToInt32(reader["experience"]),
                Education = Parser.JsonArray(reader["education"]?.ToString()),
                Skills = Parser.JsonArray(reader["skills"]?.ToString()),
                Languages = Parser.JsonArray(reader["language"]?.ToString()),
                Certifications = Parser.JsonArray(reader["certifications"]?.ToString())
            };

        }

    }
}
