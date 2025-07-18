# Security Documentation

This document outlines the security measures implemented in the Arif Tan Portfolio website, designed to meet enterprise-grade security standards suitable for Singapore's technology industry.

## 🛡️ Security Overview

The application implements multiple layers of security:

1. **Input Validation & Sanitization**
2. **Rate Limiting**
3. **CSRF Protection**
4. **Security Headers**
5. **Secrets Management**
6. **SQL Injection Prevention**

## 🔐 Security Features

### 1. Rate Limiting Middleware

**Purpose**: Prevent abuse and DDoS attacks

**Implementation**: `RateLimitingMiddleware.cs`

```csharp
public class RateLimitingMiddleware
{
    private static readonly ConcurrentDictionary<string, RateLimitInfo> _clients = new();
    
    public async Task InvokeAsync(HttpContext context)
    {
        var key = GetClientKey(context);
        var rateLimitInfo = _clients.GetOrAdd(key, _ => new RateLimitInfo());
        
        // Rate limiting logic
        if (rateLimitInfo.RequestCount >= GetRateLimit(context))
        {
            context.Response.StatusCode = 429;
            await context.Response.WriteAsync("Rate limit exceeded");
            return;
        }
        
        await _next(context);
    }
}
```

**Rate Limits**:
- **Contact Form**: 5 requests per minute
- **General Browsing**: 60 requests per minute
- **Key Generation**: Client IP + User Agent hash

**Client Identification**: Combines IP address and User Agent for unique client tracking

### 2. Security Headers Middleware

**Purpose**: Implement security headers to prevent common web vulnerabilities

**Implementation**: `SecurityHeadersMiddleware.cs`

```csharp
public class SecurityHeadersMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        var headers = context.Response.Headers;
        
        // Prevent clickjacking
        headers["X-Frame-Options"] = "DENY";
        
        // Prevent MIME type sniffing
        headers["X-Content-Type-Options"] = "nosniff";
        
        // Enable XSS protection
        headers["X-XSS-Protection"] = "1; mode=block";
        
        // Referrer policy
        headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
        
        // Content Security Policy
        headers["Content-Security-Policy"] = "default-src 'self'; ...";
        
        // Permissions Policy
        headers["Permissions-Policy"] = "camera=(), microphone=(), ...";
        
        // HSTS for HTTPS requests
        if (context.Request.IsHttps)
        {
            headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains; preload";
        }
        
        await _next(context);
    }
}
```

**Security Headers Implemented**:

| Header | Value | Purpose |
|--------|-------|---------|
| `X-Frame-Options` | `DENY` | Prevents clickjacking attacks |
| `X-Content-Type-Options` | `nosniff` | Prevents MIME type sniffing |
| `X-XSS-Protection` | `1; mode=block` | Enables XSS protection |
| `Referrer-Policy` | `strict-origin-when-cross-origin` | Controls referrer information |
| `Content-Security-Policy` | Restrictive policy | Prevents XSS and data injection |
| `Permissions-Policy` | Restrictive permissions | Disables unnecessary browser features |
| `Strict-Transport-Security` | `max-age=31536000; includeSubDomains; preload` | Enforces HTTPS |

### 3. CSRF Protection

**Purpose**: Prevent Cross-Site Request Forgery attacks

**Implementation**: ASP.NET Core built-in antiforgery system

```csharp
// Program.cs
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.SuppressXFrameOptionsHeader = false;
});
```

**Contact Form Protection**:
```html
<!-- Contact.cshtml -->
<form asp-page-handler="SendMessage" method="post">
    @Html.AntiForgeryToken()
    <!-- Form fields -->
</form>
```

**Validation**: Automatic validation on all POST requests

### 4. Input Validation & Sanitization

**Purpose**: Prevent injection attacks and ensure data integrity

**Model Validation**:
```csharp
public class ContactViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [StringLength(200)]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Message is required")]
    [StringLength(2000, MinimumLength = 10)]
    public string Message { get; set; }
}
```

**Server-Side Validation**:
```csharp
private ValidationResult ValidateContactSubmission()
{
    // Spam detection
    var suspiciousPatterns = new[]
    {
        "http://", "https://", "www.", "viagra", "casino", "loan", "bitcoin"
    };
    
    var messageContent = (Contact.Name + " " + Contact.Message).ToLowerInvariant();
    
    foreach (var pattern in suspiciousPatterns)
    {
        if (messageContent.Contains(pattern))
        {
            return new ValidationResult
            {
                IsValid = false,
                ErrorMessage = "Your message contains content that appears to be spam."
            };
        }
    }
    
    // Additional validation logic
    return new ValidationResult { IsValid = true };
}
```

**Email Domain Validation**:
```csharp
// Reject suspicious email domains
if (Contact.Email.Contains("test.com") || 
    Contact.Email.Contains("example.com") || 
    Contact.Email.Contains("tempmail"))
{
    return new ValidationResult
    {
        IsValid = false,
        ErrorMessage = "Please provide a valid email address."
    };
}
```

### 5. SQL Injection Prevention

**Purpose**: Prevent SQL injection attacks

**Entity Framework Core**: All database queries use parameterized queries automatically

```csharp
// Safe: Entity Framework automatically parameterizes
var projects = await _context.Projects
    .Where(p => p.Category == category)
    .ToListAsync();

// Safe: Raw SQL with parameters
var projects = await _context.Projects
    .FromSqlRaw("SELECT * FROM Projects WHERE Category = {0}", category)
    .ToListAsync();
```

**No String Concatenation**: All queries use EF Core's query builder or parameterized raw SQL

### 6. Secrets Management

**Purpose**: Keep sensitive data secure

**Development**: User Secrets
```bash
# Sensitive data stored in user secrets
dotnet user-secrets set "EmailSettings:Password" "app-password"
```

**Production**: Environment Variables
```bash
# Production secrets via environment variables
export EmailSettings__Password="production-password"
export EmailSettings__Username="production-email"
```

**Configuration Priority**:
1. Environment Variables (Production)
2. User Secrets (Development)
3. appsettings.json (Non-sensitive)

**Security Benefits**:
- No credentials in source code
- Separate secrets per environment
- Automatic configuration resolution

## 🔍 Security Testing

### Rate Limiting Tests

```csharp
[Fact]
public async Task RateLimit_ShouldBlockExcessiveRequests()
{
    // Test rate limiting for contact form
    var context = CreateHttpContext("/Contact");
    
    // Make 6 requests (limit is 5)
    for (int i = 0; i < 6; i++)
    {
        await _middleware.InvokeAsync(context);
    }
    
    Assert.Equal(429, context.Response.StatusCode);
}
```

### Security Headers Tests

```csharp
[Fact]
public async Task SecurityHeaders_ShouldBePresent()
{
    var context = new DefaultHttpContext();
    
    await _middleware.InvokeAsync(context);
    
    Assert.Equal("DENY", context.Response.Headers["X-Frame-Options"]);
    Assert.Equal("nosniff", context.Response.Headers["X-Content-Type-Options"]);
    Assert.True(context.Response.Headers.ContainsKey("Content-Security-Policy"));
}
```

### Input Validation Tests

```csharp
[Theory]
[InlineData("Buy viagra now!")]
[InlineData("Check out this website http://spam.com")]
public void ContactValidation_ShouldRejectSpamContent(string message)
{
    var contact = new ContactViewModel { Message = message };
    var result = ValidateModel(contact);
    
    Assert.Contains(result, v => v.ErrorMessage.Contains("spam"));
}
```

## 📊 Security Monitoring

### Request Logging

```csharp
_logger.LogWarning("Rate limit exceeded for {ClientKey} on {Path}", 
    clientKey, context.Request.Path);
```

### Security Events Logged

- Rate limit violations
- Invalid input attempts
- Suspicious email domains
- CSRF token validation failures

### Monitoring Recommendations

1. **Log Analysis**: Monitor security-related logs
2. **Rate Limiting**: Track rate limit violations
3. **Input Validation**: Monitor spam attempts
4. **Error Rates**: Track 4xx/5xx responses

## 🚨 Incident Response

### Rate Limit Violations

**Detection**: Logged as warnings with client identification

**Response**: 
1. Temporary IP blocking (manual)
2. Adjust rate limits if needed
3. Investigate patterns

### Suspected Attacks

**Indicators**:
- Multiple rate limit violations
- Suspicious input patterns
- Unusual request patterns

**Response**:
1. Review logs for attack patterns
2. Temporarily reduce rate limits
3. Consider additional security measures

## 🔧 Security Configuration

### Production Security Checklist

- [ ] **HTTPS**: Enforce HTTPS with HSTS
- [ ] **Environment Variables**: Use for all secrets
- [ ] **Rate Limiting**: Configured appropriately
- [ ] **Security Headers**: All headers implemented
- [ ] **Input Validation**: Comprehensive validation
- [ ] **Logging**: Security events logged
- [ ] **Database**: Use connection pooling
- [ ] **Updates**: Keep dependencies updated

### Security Headers Validation

**Tools for Testing**:
- [SecurityHeaders.com](https://securityheaders.com/)
- [Mozilla Observatory](https://observatory.mozilla.org/)
- Browser Developer Tools

### Content Security Policy

```
default-src 'self';
script-src 'self' 'unsafe-inline' 'unsafe-eval' 
  https://cdnjs.cloudflare.com 
  https://cdn.jsdelivr.net;
style-src 'self' 'unsafe-inline' 
  https://fonts.googleapis.com;
font-src 'self' https://fonts.gstatic.com;
img-src 'self' data: https:;
```

## 🌐 GDPR & Privacy Compliance

### Data Collection

**Personal Data Collected**:
- Name and email (contact form)
- IP address (rate limiting)
- User agent (rate limiting)

**Data Usage**:
- Contact form: Business communication only
- Rate limiting: Security purposes only
- Analytics: Anonymized website usage

**Data Retention**:
- Contact messages: Indefinite (business purposes)
- Rate limiting data: 1 hour TTL
- Logs: 30 days retention

### User Rights

- **Access**: Users can request their data
- **Deletion**: Contact form data can be deleted
- **Portability**: Data provided in readable format

## 📋 Security Audit Trail

### Security Measures Timeline

1. **Initial Implementation**: Basic ASP.NET Core security
2. **Rate Limiting**: Custom middleware implementation
3. **Security Headers**: Comprehensive header middleware
4. **Input Validation**: Enhanced spam detection
5. **Secrets Management**: User secrets + environment variables
6. **Testing**: Comprehensive security test suite

### Compliance Standards

- **OWASP Top 10**: Addresses major web vulnerabilities
- **Singapore PDPA**: Personal data protection compliance
- **Enterprise Security**: Meets enterprise-grade standards

## 🔮 Future Security Enhancements

### Planned Improvements

1. **IP Whitelisting**: For admin functions
2. **API Rate Limiting**: More granular rate limits
3. **WAF Integration**: Web Application Firewall
4. **Security Monitoring**: Real-time threat detection
5. **Automated Security Testing**: CI/CD integration

### Recommendations

1. **Regular Security Audits**: Quarterly reviews
2. **Dependency Updates**: Monthly security updates
3. **Penetration Testing**: Annual security testing
4. **Security Training**: Stay updated on threats

---

**Security is not a destination, but a journey. Stay vigilant! 🛡️**