# SecureUserPanel

ASP.NET Core MVC application for user account security practice.

---

## How to Run

Prerequisites: [.NET 8+ SDK](https://dotnet.microsoft.com/download)

```bash
cd SecureUserPanel
dotnet run
```

The SQLite database is created automatically on first run. Two test users are seeded automatically.

---

## Test Users

| Role  | Email             | Password   |
|-------|-------------------|------------|
| Admin | admin@example.com | Admin1234! |
| User  | user@example.com  | User1234!  |

> Local demo credentials only. Do not reuse these anywhere.

---

## How to Log In as Admin

1. Open `/Account/Login`
2. Enter `admin@example.com` / `Admin1234!`
3. Navigate to `/Admin`

---

## Where Password Hashing Code Is

`Data/DbSeeder.cs` — hashes seed user passwords with `BCrypt.Net.BCrypt.HashPassword(...)`.

`Controllers/AccountController.cs`:
- `Register` action — hashes the new password before saving to the database
- `Login` action — verifies the submitted password with `BCrypt.Net.BCrypt.Verify(...)`

Library used: `BCrypt.Net-Next` (NuGet). BCrypt automatically generates and stores a random salt with every hash.

---

## Where Authentication Is Configured

`Program.cs` — registers cookie authentication:

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => { ... });
```

`Controllers/AccountController.cs`:
- `SignInUserAsync` — builds a `ClaimsPrincipal` and calls `HttpContext.SignInAsync`
- `Logout` action — calls `HttpContext.SignOutAsync`

---

## Actions Protected with `[Authorize]`

| Controller         | Attribute                      | Who can access        |
|--------------------|--------------------------------|-----------------------|
| `DashboardController` (all actions) | `[Authorize]`   | Any logged-in user    |
| `AdminController` (all actions)     | `[Authorize(Roles = "Admin")]` | Admin role only |

Anonymous users visiting `/Dashboard` are redirected to `/Account/Login`.
A logged-in non-Admin user visiting `/Admin` is redirected to `/Account/AccessDenied`.

---

## Security Questions

**1. Why must passwords not be stored as plain text?**

If the database is ever leaked — through SQL injection, a stolen backup, or an insider threat — every user's password is immediately exposed. Since users often reuse passwords across sites, the damage extends far beyond your application.

**2. Why is raw SHA-256 not a good choice for passwords?**

SHA-256 is a fast general-purpose hash. A modern GPU can compute billions of SHA-256 hashes per second, making brute-force and dictionary attacks trivial. Password hashing algorithms like BCrypt are intentionally slow and computationally expensive, making bulk cracking impractical even with powerful hardware.

**3. Why do we use salt?**

A salt is a random value added to the password before hashing. It ensures two users with the same password produce different hashes, and it makes precomputed rainbow-table attacks useless because a separate table would need to be built for every unique salt.

**4. What is the difference between salt and pepper?**

Salt is random, unique per password, and stored alongside the hash in the database. Pepper is a secret value stored outside the database (e.g., in environment variables or user secrets) and shared across all passwords. Even with a full database dump, an attacker cannot verify guesses without the pepper. The downside is that losing the pepper makes all hashes unverifiable.

**5. What is the difference between authentication and authorization?**

Authentication answers "Who are you?" — verifying identity via login credentials. Authorization answers "What are you allowed to do?" — checking whether the authenticated identity has the required permission, such as the Admin role needed to access `/Admin`.

**6. Why is hiding a link in a view not enough as security?**

The view only controls what HTML is sent to the browser. Any user can type `/Admin` directly into the address bar, use `curl`, or inspect the page source to find hidden routes. The server must enforce authorization on every incoming request regardless of what the UI shows.

**7. Why can a "there is no such user" login message be a problem?**

It allows an attacker to enumerate valid email addresses by observing which emails are recognized by the system. With a confirmed list of valid emails they can run targeted phishing, credential-stuffing, or brute-force attacks. A generic "Invalid login credentials" message reveals nothing useful about what went wrong.
