using IndeedClone.Modules.IndeedClone.DTO;
using IndeedClone.Modules.IndeedClone.Helpers;
using IndeedClone.Modules.IndeedClone.RepoContracts;
using IndeedClone.Modules.Shared.Enums;
using IndeedClone.Modules.SubModules.JobPost.Enums;
using Microsoft.Data.SqlClient;



namespace IndeedClone.Modules.IndeedClone.Repositories
{
    public class IndeedCloneHomeRepository : IIndeedCloneHomeRepository
    {
        private readonly string _connectionString;

        public IndeedCloneHomeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IndeedClonePaginatedJobCardsDTO> GetLatestJobCardsAsync(int pageNumber = 1, int pageSize = 10)
        {
            var result = new List<IndeedCloneLeftJobCardsDTO>();
            var pagination = new IndeedClonePaginationDTO { CurrentPage = pageNumber, PageSize = pageSize };
            using var con = new SqlConnection(_connectionString);
            await con.OpenAsync();

            var countCmd = new SqlCommand(@"
                                          SELECT COUNT(*) 
                                          FROM job_organization jo
                                          WHERE jo.status = @Status;
                                          ", con);

            countCmd.Parameters.AddWithValue("@Status", (int)JobPostStatus.ACTIVE);
            pagination.TotalCount = (int)await countCmd.ExecuteScalarAsync();
            var offset = (pageNumber - 1) * pageSize;

            using var cmd = new SqlCommand(@"SELECT
                                    jo.job_uid,
                                    jo.company_name,
                                    jb.job_title,
                                    jb.job_location,
                                    jb.city,
                                    jb.area,
                                    jb.street_address,
                                    jd.recruitment_timeline,
                                    jp.pay_type,
                                    jp.minimum_pay,
                                    jp.maximum_pay,
                                    jp.pay_rate_type,
                                    jp.supplemented_pay,
                                    jp.benefits,
                                    jo.created,
                                    jo.edited
                                FROM job_organization jo
                                LEFT JOIN job_basics jb ON jb.job_uid = jo.job_uid
                                LEFT JOIN job_details jd ON jd.job_uid = jo.job_uid
                                LEFT JOIN job_pay_benefits jp ON jp.job_uid = jo.job_uid
                                WHERE jo.status = @Status
                                ORDER BY jo.created DESC
                                OFFSET @Offset ROWS 
                                FETCH NEXT @PageSize ROWS ONLY;
                                ", con);

            cmd.Parameters.AddWithValue("@Status", (int)JobPostStatus.ACTIVE);
            cmd.Parameters.AddWithValue("@Offset", offset);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new IndeedCloneLeftJobCardsDTO
                {
                    JobUid = reader["job_uid"].ToString(),
                    CompanyName = reader["company_name"].ToString(),
                    JobTitle = reader["job_title"].ToString(),
                    JobLocation = reader["job_location"].ToString(),
                    City = reader["city"].ToString(),
                    Area = reader["area"].ToString(),
                    StreetAddress = reader["street_address"].ToString(),
                    RecruitmentTimeline = reader["recruitment_timeline"].ToString(),
                    PayType = Enum.Parse<PayType>(reader["pay_type"].ToString()),
                    MinimumPay = Convert.ToDecimal(reader["minimum_pay"]),
                    MaximumPay = Convert.ToDecimal(reader["maximum_pay"]),
                    PayRateType = Enum.Parse<PayRateType>(reader["pay_rate_type"].ToString()),
                    SupplementedPay = Parser.ToList(reader["supplemented_pay"]?.ToString() ?? "[]"),
                    Benefits = Parser.ToList(reader["benefits"]?.ToString() ?? "[]"),
                    Created = Convert.ToDateTime(reader["created"]),
                    Edited = Convert.ToDateTime(reader["edited"]),
                    PostedAgo = DateTimeAgoHelper.GetPostedAgo(Convert.ToDateTime(reader["created"]), Convert.ToDateTime(reader["edited"]))
                });
            }

            return new IndeedClonePaginatedJobCardsDTO
            {
                Jobs = result,
                Pagination = pagination
            };
        }

        public async Task<IndeedClonePaginatedJobCardsDTO> GetFilteredJobsAsync(IndeedCloneJobSearchFilterDTO filters, int pageSize = 10)
        {
            var result = new List<IndeedCloneLeftJobCardsDTO>();
            var pagination = new IndeedClonePaginationDTO { CurrentPage = filters.Page, PageSize = pageSize };

            using var con = new SqlConnection(_connectionString);
            await con.OpenAsync();

            // Base query (exactly as you specified)
            var baseQuery = @"
                            SELECT
                                jo.job_uid,
                                jo.company_name,
                                jb.job_title,
                                jb.job_location,
                                jb.city,
                                jb.area,
                                jb.street_address,
                                jd.recruitment_timeline,
                                jd.emptype,
                                jp.pay_type,
                                jp.minimum_pay,
                                jp.maximum_pay,
                                jp.pay_rate_type,
                                jp.supplemented_pay,
                                jp.benefits,
                                jo.created,
                                jo.edited
                            FROM job_organization jo
                            LEFT JOIN job_basics jb ON jb.job_uid = jo.job_uid
                            LEFT JOIN job_details jd ON jd.job_uid = jo.job_uid
                            LEFT JOIN job_pay_benefits jp ON jp.job_uid = jo.job_uid
                            LEFT JOIN job_qualifications jq ON jq.job_uid = jo.job_uid
                            WHERE jo.status = @Status";

                                    var countBaseQuery = @"
                            SELECT COUNT(*)
                            FROM job_organization jo
                            LEFT JOIN job_basics jb ON jb.job_uid = jo.job_uid
                            LEFT JOIN job_details jd ON jd.job_uid = jo.job_uid
                            LEFT JOIN job_pay_benefits jp ON jp.job_uid = jo.job_uid
                            LEFT JOIN job_qualifications jq ON jq.job_uid = jo.job_uid
                            WHERE jo.status = @Status";

            var conditions = new List<string>();

            // Create parameter lists for count and main queries separately
            var countParameters = new List<SqlParameter>
            {
                new SqlParameter("@Status", (int)JobPostStatus.ACTIVE)
            };

            var mainParameters = new List<SqlParameter>
            {
                new SqlParameter("@Status", (int)JobPostStatus.ACTIVE)
            };

            // Add filter conditions
            if (!string.IsNullOrEmpty(filters.Keyword))
            {
                conditions.Add("(jb.job_title LIKE @Keyword OR jo.company_name LIKE @Keyword OR jb.job_location LIKE @Keyword)");
                countParameters.Add(new SqlParameter("@Keyword", $"%{filters.Keyword}%"));
                mainParameters.Add(new SqlParameter("@Keyword", $"%{filters.Keyword}%"));
            }

            if (!string.IsNullOrEmpty(filters.Location))
            {
                conditions.Add("(jb.city LIKE @Location OR jb.area LIKE @Location OR jb.job_location LIKE @Location)");
                countParameters.Add(new SqlParameter("@Location", $"%{filters.Location}%"));
                mainParameters.Add(new SqlParameter("@Location", $"%{filters.Location}%"));
            }

            if (!string.IsNullOrEmpty(filters.WorkArrangement))
            {
                conditions.Add("jb.work_arrangement = @WorkArrangement");
                countParameters.Add(new SqlParameter("@WorkArrangement", filters.WorkArrangement));
                mainParameters.Add(new SqlParameter("@WorkArrangement", filters.WorkArrangement));
            }

            if (filters.Salary.HasValue && filters.Salary > 0)
            {
                conditions.Add("jp.maximum_pay >= @Salary");
                countParameters.Add(new SqlParameter("@Salary", filters.Salary.Value));
                mainParameters.Add(new SqlParameter("@Salary", filters.Salary.Value));
            }

            if (!string.IsNullOrEmpty(filters.Company))
            {
                conditions.Add("jo.company_name LIKE @Company");
                countParameters.Add(new SqlParameter("@Company", $"%{filters.Company}%"));
                mainParameters.Add(new SqlParameter("@Company", $"%{filters.Company}%"));
            }

            if (!string.IsNullOrEmpty(filters.Language))
            {
                conditions.Add("EXISTS (SELECT 1 FROM OPENJSON(jq.language) WHERE value = @Language)");
                countParameters.Add(new SqlParameter("@Language", filters.Language));
                mainParameters.Add(new SqlParameter("@Language", filters.Language));
            }

            // Handle Job Types (comma-separated string in jd.emptype)
            if (filters.JobTypes != null && filters.JobTypes.Any())
            {
                var jobTypeConditions = new List<string>();
                for (int i = 0; i < filters.JobTypes.Count; i++)
                {
                    var paramName = $"@JobType{i}";
                    jobTypeConditions.Add($"(',' + jd.emptype + ',' LIKE '%,' + {paramName} + ',%')");

                    // FIX: Add to BOTH parameter lists!
                    countParameters.Add(new SqlParameter(paramName, filters.JobTypes[i].Trim()));
                    mainParameters.Add(new SqlParameter(paramName, filters.JobTypes[i].Trim()));
                }
                conditions.Add("(" + string.Join(" OR ", jobTypeConditions) + ")");
            }

            // Handle Education Levels (JSON array in jq.education)
            if (filters.EducationLevels != null && filters.EducationLevels.Any())
            {
                var eduConditions = new List<string>();
                for (int i = 0; i < filters.EducationLevels.Count; i++)
                {
                    var paramName = $"@Edu{i}";
                    eduConditions.Add($"EXISTS (SELECT 1 FROM OPENJSON(jq.education) WHERE value = {paramName})");
                    countParameters.Add(new SqlParameter(paramName, filters.EducationLevels[i]));
                    mainParameters.Add(new SqlParameter(paramName, filters.EducationLevels[i]));
                }
                conditions.Add("(" + string.Join(" OR ", eduConditions) + ")");
            }

            // Handle Date Posted
            if (!string.IsNullOrEmpty(filters.DatePosted))
            {
                var days = filters.DatePosted switch
                {
                    "24h" => 1,
                    "3d" => 3,
                    "7d" => 7,
                    "14d" => 14,
                    _ => 0
                };

                if (days > 0)
                {
                    conditions.Add("jo.created >= DATEADD(day, -@Days, GETDATE())");
                    countParameters.Add(new SqlParameter("@Days", days));
                    mainParameters.Add(new SqlParameter("@Days", days));
                }
            }

            // Build the complete WHERE clause
            string whereClause = conditions.Count > 0 ? " AND " + string.Join(" AND ", conditions) : "";

            // Get total count with filters
            var countQuery = countBaseQuery + whereClause;
            using var countCmd = new SqlCommand(countQuery, con);
            countCmd.Parameters.AddRange(countParameters.ToArray());
            pagination.TotalCount = (int)await countCmd.ExecuteScalarAsync();

            // Add pagination to main query
            var offset = (filters.Page - 1) * pageSize;
            var mainQuery = baseQuery + whereClause + " ORDER BY jo.created DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            using var cmd = new SqlCommand(mainQuery, con);
            cmd.Parameters.AddRange(mainParameters.ToArray());
            cmd.Parameters.AddWithValue("@Offset", offset);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                result.Add(new IndeedCloneLeftJobCardsDTO
                {
                    JobUid = reader["job_uid"].ToString(),
                    CompanyName = reader["company_name"].ToString(),
                    JobTitle = reader["job_title"].ToString(),
                    JobLocation = reader["job_location"].ToString(),
                    City = reader["city"].ToString(),
                    Area = reader["area"].ToString(),
                    StreetAddress = reader["street_address"].ToString(),
                    RecruitmentTimeline = reader["recruitment_timeline"].ToString(),
                    PayType = Enum.Parse<PayType>(reader["pay_type"].ToString()),
                    MinimumPay = Convert.ToDecimal(reader["minimum_pay"]),
                    MaximumPay = Convert.ToDecimal(reader["maximum_pay"]),
                    PayRateType = Enum.Parse<PayRateType>(reader["pay_rate_type"].ToString()),
                    EmployeeType = Parser.ToList(reader["emptype"]?.ToString() ?? "[]"),
                    SupplementedPay = Parser.ToList(reader["supplemented_pay"]?.ToString() ?? "[]"),
                    Benefits = Parser.ToList(reader["benefits"]?.ToString() ?? "[]"),
                    Created = Convert.ToDateTime(reader["created"]),
                    Edited = Convert.ToDateTime(reader["edited"]),
                    PostedAgo = DateTimeAgoHelper.GetPostedAgo(Convert.ToDateTime(reader["created"]), Convert.ToDateTime(reader["edited"]))
                });
            }

            return new IndeedClonePaginatedJobCardsDTO
            {
                Jobs = result,
                Pagination = pagination,
                JobSearchFilter = filters
            };
        }


    }
}
