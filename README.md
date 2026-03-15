# IndeedClone – ASP.NET Core MVC Job Portal (HMVC Architecture)

IndeedClone is a full-stack job portal web application inspired by Indeed.
It allows recruiters to post and manage job listings while candidates can search, view, and apply for jobs.

The project is built using **ASP.NET Core MVC with a modular HMVC architecture**, where each module is self-contained and manages its own controllers, services, repositories, and views. This approach improves maintainability, scalability, and feature isolation.

---

# Key Highlights

* Modular **HMVC architecture**
* Hybrid data access strategy (**Entity Framework Core + ADO.NET**)
* Multi-step recruiter **job posting workflow**
* **Transaction-based job activation system**
* **AJAX-powered UI interactions**
* Resume upload, preview, and download system
* Centralized error handling using **ErrorError library**
* Clean **Repository + Service layer architecture**

---

# Screenshots
**Home Page**
![image alt](https://github.com/ShubahmBorude/indeedclone-aspnet/blob/3bb7be0e940e6ba5e8586ffe6b68f8d194c93bce/Screenshot%20(200).png)
![image alt](https://github.com/ShubahmBorude/indeedclone-aspnet/blob/a20cdb9edb530be7883cfcceee3949ef13709aee/Screenshot%20(195).png)
![image alt](https://github.com/ShubahmBorude/indeedclone-aspnet/blob/2d432602f38b8f8527890860e48054041b211e02/Screenshot%20(199).png)
**Login Page**
![image alt](https://github.com/ShubahmBorude/indeedclone-aspnet/blob/97d068d9d80c777c0058b7cff84445e47645dcf4/Screenshot%20(196).png)

**Employers/Jobpost First Page**
![image alt](https://github.com/ShubahmBorude/indeedclone-aspnet/blob/97d068d9d80c777c0058b7cff84445e47645dcf4/Screenshot%20(202).png)
![image alt](https://github.com/ShubahmBorude/indeedclone-aspnet/blob/97d068d9d80c777c0058b7cff84445e47645dcf4/Screenshot%20(204).png)

**Dashboard Page**
![image alt]()

**Apply First Page**
![image alt]()


# Folder Structure

```text
.
└── IndeedClone
    ├── Emails
    │
    ├── Modules
    │   ├── IndeedClone        # Main job search module
    │   ├── JobDashboard       # Recruiter dashboard
    │   ├── Shared             # Shared components used across modules
    │   └── SubModules
    │       ├── Auth           # Login, Register, OTP (2FA), Forgot Password, Google Sign-In
    │       ├── JobApplication # Job seeker module (CV upload, experience, screening questions)
    │       └── JobPost        # Employer job posting workflow
    │
    ├── Storage                # Stores uploaded user files (resumes)
    │
    └── ThirdParty             # Custom helper libraries
```

---

# HMVC Module Architecture

This project follows a **modular HMVC (Hierarchical Model View Controller)** architecture.

Each folder inside the `Modules` directory acts as a **self-contained feature module**.

Every module typically includes its own:

* Controllers
* Models
* DTOs
* Enums
* Helpers
* ServiceContracts
* Services
* RepoContracts
* Repositories
* Views

This design ensures:

* Independent module development
* Better separation of concerns
* Improved maintainability
* Easier feature expansion

Some modules may contain additional folders depending on workflow and feature complexity.

---

# Tech Stack

* **ASP.NET Core MVC (.NET 8.0.22)**
* **C#**
* **SQL Server**
* **Entity Framework Core**
* **ADO.NET**
* **AJAX**
* **Razor Views**
* **Bootstrap**

---

# Data Access Strategy

The project uses a **hybrid data access approach**.

**Entity Framework Core**

* Used for most CRUD operations
* Provides maintainability and developer productivity

**ADO.NET**

* Used for optimized read operations
* Applied in **job search and filtering queries** to improve performance

---

# Features

## Authentication

* User registration
* Login system
* Email verification via OTP
* Forgot password workflow
* Google Sign-In integration

---

## Job Posting (Recruiter)

* Multi-step **JobPost workflow (8 pages)**
* Draft job creation
* Resume / continue draft functionality
* Transaction-based job activation
* Structured employer job creation pipeline

---

## Job Search

* Keyword and location search
* Time filtering
* Remote / work arrangement filtering
* Salary filtering
* Job type filtering
* Company filtering
* Education filtering
* Language filtering
* Pagination support

---

## Job Application

* Apply to jobs
* Resume upload
* Resume preview (PDF)
* Resume download
* Relevant experience tracking
* Employer screening questions
* Resume / continue draft functionality

---

## Recruiter Dashboard

* Employer posted jobs list
* Applicant list management
* Candidate status update (Shortlisted / Rejected)
* Live filtering
* Pagination

---

# Architecture

The backend follows a structured architecture with clear separation of responsibilities.

* Modular HMVC architecture
* SOLID principles
* Repository pattern
* Service layer abstraction
* DTO pattern
* Centralized error handling (**ErrorError library**)

This architecture helps maintain **clean code, scalability, and maintainability**.

---

# Getting Started

Follow these steps to run the project locally.

## Prerequisites

Make sure the following tools are installed:

* **.NET 8.0.22 SDK**
* **SQL Server**
* **Visual Studio 2022 or later**

---

## Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/your-username/IndeedClone.git
```

---

### 2. Open the project

Open the solution file:

```
IndeedClone.sln
```

using **Visual Studio**.

---

### 3. Configure the database

Update the connection string in:

```
appsettings.json
```

to match your **SQL Server instance**.

---

### 4. Create the database

Run **Entity Framework migrations** or create the database schema manually.

---

### 5. Run the project

Press **F5** in Visual Studio or run the project from the IDE.

---

# Future Improvements

* Role-based authorization (Admin / Recruiter / Job Seeker)
* Background job processing for email notifications
* Search performance improvements using indexing
* API layer for mobile or third-party integrations
* Unit and integration testing

---

# Author

**Shubham Borude**

Email: [shubhamborude4488@gmail.com](mailto:shubhamborude4488@gmail.com)

---

# License

This project is created for **educational and portfolio purposes**.
