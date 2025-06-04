// wwwroot/js/site.js

// Wait for DOM to be ready
document.addEventListener('DOMContentLoaded', function() {
    
    // Initialize all components
    initNavigation();
    initAnimations();
    initContactForm();
    initSkillAnimations();
    initSmoothScroll();
    initImageLazyLoading();
    initTheme();
    initThemeToggle();
});

// Navigation functionality
function initNavigation() {
    const navbar = document.querySelector('.navbar');
    const navLinks = document.querySelectorAll('.navbar-nav .nav-link');
    
    // Add active class to current page
    const currentPath = window.location.pathname;
    navLinks.forEach(link => {
        if (link.getAttribute('href') === currentPath) {
            link.classList.add('active');
        }
    });
    
    // Navbar scroll effect
    if (navbar) {
        window.addEventListener('scroll', function() {
            if (window.scrollY > 100) {
                navbar.style.backgroundColor = 'rgba(13, 110, 253, 0.95)';
            } else {
                navbar.style.backgroundColor = '';
            }
        });
    }
}

// Smooth scrolling for anchor links
function initSmoothScroll() {
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            const target = document.querySelector(this.getAttribute('href'));
            if (target) {
                const offsetTop = target.offsetTop - 80; // Account for fixed navbar
                window.scrollTo({
                    top: offsetTop,
                    behavior: 'smooth'
                });
            }
        });
    });
}

// Intersection Observer for scroll animations
function initAnimations() {
    const animatedElements = document.querySelectorAll('.animate-on-scroll');
    
    if (animatedElements.length === 0) return;
    
    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -50px 0px'
    };
    
    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);
    
    animatedElements.forEach(element => {
        observer.observe(element);
    });
}

// Skill progress bar animations
function initSkillAnimations() {
    const skillBars = document.querySelectorAll('.progress-bar');
    
    if (skillBars.length === 0) return;
    
    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const progressBar = entry.target;
                const width = progressBar.style.width;
                progressBar.style.width = '0%';
                
                setTimeout(() => {
                    progressBar.style.width = width;
                }, 200);
                
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.5 });
    
    skillBars.forEach(bar => {
        observer.observe(bar);
    });
}

// Contact form handling
function initContactForm() {
    const contactForm = document.getElementById('contactForm');
    if (!contactForm) return;
    
    contactForm.addEventListener('submit', function(e) {
        e.preventDefault();
        
        const submitBtn = contactForm.querySelector('button[type="submit"]');
        const originalText = submitBtn.innerHTML;
        
        // Show loading state
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Sending...';
        submitBtn.disabled = true;
        
        // Collect form data
        const formData = new FormData(contactForm);
        
        // Submit form
        fetch(contactForm.action, {
            method: 'POST',
            body: formData
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                showAlert('success', 'Thank you! Your message has been sent successfully.');
                contactForm.reset();
            } else {
                showAlert('danger', 'Sorry, there was an error sending your message. Please try again.');
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showAlert('danger', 'Sorry, there was an error sending your message. Please try again.');
        })
        .finally(() => {
            // Restore button state
            submitBtn.innerHTML = originalText;
            submitBtn.disabled = false;
        });
    });
}

// Show alert messages
function showAlert(type, message) {
    const alertContainer = document.getElementById('alertContainer') || createAlertContainer();
    
    const alert = document.createElement('div');
    alert.className = `alert alert-${type} alert-dismissible fade show`;
    alert.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    
    alertContainer.appendChild(alert);
    
    // Auto-dismiss after 5 seconds
    setTimeout(() => {
        if (alert.parentNode) {
            alert.remove();
        }
    }, 5000);
}

function createAlertContainer() {
    const container = document.createElement('div');
    container.id = 'alertContainer';
    container.style.position = 'fixed';
    container.style.top = '20px';
    container.style.right = '20px';
    container.style.zIndex = '1050';
    container.style.maxWidth = '400px';
    document.body.appendChild(container);
    return container;
}

// Lazy loading for images
function initImageLazyLoading() {
    const images = document.querySelectorAll('img[data-src]');
    
    if (images.length === 0) return;
    
    const imageObserver = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                const img = entry.target;
                img.src = img.dataset.src;
                img.classList.remove('lazy');
                imageObserver.unobserve(img);
            }
        });
    });
    
    images.forEach(img => imageObserver.observe(img));
}

// Utility functions
function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

function throttle(func, limit) {
    let inThrottle;
    return function() {
        const args = arguments;
        const context = this;
        if (!inThrottle) {
            func.apply(context, args);
            inThrottle = true;
            setTimeout(() => inThrottle = false, limit);
        }
    }
}

// Copy to clipboard functionality
function copyToClipboard(text, element) {
    navigator.clipboard.writeText(text).then(function() {
        const originalText = element.textContent;
        element.textContent = 'Copied!';
        element.classList.add('text-success');
        
        setTimeout(() => {
            element.textContent = originalText;
            element.classList.remove('text-success');
        }, 2000);
    });
}

// Blog search functionality (if needed)
function initBlogSearch() {
    const searchInput = document.getElementById('blogSearch');
    if (!searchInput) return;
    
    const searchPosts = debounce(function(query) {
        const posts = document.querySelectorAll('.blog-post-item');
        
        posts.forEach(post => {
            const title = post.querySelector('.blog-title').textContent.toLowerCase();
            const content = post.querySelector('.blog-excerpt').textContent.toLowerCase();
            const tags = post.querySelector('.blog-tags').textContent.toLowerCase();
            
            if (title.includes(query) || content.includes(query) || tags.includes(query)) {
                post.style.display = '';
            } else {
                post.style.display = 'none';
            }
        });
    }, 300);
    
    searchInput.addEventListener('input', function(e) {
        const query = e.target.value.toLowerCase().trim();
        if (query.length > 2) {
            searchPosts(query);
        } else {
            // Show all posts
            document.querySelectorAll('.blog-post-item').forEach(post => {
                post.style.display = '';
            });
        }
    });
}

// Portfolio filter functionality
function initPortfolioFilter() {
    const filterButtons = document.querySelectorAll('.portfolio-filter');
    const portfolioItems = document.querySelectorAll('.portfolio-item');
    
    if (filterButtons.length === 0) return;
    
    filterButtons.forEach(button => {
        button.addEventListener('click', function() {
            const filter = this.dataset.filter;
            
            // Update active button
            filterButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');
            
            // Filter items
            portfolioItems.forEach(item => {
                if (filter === 'all' || item.dataset.category === filter) {
                    item.style.display = '';
                    item.style.opacity = '1';
                } else {
                    item.style.opacity = '0';
                    setTimeout(() => {
                        item.style.display = 'none';
                    }, 300);
                }
            });
        });
    });
}

// Initialize tooltips and popovers if Bootstrap is available
document.addEventListener('DOMContentLoaded', function() {
    if (typeof bootstrap !== 'undefined') {
        // Initialize tooltips
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
        
        // Initialize popovers
        const popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
        popoverTriggerList.map(function (popoverTriggerEl) {
            return new bootstrap.Popover(popoverTriggerEl);
        });
    }
});

// Back to top button
function initBackToTop() {
    const backToTopBtn = document.createElement('button');
    backToTopBtn.innerHTML = '<i class="fas fa-arrow-up"></i>';
    backToTopBtn.className = 'btn btn-primary position-fixed';
    backToTopBtn.style.cssText = `
        bottom: 20px;
        right: 20px;
        z-index: 1000;
        border-radius: 50%;
        width: 50px;
        height: 50px;
        display: none;
        opacity: 0.8;
    `;
    backToTopBtn.setAttribute('aria-label', 'Back to top');
    
    document.body.appendChild(backToTopBtn);
    
    // Show/hide button based on scroll position
    window.addEventListener('scroll', throttle(function() {
        if (window.pageYOffset > 300) {
            backToTopBtn.style.display = 'block';
        } else {
            backToTopBtn.style.display = 'none';
        }
    }, 100));
    
    // Scroll to top when clicked
    backToTopBtn.addEventListener('click', function() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
}
function initTheme() {
    // Check for saved theme preference or default to 'light'
    const savedTheme = localStorage.getItem('theme') || 
                      (window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');
    
    setTheme(savedTheme);
}

function initThemeToggle() {
    const themeToggle = document.getElementById('themeToggle');
    if (!themeToggle) return;
    
    themeToggle.addEventListener('click', function() {
        const currentTheme = document.documentElement.getAttribute('data-theme');
        const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
        
        setTheme(newTheme);
        
        // Add a subtle animation to the toggle button
        this.style.transform = 'scale(0.95)';
        setTimeout(() => {
            this.style.transform = 'scale(1)';
        }, 150);
    });
}

function setTheme(theme) {
    // Set the theme attribute on the document element
    document.documentElement.setAttribute('data-theme', theme);
    
    // Save the preference to localStorage
    localStorage.setItem('theme', theme);
    
    // Update the toggle button icons
    updateThemeToggleIcons(theme);
    
    // Update meta theme-color for mobile browsers
    updateMetaThemeColor(theme);
    
    // Dispatch custom event for other components that might need to know about theme changes
    window.dispatchEvent(new CustomEvent('themeChanged', { detail: { theme } }));
}

function updateThemeToggleIcons(theme) {
    const lightIcon = document.querySelector('.theme-icon-light');
    const darkIcon = document.querySelector('.theme-icon-dark');
    
    if (!lightIcon || !darkIcon) return;
    
    if (theme === 'dark') {
        lightIcon.style.display = 'none';
        darkIcon.style.display = 'inline-block';
    } else {
        lightIcon.style.display = 'inline-block';
        darkIcon.style.display = 'none';
    }
}

function updateMetaThemeColor(theme) {
    const themeColorMeta = document.querySelector('meta[name="theme-color"]');
    const color = theme === 'dark' ? '#202124' : '#0d6efd';
    
    if (themeColorMeta) {
        themeColorMeta.setAttribute('content', color);
    } else {
        // Create meta tag if it doesn't exist
        const meta = document.createElement('meta');
        meta.name = 'theme-color';
        meta.content = color;
        document.head.appendChild(meta);
    }
}

// Listen for system theme changes
if (window.matchMedia) {
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', function(e) {
        // Only auto-switch if user hasn't manually set a preference
        if (!localStorage.getItem('theme')) {
            setTheme(e.matches ? 'dark' : 'light');
        }
    });
}

// Keyboard accessibility for theme toggle
document.addEventListener('keydown', function(e) {
    // Toggle theme with Ctrl/Cmd + Shift + T
    if ((e.ctrlKey || e.metaKey) && e.shiftKey && e.key === 'T') {
        e.preventDefault();
        const themeToggle = document.getElementById('themeToggle');
        if (themeToggle) {
            themeToggle.click();
        }
    }
});

// Utility function to get current theme
function getCurrentTheme() {
    return document.documentElement.getAttribute('data-theme') || 'light';
}
// Initialize back to top button
document.addEventListener('DOMContentLoaded', initBackToTop);