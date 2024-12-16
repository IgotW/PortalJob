using System.ComponentModel.Design;
using System.Security.Claims;
using JobSystem.Entities;
using JobSystem.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace JobSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CandidateRegistration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CandidateRegistration(CandidateRegViewModel model)
        {
            if (ModelState.IsValid)
            {
                CandidateAccount account = new CandidateAccount();
                account.FirstName = model.FirstName;
                account.LastName = model.LastName;
                account.Email = model.Email;
                account.Phone = model.Phone;
                account.Password = model.Password;
                account.Profession = model.Profession;
                account.YearsofExperience = model.YearsofExperience;
                account.ProfessionSummary = model.ProfessionSummary;

                try
                {
                    _context.CandidateAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} registered successfully. Please Login.";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter unique Email");
                    return View(model);
                }
                return View();
            }
            return View(model);
        }
        public IActionResult CompanyRegistration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CompanyRegistration(CompanyRegViewModel model)
        {
            if (ModelState.IsValid)
            {
                CompanyAccount account = new CompanyAccount();
                account.CompanyName = model.CompanyName;
                account.Industry = model.Industry;
                account.Email = model.Email;
                account.Phone = model.Phone;
                account.Password = model.Password;
                account.CompanyWebsite = model.CompanyWebsite;
                account.CompanyDescription = model.CompanyDescription;

                try
                {
                    _context.CompanyAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.CompanyName} registered successfully. Please Login.";
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "Please enter unique Email");
                    return View(model);
                }
            }
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var candidate = _context.CandidateAccounts.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();
                var company = _context.CompanyAccounts.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefault();
                if (candidate != null)
                {
                    //Success, create cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, candidate.Email),
                        new Claim("Name", candidate.FirstName),
                        new Claim(ClaimTypes.Role, "Candidate")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("CandidateDashPage");
                }
                else if (company != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, company.Email),
                        new Claim("CompanyId", company.CompanyID.ToString()),
                        new Claim("Name", company.CompanyName),
                        new Claim(ClaimTypes.Role, "Company")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("CompanyDashPage");
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password is incorrect");
                }
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [Authorize]
        public IActionResult CandidateDashPage(string searchBy, string searchString)
        {
            //var jobPostings = _context.JobPostings.AsQueryable();
            //// Ensure the user is logged in
            //string currentUserName = HttpContext.User.Identity.Name;
            //if (string.IsNullOrEmpty(currentUserName))
            //{
            //    return RedirectToAction("Login", "Account");
            //}

            //// Check if the search string is provided
            //if (string.IsNullOrEmpty(searchString))
            //{
            //    // Return the view with no results if the search string is empty
            //    ViewData["ErrorMessage"] = "Please enter a valid search term.";
            //    return View(new List<JobPosting>());
            //}

            //// Search for matching job postings in the database
            //var searchResults = _context.JobPostings
            //    .Where(x => x.Title.Contains(searchString) ||
            //                x.CompanyName.Contains(searchString) ||
            //                x.Location.Contains(searchString))
            //    .ToList();

            //// Check if there are any results
            //if (searchResults.Count == 0)
            //{
            //    ViewData["ErrorMessage"] = "No matching jobs found.";
            //}

            //return View(searchResults);
            // Ensure the user is logged in
            string currentUserName = HttpContext.User.Identity.Name;
            if (string.IsNullOrEmpty(currentUserName))
            {
                return RedirectToAction("Login", "Account");
            }

            // Start with all job postings
            var jobPostings = _context.JobPostings.AsQueryable();

            // Filter by job type if a specific type is selected
            if (!string.IsNullOrEmpty(searchBy))
            {
                jobPostings = jobPostings.Where(x => x.Type == searchBy);
            }

            // Apply search filter if the search string is provided
            if (!string.IsNullOrEmpty(searchString))
            {
                jobPostings = _context.JobPostings
                .Where(x => x.Title.Contains(searchString) ||
                            x.CompanyName.Contains(searchString) ||
                            x.Location.Contains(searchString));
            }

            // Convert query to a list and pass it to the view
            return View(jobPostings.ToList());
        }



        [Authorize]
        public IActionResult CompanyDashPage()
        {
            ////IEnumerable<JobPosting> objJobList = _context.JobPostings;
            ////return View(objJobList);
            ////ViewBag.Name = HttpContext.User.Identity.Name;
            //int currentCompanyId = 1;
            //var jobPostings = _context.JobPostings
            //    .Where(j => j.CompanyId == currentCompanyId)
            //    .ToList();
            //return View(jobPostings);
            //// Get the list of jobs from the database
            ////int companyId = GetCurrentCompanyId();
            ////var jobs = _context.JobPostings.Where(j => j.CompanyId == companyId).ToList();

            ////var model = new CompanyDashboardViewModel
            ////{
            ////    JobList = jobs,
            ////    CreateJobModel = new JobPostingViewModel()
            ////};

            ////return View(model);
            string currentUserName = HttpContext.User.Identity.Name;

            if (string.IsNullOrEmpty(currentUserName))
            {
                // If the user is not logged in, redirect to the login page or show an error
                return RedirectToAction("Login", "Account");
            }

            // Assuming you have a way to map the user to a company, get the current company's ID
            // For example, if you have a Users table that includes CompanyId:
            var currentCompany = _context.CompanyAccounts
                .Where(u => u.Email == currentUserName)
                .Select(u => u.CompanyID)
                .FirstOrDefault();

            if (currentCompany == null)
            {
                // If no company is associated with the user, return an appropriate response
                return View("Error", "Company not found for the current user.");
            }

            // Fetch the job postings associated with the company
            var jobPostings = _context.JobPostings
                .Where(j => j.CompanyId == currentCompany)
                .ToList();

            return View(jobPostings);
        }

        private int GetCurrentCompanyId()
        {
            // Get the CompanyId claim from the authenticated user's claims
            var companyIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CompanyId");
            if (companyIdClaim != null && int.TryParse(companyIdClaim.Value, out int companyId))
            {
                return companyId;
            }

            throw new Exception("CompanyId not found in claims.");
        }

        private int GetCurrentCandidateId()
        {
            // Get the CandidateId claim from the authenticated user's claims
            var candidateIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CandidateId");
            if (candidateIdClaim != null && int.TryParse(candidateIdClaim.Value, out int candidateId))
            {
                return candidateId;
            }

            throw new Exception("CandidateId not found in claims.");
        }


        [HttpGet]
        public IActionResult CreateJob()
        {
            //JobPostingViewModel post = new JobPostingViewModel();
            //return PartialView("_CreateJobForm", post);
            //if (ModelState.IsValid)
            //{
            //    int companyId = GetCurrentCompanyId();

            //    // Retrieve the company name from the database
            //    var company = _context.CompanyAccounts.FirstOrDefault(c => c.CompanyID == companyId);
            //    if (company == null)
            //    {
            //        ModelState.AddModelError("", "Invalid company account.");
            //        return View("CompanyDashPage", model);
            //    }

            //    JobPosting post = new JobPosting();
            //    post.CompanyId = companyId;
            //    post.CompanyName = company.CompanyName;
            //    post.Title = model.Title;
            //    post.Location = model.Location;
            //    post.Type = model.Type;
            //    post.Salary = model.Salary;
            //    post.Description = model.Description;
            //    post.PostedDate = DateTime.Now;


            //    _context.JobPostings.Add(post);
            //    _context.SaveChanges();

            //    return RedirectToAction("CompanyDashPage");
            //}

            //return View("CompanyDashPage", model);
            //return View();
            return View();
        }

        [HttpPost]
        public IActionResult CreateJob(JobPostingViewModel model)
        {

            int companyId = GetCurrentCompanyId();

            // Retrieve the company name from the database
            var company = _context.CompanyAccounts.FirstOrDefault(c => c.CompanyID == companyId);
            if (company == null)
            {
                ModelState.AddModelError("", "Invalid company account.");
                return View("CompanyDashPage", model);
            }

            JobPosting post = new JobPosting();
            post.CompanyId = companyId;
            post.CompanyName = company.CompanyName;
            post.Title = model.Title;
            post.Location = model.Location;
            post.Type = model.Type;
            post.Salary = model.Salary;
            post.Description = model.Description;
            post.PostedDate = DateTime.Now;

            _context.JobPostings.Add(post);
            _context.SaveChanges();
            return RedirectToAction("CompanyDashPage");


            //if (ModelState.IsValid)
            //{
            //    int companyId = GetCurrentCompanyId();

            //    // Retrieve the company name from the database
            //    var company = _context.CompanyAccounts.FirstOrDefault(c => c.CompanyID == companyId);
            //    if (company == null)
            //    {
            //        ModelState.AddModelError("", "Invalid company account.");
            //        return View("CompanyDashPage", model);
            //    }

            //    JobPosting post = new JobPosting();
            //    post.CompanyId = companyId;
            //    post.CompanyName = company.CompanyName;
            //    post.Title = model.Title;
            //    post.Location = model.Location;
            //    post.Type = model.Type;
            //    post.Salary = model.Salary;
            //    post.Description = model.Description;
            //    post.PostedDate = DateTime.Now;


            //    _context.JobPostings.Add(post);
            //    _context.SaveChanges();

            //    return RedirectToAction("CompanyDashPage");
            //}

            //return View("CompanyDashPage", model);

            //if (ModelState.IsValid)
            //{
            //    int companyId = GetCurrentCompanyId(); // Your method to get the current company ID

            //    var company = _context.CompanyAccounts.FirstOrDefault(c => c.CompanyID == companyId);
            //    if (company == null)
            //    {
            //        ModelState.AddModelError("", "Invalid company account.");
            //        return View("CompanyDashPage");
            //    }

            //    JobPosting post = new JobPosting
            //    {
            //        CompanyId = companyId,
            //        CompanyName = company.CompanyName,
            //        Title = model.Title,
            //        Location = model.Location,
            //        Type = model.Type,
            //        Salary = model.Salary,
            //        Description = model.Description,
            //        PostedDate = DateTime.Now
            //    };

            //    _context.JobPostings.Add(post);
            //    _context.SaveChanges();

            //    // Redirect to dashboard after saving
            //    return RedirectToAction("CompanyDashPage");
            //}

            //// Return to the same view if validation fails
            //return View("CompanyDashPage");

        }

        [HttpGet]
        public IActionResult EditJob(int id)
        {
            // Retrieve the job posting from the database using the provided ID
            var jobPosting = _context.JobPostings.Find(id);

            if (jobPosting == null)
            {
                return NotFound();  // Return a 404 if the job posting is not found
            }

            // Map the entity data to the ViewModel
            var model = new JobPostingViewModel
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                Location = jobPosting.Location,
                Type = jobPosting.Type,
                Salary = jobPosting.Salary,
                Description = jobPosting.Description,
                PostedDate = jobPosting.PostedDate
            };

            //var jobPosting = _context.JobPostings.Find(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult EditJob(JobPostingViewModel model)
        {
            // Retrieve the job posting from the database using the provided ID
            var existingJobPosting = _context.JobPostings.Find(model.Id);

            if (existingJobPosting == null)
            {
                return NotFound();  // Return a 404 if the job posting is not found
            }

            // Map values from the view model to the entity object
            existingJobPosting.Title = model.Title;
            existingJobPosting.Location = model.Location;
            existingJobPosting.Type = model.Type;
            existingJobPosting.Salary = model.Salary;
            existingJobPosting.Description = model.Description;
            existingJobPosting.PostedDate = DateTime.Now;  // Example, if you want to update posted date

            // Mark the entity as modified and save changes
            _context.Entry(existingJobPosting).State = EntityState.Modified;

            _context.SaveChanges();  // Save changes to the database

            return RedirectToAction("CompanyDashPage");
        }
        [HttpGet]
        public IActionResult DeleteJob(int id)
        {
            // Retrieve the job posting from the database using the provided ID
            var jobPosting = _context.JobPostings.Find(id);

            if (jobPosting == null)
            {
                return NotFound();  // Return a 404 if the job posting is not found
            }

            // Map the entity data to the ViewModel
            var model = new JobPostingViewModel
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                Location = jobPosting.Location,
                Type = jobPosting.Type,
                Salary = jobPosting.Salary,
                Description = jobPosting.Description,
                PostedDate = jobPosting.PostedDate
            };

            //var jobPosting = _context.JobPostings.Find(id);
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteJob(JobPostingViewModel mode, int id)
        {
            var job = _context.JobPostings.FirstOrDefault(j => j.Id == id);
            if (job == null)
            {
                return NotFound();
            }
            _context.JobPostings.Remove(job);
            _context.SaveChanges();
            return RedirectToAction("CompanyDashPage");
        }

        public ActionResult Details(int id)
        {
            var jobPosting = _context.JobPostings.Find(id);

            if (jobPosting == null)
            {
                return NotFound();  // Return a 404 if the job posting is not found
            }

            // Map the entity data to the ViewModel
            var model = new JobPostingViewModel
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                CompanyName = jobPosting.CompanyName,
                Location = jobPosting.Location,
                Type = jobPosting.Type,
                Salary = jobPosting.Salary,
                Description = jobPosting.Description,
                PostedDate = jobPosting.PostedDate
            };

            //var jobPosting = _context.JobPostings.Find(id);
            return View(model);
        }

        //[HttpPost]
        //public IActionResult DeleteJob(int id)
        //{
        //    var job = _context.JobPostings.FirstOrDefault(j => j.Id == id);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.JobPostings.Remove(job);
        //    _context.SaveChanges();

        //    return RedirectToAction("CompanyDashPage");
        //}

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Apply(ApplicationViewModel model)
        {
            int candidateId = GetCurrentCandidateId();

            // Retrieve the company name from the database
            var candidate = _context.CandidateAccounts.FirstOrDefault(c => c.CandidateID == candidateId);
            if (candidate == null)
            {
                ModelState.AddModelError("", "Invalid company account.");
                return View("CompanyDashPage", model);
            }

            Application app = new Application();
            app.CandidateId = candidateId;
            app.Resume = model.Resume;
            app.ApplicationDescription = model.ApplicationDescription;
            app.DateApplied = DateOnly.FromDateTime(DateTime.Now);

            _context.Applications.Add(app);
            _context.SaveChanges();
            return RedirectToAction("CandidateDashPage");
        }

        public ActionResult DetailsCan(int id)
        {
            var jobPosting = _context.JobPostings.Find(id);

            if (jobPosting == null)
            {
                return NotFound();  // Return a 404 if the job posting is not found
            }

            // Map the entity data to the ViewModel
            var model = new JobPostingViewModel
            {
                Id = jobPosting.Id,
                Title = jobPosting.Title,
                CompanyName = jobPosting.CompanyName,
                Location = jobPosting.Location,
                Type = jobPosting.Type,
                Salary = jobPosting.Salary,
                Description = jobPosting.Description,
                PostedDate = jobPosting.PostedDate
            };

            //var jobPosting = _context.JobPostings.Find(id);
            return View(model);
        }
    }
}
