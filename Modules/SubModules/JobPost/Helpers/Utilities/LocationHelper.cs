namespace IndeedClone.Modules.SubModules.JobPost.Helpers.Locations
{
    public static class LocationHelper
    {
            public static string[] States { get; } =
            {
                "Karnataka",
                "Telangana",
                "Tamil Nadu",
                "Maharashtra",
                "Haryana",
                "Uttar Pradesh",
                "Kerala",
                "West Bengal",
                "Andhra Pradesh",
                "Odisha",
                "Madhya Pradesh",
                "Gujarat",
                "Punjab",
                "Rajasthan"
            };

            public static string[][] Cities { get; } =
            {
                new string[] { "Bengaluru", "Mysuru", "Mangalore" },              // Karnataka
                new string[] { "Hyderabad", "Warangal" },                         // Telangana
                new string[] { "Chennai", "Coimbatore", "Madurai" },              // Tamil Nadu
                new string[] { "Pune", "Mumbai", "Nagpur" },                      // Maharashtra
                new string[] { "Gurgaon", "Faridabad" },                          // Haryana
                new string[] { "Noida", "Lucknow", "Ghaziabad" },                 // Uttar Pradesh
                new string[] { "Kochi", "Thiruvananthapuram", "Kozhikode" },      // Kerala
                new string[] { "Kolkata", "Siliguri", "Durgapur" },               // West Bengal
                new string[] { "Visakhapatnam", "Vijayawada", "Tirupati" },       // Andhra Pradesh
                new string[] { "Bhubaneswar", "Cuttack" },                        // Odisha
                new string[] { "Indore", "Bhopal", "Jabalpur" },                  // Madhya Pradesh
                new string[] { "Ahmedabad", "Gandhinagar", "Surat", "Vadodara" }, // Gujarat
                new string[] { "Mohali", "Chandigarh", "Ludhiana" },              // Punjab
                new string[] { "Jaipur", "Udaipur", "Jodhpur" }                   // Rajasthan
            };

            public static string[][][] Areas { get; } =
            {
                // Karnataka
                new string[][]
                {
                    new string[] { "Whitefield", "Electronic City", "Koramangala", "Indiranagar" }, // Bengaluru
                    new string[] { "Hebbal", "Vijayanagar" },                                       // Mysuru
                    new string[] { "Kadri", "Bejai" }                                               // Mangalore
                },
                // Telangana
                new string[][]
                {
                    new string[] { "Hitech City", "Gachibowli", "Banjara Hills", "Secunderabad" }, // Hyderabad
                    new string[] { "Hanamkonda", "Kazipet" }                                       // Warangal
                },
                // Tamil Nadu
                new string[][]
                {
                    new string[] { "T. Nagar", "Velachery", "OMR", "Anna Nagar" },                 // Chennai
                    new string[] { "RS Puram", "Peelamedu" },                                      // Coimbatore
                    new string[] { "KK Nagar", "Anna Nagar" }                                      // Madurai
                },
                // Maharashtra
                new string[][]
                {
                    new string[] { "Hinjewadi", "Magarpatta", "Baner", "Kothrud" },                // Pune
                    new string[] { "Andheri", "Powai", "Borivali", "Navi Mumbai" },                // Mumbai
                    new string[] { "Sitabuldi", "Dharampeth" }                                     // Nagpur
                },
                // Haryana
                new string[][]
                {
                    new string[] { "DLF Cyber City", "Sohna Road" },                               // Gurgaon
                    new string[] { "Sector 15", "Sector 28" }                                      // Faridabad
                },
                // Uttar Pradesh
                new string[][]
                {
                    new string[] { "Sector 62", "Sector 18" },                                     // Noida
                    new string[] { "Hazratganj", "Gomti Nagar" },                                  // Lucknow
                    new string[] { "Raj Nagar", "Indirapuram" }                                    // Ghaziabad
                },
                // Kerala
                new string[][]
                {
                    new string[] { "Infopark", "Marine Drive" },                                   // Kochi
                    new string[] { "Technopark", "Kazhakoottam" },                                 // Thiruvananthapuram
                    new string[] { "Mavoor Road", "Kallai" }                                       // Kozhikode
                },
                // West Bengal
                new string[][]
                {
                    new string[] { "Salt Lake", "New Town", "Park Street" },                       // Kolkata
                    new string[] { "Sevoke Road" },                                                // Siliguri
                    new string[] { "Benachity", "City Centre" }                                    // Durgapur
                },
                // Andhra Pradesh
                new string[][]
                {
                    new string[] { "MVP Colony", "Dwaraka Nagar" },                                // Visakhapatnam
                    new string[] { "Benz Circle", "Governorpet" },                                 // Vijayawada
                    new string[] { "Kapila Theertham", "Korlagunta" }                              // Tirupati
                },
                // Odisha
                new string[][]
                {
                    new string[] { "Patia", "Khandagiri" },                                        // Bhubaneswar
                    new string[] { "Buxi Bazaar", "College Square" }                               // Cuttack
                },
                // Madhya Pradesh
                new string[][]
                {
                    new string[] { "Vijay Nagar", "Rajwada" },                                     // Indore
                    new string[] { "MP Nagar", "Arera Colony" },                                   // Bhopal
                    new string[] { "Napier Town", "Wright Town" }                                  // Jabalpur
                },
                // Gujarat
                new string[][]
                {
                    new string[] { "Navrangpura", "SG Highway" },                                  // Ahmedabad
                    new string[] { "Infocity", "Sector 21" },                                      // Gandhinagar
                    new string[] { "Adajan", "Vesu" },                                             // Surat
                    new string[] { "Alkapuri", "Manjalpur" }                                       // Vadodara
                },
                // Punjab
                new string[][]
                {
                    new string[] { "Phase 7", "Sector 70" },                                       // Mohali
                    new string[] { "Sector 17", "Sector 22" },                                     // Chandigarh
                    new string[] { "Model Town", "Civil Lines" }                                   // Ludhiana
                },
                // Rajasthan
                new string[][]
                {
                    new string[] { "Malviya Nagar", "Vaishali Nagar" },                            // Jaipur
                    new string[] { "Hiran Magri", "Fatehpura" },                                   // Udaipur
                    new string[] { "Paota", "Ratanada" }                                           // Jodhpur
                }
    };
        
    }
}
